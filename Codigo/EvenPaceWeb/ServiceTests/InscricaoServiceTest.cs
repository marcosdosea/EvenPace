using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Moq;
using Service;

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

            var mockEventoService = new Mock<IEventosService>();
            var mockKitService = new Mock<IKitService>();
            service = new InscricaoService(context, mockEventoService.Object, mockKitService.Object);
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
        [TestMethod]
        public void Cancelar_InscricaoJaCancelada_EP90()
        {
            int idInscricaoRepetida = 10;
            int idCorredorFake = 42;

            var evento = new Evento
            {
                Id = 5,
                Nome = "Corrida de Teste EP90",
                Descricao = "Descricao do Evento",
                Cidade = "Itabaiana",
                Estado = "SE",
                Bairro = "Centro",
                Rua = "Rua Teste",
                InfoRetiradaKit = "Informacoes de retirada",
                Data = DateTime.Now.AddDays(10)
            };

            var inscricaoJaCancelada = new Inscricao
            {
                Id = idInscricaoRepetida,
                IdCorredor = idCorredorFake,
                IdEvento = 5,
                Status = "Cancelada", 
                Distancia = "5km",
                TamanhoCamisa = "G",
                IdEventoNavigation = evento
            };

            context.Eventos.Add(evento);
            context.Inscricao.Add(inscricaoJaCancelada);
            context.SaveChanges();
            
            service.Cancelar(idInscricaoRepetida, idCorredorFake);

            var resultadoInscricao = context.Inscricao.Find(idInscricaoRepetida);
    
            Assert.IsNotNull(resultadoInscricao);
            Assert.AreEqual("Cancelada", resultadoInscricao.Status, "A inscrição deveria continuar com o status Cancelada.");
        }
    }
}
