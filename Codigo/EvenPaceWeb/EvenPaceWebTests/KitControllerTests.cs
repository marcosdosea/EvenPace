using AutoMapper;
using Core;
using Core.Service;
using EvenPace.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Models;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace EvenPaceWebTests
{
    [TestClass()]
    public class KitControllerTests
    {
        private static KitController controller;
        private static Mock<IKitService> mockKitService;
        private static Mock<IEventosService> mockEventosService;
        private static Mock<IMapper> mockMapper; // USANDO MOCK AO INVÉS DE CONFIG REAL

        [TestInitialize]
        public void Initialize()
        {
            // 1. Inicializa os Mocks
            mockKitService = new Mock<IKitService>();
            mockEventosService = new Mock<IEventosService>();
            mockMapper = new Mock<IMapper>();

            // 2. Configura o Mock do Mapper (Simula o funcionamento do AutoMapper)

            // Quando pedir para transformar Kit -> KitViewModel
            mockMapper.Setup(m => m.Map<KitViewModel>(It.IsAny<Kit>()))
                .Returns((Kit source) => new KitViewModel
                {
                    Id = (uint)source.Id,
                    Nome = source.Nome,
                    Valor = source.Valor,
                    IdEvento = source.IdEvento
                });

            // Quando pedir para transformar KitViewModel -> Kit
            mockMapper.Setup(m => m.Map<Kit>(It.IsAny<KitViewModel>()))
                .Returns((KitViewModel source) => new Kit
                {
                    Id = (int)source.Id,
                    Nome = source.Nome,
                    Valor = source.Valor,
                    IdEvento = source.IdEvento
                });

            // Quando pedir uma LISTA (List<Kit> -> List<KitViewModel>)
            mockMapper.Setup(m => m.Map<List<KitViewModel>>(It.IsAny<List<Kit>>()))
                .Returns((List<Kit> source) => source.Select(k => new KitViewModel
                {
                    Id =(uint)k.Id,
                    Nome = k.Nome,
                    Valor = k.Valor,
                    IdEvento = k.IdEvento
                }).ToList());

            // 3. Setup do KitService
            mockKitService.Setup(service => service.GetAll())
                .Returns(GetTestKits());

            mockKitService.Setup(service => service.Get(1))
                .Returns(GetTargetKit());

            // 4. Setup do EventosService
            mockEventosService.Setup(s => s.Get(It.IsAny<int>()))
                .Returns(new Evento { Id = 1, Nome = "Corrida de Teste", IdOrganizacao = 1 });

            mockEventosService.Setup(s => s.GetAll())
                .Returns(GetTestEventos());

            // 5. Instancia o Controller
            controller = new KitController(mockKitService.Object, mockMapper.Object, mockEventosService.Object);

            controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new DefaultHttpContext(), Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());
        }

        [TestMethod()]
        public void IndexKit_ComIdEvento_RetornaViewComLista()
        {
            var result = controller.IndexKit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<KitViewModel>));

            List<KitViewModel>? lista = (List<KitViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod()]
        public void Create_Post_Valido_RedirecionaParaIndexKit()
        {
            var novoKit = GetNewKitModel();
            novoKit.ImagemUpload = null;

            var result = controller.Create(novoKit);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;

            Assert.AreEqual("IndexKit", redirect.ActionName);
            Assert.IsTrue(redirect.RouteValues.ContainsKey("idEvento"));
        }

        [TestMethod()]
        public void Excluir_IdExistente_RedirecionaParaIndexKit()
        {
            var result = controller.Excluir(1);

            mockKitService.Verify(s => s.Delete(1), Times.Once);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;
            Assert.AreEqual("IndexKit", redirect.ActionName);
        }

        // --- MÉTODOS AUXILIARES ---
        private KitViewModel GetNewKitModel()
        {
            return new KitViewModel { Id = 0, Nome = "Kit Teste", Valor = 200, IdEvento = 1, Descricao = "Desc" };
        }

        private static Kit GetTargetKit()
        {
            // REMOVIDO O IdOrganizacao QUE CAUSAVA ERRO
            return new Kit { Id = 1, Nome = "Kit Gold", Valor = 150, IdEvento = 1, Descricao = "Existente" };
        }

        private IEnumerable<Kit> GetTestKits()
        {
            return new List<Kit>
            {
                new Kit { Id = 1, Nome = "Kit Gold", Valor = 150, IdEvento = 1 },
                new Kit { Id = 2, Nome = "Kit Prata", Valor = 100, IdEvento = 1 },
                new Kit { Id = 3, Nome = "Kit Bronze", Valor = 80, IdEvento = 1 }
            };
        }

        private IEnumerable<Evento> GetTestEventos()
        {
            return new List<Evento>
            {
                new Evento { Id = 1, Nome = "Corrida de Teste", IdOrganizacao = 1 },
                new Evento { Id = 2, Nome = "Outra Corrida", IdOrganizacao = 1 }
            };
        }
    }
}