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

        public CarrinhoDeCompras() {}

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
    }
}