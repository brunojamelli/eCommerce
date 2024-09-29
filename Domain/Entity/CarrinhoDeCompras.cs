using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Entity;

namespace eCommerce.Domain.Entity
{
    public class CarrinhoDeCompras
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public Cliente Cliente { get; set; }

        [Required]
        [ForeignKey("CarrinhoId")]
        public List<ItemCompra> Itens { get; set; } = new List<ItemCompra>();

        [Required]
        public DateTime Data { get; set; }

        public CarrinhoDeCompras() { }

        public CarrinhoDeCompras(long id, Cliente cliente, List<ItemCompra> itens, DateTime data)
        {
            Id = id;
            Cliente = cliente;
            Itens = itens;
            Data = data;
        }

        public decimal CalcularPesoTotal()
        {
            return Itens.Sum(item => item.Produto.Peso * item.Quantidade);
        }
        
        public decimal CalcularValorTotalItens()
        {
            return Itens.Sum(item => item.Produto.Preco * item.Quantidade);
        }
        
        public decimal CalcularFrete()
        {
            decimal pesoTotal = CalcularPesoTotal();
            decimal valorFrete;
            switch (pesoTotal)
            {
                case var _ when pesoTotal <= 5.00m:
                    valorFrete = 0m;
                    break;

                case var _ when pesoTotal > 5.00m && pesoTotal < 10.00m:
                    valorFrete = pesoTotal * 2m;
                    break;

                case var _ when pesoTotal >= 10.00m && pesoTotal < 50.00m:
                    valorFrete = pesoTotal * 4m;
                    break;

                case var _ when pesoTotal > 50.00m:
                    valorFrete = pesoTotal * 7m;
                    break;

                default:
                    throw new Exception("Não existe regra para implementar o frete com o peso informado");
            }

            return valorFrete;
        }
        
        public decimal CalcularDescontoFrete()
        {
            decimal pesoTotal = CalcularPesoTotal();
            decimal frete = CalcularFrete();

            // Aplicando o desconto baseado no tipo de cliente
            switch (Cliente.Tipo)
            {
                case TipoCliente.OURO:
                    return 0; // Isenção total do frete
                case TipoCliente.PRATA:
                    return frete * 0.5m; // 50% de desconto
                case TipoCliente.BRONZE:
                default:
                    return frete; // Pagamento integral
            }
        }

        // Método para calcular o desconto com base no valor total dos itens
        public decimal CalcularDescontoItems()
        {
            decimal valorTotal = CalcularValorTotalItens();
            decimal desconto = 0;

            if (valorTotal > 1000)
            {
                desconto = valorTotal * 0.20m; // 20% de desconto
            }
            else if (valorTotal > 500)
            {
                desconto = valorTotal * 0.10m; // 10% de desconto
            }

            return desconto;
        }

        public decimal CalcularValorFinal()
        {
            decimal valorTotal = CalcularValorTotalItens();
            decimal desconto = CalcularDescontoItems();
            decimal valorComDesconto = valorTotal - desconto;
            decimal freteComDesconto = CalcularDescontoFrete();

            return valorComDesconto + freteComDesconto; // Retorna o valor final incluindo frete
        }

    }
}