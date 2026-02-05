using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;

namespace EvenPaceWebTests.Service
{
    [TestClass()]
    public class CupomServiceTests
    {
        private EvenPaceContext context;
        private ICupomService cupomService;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<EvenPaceContext>();
            builder.UseInMemoryDatabase("EvenPace"); 
            var options = builder.Options;

            context = new EvenPaceContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var cupons = new List<Cupom>
            {
                new Cupom { Id = 1, Nome = "VERAO10", Desconto = 10 },
                new Cupom { Id = 2, Nome = "NATAL25", Desconto = 25 },
                new Cupom { Id = 3, Nome = "BLACK50", Desconto = 50 }
            };

            context.AddRange(cupons);
            context.SaveChanges();

            cupomService = new CupomService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            cupomService.Create(new Cupom() { Id = 4, Nome = "PROMO2026", Desconto = 15 });

            Assert.AreEqual(4, cupomService.GetAll().Count());
            var cupom = cupomService.Get(4);
            Assert.IsNotNull(cupom);
            Assert.AreEqual("PROMO2026", cupom.Nome);
            Assert.AreEqual(15, cupom.Desconto);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            int idParaDeletar = 2; 

            cupomService.Delete(idParaDeletar);

            Assert.AreEqual(2, cupomService.GetAll().Count());
            Assert.ThrowsException<ServiceException>(() => cupomService.Get(idParaDeletar));
        }

        [TestMethod()]
        public void EditTest()
        { 
            var cupom = cupomService.Get(3);
            cupom.Nome = "BLACK_FRIDAY_EDITADO";
            cupom.Desconto = 60;

            cupomService.Edit(cupom);

            var cupomEditado = cupomService.Get(3);
            Assert.IsNotNull(cupomEditado);
            Assert.AreEqual("BLACK_FRIDAY_EDITADO", cupomEditado.Nome);
            Assert.AreEqual(60, cupomEditado.Desconto);
        }

        [TestMethod()]
        public void GetTest()
        {
            var cupom = cupomService.Get(1);

            Assert.IsNotNull(cupom);
            Assert.AreEqual("VERAO10", cupom.Nome);
            Assert.AreEqual(10, cupom.Desconto);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listaCupons = cupomService.GetAll();

            Assert.IsInstanceOfType(listaCupons, typeof(IEnumerable<Cupom>));
            Assert.IsNotNull(listaCupons);
            Assert.AreEqual(3, listaCupons.Count());
            Assert.AreEqual((int)1, listaCupons.First().Id);
            Assert.AreEqual("VERAO10", listaCupons.First().Nome);
        }

        [TestMethod()]
        public void GetByNameTest()
        {
            var resultado = cupomService.GetByName("NATAL");

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any(c => c.Nome.Contains("NATAL")));
            Assert.AreEqual("NATAL25", resultado.First().Nome);
        }
    }
}