using AutoMapper;
using Core;
using Core.Service;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using EvenPaceWeb.Controllers;
using EvenPaceWeb.Mappers;
using Mappers;
using Microsoft.AspNetCore.Identity;

namespace EvenPaceWebTests
{
    [TestClass()]
    public class OrganizacaoControllerTests
    {
        private static OrganizacaoController controller = null!;
        private static Mock<IOrganizacaoService> mockService = null!;
        private static Mock<UserManager<UsuarioIdentity>> mockUserManager = null!;
        private static Mock<SignInManager<UsuarioIdentity>> mockSignInManager = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IOrganizacaoService>();

            var userStore = new Mock<IUserStore<UsuarioIdentity>>();
            mockUserManager = new Mock<UserManager<UsuarioIdentity>>(
                userStore.Object, null, null, null, null, null, null, null, null
            );

            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<UsuarioIdentity>>();

            mockSignInManager = new Mock<SignInManager<UsuarioIdentity>>(
                mockUserManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null, null, null, null
            );

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
                mockUserManager.Object,
                mockSignInManager.Object
            );
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
        public async Task CreateTest_Post_Valid()
        {
            var result = await controller.Create(GetNewOrganizacaoModel(), "SenhaForte2026@");

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod()]
        public async Task CreateTest_Post_Invalid()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = await controller.Create(GetNewOrganizacaoModel(), "SenhaForte2026@");
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

        [TestMethod]
        public async Task Inserir_OrganizacaoComDadosJaExistente_EP91()
        {
            var modelExistente = new OrganizacaoViewModel
            {
                Nome = "José Almeida Segundo",
                Cnpj = "51658141000137",
                Cpf = " ",
                Telefone = "79999995555",
                Cep = "49500362",
                Rua = "Rua Francisco Santos",
                Numero = 1245,
                Estado = "Sergipe"
            };
            string senhaTeste = "admin";

            var listaErros = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "DuplicateUserName",
                    Description = "Erro: Já existe uma organização cadastrada com estes dados."
                }
            };
            var resultadoFalhaIdentity = IdentityResult.Failed(listaErros.ToArray());

            mockUserManager
                .Setup(um => um.CreateAsync(It.Is<UsuarioIdentity>(u => u.UserName == "51658141000137"), senhaTeste))
                .ReturnsAsync(resultadoFalhaIdentity);


            var result = await controller.Create(modelExistente, senhaTeste);

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            Assert.IsFalse(controller.ModelState.IsValid);

            var possuiMensagemDuplicado = controller.ModelState.Values
                .Any(v => v.Errors.Any(e =>
                    e.ErrorMessage.Contains("Erro: Já existe uma organização cadastrada com estes dados.")));

            Assert.IsTrue(possuiMensagemDuplicado,
                "A mensagem de erro exata exigida pelo caso de teste EP-91 não foi encontrada.");
        }

        [TestMethod]
        public void Edit_OrganizaoComNomeVazio_EP99()
        {
            var modelNomeEmBranco = new OrganizacaoViewModel
            {
                Id = 3,
                Nome = " ",
                Cnpj = "51658141000137",
                Cpf = " ",
                Telefone = "79998024485",
                Cep = " ",
                Rua = "Rua Francisco Santos",
                Numero = 1245,
                Estado = "Sergipe"
            };


            controller.ModelState.AddModelError("Nome", "O Nome é Obrigatório");

            var result = controller.Edit(modelNomeEmBranco);

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            Assert.IsFalse(controller.ModelState.IsValid);

            var possuiAvisoNomeObrigatorio = controller.ModelState["Nome"]?.Errors
                .Any(e => e.ErrorMessage.Contains("O Nome é Obrigatório"));

            Assert.IsTrue(possuiAvisoNomeObrigatorio == true,
                "O aviso de que 'O Nome é Obrigatório' não foi incluído no ModelState.");
        }
    }
}