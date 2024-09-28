using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.Services;
using eCommerce.Domain.DTO;
using eCommerce.Domain.Entity;
using eCommerce.Services;
using Ecommerce.Entity;
using Moq;

namespace eCommerce.Tests
{
    public class CompraServiceUnitTest
    {
        private readonly Mock<ClienteService> _clienteServiceMock;
        private readonly Mock<CarrinhoDeComprasService> _carrinhoServiceMock;
        private readonly CompraService _compraService;
        public CompraServiceUnitTest()
        {
            _clienteServiceMock = new Mock<ClienteService>();
            _carrinhoServiceMock = new Mock<CarrinhoDeComprasService>();

            _compraService = new CompraService(_carrinhoServiceMock.Object, _clienteServiceMock.Object, null, null);
        }

        [Fact]
        public async Task FinalizarCompraAsync_DeveCalcularFreteParaPesoAte5kgSemFrete()
        {
            // Arrange
            var carrinho = CriarCarrinho(5); // 5kg
            var cliente = CriarCliente(TipoCliente.BRONZE);

              _carrinhoServiceMock
                .Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(carrinho);
            _clienteServiceMock.Setup(x => x.BuscarPorId(It.IsAny<long>()))
                .(cliente);

            // Act
            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(1, 1);

            // Assert
            Assert.True(compraDTO.Sucesso);
            Assert.Equal(500, _compraService.CalcularCustoTotal(carrinho)); // Sem frete
        }

        [Fact]
        public async Task FinalizarCompraAsync_DeveAplicarFreteDe2ReaisPorKgParaPesoEntre5e10kg()
        {
            // Arrange
            var carrinho = CriarCarrinho(7); // 7kg
            var cliente = CriarCliente(TipoCliente.BRONZE);

            _carrinhoServiceMock.Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.IsAny<long>(), cliente))
                .(carrinho);
            _clienteServiceMock.Setup(x => x.BuscarPorId(It.IsAny<long>()))
                .(cliente);

            // Act
            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(1, 1);

            // Assert
            Assert.True(compraDTO.Sucesso);
            Assert.Equal(514, _compraService.CalcularCustoTotal(carrinho)); // 7kg x R$2,00 = R$14, total R$ 500 + R$14
        }

        [Fact]
        public async Task FinalizarCompraAsync_DeveAplicarDescontoDe10PorcentoParaComprasAcimaDe500()
        {
            // Arrange
            var carrinho = CriarCarrinho(4, 600); // 4kg, valor dos itens = R$ 600
            var cliente = CriarCliente(TipoCliente.BRONZE);

            _carrinhoServiceMock.Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.IsAny<long>(), cliente))
                .(carrinho);
            _clienteServiceMock.Setup(x => x.BuscarPorId(It.IsAny<long>()))
                .(cliente);

            // Act
            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(1, 1);

            // Assert
            Assert.True(compraDTO.Sucesso);
            Assert.Equal(540, _compraService.CalcularCustoTotal(carrinho)); // R$600 - 10% de desconto
        }

        [Fact]
        public async Task FinalizarCompraAsync_DeveAplicarDescontoDe20PorcentoParaComprasAcimaDe1000()
        {
            // Arrange
            var carrinho = CriarCarrinho(4, 1200); // 4kg, valor dos itens = R$ 1200
            var cliente = CriarCliente(TipoCliente.BRONZE);

            _carrinhoServiceMock.Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.IsAny<long>(), cliente))
                .(carrinho);
            _clienteServiceMock.Setup(x => x.BuscarPorId(It.IsAny<long>()))
                .(cliente);

            // Act
            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(1, 1);

            // Assert
            Assert.True(compraDTO.Sucesso);
            Assert.Equal(960, _compraService.CalcularCustoTotal(carrinho)); // R$1200 - 20% de desconto
        }

        [Fact]
        public async Task FinalizarCompraAsync_DeveAplicarFreteIsentoParaClienteOuro()
        {
            // Arrange
            var carrinho = CriarCarrinho(15); // 15kg
            var cliente = CriarCliente(TipoCliente.OURO);

            _carrinhoServiceMock.Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.IsAny<long>(), cliente)).(carrinho);
            _clienteServiceMock.Setup(x => x.BuscarPorId(It.IsAny<long>())).(cliente);

            // Act
            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(1, 1);

            // Assert
            Assert.True(compraDTO.Sucesso);
            Assert.Equal(500, _compraService.CalcularCustoTotal(carrinho)); // Cliente ouro não paga frete
        }

        // Métodos Auxiliares
        private CarrinhoDeCompras CriarCarrinho(int pesoTotal, decimal valorItens = 500)
        {
            return new CarrinhoDeCompras
            {
                Itens = new List<ItemCompra>
                {
                    new ItemCompra { Produto = new Produto { Peso = pesoTotal, Preco = valorItens }, Quantidade = 1 }
                }
            };
        }

        private Cliente CriarCliente(TipoCliente tipo)
        {
            return new Cliente { Tipo = tipo };
        }
    }
}