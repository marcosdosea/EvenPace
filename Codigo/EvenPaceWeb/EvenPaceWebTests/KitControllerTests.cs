using AutoMapper;
using Core;
using Core.Service;
using EvenPace.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures; 
using Microsoft.AspNetCore.Http;
using Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace EvenPaceWebTests
{
    [TestClass()]
    public class KitControllerTests
    {
        // Variáveis de instância (não static) para isolamento dos testes
        private KitController controller = null!;
        private Mock<IKitService> mockKitService=  null!;
        private Mock<IEventosService> mockEventosService = null!;
        private Mock<IMapper> mockMapper = null!;

        [TestInitialize]
        public void Initialize()
        {
            // 1. Inicializa os Mocks
            mockKitService = new Mock<IKitService>();
            mockEventosService = new Mock<IEventosService>();
            mockMapper = new Mock<IMapper>();

            // 2. Configura o Mock do Mapper (Simula o funcionamento do AutoMapper)

            // De Entity -> ViewModel
            mockMapper.Setup(m => m.Map<KitViewModel>(It.IsAny<Kit>()))
                .Returns((Kit source) => new KitViewModel
                {
                    Id = (uint)source.Id, 
                    Nome = source.Nome,
                    Valor = source.Valor,
                    IdEvento = source.IdEvento
                });

            // De ViewModel -> Entity
            mockMapper.Setup(m => m.Map<Kit>(It.IsAny<KitViewModel>()))
                .Returns((KitViewModel source) => new Kit
                {
                    Id = (int)source.Id,
                    Nome = source.Nome,
                    Valor = source.Valor,
                    IdEvento = source.IdEvento
                });

            // De List<Entity> -> List<ViewModel>
            mockMapper.Setup(m => m.Map<List<KitViewModel>>(It.IsAny<List<Kit>>()))
                .Returns((List<Kit> source) => source.Select(k => new KitViewModel
                {
                    Id = (uint)k.Id,
                    Nome = k.Nome,
                    Valor = k.Valor,
                    IdEvento = k.IdEvento
                }).ToList());

            // 3. Setup do KitService
            mockKitService.Setup(service => service.GetAll())
                .Returns(GetTestKits());

            mockKitService.Setup(service => service.Get(1))
                .Returns(GetTargetKit());

            // 4. Setup do EventosService (Necessário para o IndexKit funcionar)
            mockEventosService.Setup(s => s.Get(It.IsAny<int>()))
                .Returns(new Evento { Id = 1, Nome = "Corrida de Teste", IdOrganizacao = 1 });

            mockEventosService.Setup(s => s.GetAll())
                .Returns(GetTestEventos());

            // 5. Instancia o Controller
            controller = new KitController(mockKitService.Object, mockMapper.Object, mockEventosService.Object);

            // Configura o TempData (essencial para evitar NullReferenceException ao usar TempData["Mensagem"])
            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );
        }

        [TestMethod()]
        public void IndexKit_ComIdEvento_RetornaViewComLista()
        {
            // Act
            var result = controller.IndexKit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<KitViewModel>));
            List<KitViewModel>? lista = (List<KitViewModel>?)viewResult.ViewData.Model;

            Assert.IsNotNull(lista);
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod()]
        public void Create_Post_Valido_RedirecionaParaIndexKit()
        {
            // Arrange
            var novoKit = GetNewKitModel();
            novoKit.ImagemUpload = null; // Simula envio sem imagem

            // Act
            var result = controller.Create(novoKit);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;

            Assert.AreEqual("IndexKit", redirect.ActionName);
            Assert.IsTrue(redirect.RouteValues.ContainsKey("idEvento"));
        }

        [TestMethod()]
        public void Excluir_IdExistente_RedirecionaParaIndexKit()
        {
            // Act
            var result = controller.Delete(1);

            // Assert
            mockKitService.Verify(s => s.Delete(1), Times.Once); // Verifica se o serviço foi chamado

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirect = (RedirectToActionResult)result;
            Assert.AreEqual("IndexKit", redirect.ActionName);
        }

        // --- MÉTODOS AUXILIARES (Massa de Dados) ---
        private KitViewModel GetNewKitModel()
        {
            return new KitViewModel { Id = 0, Nome = "Kit Teste", Valor = 200, IdEvento = 1, Descricao = "Desc" };
        }

        private static Kit GetTargetKit()
        {
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