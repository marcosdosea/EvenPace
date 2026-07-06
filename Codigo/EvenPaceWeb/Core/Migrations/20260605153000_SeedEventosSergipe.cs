using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Core.Migrations
{
    [DbContext(typeof(EvenPaceContext))]
    [Migration("20260605153000_SeedEventosSergipe")]
    public partial class SeedEventosSergipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE i FROM `Inscricao` i
                INNER JOIN `Evento` e ON e.`id` = i.`idEvento`
                WHERE e.`id` IN (1, 2, 3, 4, 900001, 900002, 900003, 900004, 900005, 900006, 900007, 900008)
                   OR e.`imagem` IN ('evento.png', 'evento2.png', 'evento3.png', 'evento4.png');

                DELETE c FROM `Cupom` c
                INNER JOIN `Evento` e ON e.`id` = c.`idEvento`
                WHERE e.`id` IN (1, 2, 3, 4, 900001, 900002, 900003, 900004, 900005, 900006, 900007, 900008)
                   OR e.`imagem` IN ('evento.png', 'evento2.png', 'evento3.png', 'evento4.png');

                DELETE k FROM `Kit` k
                INNER JOIN `Evento` e ON e.`id` = k.`idEvento`
                WHERE e.`id` IN (1, 2, 3, 4, 900001, 900002, 900003, 900004, 900005, 900006, 900007, 900008)
                   OR e.`imagem` IN ('evento.png', 'evento2.png', 'evento3.png', 'evento4.png');

                DELETE FROM `Evento`
                WHERE `id` IN (1, 2, 3, 4, 900001, 900002, 900003, 900004, 900005, 900006, 900007, 900008)
                   OR `imagem` IN ('evento.png', 'evento2.png', 'evento3.png', 'evento4.png');

                INSERT INTO `Evento` (`id`, `data`, `numeroParticipantes`, `discricao`, `distancia3`, `distancia5`, `distancia7`, `distancia10`, `distancia15`, `distancia21`, `distancia42`, `rua`, `bairro`, `cidade`, `estado`, `infoRetiradaKit`, `idOrganizacao`, `nome`, `imagem`)
                SELECT 900001, '2026-07-12 06:30:00', 350, 'Corrida costeira pela Orla de Atalaia, com percurso plano e visual do mar.', 0, 1, 0, 1, 0, 0, 0, 'Orla de Atalaia', 'Atalaia', 'Aracaju', 'Sergipe', 'Retirada no local do evento', o.`id`, 'Circuito Orla de Atalaia', 'eventos-sergipe/circuito-orla-atalaia.png'
                FROM `Organizacao` o ORDER BY o.`id` LIMIT 1;

                INSERT INTO `Evento` (`id`, `data`, `numeroParticipantes`, `discricao`, `distancia3`, `distancia5`, `distancia7`, `distancia10`, `distancia15`, `distancia21`, `distancia42`, `rua`, `bairro`, `cidade`, `estado`, `infoRetiradaKit`, `idOrganizacao`, `nome`, `imagem`)
                SELECT 900002, '2026-07-19 07:00:00', 220, 'Desafio com vista para os canions e clima de aventura no sertao sergipano.', 0, 0, 0, 1, 0, 0, 0, 'Rota do Xingo', 'Centro', 'Caninde', 'Sergipe', 'Retirada no local do evento', o.`id`, 'Desafio Xingo 10K', 'eventos-sergipe/desafio-xingo-10k.png'
                FROM `Organizacao` o ORDER BY o.`id` LIMIT 1;

                INSERT INTO `Evento` (`id`, `data`, `numeroParticipantes`, `discricao`, `distancia3`, `distancia5`, `distancia7`, `distancia10`, `distancia15`, `distancia21`, `distancia42`, `rua`, `bairro`, `cidade`, `estado`, `infoRetiradaKit`, `idOrganizacao`, `nome`, `imagem`)
                SELECT 900003, '2026-08-02 06:45:00', 260, 'Prova pelo centro historico de Sao Cristovao, unindo esporte e cultura.', 0, 1, 1, 0, 0, 0, 0, 'Praca Sao Francisco', 'Centro', 'Sao Cristovao', 'Sergipe', 'Retirada no local do evento', o.`id`, 'Corrida Historica Sao Cristovao', 'eventos-sergipe/corrida-historica-sao-cristovao.png'
                FROM `Organizacao` o ORDER BY o.`id` LIMIT 1;

                INSERT INTO `Evento` (`id`, `data`, `numeroParticipantes`, `discricao`, `distancia3`, `distancia5`, `distancia7`, `distancia10`, `distancia15`, `distancia21`, `distancia42`, `rua`, `bairro`, `cidade`, `estado`, `infoRetiradaKit`, `idOrganizacao`, `nome`, `imagem`)
                SELECT 900004, '2026-08-16 06:00:00', 180, 'Trail run na Serra de Itabaiana, com subidas leves e natureza no caminho.', 0, 1, 0, 1, 1, 0, 0, 'Parque dos Falcoes', 'Serra', 'Itabaiana', 'Sergipe', 'Retirada no local do evento', o.`id`, 'Trail Serra de Itabaiana', 'eventos-sergipe/trail-serra-itabaiana.png'
                FROM `Organizacao` o ORDER BY o.`id` LIMIT 1;

                INSERT INTO `Evento` (`id`, `data`, `numeroParticipantes`, `discricao`, `distancia3`, `distancia5`, `distancia7`, `distancia10`, `distancia15`, `distancia21`, `distancia42`, `rua`, `bairro`, `cidade`, `estado`, `infoRetiradaKit`, `idOrganizacao`, `nome`, `imagem`)
                SELECT 900005, '2026-08-30 05:50:00', 240, 'Meia maratona com largada proxima a Praia do Saco e clima de litoral.', 0, 0, 0, 0, 0, 1, 0, 'Praia do Saco', 'Litoral', 'Estancia', 'Sergipe', 'Retirada no local do evento', o.`id`, 'Meia Praia do Saco', 'eventos-sergipe/meia-praia-do-saco.png'
                FROM `Organizacao` o ORDER BY o.`id` LIMIT 1;

                INSERT INTO `Evento` (`id`, `data`, `numeroParticipantes`, `discricao`, `distancia3`, `distancia5`, `distancia7`, `distancia10`, `distancia15`, `distancia21`, `distancia42`, `rua`, `bairro`, `cidade`, `estado`, `infoRetiradaKit`, `idOrganizacao`, `nome`, `imagem`)
                SELECT 900006, '2026-09-13 07:00:00', 300, 'Corrida urbana no coracao de Lagarto, pensada para atletas iniciantes e experientes.', 1, 1, 0, 1, 0, 0, 0, 'Avenida Rotary', 'Centro', 'Lagarto', 'Sergipe', 'Retirada no local do evento', o.`id`, 'Corrida Cidade de Lagarto', 'eventos-sergipe/corrida-cidade-lagarto.png'
                FROM `Organizacao` o ORDER BY o.`id` LIMIT 1;

                INSERT INTO `Evento` (`id`, `data`, `numeroParticipantes`, `discricao`, `distancia3`, `distancia5`, `distancia7`, `distancia10`, `distancia15`, `distancia21`, `distancia42`, `rua`, `bairro`, `cidade`, `estado`, `infoRetiradaKit`, `idOrganizacao`, `nome`, `imagem`)
                SELECT 900007, '2026-09-27 06:20:00', 160, 'Eco run em Pirambu com rota costeira, dunas e foco em experiencia ao ar livre.', 0, 1, 1, 0, 0, 0, 0, 'Orla de Pirambu', 'Litoral', 'Pirambu', 'Sergipe', 'Retirada no local do evento', o.`id`, 'Eco Run Pirambu', 'eventos-sergipe/eco-run-pirambu.png'
                FROM `Organizacao` o ORDER BY o.`id` LIMIT 1;

                INSERT INTO `Evento` (`id`, `data`, `numeroParticipantes`, `discricao`, `distancia3`, `distancia5`, `distancia7`, `distancia10`, `distancia15`, `distancia21`, `distancia42`, `rua`, `bairro`, `cidade`, `estado`, `infoRetiradaKit`, `idOrganizacao`, `nome`, `imagem`)
                SELECT 900008, '2026-10-11 07:10:00', 210, 'Prova regional de 7 km pelas ruas de Tobias Barreto, com energia comunitaria.', 0, 0, 1, 0, 0, 0, 0, 'Avenida Sete de Junho', 'Centro', 'Tobias Barreto', 'Sergipe', 'Retirada no local do evento', o.`id`, '7K Tobias Barreto', 'eventos-sergipe/7k-tobias-barreto.png'
                FROM `Organizacao` o ORDER BY o.`id` LIMIT 1;

                INSERT INTO `Kit` (`id`, `valor`, `nome`, `descricao`, `disponibilidadeP`, `disponibilidadeG`, `disponibilidadeM`, `utilizadaP`, `utilizadaG`, `utilizadaM`, `idEvento`, `dataRetirada`, `imagem`)
                VALUES
                (910001, 59.90, 'Kit Atleta', 'Camisa, numero e chip', 80, 80, 80, 0, 0, 0, 900001, '2026-07-11 10:00:00', 'kit1.png'),
                (910002, 64.90, 'Kit Atleta', 'Camisa, numero e chip', 70, 70, 70, 0, 0, 0, 900002, '2026-07-18 10:00:00', 'kit1.png'),
                (910003, 54.90, 'Kit Atleta', 'Camisa, numero e chip', 70, 70, 70, 0, 0, 0, 900003, '2026-08-01 10:00:00', 'kit1.png'),
                (910004, 69.90, 'Kit Trail', 'Camisa, numero e chip', 60, 60, 60, 0, 0, 0, 900004, '2026-08-15 10:00:00', 'kit1.png'),
                (910005, 89.90, 'Kit Meia', 'Camisa, numero e chip', 80, 80, 80, 0, 0, 0, 900005, '2026-08-29 10:00:00', 'kit1.png'),
                (910006, 49.90, 'Kit Atleta', 'Camisa, numero e chip', 100, 100, 100, 0, 0, 0, 900006, '2026-09-12 10:00:00', 'kit1.png'),
                (910007, 57.90, 'Kit Eco', 'Camisa, numero e chip', 55, 55, 55, 0, 0, 0, 900007, '2026-09-26 10:00:00', 'kit1.png'),
                (910008, 52.90, 'Kit Atleta', 'Camisa, numero e chip', 70, 70, 70, 0, 0, 0, 900008, '2026-10-10 10:00:00', 'kit1.png');
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM `Inscricao` WHERE `idEvento` BETWEEN 900001 AND 900008;
                DELETE FROM `Cupom` WHERE `idEvento` BETWEEN 900001 AND 900008;
                DELETE FROM `Kit` WHERE `idEvento` BETWEEN 900001 AND 900008;
                DELETE FROM `Evento` WHERE `id` BETWEEN 900001 AND 900008;
                """);
        }
    }
}
