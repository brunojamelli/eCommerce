using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Domain.Entity;
using Ecommerce.Entity;

namespace eCommerce.Tests
{
    public class CalculaFreteProdutoTest
    {
        [Fact]
        public void TestCalcularFrete_PesoTotalAte5Kg_FreteGratis()
        {
            // Arrange
            var cliente = new Cliente { Tipo = TipoCliente.BRONZE }; // Cliente Bronze
            var produtos = new List<ItemCompra>
            {
                new ItemCompra(1, new Produto(1, "Produto 1", "Descricao 1", 100m, 2.5m, TipoProduto.ELETRONICO), 1),
                new ItemCompra(2, new Produto(2, "Produto 2", "Descricao 2", 50m, 2.5m, TipoProduto.ELETRONICO), 1)
            };

            var carrinho = new CarrinhoDeCompras(1, cliente, produtos, DateTime.Now);

            // Act
            var frete = carrinho.CalcularFrete(); // Função para calcular o frete

            // Assert
            Assert.Equal(0m, frete); // Frete deve ser gratuito
        }
        // [Fact]
        // public void TestCalcularFrete_PesoEntre5E10()
        // {
        //     // Criando produtos de teste com peso entre 5.01 e 9.99 kg
        //     Produto produto1 = new Produto(1, "Notebook Dell", "Notebook para trabalho", 2000.00m, 5.01m, TipoProduto.ELETRONICO); // Peso de 6 kg
        //     Produto produto2 = new Produto(2, "Impressora HP", "Impressora multifuncional", 1000.99m, 9.99m, TipoProduto.ELETRONICO); // Peso de 9 kg

        //     // Verificações
        //     Assert.Equal(2, produto1.CalcularFrete());
        //     Assert.Equal(2, produto2.CalcularFrete());
        // }

        // [Fact]
        // public void TestCalcularFrete_PesoIgualA10()
        // {
        //     // Criando produto de teste com peso de 10 kg
        //     Produto produto = new Produto(3, "TV Samsung 40\"", "Smart TV 4K", 2000.00m, 10.00m, TipoProduto.ELETRONICO); // Peso de 10 kg

        //     // Verificação
        //     Assert.Throws<Exception>(() => produto.CalcularFrete());

        // }

        // [Fact]
        // public void TestCalcularFrete_PesoEntre10E50()
        // {
        //     // Criando produtos de teste com peso entre 10.01 e 49.99 kg
        //     Produto produto1 = new Produto(4, "Geladeira Brastemp", "Geladeira duplex", 3500.00m, 10.01m, TipoProduto.ELETRONICO); // Peso de 20 kg
        //     Produto produto2 = new Produto(5, "Máquina de Lavar", "Máquina de lavar roupa", 2500.00m, 49.99m, TipoProduto.ELETRONICO); // Peso de 45 kg

        //     // Verificações
        //     Assert.Equal(4, produto1.CalcularFrete());
        //     Assert.Equal(4, produto2.CalcularFrete());
        // }

        // [Fact]
        // public void TestCalcularFrete_PesoIgualA50()
        // {
        //     // Criando produto de teste com peso de 50 kg
        //     Produto produto = new Produto(6, "Sofá 3 lugares", "Sofá confortável", 1500.00m, 50.00m, TipoProduto.MOVEL); // Peso de 50 kg

        //     // Verificação
        //     Assert.Throws<Exception>(() => produto.CalcularFrete());

        // }

        // [Fact]
        // public void TestCalcularFrete_PesoMaiorQue50()
        // {
        //     // Criando produtos de teste com peso maior que 50 kg
        //     Produto produto1 = new Produto(7, "Cama Box", "Cama box king size", 1200.00m, 50.01m, TipoProduto.MOVEL); // Peso de 55 kg
        //     Produto produto2 = new Produto(8, "Armário de Cozinha", "Armário planejado", 3000.00m, 100.54m, TipoProduto.MOVEL); // Peso de 75 kg

        //     // Verificações
        //     Assert.Equal(7, produto1.CalcularFrete());
        //     Assert.Equal(7, produto2.CalcularFrete());
        // }
    }
}