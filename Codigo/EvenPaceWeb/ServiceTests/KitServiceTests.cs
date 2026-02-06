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
            // 1. Configura o Banco em Memória (Simulação)
            var builder = new DbContextOptionsBuilder<EvenPaceContext>();
            builder.UseInMemoryDatabase("EvenPace_Kits"); // Nome único para não misturar com Cupons
            var options = builder.Options;

            context = new EvenPaceContext(options);
            context.Database.EnsureDeleted(); // Garante que começa limpo
            context.Database.EnsureCreated();

            // 2. Prepara dados fictícios (Seed)
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

            // 3. Instancia o Service passando o contexto simulado
            kitService = new KitService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Act
            // Cria um novo Kit (ID 4)
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

            // Assert
            Assert.AreEqual(4, kitService.GetAll().Count()); // Tinha 3, agora deve ter 4
            var kitRecuperado = kitService.Get(4);
            Assert.IsNotNull(kitRecuperado);
            Assert.AreEqual("Kit Teste Criação", kitRecuperado.Nome);
            Assert.AreEqual(20.00m, kitRecuperado.Valor);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Arrange
            int idParaDeletar = 2; // "Kit Premium"

            // Act
            kitService.Delete(idParaDeletar);

            // Assert
            // 1. O número total deve cair de 3 para 2
            Assert.AreEqual(2, kitService.GetAll().Count());

            // 2. Tentar buscar o ID deletado deve retornar null
            // (Nota: No seu KitService.cs original, o Get retorna null se não achar,
            // diferente do CupomService que lançava exceção, então verificamos IsNull)
            var kitDeletado = kitService.Get(idParaDeletar);
            Assert.IsNull(kitDeletado, "O kit deveria ser nulo após ser deletado.");
        }

        [TestMethod()]
public void EditTest()
{
    // Act 
    var kit = kitService.Get(3);
    kit.Nome = "Kit VIP Editado";
    kit.Valor = 200.00m;
    kit.DisponibilidadeG = 0;

    kitService.Edit(kit);

    // --- DICA: Força o contexto a esquecer a entidade para garantir que o Get busque do banco atualizado ---
    // (Isso depende de como seu contexto é exposto, se não tiver acesso fácil, seu teste atual serve)
    context.Entry(kit).State = EntityState.Detached; 

    // Assert
    var kitEditado = kitService.Get(3);
    Assert.IsNotNull(kitEditado);
    Assert.AreEqual("Kit VIP Editado", kitEditado.Nome);
    Assert.AreEqual(200.00m, kitEditado.Valor);
    Assert.AreEqual(0, kitEditado.DisponibilidadeG);
}

        [TestMethod()]
        public void GetTest()
        {
            // Act
            var kit = kitService.Get(1); // "Kit Básico"

            // Assert
            Assert.IsNotNull(kit);
            Assert.AreEqual("Kit Básico", kit.Nome);
            Assert.AreEqual(50.00m, kit.Valor);
            Assert.AreEqual(1, (int)kit.IdEvento); // Verifica se veio o evento certo
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaKits = kitService.GetAll();

            // Assert
            Assert.IsInstanceOfType(listaKits, typeof(IEnumerable<Kit>));
            Assert.IsNotNull(listaKits);
            Assert.AreEqual(3, listaKits.Count()); // Inicializamos com 3

            // Verifica o primeiro item da lista
            var primeiroKit = listaKits.First();
            Assert.AreEqual((int)1, primeiroKit.Id);
            Assert.AreEqual("Kit Básico", primeiroKit.Nome);
        }
    }
}
