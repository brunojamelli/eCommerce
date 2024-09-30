using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Domain.DTO;

namespace eCommerce.External.Stub
{
    public class EstoqueExternalStubSempre : IEstoqueExternal
    {
        public EstoqueBaixaDTO DarBaixa(List<long> produtosIds, List<long> produtosQuantidades)
        {
            return new EstoqueBaixaDTO(true);
        }
        public DisponibilidadeDTO VerificarDisponibilidade(List<long> produtosIds, List<long> produtosQuantidades)
        {
            return new DisponibilidadeDTO(true, new List<long>());
        }
    }
}