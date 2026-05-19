using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;

namespace EvenPaceWebTests.Service
{
    [TestClass()]
    public class KitServiceTests
    {
        private EvenPaceContext context = null!;
        private IKitService kitService = null!;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<EvenPaceContext>();
            builder.UseInMemoryDatabase("EvenPace_Kits");
            var options = builder.Options;

            context = new EvenPaceContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var kits = new List<Kit>
            {
                new Kit {
                    Id = 1,
                    Nome = "Kit Básico",
                    Valor = 50.00m,
                    Descricao = "Camisa e Medalha",
                    IdEvento = 1,
                    Imagem = "img1.jpg",
                    DisponibilidadeP = 10, DisponibilidadeM = 10, DisponibilidadeG = 10
                },
                new Kit {
                    Id = 2,
                    Nome = "Kit Premium",
                    Valor = 80.00m,
                    Descricao = "Camisa, Medalha e Boné",
                    IdEvento = 1,
                    Imagem = "img2.jpg",
                    DisponibilidadeP = 5, DisponibilidadeM = 5, DisponibilidadeG = 5
                },
                new Kit {
                    Id = 3,
                    Nome = "Kit VIP",
                    Valor = 150.00m,
                    Descricao = "Completo + Acesso VIP",
                    IdEvento = 2,
                    Imagem = "img3.jpg",
                    DisponibilidadeP = 2, DisponibilidadeM = 2, DisponibilidadeG = 2
                }
            };

            context.AddRange(kits);
            context.SaveChanges();

            kitService = new KitService(context);
        }

        [TestMethod]
        public void ExcluirKitValido_EP41()
        {
            var kitValido = new Kit
            {
                Id = 5,
                Nome = "Kit simples",
                Valor = 50.00m,
                Descricao = "Kit vem com camiseta, número do peito e bolsa",
                IdEvento = 1,
                Imagem = "teste.jpg",
                DisponibilidadeP = 50,
                DisponibilidadeM = 100,
                DisponibilidadeG = 150
            };

            kitService.Create(kitValido);

            Assert.IsNotNull(kitService.Get(5));

            var idDeletar = 5;
            kitService.Delete(idDeletar);

            var kitRemovido = kitService.Get(idDeletar);
            Assert.IsNull(kitRemovido, "O kit deveria ser nulo após a exclusão.");

            Assert.AreEqual(3, kitService.GetAll().Count(), "A contagem total de kits está incorreta após a exclusão.");
        }

        [TestMethod()]
        public void EditarKitValido_EP43()
        {
            var kitOriginal = kitService.Get(1);
            Assert.IsNotNull(kitOriginal, "O kit deveria existir para ser editado.");

            string novoNome = "Kit Básico Atualizado";
            decimal novoValor = 60.00m;

            kitOriginal.Nome = novoNome;
            kitOriginal.Valor = novoValor;

            kitService.Edit(kitOriginal);

            context.Entry(kitOriginal).State = EntityState.Detached;

            var kitAtualizado = kitService.Get(1);

            Assert.IsNotNull(kitAtualizado);
            Assert.AreEqual(novoNome, kitAtualizado.Nome, "O nome do kit não foi atualizado corretamente.");
            Assert.AreEqual(novoValor, kitAtualizado.Valor, "O valor do kit não foi atualizado corretamente.");

            Assert.AreEqual(1, kitAtualizado.Id, "O ID do kit não deveria ter sido alterado após a edição.");
        }

        [TestMethod()]
        public void EditarKitInvalido_EP44()
        {

            var kit = kitService.Get(1);

            kit.DisponibilidadeM = 50;

            kitService.Edit(kit); 

            kit.DisponibilidadeM = 30;

            Assert.ThrowsException<Exception>(() =>
            {
                kitService.Edit(kit);
            }, "O sistema deveria impedir a redução da disponibilidade abaixo do número de inscritos.");
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novoKit = new Kit
            {
                Id = 4,
                Nome = "Kit Teste Criação",
                Valor = 20.00m,
                Descricao = "Teste",
                IdEvento = 1,
                Imagem = "teste.jpg"
            };

            kitService.Create(novoKit);

            Assert.AreEqual(4, kitService.GetAll().Count());
            var kitRecuperado = kitService.Get(4);
            Assert.IsNotNull(kitRecuperado);
            Assert.AreEqual("Kit Teste Criação", kitRecuperado.Nome);
            Assert.AreEqual(20.00m, kitRecuperado.Valor);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            int idParaDeletar = 2;

            kitService.Delete(idParaDeletar);

            Assert.AreEqual(2, kitService.GetAll().Count());

            var kitDeletado = kitService.Get(idParaDeletar);
            Assert.IsNull(kitDeletado, "O kit deveria ser nulo após ser deletado.");
        }

        [TestMethod()]
        public void EditTest()
        {
            var kit = kitService.Get(3);
            kit.Nome = "Kit VIP Editado";
            kit.Valor = 200.00m;
            kit.DisponibilidadeG = 0;

            kitService.Edit(kit);

            context.Entry(kit).State = EntityState.Detached;

            var kitEditado = kitService.Get(3);
            Assert.IsNotNull(kitEditado);
            Assert.AreEqual("Kit VIP Editado", kitEditado.Nome);
            Assert.AreEqual(200.00m, kitEditado.Valor);
            Assert.AreEqual(0, kitEditado.DisponibilidadeG);
        }

        [TestMethod()]
        public void GetTest()
        {
            var kit = kitService.Get(1);

            Assert.IsNotNull(kit);
            Assert.AreEqual("Kit Básico", kit.Nome);
            Assert.AreEqual(50.00m, kit.Valor);
            Assert.AreEqual(1, (int)kit.IdEvento);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listaKits = kitService.GetAll();

            Assert.IsInstanceOfType(listaKits, typeof(IEnumerable<Kit>));
            Assert.IsNotNull(listaKits);
            Assert.AreEqual(3, listaKits.Count());

            var primeiroKit = listaKits.First();
            Assert.AreEqual((int)1, primeiroKit.Id);
            Assert.AreEqual("Kit Básico", primeiroKit.Nome);
        }
    }
}
