using Core;
using Core.Service;
using Core.Service.Dtos;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Processa o cancelamento lógico da inscrição, avaliando previamente a compatibilidade entre titular e evento, bem como restrições de prazo temporal estipuladas.
        /// </summary>
        /// <param name="idInscricao">O ID da inscrição no banco de dados.</param>
        /// <param name="idCorredor">A identificação numérica do solicitante logado.</param>
        /// <exception cref="Exception">Apresenta exceções descritivas em casos de incompatibilidade de usuário, registro inexistente ou expiração temporal em relação ao evento correspondente.</exception>
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

        /// <summary>
        /// Salva uma nova requisição de participação (inscrição) na tabela baseada nos dados do formulário submetido pelo atleta.
        /// </summary>
        /// <param name="inscricao">Objeto contendo as seleções de prova e relacionamento relativas ao usuário.</param>
        /// <returns>Identificador recém-gerado da inscrição validada no banco.</returns>
        public int Create(Inscricao inscricao)
        {
            _context.Add(inscricao);
            _context.SaveChanges();
            return inscricao.Id;
        }

        /// <summary>
        /// Promove alterações em um registro de inscrição ativado na base.
        /// </summary>
        /// <param name="inscricao">Objeto populado com os valores superpostos à entidade antiga.</param>
        public void Edit(Inscricao inscricao)
        {
            _context.Update(inscricao);
            _context.SaveChanges();
        }

        /// <summary>
        /// Remove permanentemente o vínculo de inscrição dos servidores persistentes.
        /// </summary>
        /// <param name="id">Chave de registro exclusivo da inscrição a ser decomposta.</param>
        public void Delete(int id)
        {
            var _inscricao = _context.Inscricao.Find(id);
            _context.Remove(_inscricao);
            _context.SaveChanges();
        }

        /// <summary>
        /// Seleciona as características de uma inscrição fornecendo o seu respectivo Id.
        /// </summary>
        /// <param name="id">Id da inscrição requisitada.</param>
        /// <returns>Entidade localizada de Inscrição.</returns>
        public Inscricao Get(int id)
        {
            return _context.Inscricao.FirstOrDefault(i => i.Id == id);
        }

        /// <summary>
        /// Fornece o controle integral de todas as inscrições criadas historicamente sem filtro específico de evento.
        /// </summary>
        /// <returns>Iterador abrangendo todo o contexto de inscrições inseridas.</returns>
        public IEnumerable<Inscricao> GetAll()
        {
            return _context.Inscricao;
        }

        /// <summary>
        /// Resgata toda a segmentação de ingressos comprados referentes a um mesmo campeonato esportivo, embutindo os relacionamentos vinculados.
        /// </summary>
        /// <param name="idEvento">Código-alvo da busca agrupadora.</param>
        /// <returns>Conjunto mapeado com navegabilidade para kit, corredor e detalhamento do evento correspondente.</returns>
        public IEnumerable<Inscricao> GetAllByEvento(int idEvento)
        {
            return _context.Inscricao
                .Include(i => i.IdKitNavigation)
                .Include(i => i.IdCorredorNavigation)
                .Include(i => i.IdEventoNavigation)
                .Where(i => i.IdEvento == idEvento)
                .ToList();
        }

        /// <summary>
        /// Produz uma estrutura unificada aglomerando os dados básicos de um evento bem como as alternativas de aquisição (Kits) correspondentes a ele, viabilizando o carregamento de telas interativas ao usuário.
        /// </summary>
        /// <param name="idEvento">A identificação de contexto do evento a ser analisado.</param>
        /// <returns>Instância de Dto com variáveis agrupadas para a visualização.</returns>
        /// <exception cref="InvalidOperationException">Exceção forçada quando o evento consultado não reside ativamente no diretório de informações.</exception>
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

        /// <summary>
        /// Analisa as condicionalidades temporais e coleta as informações essenciais necessárias para preencher corretamente a confirmação em tela de um cancelamento de inscrição iminente.
        /// </summary>
        /// <param name="idInscricao">Id da requisição do participante.</param>
        /// <returns>Classe Result providenciando o sucesso ou a justificativa explícita de recusa e, caso aprovada, as propriedades da inscrição a serem apresentadas.</returns>
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

        /// <summary>
        /// Efetiva e homologa a retirada do material desportivo, chaveando as informações de validação do banco para confirmar o recebimento pelo participante.
        /// </summary>
        /// <param name="idInscricao">A chave primária correspondente à inscrição e respectivo ticket validado no posto de controle.</param>
        public void ConfirmarRetiradaKit(int idInscricao)
        {
            var inscricao = _context.Inscricao.Find(idInscricao);

            if (inscricao != null)
            {
                inscricao.StatusRetiradaKit = true;

                _context.SaveChanges();
            }
        }
    }
}