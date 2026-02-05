using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class OrganizacaoServiceTests
    {
        private EvenPaceContext _context = null!;
        private IOrganizacaoService _organizacaoService = null!;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<EvenPaceContext>();
            builder.UseInMemoryDatabase("EvenPaceOrganizacao");
            var options = builder.Options;

            _context = new EvenPaceContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var organizacoes = new List<Organizacao>
            {
                new Organizacao {
                    Id = 1, Nome = "Organizacao Alfa", Cnpj = "11111111000111",
                    Bairro = "Centro", Cep = "49500000", Cidade = "Itabaiana",
                    Email = "alfa@email.com", Estado = "SE", Rua = "Rua A",
                    Senha = "123", Telefone = "79999999991"
                },
                new Organizacao {
                    Id = 2, Nome = "Organizacao Beta", Cnpj = "22222222000122",
                    Bairro = "Centro", Cep = "49500000", Cidade = "Itabaiana",
                    Email = "beta@email.com", Estado = "SE", Rua = "Rua B",
                    Senha = "123", Telefone = "79999999992"
                },
                new Organizacao {
                    Id = 3, Nome = "Organizacao Gama", Cnpj = "33333333000133",
                    Bairro = "Centro", Cep = "49500000", Cidade = "Itabaiana",
                    Email = "gama@email.com", Estado = "SE", Rua = "Rua C",
                    Senha = "123", Telefone = "79999999993"
                }
            };

            _context.AddRange(organizacoes);
            _context.SaveChanges();

            _organizacaoService = new OrganizacaoService(_context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novaOrg = new Organizacao
            {
                Nome = "Nova Organizacao",
                Cnpj = "44444444000144",
                Bairro = "Centro",
                Cep = "49500000",
                Cidade = "Itabaiana",
                Email = "nova@email.com",
                Estado = "SE",
                Rua = "Rua D",
                Senha = "123",
                Telefone = "79999999994"
            };

            _organizacaoService.Create(novaOrg);

            Assert.AreEqual(4, _organizacaoService.GetAll().Count());
            var orgDB = _organizacaoService.GetAll().Last();
            Assert.AreEqual("Nova Organizacao", orgDB.Nome);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            int idParaDeletar = 2;
            _organizacaoService.Delete(idParaDeletar);

            Assert.AreEqual(2, _organizacaoService.GetAll().Count());
            Assert.IsNull(_organizacaoService.Get(idParaDeletar));
        }

        [TestMethod()]
        public void EditTest()
        {
            var org = _organizacaoService.Get(3);
            Assert.IsNotNull(org);

            org.Nome = "Organizacao Gama Editada";
            _organizacaoService.Edit(org);

            var orgEditada = _organizacaoService.Get(3);
            Assert.AreEqual("Organizacao Gama Editada", orgEditada!.Nome);
        }

        [TestMethod()]
        public void GetTest()
        {
            var org = _organizacaoService.Get(1);
            Assert.IsNotNull(org);
            Assert.AreEqual("Organizacao Alfa", org.Nome);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var lista = _organizacaoService.GetAll();
            Assert.AreEqual(3, lista.Count());
        }
    }
}