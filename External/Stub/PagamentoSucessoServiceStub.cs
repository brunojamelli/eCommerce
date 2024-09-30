using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Domain.DTO;

namespace eCommerce.External.Stub
{
    public class PagamentoSucessoServiceStub : IPagamentoExternal
    {
        public PagamentoDTO AutorizarPagamento(long clienteId, double custoTotal)
        {
            bool autorizado = true;
            long transacaoId = autorizado ? DateTime.Now.Ticks : 0;

            return new PagamentoDTO(autorizado, transacaoId);
        }

        public void CancelarPagamento(long clienteId, long pagamentoTransacaoId)
        {
            Console.WriteLine($"Pagamento com transação ID {pagamentoTransacaoId} cancelado para o cliente {clienteId}.");;
        }
    }
}