using AutoMapper;
using Core;
using Core.Service;
using EvenPace.Controllers;
using EvenPaceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Models;

namespace EvenPaceWebTests
{
    [TestClass]
    public class InscricaoControllerTests
    {
        private InscricaoController controller;

        private Mock<IInscricaoService> mockInscricaoService;
        private Mock<IEventosService> mockEventoService;
        private Mock<ICorredorService>  mockCorredorService;
        private Mock<IKitService> mockKitService;

        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            mockInscricaoService = new Mock<IInscricaoService>();
            mockEventoService = new Mock<IEventosService>();
            mockCorredorService = new Mock<ICorredorService>();
            mockKitService = new Mock<IKitService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Kit, KitViewModel>();
                cfg.CreateMap<Inscricao, InscricaoViewModel>();
            });

            mapper = config.CreateMapper();

            mockEventoService.Setup(s => s.Get(1))
                .Returns(GetEvento());

            mockKitService.Setup(s => s.GetKitsPorEvento(1))
                .Returns(GetKits());
            
            mockInscricaoService
                .Setup(s => s.GetAllByEvento(1))
                .Returns(GetInscricoes());
            
            controller = new InscricaoController(
                mockInscricaoService.Object,
                mockEventoService.Object,
                mockKitService.Object,
                mockCorredorService.Object,
                mapper
            );
        }
        
        [TestMethod]
        public void TelaInscricao_Get_Valido()
        {
            var result = controller.TelaInscricao(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var view = (ViewResult)result;
            Assert.IsInstanceOfType(view.Model, typeof(TelaInscricaoViewModel));

            var vm = (TelaInscricaoViewModel)view.Model;
            Assert.AreEqual(1, vm.IdEvento);
            Assert.AreEqual("Corrida Teste", vm.NomeEvento);
        }

        [TestMethod]
        public void TelaInscricao_Get_IdZero_RetornaBadRequest()
        {
            var result = controller.TelaInscricao(0);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        
        [TestMethod]
        public void Tela1_Get_Valido()
        {
            var result = controller.Tela1(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var view = (ViewResult)result;
            Assert.AreEqual("Tela1", view.ViewName);
        }

        [TestMethod]
        public void Tela1_Get_IdZero_RetornaContent()
        {
            var result = controller.Tela1(0);

            Assert.IsInstanceOfType(result, typeof(ContentResult));
        }

        private Evento GetEvento()
        {
            return new Evento
            {
                Id = 1,
                Nome = "Corrida Teste",
                Cidade = "São Paulo",
                Data = DateTime.Today,
                Discricao = "Evento de teste"
            };
        }

        private IEnumerable<Kit> GetKits()
        {
            return new List<Kit>
            {
                new Kit
                {
                    Id = 1,
                    Nome = "Kit Básico",
                    Descricao = "Camiseta",
                    Valor = 99
                }
            };
        }

        private TelaInscricaoViewModel GetTelaInscricaoVM()
        {
            return new TelaInscricaoViewModel
            {
                IdEvento = 1,
                Inscricao = new InscricaoViewModel
                {
                    DistanciaPercorida = 5,
                    Tempo = new TimeSpan(0, 0, 30, 0),
                    Posicao = 1,
                    IdKit = 1,
                    IdEvento = 1,
                    IdCorredor = 1,
                    IdAvaliacaoEvento = 1,
                    DataInscricao = DateTime.Today,
                    Status = "Pendente"
                }
            };
        }
        
        private IEnumerable<Inscricao> GetInscricoes()
        {
            return new List<Inscricao>
            {
                new Inscricao { Id = 1, Status = "Confirmada", IdEvento = 1 },
                new Inscricao { Id = 2, Status = "Pendente",   IdEvento = 1 }
            };
        }
        
        [TestMethod]
        public void GetAllByEvento_IdValido_RetornaViewComLista()
        {
            // Act
            var result = controller.GetAllByEvento(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var view = (ViewResult)result;
            Assert.IsNotNull(view.Model);

            var model = view.Model as List<InscricaoViewModel>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
            Assert.IsTrue(model.All(i => i.IdEvento == 1));
        }
    }
}
