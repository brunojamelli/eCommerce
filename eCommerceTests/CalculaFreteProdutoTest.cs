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
        [Fact(DisplayName = "Teste para carrinho com peso total até 5 kg (frete gratuito):")]
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
        [Fact(DisplayName = "Teste para carrinho com peso total entre 5 e 10 kg (R$ 2,00/kg):")]
        public void TestCalcularFrete_PesoTotalEntre5E10Kg_Frete2PorKg()
        {
            // Arrange
            var cliente = new Cliente { Tipo = TipoCliente.BRONZE }; // Cliente Bronze
            var produtos = new List<ItemCompra>
            {
                new ItemCompra(1, new Produto(1, "Produto 1", "Descricao 1", 100m, 3.0m, TipoProduto.ELETRONICO), 1),
                new ItemCompra(2, new Produto(2, "Produto 2", "Descricao 2", 50m, 3.5m, TipoProduto.ELETRONICO), 1)
            };

            var carrinho = new CarrinhoDeCompras(1, cliente, produtos, DateTime.Now);

            // Act
            var frete = carrinho.CalcularFrete();

            // Assert
            Assert.Equal(13m, frete); // (6.5 kg * R$ 2,00 = R$ 13,00)
        }

    }
}