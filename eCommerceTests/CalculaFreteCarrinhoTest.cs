using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Domain.Entity;
using Ecommerce.Entity;

namespace eCommerce.Tests
{
    public class CalculaFreteCarrinhoTest
    {
        [Theory(DisplayName = "Teste para carrinho com peso total até 5 kg (frete gratuito):")]
        [InlineData(2.50, 2.49, 0)]
        [InlineData(2.50, 2.50, 0)]
        [InlineData(2.50, 2.51, 0)]
        [InlineData(1.00, 1.94, 0)]
        public void TestCalcularFrete_PesoTotalAte5Kg_FreteGratis(decimal pesoProd1, decimal pesoProd2, decimal freteEsperado)
        {
            // Arrange
            // Produtos de exemplo
            var produto1 = new Produto(1, "Livro - Programação C#", "Livro sobre C#", 99.90m, pesoProd1, TipoProduto.LIVRO);
            var produto2 = new Produto(2, "Fone de Ouvido Bluetooth", "Fone sem fio com tecnologia Bluetooth", 199.99m, pesoProd2, TipoProduto.ELETRONICO);

            // Carrinho de exemplo
            var itens = new List<ItemCompra>
            {
                new ItemCompra(1, produto1, 1),
                new ItemCompra(2, produto2, 1)
            };

            var carrinho = new CarrinhoDeCompras(1, new Cliente(1, "Bruno", TipoCliente.BRONZE), itens, DateTime.Now);

            // Act
            var frete = carrinho.CalcularFrete(); // Função para calcular o frete

            // Assert
            Assert.Equal(freteEsperado, frete); // Frete deve ser gratuito
        }

        [Theory(DisplayName = "Teste para carrinho com peso total entre 5 e 10 kg (R$ 2,00/kg):")]
        [InlineData(2.50, 2.50, 10)]
        [InlineData(5.00, 5.00, 20)]
        [InlineData(2.50, 2.51, 10.02)]
        [InlineData(2.50, 2.52, 10.04)]
        [InlineData(4.00, 5.98, 19.96)]
        [InlineData(4.00, 5.99, 19.98)]
        [InlineData(4.00, 3.23, 14.46)]
        public void TestCalcularFrete_PesoTotalEntre5E10Kg(decimal pesoProd1, decimal pesoProd2, decimal freteEsperado)
        {
            // Arrange
            // Produtos de exemplo
            var produto1 = new Produto(1, "Notebook - Dell Inspiron", "Notebook para trabalho e estudos", 3500m, pesoProd1, TipoProduto.ELETRONICO);
            var produto2 = new Produto(2, "Teclado Mecânico Gamer", "Teclado RGB com teclas mecânicas", 499.99m, pesoProd2, TipoProduto.ELETRONICO);

            // Carrinho de exemplo
            var itens = new List<ItemCompra>
            {
                new ItemCompra(1, produto1, 1),
                new ItemCompra(2, produto2, 1)
            };

            var carrinho = new CarrinhoDeCompras(1, new Cliente(1, "Bruno", TipoCliente.BRONZE), itens, DateTime.Now);

            // Act
            var frete = carrinho.CalcularFrete();

            // Assert
            Assert.Equal(freteEsperado, frete); // Peso total: 6.1 kg, frete: 6.1 kg * R$ 2,00
        }
        [Theory(DisplayName = "Teste para carrinho com peso entre 10 kg e 50 kg (R$ 4,00 por kg):")]
        [InlineData(5.00, 5.01, 40.04)]
        [InlineData(5.00, 5.02, 40.08)]
        [InlineData(20.00, 29.98, 199.92)]
        [InlineData(10.00, 39.99, 199.96)]
        [InlineData(3.00, 7.00, 40.00)]
        [InlineData(35.00, 15.00, 200.00)]
        [InlineData(10.01, 20.00, 120.04)]
        public void TestCalcularFrete_PesoEntre10E50kg(decimal pesoProd1, decimal pesoProd2, decimal freteEsperado)
        {
            // Produtos de exemplo
            var produto1 = new Produto(1, "TV LED 50\"", "Smart TV 4K Ultra HD", 2799.99m, pesoProd1, TipoProduto.ELETRONICO);
            var produto2 = new Produto(2, "Barbeador Elétrico", "Barbeador sem fio", 399.99m, pesoProd2, TipoProduto.ELETRONICO);

            // Carrinho de exemplo
            var itens = new List<ItemCompra>
            {
                new ItemCompra(1, produto1, 1),
                new ItemCompra(2, produto2, 1)
            };

            var carrinho = new CarrinhoDeCompras(1, new Cliente(1, "Bruno", TipoCliente.BRONZE), itens, DateTime.Now);

            // Calcula o frete
            decimal frete = carrinho.CalcularFrete();

            // Assert - frete deve ser calculado como 4,00 por kg
            Assert.Equal(freteEsperado, frete); // Peso total: 13.5 kg, frete: 13.5 kg * R$ 4,00
        }

        [Fact(DisplayName = "Teste para carrinho com peso acima de 50 kg (R$ 7,00 por kg):")]
        public void TestCalcularFrete_PesoAcima50kg()
        {
            // Produtos de exemplo
            var produto1 = new Produto(1, "Máquina de Lavar 12kg", "Máquina de lavar roupas", 1999.99m, 51.5m, TipoProduto.ELETRONICO);

            // Carrinho de exemplo
            var itens = new List<ItemCompra>
            {
                new ItemCompra(1, produto1, 1)
            };

            var carrinho = new CarrinhoDeCompras(1, new Cliente(1, "Bruno", TipoCliente.BRONZE), itens, DateTime.Now);

            // Calcula o frete
            decimal frete = carrinho.CalcularFrete();

            // Assert - frete deve ser calculado como 7,00 por kg
            Assert.Equal(360.5m, frete); // Peso total: 51.5 kg, frete: 51.5 kg * R$ 7,00
        }

        [Theory(DisplayName = "Teste para carrinho com peso total igual a 50 kg (erro) e maior que 50 (R$ 7,00 por kg):")]
        [InlineData(30.00, 20.00, 200.00)]
        [InlineData(30.00, 20.01, 350.07)]
        [InlineData(30.00, 20.02, 350.14)]
        [InlineData(60.00, 40.54, 703.78)]
        public void TestCalcularFrete_PesoIgual50kg_E_MaiorQue50(decimal pesoProd1, decimal pesoProd2, decimal freteEsperado)
        {
            // Produtos de exemplo
            var produto1 = new Produto(1, "Geladeira", "Geladeira Frost Free 400L", 2999.99m, pesoProd1, TipoProduto.ELETRONICO);
            var produto2 = new Produto(2, "Micro-ondas", "Micro-ondas 30L", 499.99m, pesoProd2, TipoProduto.ELETRONICO);

            // Carrinho de exemplo
            var itens = new List<ItemCompra>
            {
                new ItemCompra(1, produto1, 1),
                new ItemCompra(2, produto2, 1)
            };

            var carrinho = new CarrinhoDeCompras(1, new Cliente(1, "Bruno", TipoCliente.BRONZE), itens, DateTime.Now);

            // Calcula o frete
            decimal frete = carrinho.CalcularFrete();

            // Assert - frete deve ser calculado como 4,00 por kg
            Assert.Equal(freteEsperado, frete); // Peso total: 50 kg, frete: 50 kg * R$ 4,00
        }
    }
}