using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Domain.Entity;
using Ecommerce.Entity;

namespace eCommerceTests
{
    public class CalcularDescontoItemsCarrinhoTest
    {
        [Fact(DisplayName = "Teste: Frete para cliente Ouro deve ser zero.")]
        public void TestFreteClienteOuro()
        {
            var clienteOuro = new Cliente { Id = 1, Nome = "Cliente Ouro", Tipo = TipoCliente.OURO };
            var itens = new List<ItemCompra>
            {
                new ItemCompra(1, new Produto(1, "Produto A", "Descrição A", 10, 2, TipoProduto.ELETRONICO), 1),
                new ItemCompra(2, new Produto(2, "Produto B", "Descrição B", 20, 3, TipoProduto.LIVRO), 1)
            };

            var carrinho = new CarrinhoDeCompras(1, clienteOuro, itens, DateTime.Now);

            decimal frete = carrinho.CalcularDescontoFrete();

            Assert.Equal(0, frete);
        }

        [Fact(DisplayName = "Teste: Frete para cliente Prata deve ter 50% de desconto.")]
        public void TestFreteClientePrata()
        {
            var clientePrata = new Cliente { Id = 2, Nome = "Cliente Prata", Tipo = TipoCliente.PRATA };
            var itens = new List<ItemCompra>
        {
            new ItemCompra(1, new Produto(1, "Produto A", "Descrição A", 10, 2, TipoProduto.LIVRO), 1),
            new ItemCompra(2, new Produto(2, "Produto B", "Descrição B", 20, 3, TipoProduto.ELETRONICO), 1)
        };

            var carrinho = new CarrinhoDeCompras(2, clientePrata, itens, DateTime.Now);

            decimal frete = carrinho.CalcularDescontoFrete();
            decimal freteEsperado = carrinho.CalcularFrete() * 0.5m; // 50% de desconto

            Assert.Equal(freteEsperado, frete);
        }

        [Fact(DisplayName = "Teste: Frete para cliente Bronze deve ser integral.")]
        public void TestFreteClienteBronze()
        {
            var clienteBronze = new Cliente { Id = 3, Nome = "Cliente Bronze", Tipo = TipoCliente.BRONZE };
            var itens = new List<ItemCompra>
            {
                new ItemCompra(1, new Produto(1, "Produto A", "Descrição A", 10, 2, TipoProduto.LIVRO), 1),
                new ItemCompra(2, new Produto(2, "Produto B", "Descrição B", 20, 3, TipoProduto.ROUPA), 1)
            };

            var carrinho = new CarrinhoDeCompras(3, clienteBronze, itens, DateTime.Now);

            decimal frete = carrinho.CalcularDescontoFrete();
            decimal freteEsperado = carrinho.CalcularFrete(); // Frete integral

            Assert.Equal(freteEsperado, frete);
        }

        [Theory(DisplayName = "Teste: Frete para clientes com diferentes pesos totais.")]
        [InlineData(5.00, 0)] // Cliente Ouro
        [InlineData(6.00, 12.00)] // Cliente Prata (50% de 24.00)
        [InlineData(10.00, 40.00)] // Cliente Bronze (40.00 integral)
        public void TestFreteComDiferentesPesos(decimal pesoTotal, decimal freteEsperado)
        {
            var clientePrata = new Cliente { Id = 2, Nome = "Cliente Prata", Tipo = TipoCliente.PRATA };
            var itens = new List<ItemCompra>
            {
                new ItemCompra(1, new Produto(1, "Produto A", "Descrição A", 10, pesoTotal, TipoProduto.ROUPA), 1)
            };

            var carrinho = new CarrinhoDeCompras(1, clientePrata, itens, DateTime.Now);

            decimal frete = carrinho.CalcularDescontoFrete();

            Assert.Equal(freteEsperado, frete);
        }

        [Fact(DisplayName = "Teste: Desconto de 10% para carrinho acima de R$ 500,00.")]
        public void TestDescontoDe10PorCento()
        {
            // Arrange
            var carrinho = new CarrinhoDeCompras(1, new Cliente { Id = 1, Tipo = TipoCliente.BRONZE }, new List<ItemCompra>(), DateTime.Now);
            decimal valorTotal = 600.00m; // Valor total acima de R$ 500,00
            decimal descontoEsperado = valorTotal * 0.10m;

            // Act
            var descontoCalculado = carrinho.CalcularDescontoItems();

            // Assert
            Assert.Equal(descontoEsperado, descontoCalculado);
        }

        [Fact(DisplayName = "Teste: Desconto de 20% para carrinho acima de R$ 1000,00.")]
        public void TestDescontoDe20PorCento()
        {
            // Arrange
            var carrinho = new CarrinhoDeCompras(1, new Cliente { Id = 1, Tipo = TipoCliente.BRONZE }, new List<ItemCompra>(), DateTime.Now);
            decimal valorTotal = 1200.00m; // Valor total acima de R$ 1000,00
            decimal descontoEsperado = valorTotal * 0.20m;

            // Act
            var descontoCalculado = carrinho.CalcularDescontoItems();

            // Assert
            Assert.Equal(descontoEsperado, descontoCalculado);
        }

        [Fact(DisplayName = "Teste: Sem desconto para carrinho até R$ 500,00.")]
        public void TestSemDesconto()
        {
            // Arrange
            var carrinho = new CarrinhoDeCompras(1, new Cliente { Id = 1, Tipo = TipoCliente.BRONZE }, new List<ItemCompra>(), DateTime.Now);
            decimal valorTotal = 400.00m; // Valor total abaixo de R$ 500,00
            decimal descontoEsperado = 0m;

            // Act
            var descontoCalculado = carrinho.CalcularDescontoItems();

            // Assert
            Assert.Equal(descontoEsperado, descontoCalculado);
        }

    }
}