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
        [Fact(DisplayName = "Teste para carrinho com peso total até 5 kg (frete gratuito):")]
        public void TestCalcularFrete_PesoTotalAte5Kg_FreteGratis()
        {
            // Arrange
            // Produtos de exemplo
            var produto1 = new Produto(1, "Livro - Programação C#", "Livro sobre C#", 99.90m, 2.5m, TipoProduto.LIVRO);
            var produto2 = new Produto(2, "Fone de Ouvido Bluetooth", "Fone sem fio com tecnologia Bluetooth", 199.99m, 2.5m, TipoProduto.ELETRONICO);
            
            // Carrinho de exemplo
            var itens = new List<ItemCompra>
            {
                new ItemCompra(1, produto1, 1),
                new ItemCompra(2, produto2, 1)
            };

            var carrinho = new CarrinhoDeCompras(1, new Cliente(1,"Bruno", TipoCliente.BRONZE), itens, DateTime.Now);

            // Act
            var frete = carrinho.CalcularFrete(); // Função para calcular o frete

            // Assert
            Assert.Equal(0m, frete); // Frete deve ser gratuito
        }
        [Fact(DisplayName = "Teste para carrinho com peso total entre 5 e 10 kg (R$ 2,00/kg):")]
        public void TestCalcularFrete_PesoTotalEntre5E10Kg_Frete2PorKg()
        {
            // Arrange
           // Produtos de exemplo
            var produto1 = new Produto(1, "Notebook - Dell Inspiron", "Notebook para trabalho e estudos", 3500m, 3.5m, TipoProduto.ELETRONICO);
            var produto2 = new Produto(2, "Teclado Mecânico Gamer", "Teclado RGB com teclas mecânicas", 499.99m, 2.6m, TipoProduto.ELETRONICO);
            
            // Carrinho de exemplo
            var itens = new List<ItemCompra>
            {
                new ItemCompra(1, produto1, 1),
                new ItemCompra(2, produto2, 1)
            };

            var carrinho = new CarrinhoDeCompras(1, new Cliente(1,"Bruno", TipoCliente.BRONZE), itens, DateTime.Now);

            // Act
            var frete = carrinho.CalcularFrete();

            // Assert
            Assert.Equal(13m, frete); // (6.5 kg * R$ 2,00 = R$ 13,00)
        }

    }
}