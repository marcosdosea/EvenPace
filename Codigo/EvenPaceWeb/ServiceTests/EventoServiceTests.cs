using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvenPaceWebTests.Service
{
    [TestClass()]
    public class EventoServiceTests
    {
        private EvenPaceContext context = null!;
        private IEventosService eventosService = null!;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<EvenPaceContext>();
            builder.UseInMemoryDatabase("EvenPace_Eventos"); 
            var options = builder.Options;

            context = new EvenPaceContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var eventos = new List<Evento>
            {
                new Evento
                {
                    Id = 1,
                    Nome = "Corrida de Verão",
                    IdOrganizacao = 1,
                    NumeroParticipantes = 100,
                    Descricao = "Corrida no verão",
                    Data = DateTime.Now.AddDays(10),
                    
                    Distancia3 = true,
                    Distancia5 = true,
                    Distancia7 = false,
                    Distancia10 = false,
                    Distancia15 = false,
                    Distancia21 = false,
                    Distancia42 = false,

                    Rua = "Rua A",
                    Bairro = "Centro",
                    Cidade = "Aracaju",
                    Estado = "SE",
                    InfoRetiradaKit = "No local",
                },
                new Evento
                {
                    Id = 2,
                    Nome = "Maratona Noturna",
                    IdOrganizacao = 1,
                    Data = DateTime.Now.AddDays(20),
                    Descricao = "Descrição padrão",
                    NumeroParticipantes = 100,

                    Distancia3 = true,
                    Distancia5 = true,
                    Distancia7 = false,
                    Distancia10 = false,
                    Distancia15 = false,
                    Distancia21 = false,
                    Distancia42 = false,

                    Rua = "Rua A",
                    Bairro = "Centro",
                    Cidade = "Aracaju",
                    Estado = "SE",
                    InfoRetiradaKit = "No local",
                },
                new Evento
                {
                    Id = 3,
                    Nome = "Evento Beneficente",
                    IdOrganizacao = 2,
                    Data = DateTime.Now.AddDays(10),
                    Descricao = "Descrição padrão",
                    NumeroParticipantes = 100,

                    Distancia3 = true,
                    Distancia5 = true,
                    Distancia7 = false,
                    Distancia10 = false,
                    Distancia15 = false,
                    Distancia21 = false,
                    Distancia42 = false,

                    Rua = "Rua A",
                    Bairro = "Centro",
                    Cidade = "Aracaju",
                    Estado = "SE",
                    InfoRetiradaKit = "No local",
                }
            };

            context.AddRange(eventos);
            context.SaveChanges();

            eventosService = new EventoService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novoEvento = new Evento
            {
                Id = 4,
                Nome = "Novo Evento Teste",
                IdOrganizacao = 1,
                Data = DateTime.Now.AddMonths(1),
                Descricao = "Teste de criação",
                NumeroParticipantes = 100,

                Distancia3 = false,
                Distancia5 = true,
                Distancia7 = false,
                Distancia10 = false,
                Distancia15 = false,
                Distancia21 = false,
                Distancia42 = false,

                Rua = "Rua texte",
                Bairro = "Bairro de Texte",
                Cidade = "Cidade de Teste",
                Estado = "SE",
                InfoRetiradaKit = "No local",
            };

            eventosService.Create(novoEvento);

            Assert.AreEqual(4, eventosService.GetAll().Count()); 

            var recuperado = eventosService.Get(4);
            Assert.IsNotNull(recuperado);
            Assert.AreEqual("Novo Evento Teste", recuperado.Nome);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            int idParaDeletar = 1;

            eventosService.Delete(idParaDeletar);

            Assert.AreEqual(2, eventosService.GetAll().Count()); 
            Assert.IsNull(eventosService.Get(idParaDeletar));
        }

        [TestMethod()]
        public void EditTest()
        {
            var evento = eventosService.Get(2); 
            evento.Nome = "Maratona Editada";
            evento.Descricao = "Nova descrição";

            eventosService.Edit(evento);

            var eventoEditado = eventosService.Get(2);
            Assert.IsNotNull(eventoEditado);
            Assert.AreEqual("Maratona Editada", eventoEditado.Nome);
            Assert.AreEqual("Nova descrição", eventoEditado.Descricao);
        }

        [TestMethod()]
        public void GetTest()
        {
            var evento = eventosService.Get(3);

            Assert.IsNotNull(evento);
            Assert.AreEqual("Evento Beneficente", evento.Nome);
            Assert.AreEqual(2, evento.IdOrganizacao);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var lista = eventosService.GetAll();

            Assert.IsNotNull(lista);
            Assert.AreEqual(3, lista.Count());

            var primeiro = lista.First();
            Assert.AreEqual(1, primeiro.Id);
        }
    }
}