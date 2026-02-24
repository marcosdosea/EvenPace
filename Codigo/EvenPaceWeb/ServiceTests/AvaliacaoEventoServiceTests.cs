using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System;
using System.Linq;

namespace EvenPaceWebTests.Service
{
    [TestClass()]
    public class AvaliacaoEventoServiceTest
    {
        private EvenPaceContext _context;
        private IAvaliacaoEventoService _service;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<EvenPaceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new EvenPaceContext(options);
            _service = new AvaliacaoEventoService(_context);
        }

        [TestMethod()]
        public void CreateTest_DeveSalvarAvaliacao()
        {
            // Arrange
            var avaliacao = new AvaliacaoEvento
            {
                Estrela = 5,
                Comentario = "Evento excelente!"
            };

            // Act
            var id = _service.Create(avaliacao);

            // Assert
            Assert.AreEqual(1, _context.AvaliacaoEventos.Count());
            Assert.AreEqual(id, avaliacao.Id);
        }

        [TestMethod()]
        public void GetTest_DeveRetornarAvaliacao()
        {
            // Arrange
            var avaliacao = new AvaliacaoEvento
            {
                Estrela = 4,
                Comentario = "Muito bom"
            };

            var id = _service.Create(avaliacao);

            // Act
            var result = _service.Get(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Muito bom", result.Comentario);
        }

        [TestMethod()]
        public void GetAllTest_DeveRetornarTodasAvaliacoes()
        {
            // Arrange
            _service.Create(new AvaliacaoEvento { Estrela = 5, Comentario = "Ótimo" });
            _service.Create(new AvaliacaoEvento { Estrela = 3, Comentario = "Regular" });

            // Act
            var lista = _service.GetAll();

            // Assert
            Assert.AreEqual(2, lista.Count());
        }

        [TestMethod()]
        public void EditTest_DeveAtualizarAvaliacao()
        {
            // Arrange
            var avaliacao = new AvaliacaoEvento
            {
                Estrela = 2,
                Comentario = "Ruim"
            };

            var id = _service.Create(avaliacao);

            avaliacao.Id = id;
            avaliacao.Estrela = 4;
            avaliacao.Comentario = "Melhorou";

            // Act
            _service.Edit(avaliacao);

            var atualizado = _service.Get(id);

            // Assert
            Assert.AreEqual(4, atualizado.Estrela);
            Assert.AreEqual("Melhorou", atualizado.Comentario);
        }

        [TestMethod()]
        public void DeleteTest_DeveRemoverAvaliacao()
        {
            // Arrange
            var avaliacao = new AvaliacaoEvento
            {
                Estrela = 1,
                Comentario = "Péssimo"
            };

            var id = _service.Create(avaliacao);

            // Act
            _service.Delete(id);

            var result = _service.Get(id);

            // Assert
            Assert.IsNull(result);
        }
    }
}
