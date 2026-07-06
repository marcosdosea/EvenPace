using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Dtos
{
    public class ResultadoPagamentoDto
    {
        public bool Success { get; set; }

        /// <summary>approved | rejected | pending | in_process</summary>
        public string? Status { get; set; }

        public int IdPagamento { get; set; }
        public string? IdTransacaoMP { get; set; }
        public string? MensagemErro { get; set; }
    }
}
