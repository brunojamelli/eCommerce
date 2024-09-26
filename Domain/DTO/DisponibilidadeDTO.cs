using System.Collections.Generic;

namespace eCommerce.Domain.DTO
{
    public class DisponibilidadeDTO
    {
        public bool Disponivel { get; init; }
        public List<long> IdsProdutosIndisponiveis { get; init; }
        //TODO: Id produto indisponível será imutável?

        public DisponibilidadeDTO(bool disponivel, List<long> idsProdutosIndisponiveis)
        {
            Disponivel = disponivel;
            IdsProdutosIndisponiveis = idsProdutosIndisponiveis;
        }
    }
}
