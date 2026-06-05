using Core;
using Microsoft.EntityFrameworkCore;
using Service;

namespace EvenPaceWebTests.Service
{
    [TestClass]
    public class InscricaoServiceTest
    {
        private EvenPaceContext context = null!;
        private InscricaoService service = null!;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<EvenPaceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new EvenPaceContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            service = new InscricaoService(context);
        }

        [TestMethod]
        public async Task CreateAsync_DeveAdicionarInscricaoESalvar()
        {
            SeedEventoComKit();

            var inscricao = new Inscricao
            {
                Status = "Pendente",
                Distancia = "5km",
                TamanhoCamisa = "M",
                IdEvento = 1,
                IdCorredor = 3,
                IdKit = 1
            };

            await service.CreateAsync(inscricao);

            Assert.AreEqual(1, await context.Inscricao.CountAsync());
        }

        [TestMethod]
        public async Task GetAsync_DeveRetornarInscricaoPorId()
        {
            var inscricao = new Inscricao
            {
                Id = 1,
                Status = "Confirmada",
                Distancia = "10km",
                TamanhoCamisa = "G",
                IdEvento = 1,
                IdCorredor = 1,
                IdKit = 1
            };

            context.Inscricao.Add(inscricao);
            await context.SaveChangesAsync();

            var result = await service.GetAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task GetAllAsync_DeveRetornarListaDeInscricoes()
        {
            context.Inscricao.AddRange(
                new Inscricao
                {
                    Id = 1,
                    Status = "Confirmada",
                    Distancia = "5km",
                    TamanhoCamisa = "M",
                    IdEvento = 1,
                    IdCorredor = 1,
                    IdKit = 1
                },
                new Inscricao
                {
                    Id = 2,
                    Status = "Pendente",
                    Distancia = "10km",
                    TamanhoCamisa = "G",
                    IdEvento = 1,
                    IdCorredor = 2,
                    IdKit = 1
                }
            );

            await context.SaveChangesAsync();

            var result = await service.GetAllAsync();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task CancelarAsync_DeveAlterarStatusParaCancelada()
        {
            SeedEventoComKit();

            var inscricao = new Inscricao
            {
                Id = 1,
                Status = "Confirmada",
                Distancia = "5km",
                TamanhoCamisa = "M",
                IdEvento = 1,
                IdCorredor = 1,
                IdKit = 1
            };

            context.Inscricao.Add(inscricao);
            await context.SaveChangesAsync();

            await service.CancelarAsync(1, 1);

            var result = await context.Inscricao.FirstAsync();
            Assert.AreEqual("Cancelada", result.Status);
        }

        [TestMethod]
        public async Task CancelarAsync_InscricaoNaoExiste_LancaExcecao()
        {
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                service.CancelarAsync(99, 1)
            );
        }

        [TestMethod]
        public async Task CreateAsync_InscricaoDuplicadaMesmoEvento_LancaExcecao()
        {
            SeedEventoComKit();

            context.Inscricao.Add(new Inscricao
            {
                Id = 1,
                Status = "Pendente",
                Distancia = "5km",
                TamanhoCamisa = "M",
                IdEvento = 1,
                IdCorredor = 1,
                IdKit = 1
            });
            await context.SaveChangesAsync();

            var inscricaoDuplicada = new Inscricao
            {
                Status = "Pendente",
                Distancia = "5km",
                TamanhoCamisa = "M",
                IdEvento = 1,
                IdCorredor = 1,
                IdKit = 1
            };

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                service.CreateAsync(inscricaoDuplicada)
            );
        }

        [TestMethod]
        public async Task CreateAsync_EventoExpirado_LancaExcecao()
        {
            context.Eventos.Add(new Evento
            {
                Id = 1,
                Nome = "Evento expirado",
                Descricao = "Descrição",
                Cidade = "São Paulo",
                Estado = "SP",
                Bairro = "Centro",
                Rua = "Rua A",
                InfoRetiradaKit = "Retirada no local",
                Data = DateTime.Now.AddDays(-1)
            });

            context.Kits.Add(new Kit
            {
                Id = 1,
                Nome = "Kit A",
                Descricao = "Descrição",
                Valor = 100,
                DisponibilidadeP = 1,
                DisponibilidadeM = 1,
                DisponibilidadeG = 1,
                UtilizadaP = 0,
                UtilizadaM = 0,
                UtilizadaG = 0,
                IdEvento = 1,
                DataRetirada = DateTime.Now.AddDays(-2)
            });

            await context.SaveChangesAsync();

            var inscricao = new Inscricao
            {
                Status = "Pendente",
                Distancia = "5km",
                TamanhoCamisa = "M",
                IdEvento = 1,
                IdCorredor = 1,
                IdKit = 1
            };

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                service.CreateAsync(inscricao)
            );
        }

        private void SeedEventoComKit()
        {
            context.Eventos.Add(new Evento
            {
                Id = 1,
                Nome = "Evento Teste",
                Descricao = "Descrição teste",
                Cidade = "São Paulo",
                Estado = "SP",
                Bairro = "Centro",
                Rua = "Rua A",
                InfoRetiradaKit = "Retirada no local",
                Data = DateTime.Now.AddDays(5),
                Distancia5 = true,
                Distancia10 = true
            });

            context.Kits.Add(new Kit
            {
                Id = 1,
                Nome = "Kit Básico",
                Descricao = "Descrição kit",
                Valor = 100,
                DisponibilidadeP = 10,
                DisponibilidadeM = 10,
                DisponibilidadeG = 10,
                UtilizadaP = 0,
                UtilizadaM = 0,
                UtilizadaG = 0,
                IdEvento = 1,
                DataRetirada = DateTime.Now.AddDays(2)
            });

            context.SaveChanges();
        }
    }
}
