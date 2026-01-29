using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using EvenPace.Controllers;
using Mappers;

namespace EvenPaceWebTests
{
    [TestClass()]
    public class CorredorControllerTest
    {
        private static CorredorController controller;
        private static Mock<ICorredorService> mockService;
        private static Mock<IAvaliacaoEventoService> mockAvaliacaoEventoService;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<ICorredorService>();
            mockAvaliacaoEventoService = new Mock<IAvaliacaoEventoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CorredorProfile())).CreateMapper();

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

            mockService.Setup(service => service.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GetTargetCorredor());

            controller = new CorredorController(mockService.Object, mockAvaliacaoEventoService.Object, mapper);
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

        [TestMethod()]
        public void LoginTest_Valido()
        {
            var result = controller.Login("feaaa@gmail.com", "123456");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CorredorViewModel));

            CorredorViewModel model = (CorredorViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Felipe", model.Nome);
        }

        private CorredorViewModel GetNewCorredorModel()
        {
            return new CorredorViewModel
            {
                Id = 4,
                CPF = "55544433322",
                Nome = "Novo",
                Email = "novo@gmail.com",
                DataNascimento = new DateTime(2005, 05, 05),
                Senha = "123456"
            };
        }

        private static Corredor GetTargetCorredor()
        {
            return new Corredor
            {
                Id = 1,
                Cpf = "10101010101",
                Nome = "Felipe",
                Email = "feaaa@gmail.com",
                DataNascimento = new DateTime(2010, 10, 10),
                Senha = "123456"
            };
        }

        private CorredorViewModel GetTargetCorredorModel()
        {
            return new CorredorViewModel
            {
                Id = 1,
                CPF = "10101010101",
                Nome = "Felipe",
                Email = "feaaa@gmail.com",
                DataNascimento = new DateTime(2010, 10, 10),
                Senha = "123456"
            };
        }
    }
}