using AutoMapper;
using Core;
using Core.Service;
using EvenPace.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EvenPaceWebTests
{
    [TestClass()]
    public class EventoControllerTests
    {
        private EventoController controller = null!;
        private Mock<IEventosService> mockEventosService = null!;
        private Mock<IKitService> mockKitService = null!;
        private Mock<IMapper> mockMapper = null!;

        [TestInitialize]
        public void Initialize()
        {
            // 1. Inicializa os Mocks
            mockEventosService = new Mock<IEventosService>();
            mockKitService = new Mock<IKitService>();
            mockMapper = new Mock<IMapper>();

            // 2. Configura o Mock do Mapper
            // Entity -> ViewModel
            mockMapper.Setup(m => m.Map<EventoViewModel>(It.IsAny<Evento>()))
                .Returns((Evento source) => new EventoViewModel
                {
                    Id = (uint)source.Id,
                    Nome = source.Nome,
                    IdOrganizacao = (uint)source.IdOrganizacao,
                    DataOnly = source.Data.Date
                });

            // ViewModel -> Entity
            mockMapper.Setup(m => m.Map<Evento>(It.IsAny<EventoViewModel>()))
                .Returns((EventoViewModel source) => new Evento
                {
                    Id = (int)source.Id,
                    Nome = source.Nome,
                    IdOrganizacao = (int)source.IdOrganizacao,
                    Data = source.DataOnly ?? DateTime.Now
                });

            // List<Entity> -> List<ViewModel>
            mockMapper.Setup(m => m.Map<List<EventoViewModel>>(It.IsAny<List<Evento>>()))
                .Returns((List<Evento> source) => source.Select(e => new EventoViewModel
                {
                    Id = (uint)e.Id,
                    Nome = e.Nome,
                    IdOrganizacao = (uint)e.IdOrganizacao
                }).ToList());

            // 3. Setup do Service (Dados Iniciais)
            mockEventosService.Setup(s => s.GetAll())
                .Returns(GetTestEventos());

            mockEventosService.Setup(s => s.Get(1))
                .Returns(GetTargetEvento());

            // Setup do KitService (Necessário para o Delete, pois ele busca kits do evento)
            mockKitService.Setup(s => s.GetAll())
                .Returns(new List<Kit>()); // Retorna lista vazia para simplificar o delete

            // 4. Contexto in-memory para o controller (exigido pelo construtor)
            var options = new DbContextOptionsBuilder<EvenPaceContext>()
                .UseInMemoryDatabase("EventoControllerTests_" + Guid.NewGuid().ToString())
                .Options;
            var context = new EvenPaceContext(options);

            controller = new EventoController(mockEventosService.Object, mockKitService.Object, mockMapper.Object, context);

            // Configura TempData
            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );
        }

        [TestMethod()]
        public void Index_RetornaViewComListaDeEventosDaOrganizacao()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;

            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<EventoViewModel>));
            List<EventoViewModel>? lista = (List<EventoViewModel>?)viewResult.ViewData.Model;

            Assert.IsNotNull(lista);
            // Espera-se 2 eventos pois o filtro no controller é IdOrganizacao == 1 (e o GetTestEventos tem 2 com esse ID)
            Assert.AreEqual(2, lista.Count);
        }

        [TestMethod()]
        public void Details_IdExistente_RetornaViewComModel()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            var model = (EventoViewModel?)viewResult.ViewData.Model;

            Assert.IsNotNull(model);
            Assert.AreEqual(1, (int)model.Id);
            Assert.AreEqual("Corrida de Verão", model.Nome);
        }

        [TestMethod()]
        public void Create_Post_Valido_RedirecionaParaIndex()
        {
            // Arrange
            var novoEvento = new EventoViewModel
            {
                Id = 0,
                Nome = "Corrida Nova",
                IdOrganizacao = 1,
                DataOnly = DateTime.Now.Date,
                HoraOnly = DateTime.Now.TimeOfDay
            };
            novoEvento.ImagemUpload = null; // Sem upload para teste unitário simples

            // Act
            var result = controller.Create(novoEvento);

            // Assert
            // Verifica se o método Create do serviço foi chamado
            mockEventosService.Verify(s => s.Create(It.IsAny<Evento>()), Times.Once);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod()]
        public void Edit_Post_Valido_RedirecionaParaIndex()
        {
            // Arrange
            int idEdicao = 1;
            var eventoEditado = new EventoViewModel
            {
                Id = (uint)idEdicao,
                Nome = "Corrida Editada",
                IdOrganizacao = 1,
                DataOnly = DateTime.Now.Date,
                HoraOnly = DateTime.Now.TimeOfDay
            };

            // Act
            var result = controller.Edit(idEdicao, eventoEditado);

            // Assert
            mockEventosService.Verify(s => s.Edit(It.IsAny<Evento>()), Times.Once);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod()]
        public void Delete_IdExistente_RedirecionaParaIndex()
        {
            // Act
            var result = controller.Delete(1);

            // Assert
            // Verifica se deletou o evento
            mockEventosService.Verify(s => s.Delete(1), Times.Once);

            // Verifica se tentou buscar kits vinculados (lógica do controller)
            mockKitService.Verify(s => s.GetAll(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        // --- MÉTODOS AUXILIARES (Massa de Dados) ---

        private static Evento GetTargetEvento()
        {
            return new Evento
            {
                Id = 1,
                Nome = "Corrida de Verão",
                IdOrganizacao = 1,
                Data = DateTime.Now
            };
        }

        private IEnumerable<Evento> GetTestEventos()
        {
            return new List<Evento>
            {
                new Evento { Id = 1, Nome = "Corrida de Verão", IdOrganizacao = 1, Data = DateTime.Now },
                new Evento { Id = 2, Nome = "Maratona Noturna", IdOrganizacao = 1, Data = DateTime.Now.AddDays(10) },
                new Evento { Id = 3, Nome = "Evento Outra Org", IdOrganizacao = 99, Data = DateTime.Now } // Não deve vir no Index
            };
        }
    }
}