using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int IdInscricao { get; set; }
        public decimal ValorPago { get; set; }

        /// <summary>Status retornado pelo Mercado Pago: approved, rejected, pending, in_process</summary>
        public string Status { get; set; } = null!;

        /// <summary>Bandeira: visa, master, elo, pix…</summary>
        public string FormaPagamento { get; set; } = null!;

        /// <summary>ID da transação no Mercado Pago</summary>
        public string? IdTransacaoMP { get; set; }

        public DateTime DataPagamento { get; set; }
        public int Parcelas { get; set; } = 1;

        public virtual Inscricao IdInscricaoNavigation { get; set; } = null!;

    

      
    }
}
