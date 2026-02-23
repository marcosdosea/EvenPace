using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using EvenPace.Controllers;
using EvenPaceWeb.Areas.Identity.Data;
using Mappers;
using Microsoft.AspNetCore.Identity;

namespace EvenPaceWebTests
{
    [TestClass()]
    public class CorredorControllerTest
    {
        private static CorredorController controller;
        private static Mock<ICorredorService> mockService;
        private Mock<UserManager<UsuarioIdentity>> mockUserManager;
        private Mock<SignInManager<UsuarioIdentity>> mockSignInManager;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<ICorredorService>();
            
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CorredorProfile())).CreateMapper();

            var userStoreMock = new Mock<IUserStore<UsuarioIdentity>>();
            mockUserManager = new Mock<UserManager<UsuarioIdentity>>(
                userStoreMock.Object,
                null, null, null, null, null, null, null, null
            );
            
            mockSignInManager = new Mock<SignInManager<UsuarioIdentity>>(
                mockUserManager.Object,
                new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<UsuarioIdentity>>().Object,
                null, null, null, null
            );
            
            mockSignInManager
                .Setup(s => s.PasswordSignInAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    false,
                    false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetCorredor());

            mockService.Setup(service => service.Get(999))
                .Returns((Corredor)null!);

            mockService.Setup(service => service.Edit(It.IsAny<Corredor>()))
                .Verifiable();

            mockService.Setup(service => service.Create(It.IsAny<Corredor>()))
                .Returns(4)
                .Verifiable();

            mockService.Setup(service => service.Delete(It.IsAny<int>()))
                .Verifiable();
            
            controller = new CorredorController(
                mockService.Object,
                mapper,
                mockUserManager.Object,
                mockSignInManager.Object
            );
        }

        [TestMethod()]
        public void GetTest_Valido()
        {
            var result = controller.Get(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CorredorViewModel));

            CorredorViewModel model = (CorredorViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Felipe", model.Nome);
        }

        [TestMethod()]
        public void EditTest_Get_Valid()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CorredorViewModel));

            CorredorViewModel model = (CorredorViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Felipe", model.Nome);
        }

        [TestMethod()]
        public void EditTest_Get_IdInexistente_RetornaViewComModelVazio()
        {
            var result = controller.Edit(999);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CorredorViewModel));

            CorredorViewModel model = (CorredorViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(0, model.Id);
        }

        [TestMethod()]
        public void CreateTest_Get_Valido()
        {
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void CreateTest_Post_Valid()
        {
            var result = controller.Create(GetNewCorredorModel());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            mockService.Verify(s => s.Create(It.IsAny<Corredor>()), Times.Once);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalid()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Create(GetNewCorredorModel());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            mockService.Verify(s => s.Create(It.IsAny<Corredor>()), Times.Never);
        }

        [TestMethod()]
        public void EditTest_Post_Valid()
        {
            var result = controller.Edit(GetTargetCorredorModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Get", redirect.ActionName);
            Assert.AreEqual(1, redirect.RouteValues!["id"]);

            mockService.Verify(s => s.Edit(It.IsAny<Corredor>()), Times.Once);
        }

        [TestMethod()]
        public void EditTest_Post_Invalid()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Edit(GetTargetCorredorModel());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            mockService.Verify(s => s.Edit(It.IsAny<Corredor>()), Times.Never);
        }

        [TestMethod()]
        public void DeleteTest_Get_Valid()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CorredorViewModel));

            CorredorViewModel model = (CorredorViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Felipe", model.Nome);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valid()
        {
            var result = controller.Delete(1, GetTargetCorredorModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);

            mockService.Verify(s => s.Delete(1), Times.Once);
        }

        private CorredorViewModel GetNewCorredorModel()
        {
            return new CorredorViewModel
            {
                Id = 4,
                CPF = "55544433322",
                Nome = "Novo",
                DataNascimento = new DateTime(2005, 05, 05)
            };
        }

        private static Corredor GetTargetCorredor()
        {
            return new Corredor
            {
                Id = 1,
                Cpf = "10101010101",
                Nome = "Felipe",
                DataNascimento = new DateTime(2010, 10, 10)
            };
        }

        private CorredorViewModel GetTargetCorredorModel()
        {
            return new CorredorViewModel
            {
                Id = 1,
                CPF = "10101010101",
                Nome = "Felipe",
                DataNascimento = new DateTime(2010, 10, 10)
            };
        }
    }
}