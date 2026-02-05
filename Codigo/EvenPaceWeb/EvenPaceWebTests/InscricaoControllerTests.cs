using AutoMapper;
using Core;
using Core.Service;
using EvenPace.Controllers;
using EvenPaceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Models;
using System;
using System.Collections.Generic;

namespace EvenPaceWebTests
{
    [TestClass]
    public class InscricaoControllerTests
    {
        private InscricaoController controller;
        private Mock<IInscricaoService> mockInscricaoService;
        private Mock<IEventosService> mockEventoService;
        private Mock<ICorredorService> mockCorredorService;
        private Mock<IKitService> mockKitService;
        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            mockInscricaoService = new Mock<IInscricaoService>();
            mockEventoService = new Mock<IEventosService>();
            mockCorredorService = new Mock<ICorredorService>();
            mockKitService = new Mock<IKitService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Kit, KitViewModel>();
                cfg.CreateMap<Inscricao, InscricaoViewModel>();
            });

            mapper = config.CreateMapper();

            mockEventoService.Setup(s => s.Get(1))
                .Returns(GetEvento());

            mockKitService.Setup(s => s.GetKitsPorEvento(1))
                .Returns(GetKits());

            controller = new InscricaoController(
                mockInscricaoService.Object,
                mockEventoService.Object,
                mockKitService.Object,
                mockCorredorService.Object,
                mapper
            );
        }

        [TestMethod]
        public void TelaInscricao_Get_Valido()
        {
            var result = controller.TelaInscricao(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var view = (ViewResult)result;
            Assert.IsInstanceOfType(view.Model, typeof(TelaInscricaoViewModel));

            var vm = (TelaInscricaoViewModel)view.Model;
            Assert.AreEqual(1, vm.IdEvento);
            Assert.AreEqual("Corrida Teste", vm.NomeEvento);
        }

        [TestMethod]
        public void TelaInscricao_Get_IdZero_RetornaBadRequest()
        {
            var result = controller.TelaInscricao(0);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Tela1_Get_Valido()
        {
            var result = controller.Tela1(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var view = (ViewResult)result;
            Assert.AreEqual("Tela1", view.ViewName);
        }

        [TestMethod]
        public void Tela1_Get_IdZero_RetornaContent()
        {
            var result = controller.Tela1(0);

            Assert.IsInstanceOfType(result, typeof(ContentResult));
        }

        [TestMethod]
        public void Cancelar_Get_ComIdValido_RetornaView()
        {
            mockInscricaoService.Setup(s => s.Get(1))
                .Returns(new Inscricao
                {
                    Id = 1,
                    IdEvento = 1,
                    Distancia = "5km",
                    DataInscricao = DateTime.Today
                });

            mockEventoService.Setup(s => s.Get(1))
                .Returns(new Evento
                {
                    Id = 1,
                    Nome = "Evento Teste",
                    Cidade = "Cidade",
                    Data = DateTime.Now.AddDays(10)
                });

            var result = controller.Cancelar(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cancelar_Get_InscricaoInexistente_RetornaNotFound()
        {
            mockInscricaoService.Setup(s => s.Get(1))
                .Returns((Inscricao)null);

            var result = controller.Cancelar(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        private Evento GetEvento()
        {
            return new Evento
            {
                Id = 1,
                Nome = "Corrida Teste",
                Cidade = "São Paulo",
                Data = DateTime.Today,
                Descricao = "Evento de teste"
            };
        }

        private IEnumerable<Kit> GetKits()
        {
            return new List<Kit>
            {
                new Kit
                {
                    Id = 1,
                    Nome = "Kit Básico",
                    Descricao = "Camiseta",
                    Valor = 99
                }
            };
        }
    }
}
