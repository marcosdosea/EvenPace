using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service.Dtos;

namespace Core.Service
{
    public interface IPagamentoService
    {
        /// <summary>Processa pagamento no Mercado Pago e salva no banco.</summary>
        Task<ResultadoPagamentoDto> ProcessarAsync(ProcessarPagamentoDto dto);

        /// <summary>Retorna o valor do kit da inscrição.</summary>
        Task<decimal> ObterValorKitAsync(int idInscricao);
    }
}
