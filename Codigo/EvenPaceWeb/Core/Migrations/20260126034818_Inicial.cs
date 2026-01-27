using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    email = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    senha = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AvaliacaoEvento",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    comentario = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    estrela = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Corredor",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cpf = table.Column<string>(type: "char(11)", fixedLength: true, maxLength: 11, nullable: false),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    email = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    dataNascimento = table.Column<DateTime>(type: "date", nullable: false),
                    senha = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Organizacao",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cnpj = table.Column<string>(type: "char(14)", fixedLength: true, maxLength: 14, nullable: false),
                    cpf = table.Column<string>(type: "char(11)", fixedLength: true, maxLength: 11, nullable: true),
                    telefone = table.Column<int>(type: "int", nullable: false),
                    cep = table.Column<string>(type: "char(8)", fixedLength: true, maxLength: 8, nullable: false),
                    rua = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    bairro = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    cidade = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    numero = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    email = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    senha = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    statusSituacao = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Administrador_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Organizacao_Admistrador1",
                        column: x => x.Administrador_id,
                        principalTable: "Administrador",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CartaoCredito",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    numero = table.Column<string>(type: "char(16)", fixedLength: true, maxLength: 16, nullable: false),
                    dataValidade = table.Column<DateTime>(type: "date", nullable: false),
                    codeSeguranca = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    idCorredor = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_CartaoCredito_Corredor1",
                        column: x => x.idCorredor,
                        principalTable: "Corredor",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    data = table.Column<DateTime>(type: "datetime", nullable: false),
                    numeroParticipantes = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false),
                    distancia3 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia5 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia7 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia10 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia15 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia21 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia42 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    rua = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    bairro = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    cidade = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    estado = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    infoRetiradaKit = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    idOrganizacao = table.Column<uint>(type: "int unsigned", nullable: false),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Evento_Organizacao",
                        column: x => x.idOrganizacao,
                        principalTable: "Organizacao",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cupom",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    desconto = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    dataInicio = table.Column<DateTime>(type: "date", nullable: false),
                    dataTermino = table.Column<DateTime>(type: "date", nullable: false),
                    quantidadeUtilizada = table.Column<int>(type: "int", nullable: false),
                    quantiadeDisponibilizada = table.Column<int>(type: "int", nullable: false),
                    idEvento = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Cupom_Evento1",
                        column: x => x.idEvento,
                        principalTable: "Evento",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kit",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    valor = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    descricao = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    disponibilidadeP = table.Column<int>(type: "int", nullable: false),
                    disponibilidadeG = table.Column<int>(type: "int", nullable: false),
                    disponibilidadeM = table.Column<int>(type: "int", nullable: false),
                    utilizadaP = table.Column<sbyte>(type: "tinyint", nullable: false),
                    utilizadaG = table.Column<sbyte>(type: "tinyint", nullable: false),
                    utilizadaM = table.Column<sbyte>(type: "tinyint", nullable: false),
                    idEvento = table.Column<uint>(type: "int unsigned", nullable: false),
                    statusRetiradaKit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    dataRetirada = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Kit_Evento1",
                        column: x => x.idEvento,
                        principalTable: "Evento",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inscricao",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    status = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    dataInscricao = table.Column<DateTime>(type: "date", nullable: false),
                    distancia = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    tamanhoCamisa = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    tempo = table.Column<TimeSpan>(type: "time", nullable: false),
                    posicao = table.Column<int>(type: "int", nullable: false),
                    idKit = table.Column<uint>(type: "int unsigned", nullable: false),
                    idEvento = table.Column<uint>(type: "int unsigned", nullable: false),
                    idCorredor = table.Column<uint>(type: "int unsigned", nullable: false),
                    idAvaliacaoEvento = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_Inscricao_AvaliacaoEvento1",
                        column: x => x.idAvaliacaoEvento,
                        principalTable: "AvaliacaoEvento",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Inscricao_Corredor1",
                        column: x => x.idCorredor,
                        principalTable: "Corredor",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Inscricao_Evento1",
                        column: x => x.idEvento,
                        principalTable: "Evento",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_Inscricao_Kit1",
                        column: x => x.idKit,
                        principalTable: "Kit",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "email_UNIQUE",
                table: "Administrador",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_CartaoCredito_Corredor1_idx",
                table: "CartaoCredito",
                column: "idCorredor");

            migrationBuilder.CreateIndex(
                name: "idCorredor_UNIQUE",
                table: "CartaoCredito",
                column: "idCorredor",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "numero_UNIQUE",
                table: "CartaoCredito",
                column: "numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "cpf_UNIQUE",
                table: "Corredor",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "email_UNIQUE1",
                table: "Corredor",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_Cupom_Evento1_idx",
                table: "Cupom",
                column: "idEvento");

            migrationBuilder.CreateIndex(
                name: "fk_Evento_Organizacao_idx",
                table: "Evento",
                column: "idOrganizacao");

            migrationBuilder.CreateIndex(
                name: "fk_Inscricao_AvaliacaoEvento1_idx",
                table: "Inscricao",
                column: "idAvaliacaoEvento");

            migrationBuilder.CreateIndex(
                name: "fk_Inscricao_Corredor1_idx",
                table: "Inscricao",
                column: "idCorredor");

            migrationBuilder.CreateIndex(
                name: "fk_Inscricao_Evento1_idx",
                table: "Inscricao",
                column: "idEvento");

            migrationBuilder.CreateIndex(
                name: "fk_Inscricao_Kit1_idx",
                table: "Inscricao",
                column: "idKit");

            migrationBuilder.CreateIndex(
                name: "fk_Kit_Evento1_idx",
                table: "Kit",
                column: "idEvento");

            migrationBuilder.CreateIndex(
                name: "cnpj_UNIQUE",
                table: "Organizacao",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "email_UNIQUE2",
                table: "Organizacao",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_Organizacao_Admistrador1_idx",
                table: "Organizacao",
                column: "Administrador_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartaoCredito");

            migrationBuilder.DropTable(
                name: "Cupom");

            migrationBuilder.DropTable(
                name: "Inscricao");

            migrationBuilder.DropTable(
                name: "AvaliacaoEvento");

            migrationBuilder.DropTable(
                name: "Corredor");

            migrationBuilder.DropTable(
                name: "Kit");

            migrationBuilder.DropTable(
                name: "Evento");

            migrationBuilder.DropTable(
                name: "Organizacao");

            migrationBuilder.DropTable(
                name: "Administrador");
        }
    }
}
