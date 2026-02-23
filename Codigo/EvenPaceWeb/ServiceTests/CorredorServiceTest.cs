using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvenPaceWebTests.Service
{
    [TestClass()]
    public class CorredorServiceTest
    {
        private EvenPaceContext context = null!;
        private ICorredorService corredorService = null!;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<EvenPaceContext>();
            builder.UseInMemoryDatabase("EvenPaceTest");

            var options = builder.Options;

            context = new EvenPaceContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var corredores = new List<Corredor>
            {
                new Corredor
                {
                    Id = 1,
                    Cpf = "10101010101",
                    Nome = "Felipe",
                    DataNascimento = new DateTime(2010, 10, 10)
                },
                new Corredor
                {
                    Id = 6,
                    Cpf = "10931094882",
                    Nome = "João Lucas",
                    DataNascimento = new DateTime(2000, 01, 01)
                },
                new Corredor
                {
                    Id = 3,
                    Cpf = "22233344455",
                    Nome = "Maria",
                    DataNascimento = new DateTime(1999, 12, 31)
                }
            };

            context.AddRange(corredores);
            context.SaveChanges();

            corredorService = new CorredorService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            corredorService.Create(new Corredor
            {
                Id = 4,
                Cpf = "55544433322",
                Nome = "Novo Corredor",
                DataNascimento = new DateTime(2005, 05, 05)
            });

            Assert.AreEqual(4, corredorService.GetAll().Count());

            var corredor = corredorService.Get(4);
            Assert.IsNotNull(corredor);
            Assert.AreEqual("Novo Corredor", corredor.Nome);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            corredorService.Delete(1);

            Assert.AreEqual(2, corredorService.GetAll().Count());
            Assert.IsNull(corredorService.Get(1));
        }

        [TestMethod()]
        public void EditTest()
        {
            var corredor = corredorService.Get(3);
            Assert.IsNotNull(corredor);

            corredor.Nome = "Maria Editada";
            corredorService.Edit(corredor);

            var corredorEditado = corredorService.Get(3);
            Assert.IsNotNull(corredorEditado);
            Assert.AreEqual("Maria Editada", corredorEditado.Nome);
        }

        [TestMethod()]
        public void GetTest()
        {
            var corredor = corredorService.Get(1);

            Assert.IsNotNull(corredor);
            Assert.AreEqual("Felipe", corredor.Nome);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var lista = corredorService.GetAll();

            Assert.IsInstanceOfType(lista, typeof(IEnumerable<Corredor>));
            Assert.IsNotNull(lista);
            Assert.AreEqual(3, lista.Count());
        }

        [TestMethod()]
        public void GetByNameTest()
        {
            var resultado = corredorService.GetByName("João");

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any(c => c.Nome.Contains("João")));
            Assert.AreEqual("João Lucas", resultado.First().Nome);
        }
    }
}