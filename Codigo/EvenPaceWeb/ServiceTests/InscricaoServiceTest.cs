using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System.Collections.Generic;
using System.Linq;

namespace EvenPaceWebTests.Service
{
    [TestClass]
    public class InscricaoServiceTest
    {
        private EvenPaceContext context;
        private InscricaoService service;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<EvenPaceContext>()
                .UseInMemoryDatabase("InscricaoTestDB")
                .Options;

            context = new EvenPaceContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            service = new InscricaoService(context);
        }

        [TestMethod]
        public void Create_DeveAdicionarInscricaoESalvar()
        {
            var inscricao = new Inscricao
            {
                Status = "Pendente",
                DistanciaPercorida = 5,
                Tempo = new TimeSpan(0, 0, 30, 0),
                Posicao = 1,
                IdEvento = 2,
                IdCorredor = 3,
                IdKit = 1,
                IdAvaliacaoEvento = 1
            };

            service.Create(inscricao);

            Assert.AreEqual(1, context.Inscricaos.Count());
        }

        [TestMethod]
        public void Get_DeveRetornarInscricaoPorId()
        {
            var inscricao = new Inscricao
            {
                Id = 1,
                Status = "Confirmada",
                DistanciaPercorida = 10,
                Tempo = new TimeSpan(0, 0, 30, 0),
                Posicao = 1,
                IdEvento = 1,
                IdCorredor = 1,
                IdKit = 1,
                IdAvaliacaoEvento = 1
            };

            context.Inscricaos.Add(inscricao);
            context.SaveChanges();

            var result = service.Get(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void GetAll_DeveRetornarListaDeInscricoes()
        {
            context.Inscricaos.AddRange(
                new Inscricao
                {
                    Id = 1,
                    Status = "Confirmada",
                    DistanciaPercorida = 5,
                    Tempo = new TimeSpan(0, 0, 30, 0),
                    Posicao = 1,
                    IdEvento = 1,
                    IdCorredor = 1,
                    IdKit = 1,
                    IdAvaliacaoEvento = 1
                },
                new Inscricao
                {
                    Id = 2,
                    Status = "Pendente",
                    DistanciaPercorida = 10,
                    Tempo = new TimeSpan(0, 0, 30, 0),
                    Posicao = 1,
                    IdEvento = 1,
                    IdCorredor = 2,
                    IdKit = 1,
                    IdAvaliacaoEvento = 1
                }
            );
            context.SaveChanges();

            var result = service.GetAll();

            Assert.AreEqual(2, result.Count());
        }
        
        [TestMethod]
        public void GetAllByEvento_DeveRetornarSomenteInscricoesDoEvento()
        {
            // Arrange
            var organizacao = new Organizacao
            {
                Id = 1,
                Cnpj = "12345678000199",
                Nome = "Org Teste",
                Email = "org@teste.com",
                                
                Rua = "Rua do Evento",
                Bairro = "Centro",
                Cidade = "Aracaju",
                Estado = "SE",
                Cep = "44433321",
                Telefone = "12343221",
                Senha = "as123"
            };

            var evento = new Evento
            {
                Id = 1,
                Nome = "Evento Teste",
                Data = DateTime.Now,
                NumeroParticipantes = 100,
                IdOrganizacao = 1,
                
                Rua = "Rua do Evento",
                Bairro = "Centro",
                Cidade = "Aracaju",
                Estado = "SE",
                Discricao = "Evento de corrida de rua",
                InfoRetiradaKit = "Retirada no dia anterior ao evento"
            };

            context.Organizacaos.Add(organizacao);
            context.Eventos.Add(evento);

            context.Corredors.AddRange(
                new Corredor { Id = 1, Nome = "João", Cpf = "12345678900", Email = "joao@teste.com", Senha = "12312"},
                new Corredor { Id = 2, Nome = "Maria", Cpf = "98765432100", Email = "maria@teste.com", Senha = "12312" },
                new Corredor { Id = 3, Nome = "Carlos", Cpf = "11122233344", Email = "carlos@teste.com", Senha = "12312" }
            );

            context.Kits.Add(new Kit
            {
                Id = 1,
                Nome = "Kit Básico",
                Valor = 50,
                Descricao = "123dsaaCEF"
            });

            context.SaveChanges();

            context.Inscricaos.AddRange(
                new Inscricao { Id = 1, Status = "Confirmada", IdEvento = 1, IdCorredor = 1, IdKit = 1 },
                new Inscricao { Id = 2, Status = "Pendente",   IdEvento = 1, IdCorredor = 2, IdKit = 1 },
                new Inscricao { Id = 3, Status = "Confirmada", IdEvento = 2, IdCorredor = 3, IdKit = 1 }
            );

            context.SaveChanges();

            // Act
            var result = service.GetAllByEvento(1);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(i => i.IdEvento == 1));
        }

    }
}
