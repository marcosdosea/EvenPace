using AutoMapper;
using Core;
using Core.Service;
using Core.Service.Dtos;
using EvenPace.Controllers;
using EvenPaceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvenPaceWebTests
{
    [TestClass]
    public class InscricaoControllerTests
    {
        private InscricaoController controller;
        private Mock<IInscricaoService> mockInscricaoService;
        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            mockInscricaoService = new Mock<IInscricaoService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Kit, KitViewModel>();
                cfg.CreateMap<Inscricao, InscricaoViewModel>();
            });

            mapper = config.CreateMapper();

            controller = new InscricaoController(mockInscricaoService.Object, mapper);

            mockInscricaoService
                .Setup(s => s.GetDadosTelaInscricao(1))
                .Returns(new DadosTelaInscricaoDto
                {
                    IdEvento = 1,
                    NomeEvento = "Corrida Teste",
                    Local = "São Paulo",
                    DataEvento = DateTime.Today,
                    Descricao = "Evento de teste",
                    Kits = GetKits()
                });

            mockInscricaoService
                .Setup(s => s.GetAllByEvento(1))
                .Returns(GetInscricoes());
        }

        [TestMethod]
        public void TelaInscricao_Get_Valido()
        {
            var result = controller.Index(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var view = (ViewResult)result;
            Assert.IsInstanceOfType(view.Model, typeof(TelaInscricaoViewModel));

            var vm = (TelaInscricaoViewModel)view.Model;
            Assert.AreEqual(1, vm.IdEvento);
            Assert.AreEqual("Corrida Teste", vm.NomeEvento);
        }

        [TestMethod]
        public void Tela1_Get_Valido()
        {
            var result = controller.Index(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var view = (ViewResult)result;
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cancelar_Get_ComIdValido_RetornaView()
        {
            mockInscricaoService
                .Setup(s => s.GetDadosTelaDelete(1))
                .Returns(new GetDadosTelaDeleteResult
                {
                    Success = true,
                    Data = new DadosTelaDeleteDto
                    {
                        NomeEvento = "Evento Teste",
                        DataEvento = DateTime.Now.AddDays(10),
                        Local = "Cidade",
                        NomeKit = "Sem kit",
                        IdInscricao = 1,
                        Distancia = "5km",
                        TamanhoCamisa = "M",
                        DataInscricao = DateTime.Today
                    }
                });

            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cancelar_Get_InscricaoInexistente_RetornaNotFound()
        {
            mockInscricaoService
                .Setup(s => s.GetDadosTelaDelete(1))
                .Returns(new GetDadosTelaDeleteResult { Success = false, ErrorType = "NotFound" });

            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void GetAllByEvento_IdValido_RetornaViewComLista()
        {
            var result = controller.GetAllByEvento(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var view = (ViewResult)result;
            Assert.IsNotNull(view.Model);

            var model = view.Model as List<InscricaoViewModel>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
            Assert.IsTrue(model.All(i => i.IdEvento == 1));
        }

        private static IEnumerable<Kit> GetKits()
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
