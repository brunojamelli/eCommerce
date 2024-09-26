namespace eCommerce.Domain.DTO
{
    public class ProdutoDTO
    {
        public int IdProduto { get; init; }
        public string? NomeProduto { get; init; }
        public int Preco { get; init; }
        public int Quantidade {get; init;}

    }
}
