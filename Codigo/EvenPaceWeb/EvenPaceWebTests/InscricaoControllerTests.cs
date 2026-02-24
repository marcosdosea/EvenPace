using AutoMapper;
using Core.Service;
using EvenPace.Controllers;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Models;
using Core;

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
        private Mock<UserManager<UsuarioIdentity>> mockUserManager;
       
        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            mockInscricaoService = new Mock<IInscricaoService>();
            mockEventoService = new Mock<IEventosService>();
            mockKitService = new Mock<IKitService>();
            mockCorredorService = new Mock<ICorredorService>();

            var userStore = new Mock<IUserStore<UsuarioIdentity>>();

            mockUserManager = new Mock<UserManager<UsuarioIdentity>>(
                userStore.Object,
                null, null, null, null, null, null, null, null
            );

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
                mockUserManager.Object,
                mapper
            );

            
            mockEventoService
                .Setup(s => s.Get(It.IsAny<int>()))
                .Returns(new Evento
                {
                    Id = 1,
                    Nome = "Corrida Teste",
                    Cidade = "São Paulo",
                    Data = DateTime.Now.AddDays(10),
                    Descricao = "Evento teste"
                });

            mockKitService
                .Setup(s => s.Get(It.IsAny<int>()))
                .Returns(new Kit
                {
                    Id = 1,
                    Nome = "Kit Básico"
                });

            mockInscricaoService
                .Setup(s => s.GetAllByEvento(1))
                .Returns(GetInscricoes());
        }

       [TestMethod]
        public void Delete_Get_InscricaoInexistente_RetornaNotFound()
        {
            mockInscricaoService
                .Setup(s => s.Get(1))
                .Returns((Inscricao)null);

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