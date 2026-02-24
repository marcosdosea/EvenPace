using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System;
using System.Linq;

namespace EvenPaceWebTests.Service
{
    [TestClass()]
    public class AvaliacaoEventoServiceTest
    {
        private EvenPaceContext context;
        private IAvaliacaoEventoService avaliacaoService;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<EvenPaceContext>();
            builder.UseInMemoryDatabase("EvenPaceAvaliacaoTest");

            context = new EvenPaceContext(builder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Evento que já ocorreu
            var evento = new Evento
            {
                Id = 1,
                Nome = "Corrida 5K",
                Data = DateTime.Now.AddDays(-2)
            };

            // Inscrição válida ligada ao evento
            var inscricao = new Inscricao
            {
                Id = 1,
                IdEvento = 1,
                IdCorredor = 1
            };

            context.Eventos.Add(evento);
            context.Inscricao.Add(inscricao);
            context.SaveChanges();

            avaliacaoService = new AvaliacaoEventoService(context);
        }

        // -------------------------
        // TESTE VÁLIDO
        // -------------------------
        [TestMethod()]
        public void CreateTest_Valido()
        {
            var inscricao = context.Inscricao.First();

            var avaliacao = new AvaliacaoEvento
            {
                Id = 1,
                Estrela = 5,
                Comentario = "Excelente corrida!"
            };

            avaliacao.Inscricaos.Add(inscricao);

            avaliacaoService.Create(avaliacao);

            Assert.AreEqual(1, context.AvaliacaoEventos.Count());
        }

        // -------------------------
        // EVENTO NÃO OCORREU
        // -------------------------
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CreateTest_EventoNaoOcorreu_DeveLancarExcecao()
        {
            var eventoFuturo = new Evento
            {
                Id = 2,
                Nome = "Corrida 10K",
                Data = DateTime.Now.AddDays(5)
            };

            var inscricaoFutura = new Inscricao
            {
                Id = 2,
                IdEvento = 2,
                IdCorredor = 1
            };

            context.Eventos.Add(eventoFuturo);
            context.Inscricao.Add(inscricaoFutura);
            context.SaveChanges();

            var avaliacao = new AvaliacaoEvento
            {
                Id = 2,
                Estrela = 4,
                Comentario = "Boa"
            };

            avaliacao.Inscricaos.Add(inscricaoFutura);

            avaliacaoService.Create(avaliacao);
        }

        // -------------------------
        // INSCRIÇÃO INEXISTENTE
        // -------------------------
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CreateTest_InscricaoInexistente_DeveLancarExcecao()
        {
            var avaliacao = new AvaliacaoEvento
            {
                Id = 3,
                Estrela = 3,
                Comentario = "Ok"
            };

            avaliacaoService.Create(avaliacao);
        }

        // -------------------------
        // AVALIAÇÃO DUPLICADA
        // -------------------------
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CreateTest_AvaliacaoDuplicada_DeveLancarExcecao()
        {
            var inscricao = context.Inscricao.First();

            var avaliacao = new AvaliacaoEvento
            {
                Id = 4,
                Estrela = 5,
                Comentario = "Muito boa"
            };

            avaliacao.Inscricaos.Add(inscricao);

            avaliacaoService.Create(avaliacao);
            avaliacaoService.Create(avaliacao);
        }
    }
}
