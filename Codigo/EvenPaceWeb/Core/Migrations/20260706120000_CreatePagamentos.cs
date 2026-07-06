using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Core.Migrations
{
    [DbContext(typeof(EvenPaceContext))]
    [Migration("20260706120000_CreatePagamentos")]
    /// <inheritdoc />
    public partial class CreatePagamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE Pagamentos (
                    id INT UNSIGNED NOT NULL AUTO_INCREMENT,
                    idInscricao INT UNSIGNED NOT NULL,
                    valorPago DECIMAL(10,2) NOT NULL,
                    status VARCHAR(50) NOT NULL,
                    formaPagamento VARCHAR(50) NOT NULL,
                    idTransacaoMP VARCHAR(100) NULL,
                    dataPagamento DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    parcelas INT NOT NULL DEFAULT 1,

                    PRIMARY KEY (id),

                    INDEX IX_Pagamentos_idInscricao (idInscricao),

                    CONSTRAINT FK_Pagamentos_Inscricao
                        FOREIGN KEY (idInscricao)
                        REFERENCES Inscricao(id)
                        ON DELETE RESTRICT
                        ON UPDATE CASCADE
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamentos");
        }
    }
}
