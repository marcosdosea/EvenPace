using AutoMapper;
using Core;
using Core.Service;
using EvenPace.Controllers;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Models;

namespace EvenPaceWebTests
{
    [TestClass]
    public class InscricaoControllerTests
    {
        private InscricaoController controller = null!;
        private Mock<IInscricaoService> mockInscricaoService = null!;
        private Mock<ICorredorService> mockCorredorService = null!;
        private Mock<UserManager<UsuarioIdentity>> mockUserManager = null!;
        private IMapper mapper = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockInscricaoService = new Mock<IInscricaoService>();
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
                mockCorredorService.Object,
                mockUserManager.Object,
                mapper
            );
        }

        [TestMethod]
        public async Task Delete_Get_InscricaoInexistente_RetornaNotFound()
        {
            mockInscricaoService
                .Setup(s => s.GetAsync(1))
                .ReturnsAsync((Inscricao?)null);

            var result = await controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetAllByEvento_IdValido_RetornaViewComLista()
        {
            mockInscricaoService
                .Setup(s => s.GetAllByEventoAsync(1))
                .ReturnsAsync(GetInscricoes());

            var result = await controller.GetAllByEvento(1);

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
