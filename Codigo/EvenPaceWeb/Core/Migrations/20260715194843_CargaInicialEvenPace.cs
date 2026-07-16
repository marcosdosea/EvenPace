using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class CargaInicialEvenPace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AvaliacaoEvento",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    comentario = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estrela = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Corredor",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cpf = table.Column<string>(type: "char(11)", fixedLength: true, maxLength: 11, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dataNascimento = table.Column<DateTime>(type: "date", nullable: false),
                    foto_perfil = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Organizacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "char(70)", fixedLength: true, maxLength: 70, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cnpj = table.Column<string>(type: "char(14)", fixedLength: true, maxLength: 14, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cpf = table.Column<string>(type: "char(11)", fixedLength: true, maxLength: 11, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cep = table.Column<string>(type: "char(8)", fixedLength: true, maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rua = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bairro = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cidade = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numero = table.Column<int>(type: "int", maxLength: 14, nullable: false),
                    estado = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CartaoCredito",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    numero = table.Column<string>(type: "char(16)", fixedLength: true, maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dataValidade = table.Column<DateTime>(type: "date", nullable: false),
                    codeSeguranca = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    idCorredor = table.Column<int>(type: "int", nullable: false)
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
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    data = table.Column<DateTime>(type: "datetime", nullable: false),
                    imagem = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numeroParticipantes = table.Column<int>(type: "int", nullable: false),
                    discricao = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    distancia3 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia5 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia7 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia10 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia15 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia21 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    distancia42 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    rua = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bairro = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cidade = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    infoRetiradaKit = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    idOrganizacao = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cupom",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    desconto = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    dataInicio = table.Column<DateTime>(type: "date", nullable: false),
                    dataTermino = table.Column<DateTime>(type: "date", nullable: false),
                    quantidadeUtilizada = table.Column<int>(type: "int", nullable: false),
                    quantiadeDisponibilizada = table.Column<int>(type: "int", nullable: false),
                    idEvento = table.Column<int>(type: "int", nullable: false)
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
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    valor = table.Column<decimal>(type: "decimal(10)", precision: 10, nullable: false),
                    nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descricao = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    disponibilidadeP = table.Column<int>(type: "int", nullable: false),
                    disponibilidadeG = table.Column<int>(type: "int", nullable: false),
                    disponibilidadeM = table.Column<int>(type: "int", nullable: false),
                    utilizadaP = table.Column<sbyte>(type: "tinyint", nullable: false),
                    utilizadaG = table.Column<sbyte>(type: "tinyint", nullable: false),
                    utilizadaM = table.Column<sbyte>(type: "tinyint", nullable: false),
                    idEvento = table.Column<int>(type: "int", nullable: false),
                    dataRetirada = table.Column<DateTime>(type: "datetime", nullable: false),
                    imagem = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inscricao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    status = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dataInscricao = table.Column<DateTime>(type: "date", nullable: false),
                    distancia = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tamanhoCamisa = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    statusRetiradaKit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    tempo = table.Column<TimeSpan>(type: "time", nullable: true),
                    posicao = table.Column<int>(type: "int", nullable: true),
                    idAvaliacaoEvento = table.Column<int>(type: "int", nullable: true),
                    idKit = table.Column<int>(type: "int", nullable: true),
                    idEvento = table.Column<int>(type: "int", nullable: false),
                    idCorredor = table.Column<int>(type: "int", nullable: false)
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
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idInscricao = table.Column<int>(type: "int", nullable: false),
                    valorPago = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    formaPagamento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    idTransacaoMP = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dataPagamento = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    parcelas = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Inscricao",
                        column: x => x.idInscricao,
                        principalTable: "Inscricao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "IX_Pagamentos_idInscricao",
                table: "Pagamentos",
                column: "idInscricao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrador");

            migrationBuilder.DropTable(
                name: "CartaoCredito");

            migrationBuilder.DropTable(
                name: "Cupom");

            migrationBuilder.DropTable(
                name: "Pagamentos");

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
        }
    }
}
