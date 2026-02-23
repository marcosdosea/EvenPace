using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using EvenPaceWeb.Controllers;
using EvenPaceWeb.Mappers;
using Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvenPaceWebTests
{
    [TestClass()]
    public class OrganizacaoControllerTests
    {
        private static OrganizacaoController controller = null!;
        private static Mock<IOrganizacaoService> mockService = null!;

        private static Mock<IAuthService> mockAuthService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IOrganizacaoService>();
            mockAuthService = new Mock<IAuthService>(); 

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new OrganizacaoProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestOrganizacoes());

            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetOrganizacao());

            mockService.Setup(service => service.Edit(It.IsAny<Organizacao>()))
                .Verifiable();

            mockService.Setup(service => service.Create(It.IsAny<Organizacao>()))
                .Verifiable();

            mockService.Setup(service => service.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new OrganizacaoController(
                mockService.Object,
                mapper,
                mockAuthService.Object
            );
        }

        [TestMethod()]
        public void LoginTest_Get_Valido()
        {
            var result = controller.Login();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public async Task Login_ComCredenciaisValidas_RedirecionaParaEventos()
        {
            mockAuthService.Setup(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(true);

            var result = await controller.Login("org@email.com", "SenhaValida123");

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Evento", redirectResult.ControllerName);
        }

        [TestMethod()]
        public async Task Login_ComCredenciaisInvalidas_RetornaViewComErro()
        {
            mockAuthService.Setup(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(false);

            var result = await controller.Login("invalido@email.com", "SenhaErrada");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsTrue(controller.ModelState.ContainsKey("")); 
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            var lista = viewResult.ViewData.Model as List<OrganizacaoViewModel>;
            Assert.IsNotNull(lista);
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            var model = viewResult.ViewData.Model as OrganizacaoViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual("Organizacao A", model.Nome);
            Assert.AreEqual("11111111000111", model.Cnpj);
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
            var result = controller.Create(GetNewOrganizacaoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalid()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Create(GetNewOrganizacaoModel());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
        }

        [TestMethod()]
        public void EditTest_Get_Valid()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            var model = viewResult.ViewData.Model as OrganizacaoViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual("Organizacao A", model.Nome);
        }

        [TestMethod()]
        public void EditTest_Post_Valid()
        {
            var result = controller.Edit(GetTargetOrganizacaoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod()]
        public void DeleteTest_Get_Valid()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            var model = viewResult.ViewData.Model as OrganizacaoViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual("Organizacao A", model.Nome);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valid()
        {
            var result = controller.Delete(1, GetTargetOrganizacaoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        private OrganizacaoViewModel GetNewOrganizacaoModel()
        {
            return new OrganizacaoViewModel { Id = 4, Nome = "Nova Org 2026", Cnpj = "44444444000144" };
        }

        private static Organizacao GetTargetOrganizacao()
        {
            return new Organizacao { Id = 1, Nome = "Organizacao A", Cnpj = "11111111000111" };
        }

        private OrganizacaoViewModel GetTargetOrganizacaoModel()
        {
            return new OrganizacaoViewModel { Id = 1, Nome = "Organizacao A", Cnpj = "11111111000111" };
        }

        private IEnumerable<Organizacao> GetTestOrganizacoes()
        {
            return new List<Organizacao>
            {
                new Organizacao { Id = 1, Nome = "Organizacao A", Cnpj = "11111111000111" },
                new Organizacao { Id = 2, Nome = "Organizacao B", Cnpj = "22222222000122" },
                new Organizacao { Id = 3, Nome = "Organizacao C", Cnpj = "33333333000133" }
            };
        }
    }
}