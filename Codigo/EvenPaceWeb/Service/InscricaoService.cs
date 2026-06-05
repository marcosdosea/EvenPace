using Core;
using Core.Service;
using Core.Service.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class InscricaoService : IInscricaoService
    {
        private static readonly HashSet<string> TamanhosCamisaValidos = new(StringComparer.OrdinalIgnoreCase)
        {
            "P",
            "M",
            "G",
            "GG"
        };

        private readonly EvenPaceContext _context;

        public InscricaoService(EvenPaceContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Inscricao inscricao)
        {
            if (inscricao is null)
                throw new ArgumentNullException(nameof(inscricao));

            await ValidarInscricaoAsync(inscricao);

            await _context.Inscricao.AddAsync(inscricao);
            await _context.SaveChangesAsync();
            return inscricao.Id;
        }

        public async Task EditAsync(Inscricao inscricao)
        {
            if (inscricao is null)
                throw new ArgumentNullException(nameof(inscricao));

            _context.Entry(inscricao).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var inscricao = await _context.Inscricao.FirstOrDefaultAsync(i => i.Id == id);
            if (inscricao is null)
                return;

            _context.Inscricao.Remove(inscricao);
            await _context.SaveChangesAsync();
        }

        public async Task<Inscricao?> GetAsync(int id)
        {
            return await _context.Inscricao
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Inscricao>> GetAllAsync()
        {
            return await _context.Inscricao
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Inscricao>> GetAllByEventoAsync(int idEvento)
        {
            return await _context.Inscricao
                .AsNoTracking()
                .Include(i => i.IdKitNavigation)
                .Include(i => i.IdCorredorNavigation)
                .Include(i => i.IdEventoNavigation)
                .Where(i => i.IdEvento == idEvento)
                .OrderBy(i => i.Id)
                .ToListAsync();
        }

        public async Task<DadosTelaInscricaoDto> GetDadosTelaInscricaoAsync(int idEvento)
        {
            var evento = await _context.Eventos
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == idEvento);

            if (evento is null)
                throw new InvalidOperationException($"Evento {idEvento} não existe no banco.");

            var kits = await _context.Kits
                .AsNoTracking()
                .Where(k => k.IdEvento == idEvento)
                .OrderBy(k => k.Id)
                .ToListAsync();

            return new DadosTelaInscricaoDto
            {
                IdEvento = evento.Id,
                NomeEvento = evento.Nome,
                Local = evento.Cidade,
                DataEvento = evento.Data,
                Descricao = evento.Descricao,
                ImagemEvento = evento.Imagem,
                InfoRetiradaKit = evento.InfoRetiradaKit,
                Percursos = ObterPercursosDisponiveis(evento),
                Kits = kits
            };
        }

        public async Task<GetDadosTelaDeleteResult> GetDadosTelaDeleteAsync(int idInscricao)
        {
            var inscricao = await _context.Inscricao
                .AsNoTracking()
                .Include(i => i.IdEventoNavigation)
                .Include(i => i.IdCorredorNavigation)
                .Include(i => i.IdKitNavigation)
                .FirstOrDefaultAsync(i => i.Id == idInscricao);

            if (inscricao is null)
                return new GetDadosTelaDeleteResult { Success = false, ErrorType = "NotFound" };

            if (inscricao.IdEventoNavigation.Data < DateTime.Now)
                return new GetDadosTelaDeleteResult { Success = false, ErrorType = "EventoExpirado" };

            return new GetDadosTelaDeleteResult
            {
                Success = true,
                Data = new DadosTelaDeleteDto
                {
                    NomeEvento = inscricao.IdEventoNavigation.Nome,
                    DataEvento = inscricao.IdEventoNavigation.Data,
                    Local = inscricao.IdEventoNavigation.Cidade,
                    NomeCorredor = inscricao.IdCorredorNavigation?.Nome ?? "Corredor",
                    NomeKit = inscricao.IdKitNavigation?.Nome ?? "Sem kit",
                    IdInscricao = inscricao.Id,
                    Distancia = inscricao.Distancia,
                    TamanhoCamisa = inscricao.TamanhoCamisa,
                    DataInscricao = inscricao.DataInscricao
                }
            };
        }

        public async Task CancelarAsync(int idInscricao, int idCorredor)
        {
            var inscricao = await _context.Inscricao
                .Include(i => i.IdEventoNavigation)
                .FirstOrDefaultAsync(i => i.Id == idInscricao && i.IdCorredor == idCorredor);

            if (inscricao is null)
                throw new InvalidOperationException("Inscrição não encontrada ou não pertence ao corredor.");

            if (inscricao.IdEventoNavigation.Data < DateTime.Now)
                throw new InvalidOperationException("Não é possível cancelar após a data do evento.");

            if (string.Equals(inscricao.Status, "Cancelada", StringComparison.OrdinalIgnoreCase))
                return;

            inscricao.Status = "Cancelada";
            await _context.SaveChangesAsync();
        }

        public async Task ConfirmarRetiradaKitAsync(int idInscricao)
        {
            var inscricao = await _context.Inscricao.FirstOrDefaultAsync(i => i.Id == idInscricao);

            if (inscricao is null || inscricao.StatusRetiradaKit)
                return;

            inscricao.StatusRetiradaKit = true;
            await _context.SaveChangesAsync();
        }

        private async Task ValidarInscricaoAsync(Inscricao inscricao)
        {
            if (inscricao.IdCorredor <= 0)
                throw new InvalidOperationException("Corredor inválido para a inscrição.");

            var evento = await _context.Eventos
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == inscricao.IdEvento);

            if (evento is null)
                throw new InvalidOperationException($"Evento {inscricao.IdEvento} não existe no banco.");

            if (evento.Data < DateTime.Now)
                throw new InvalidOperationException("Não é possível realizar a inscrição porque a data do evento já expirou.");

            if (inscricao.IdKit is null || inscricao.IdKit <= 0)
                throw new InvalidOperationException("Selecione um kit para continuar.");

            var kitPertenceAoEvento = await _context.Kits
                .AsNoTracking()
                .AnyAsync(k => k.Id == inscricao.IdKit.Value && k.IdEvento == inscricao.IdEvento);

            if (!kitPertenceAoEvento)
                throw new InvalidOperationException("O kit selecionado não pertence ao evento informado.");

            var percursosDisponiveis = ObterPercursosDisponiveis(evento);
            if (string.IsNullOrWhiteSpace(inscricao.Distancia) ||
                !percursosDisponiveis.Contains(inscricao.Distancia, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Selecione uma distância válida para este evento.");
            }

            if (string.IsNullOrWhiteSpace(inscricao.TamanhoCamisa) ||
                !TamanhosCamisaValidos.Contains(inscricao.TamanhoCamisa))
            {
                throw new InvalidOperationException("Selecione um tamanho de camisa válido.");
            }

            var possuiInscricaoAtiva = await _context.Inscricao
                .AsNoTracking()
                .AnyAsync(i =>
                    i.IdCorredor == inscricao.IdCorredor &&
                    i.IdEvento == inscricao.IdEvento &&
                    !string.Equals(i.Status, "Cancelada", StringComparison.OrdinalIgnoreCase));

            if (possuiInscricaoAtiva)
                throw new InvalidOperationException("Você já possui uma inscrição para este evento.");
        }

        private static List<string> ObterPercursosDisponiveis(Evento evento)
        {
            var percursos = new List<string>();

            if (evento.Distancia3) percursos.Add("3km");
            if (evento.Distancia5) percursos.Add("5km");
            if (evento.Distancia7) percursos.Add("7km");
            if (evento.Distancia10) percursos.Add("10km");
            if (evento.Distancia15) percursos.Add("15km");
            if (evento.Distancia21) percursos.Add("21km");
            if (evento.Distancia42) percursos.Add("42km");

            return percursos;
        }
    }
}
