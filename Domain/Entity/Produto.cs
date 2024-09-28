
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eCommerce.Domain.Entity;

namespace Ecommerce.Entity
{
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Required]
        public decimal Preco { get; set; }

        public decimal Peso { get; set; }

        [Required]
        public TipoProduto Tipo { get; set; }

        public Produto() { }

        public Produto(long id, string nome, string descricao, decimal preco, decimal peso, TipoProduto tipo)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Peso = peso;
            Tipo = tipo;
        }

        public decimal CalcularFrete()
        {
            var peso = this.Peso;
            return peso switch
            {
                <= 5.00m => 0,
                > 5.00m  and < 10.00m => 2,
                > 10.00m and < 50.00m => 4,
                > 50.00m => 7,
                _ => throw new Exception("Não existe regra para implementar o frete com o peso informado"),
            };
        }
    }
}
