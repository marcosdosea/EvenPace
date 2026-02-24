using AutoMapper;
using Core;
using Core.Service;
using Core.Service.Dtos;
using EvenPace.Controllers;
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
        private Mock<IKitService> mockKitService;
        private Mock<ICorredorService> mockCorredorService;
        private IMapper mapper;
        
        [TestInitialize]
        public void Initialize()
        {
            mockInscricaoService = new Mock<IInscricaoService>();
            mockEventoService = new Mock<IEventosService>();
            mockKitService = new Mock<IKitService>();
            mockCorredorService = new Mock<ICorredorService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Kit, KitViewModel>();
                cfg.CreateMap<Inscricao, InscricaoViewModel>();
            });

            mapper = config.CreateMapper();

            controller = new InscricaoController(
                mockInscricaoService.Object,
                mockEventoService.Object,
                mockKitService.Object,
                mockCorredorService.Object,
                mapper
            );

            mockInscricaoService
                .Setup(s => s.GetDadosTelaInscricao(1))
                .Returns(new DadosTelaInscricaoDto
                {
                    IdEvento = 1,
                    NomeEvento = "Corrida Teste",
                    Local = "São Paulo",
                    DataEvento = DateTime.Today,
                    Descricao = "Evento de teste",
                    Kits = GetKits()
                });

            mockInscricaoService
                .Setup(s => s.GetAllByEvento(1))
                .Returns(GetInscricoes());
        }

        [TestMethod]
        public void Cancelar_Get_InscricaoInexistente_RetornaNotFound()
        {
            mockInscricaoService
                .Setup(s => s.GetDadosTelaDelete(1))
                .Returns(new GetDadosTelaDeleteResult { Success = false, ErrorType = "NotFound" });

            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void GetAllByEvento_IdValido_RetornaViewComLista()
        {
            var result = controller.GetAllByEvento(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var view = (ViewResult)result;
            Assert.IsNotNull(view.Model);

            var model = view.Model as List<InscricaoViewModel>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
            Assert.IsTrue(model.All(i => i.IdEvento == 1));
        }

        private static IEnumerable<Kit> GetKits()
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

        private static IEnumerable<Inscricao> GetInscricoes()
        {
            return new List<Inscricao>
            {
                new Inscricao
                {
                    Id = 1,
                    IdEvento = 1,
                    Distancia = "5",
                    TamanhoCamisa = "M",
                    IdKit = 1
                },
                new Inscricao
                {
                    Id = 2,
                    IdEvento = 1,
                    Distancia = "10",
                    TamanhoCamisa = "G",
                    IdKit = 1
                }
            };
        }
    }
}
