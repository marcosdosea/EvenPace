using Core;
using Core.Service;
using Core.Service.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Service
{
    public class InscricaoService : IInscricaoService
    {
        private readonly EvenPaceContext _context;
        private readonly IEventosService _eventoService;
        private readonly IKitService _kitService;

        public InscricaoService(
            EvenPaceContext context,
            IEventosService eventoService,
            IKitService kitService)
        {
            _context = context;
            _eventoService = eventoService;
            _kitService = kitService;
        }

        public void Cancelar(int idInscricao, int idCorredor)
        {
            var inscricao = _context.Inscricao
                .Include(i => i.IdEventoNavigation)
                .FirstOrDefault(i => i.Id == idInscricao && i.IdCorredor == idCorredor);

            if (inscricao == null)
                throw new Exception("Inscrição não encontrada ou não pertence ao corredor.");

            if (inscricao.IdEventoNavigation.Data < DateTime.Now)
                throw new Exception("Não é possível cancelar após a data do evento.");

            if (inscricao.Status == "Cancelada")
                return;

            inscricao.Status = "Cancelada";

            _context.Update(inscricao);
            _context.SaveChanges();
        }

        public int Create(Inscricao inscricao)
        {
            _context.Add(inscricao);
            _context.SaveChanges();
            return inscricao.Id;
        }

        public void Edit(Inscricao inscricao)
        {
            _context.Update(inscricao);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var _inscricao = _context.Inscricao.Find(id);
            _context.Remove(_inscricao);
            _context.SaveChanges();            
        }

        public Inscricao Get(int id)
        {
            return _context.Inscricao.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Inscricao> GetAll()
        {
            return _context.Inscricao;
        }
        
        public IEnumerable<Inscricao> GetAllByEvento(int idEvento)
        {
            return _context.Inscricao
                .Include(i => i.IdKitNavigation)
                .Include(i => i.IdCorredorNavigation)
                .Include(i => i.IdEventoNavigation)
                .Where(i => i.IdEvento == idEvento)
                .ToList();
        }

        public DadosTelaInscricaoDto GetDadosTelaInscricao(int idEvento)
        {
            var evento = _eventoService.Get(idEvento);
            if (evento == null)
                throw new InvalidOperationException($"Evento {idEvento} não existe no banco.");

            var kits = _kitService.GetKitsPorEvento(idEvento);

            return new DadosTelaInscricaoDto
            {
                IdEvento = evento.Id,
                NomeEvento = evento.Nome,
                Local = evento.Cidade,
                DataEvento = evento.Data,
                Descricao = evento.Descricao,
                ImagemEvento = evento.Imagem,
                Kits = kits
            };
        }

        public GetDadosTelaDeleteResult GetDadosTelaDelete(int idInscricao)
        {
            var inscricao = _context.Inscricao
                .Include(i => i.IdEventoNavigation)
                .Include(i => i.IdKitNavigation)
                .FirstOrDefault(i => i.Id == idInscricao);

            if (inscricao == null)
                return new GetDadosTelaDeleteResult { Success = false, ErrorType = "NotFound" };

            if (inscricao.IdEventoNavigation.Data < DateTime.Now)
                return new GetDadosTelaDeleteResult { Success = false, ErrorType = "EventoExpirado" };

            var kit = inscricao.IdKit.HasValue ? _kitService.Get(inscricao.IdKit.Value) : null;
            var nomeKit = kit?.Nome ?? "Sem kit";

            return new GetDadosTelaDeleteResult
            {
                Success = true,
                Data = new DadosTelaDeleteDto
                {
                    NomeEvento = inscricao.IdEventoNavigation.Nome,
                    DataEvento = inscricao.IdEventoNavigation.Data,
                    Local = inscricao.IdEventoNavigation.Cidade,
                    NomeKit = nomeKit,
                    IdInscricao = inscricao.Id,
                    Distancia = inscricao.Distancia,
                    TamanhoCamisa = inscricao.TamanhoCamisa,
                    DataInscricao = inscricao.DataInscricao
                }
            };
        }
    }
}
