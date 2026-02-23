using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;

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

            // Inscrição válida
            var inscricao = new Inscricao
            {
                Id = 1,
                IdEvento = 1,
                IdCorredor = 1
            };

            context.Add(evento);
            context.Add(inscricao);
            context.SaveChanges();

            avaliacaoService = new AvaliacaoEventoService(context);
        }

        [TestMethod()]
        public void CreateTest_Valido()
        {
            var avaliacao = new AvaliacaoEvento
            {
                Id = 1,
                Estrela = 5,
                Comentario = "Excelente corrida!"
            };

            avaliacaoService.Create(avaliacao);

            Assert.AreEqual(1, context.AvaliacaoEventos.Count());
        }

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

            context.Add(eventoFuturo);
            context.SaveChanges();

            var avaliacao = new AvaliacaoEvento
            {
                Id = 2,
                Estrela = 4,
                Comentario = "Boa"
            };

            avaliacaoService.Create(avaliacao);
        }

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

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CreateTest_AvaliacaoDuplicada_DeveLancarExcecao()
        {
            var avaliacao = new AvaliacaoEvento
            {
                Id = 4,
                Estrela = 5,
                Comentario = "Muito boa"
            };

            avaliacaoService.Create(avaliacao);
            avaliacaoService.Create(avaliacao); // segunda tentativa
        }
    }
}
