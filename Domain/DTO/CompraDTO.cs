namespace eCommerce.Domain.DTO
{
    public class CompraDTO
    {
        public bool Sucesso { get; init; }
        public long? TransacaoPagamentoId { get; init; }
        public string Mensagem { get; init; }

        //TODO: Frete?

        public CompraDTO(bool sucesso, long? transacaoPagamentoId, string mensagem)
        {
            Sucesso = sucesso;
            TransacaoPagamentoId = transacaoPagamentoId;
            Mensagem = mensagem;
        }
    }
}
