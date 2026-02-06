using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System;
using System.Linq;

namespace EvenPaceWebTests.Service
{
    [TestClass]
    public class InscricaoServiceTest
    {
        private EvenPaceContext context;
        private InscricaoService service;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<EvenPaceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new EvenPaceContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            service = new InscricaoService(context);
        }

        [TestMethod]
        public void Create_DeveAdicionarInscricaoESalvar()
        {
            var inscricao = new Inscricao
            {
                Status = "Pendente",
                Distancia = "5km",
                TamanhoCamisa = "M",
                IdEvento = 2,
                IdCorredor = 3,
                IdKit = 1
            };

            service.Create(inscricao);

            Assert.AreEqual(1, context.Inscricao.Count());
        }

        [TestMethod]
        public void Get_DeveRetornarInscricaoPorId()
        {
            var inscricao = new Inscricao
            {
                Id = 1,
                Status = "Confirmada",
                Distancia = "10km",
                TamanhoCamisa = "G",
                IdEvento = 1,
                IdCorredor = 1,
                IdKit = 1
            };

            context.Inscricao.Add(inscricao);
            context.SaveChanges();

            var result = service.Get(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void GetAll_DeveRetornarListaDeInscricoes()
        {
            context.Inscricao.AddRange(
                new Inscricao
                {
                    Id = 1,
                    Status = "Confirmada",
                    Distancia = "5km",
                    TamanhoCamisa = "M",
                    IdEvento = 1,
                    IdCorredor = 1,
                    IdKit = 1
                },
                new Inscricao
                {
                    Id = 2,
                    Status = "Pendente",
                    Distancia = "10km",
                    TamanhoCamisa = "G",
                    IdEvento = 1,
                    IdCorredor = 2,
                    IdKit = 1
                }
            );

            context.SaveChanges();

            var result = service.GetAll();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Cancelar_DeveAlterarStatusParaCancelada()
        {
            var evento = new Evento
            {
                Id = 1,
                Nome = "Evento Teste",
                Descricao = "Descrição teste",
                Cidade = "São Paulo",
                Estado = "SP",
                Bairro = "Centro",
                Rua = "Rua A",
                InfoRetiradaKit = "Retirada no local",
                Data = DateTime.Now.AddDays(5)
            };


            var inscricao = new Inscricao
            {
                Id = 1,
                Status = "Confirmada",
                Distancia = "5km",
                TamanhoCamisa = "M",
                IdEvento = 1,
                IdCorredor = 1,
                IdKit = 1,
                IdEventoNavigation = evento
            };

            context.Eventos.Add(evento);
            context.Inscricao.Add(inscricao);
            context.SaveChanges();

            service.Cancelar(1, 1);

            var result = context.Inscricao.First();
            Assert.AreEqual("Cancelada", result.Status);
        }

        [TestMethod]
        public void Cancelar_InscricaoNaoExiste_LancaExcecao()
        {
            Assert.ThrowsException<Exception>(() =>
                service.Cancelar(99, 1)
            );
        }
    }
}
