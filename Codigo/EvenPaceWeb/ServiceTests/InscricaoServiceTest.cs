using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System.Collections.Generic;
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
                .UseInMemoryDatabase("InscricaoTestDB")
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
    }

}
