using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using EvenPaceWeb.Controllers;
using EvenPaceWeb.Mappers;

namespace EvenPaceWebTests
{
    [TestClass()]
    public class CupomControllerTests
    {
       
        private static CupomController controller = null!;
        private static Mock<ICupomService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<ICupomService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CupomProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestCupons());

            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetCupom());

            mockService.Setup(service => service.Edit(It.IsAny<Cupom>()))
                .Verifiable();

            mockService.Setup(service => service.Create(It.IsAny<Cupom>()))
                .Verifiable();

            controller = new CupomController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            // Verificação segura de tipo
            var lista = viewResult.ViewData.Model as List<CupomViewModel>;
            Assert.IsNotNull(lista, "A Model não deveria ser nula");
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            // Conversão segura para evitar CS8600/CS8602
            var model = viewResult.ViewData.Model as CupomViewModel;
            Assert.IsNotNull(model, "A Model retornada não pode ser nula");

            Assert.AreEqual("PROMO10", model.Nome);
            Assert.AreEqual(10, model.Desconto);
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
            var result = controller.Create(GetNewCupomModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalid()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Create(GetNewCupomModel());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
        }

        [TestMethod()]
        public void EditTest_Get_Valid()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            var model = viewResult.ViewData.Model as CupomViewModel;
            Assert.IsNotNull(model);

            Assert.AreEqual("PROMO10", model.Nome);
        }

        [TestMethod()]
        public void EditTest_Post_Valid()
        {
            var result = controller.Edit(GetTargetCupomModel());

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

            var model = viewResult.ViewData.Model as CupomViewModel;
            Assert.IsNotNull(model);

            Assert.AreEqual("PROMO10", model.Nome);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valid()
        {
            var result = controller.Delete(1, GetTargetCupomModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        private CupomViewModel GetNewCupomModel()
        {
            return new CupomViewModel { Id = 4, Nome = "NOVO2026", Desconto = 5 };
        }

        private static Cupom GetTargetCupom()
        {
            return new Cupom { Id = 1, Nome = "PROMO10", Desconto = 10 };
        }

        private CupomViewModel GetTargetCupomModel()
        {
            return new CupomViewModel { Id = 1, Nome = "PROMO10", Desconto = 10 };
        }

        private IEnumerable<Cupom> GetTestCupons()
        {
            return new List<Cupom>
            {
                new Cupom { Id = 1, Nome = "PROMO10", Desconto = 10 },
                new Cupom { Id = 2, Nome = "VERAO20", Desconto = 20 },
                new Cupom { Id = 3, Nome = "BLACK50", Desconto = 50 }
            };
        }
    }
}