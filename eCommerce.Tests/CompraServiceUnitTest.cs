using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.Services;
using eCommerce.Domain.DTO;
using eCommerce.Domain.Entity;
using eCommerce.External;
using eCommerce.External.Fake;
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
        private readonly IEstoqueExternal _estoqueExternal;
        private readonly IPagamentoExternal _pagamentoExternal;
        public CompraServiceUnitTest()
        {
            _clienteServiceMock = new Mock<ClienteService>();
            _carrinhoServiceMock = new Mock<CarrinhoDeComprasService>();
            _estoqueExternal = new EstoqueSimulado();
            _pagamentoExternal = new PagamentoSimulado();

            _compraService = new CompraService(_carrinhoServiceMock.Object, _clienteServiceMock.Object, _estoqueExternal, _pagamentoExternal);
        }

        [Fact]
        public async Task FinalizarCompraAsync_CarrinhoComPesoAte5kgClienteBronze_SemFrete()
        {
            var carrinhoId = 1L;
            var cliente = CriarCliente(TipoCliente.BRONZE);
            var carrinho = CriarCarrinho(5); // 5kg total, sem frete

            _carrinhoServiceMock
                .Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.Is<long>(id => id == carrinhoId), It.Is<Cliente>(c => c == cliente)));

            _clienteServiceMock
                .Setup(x => x.BuscarPorId(It.IsAny<long>()));

            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(carrinhoId, cliente.Id);

            Assert.True(compraDTO.Sucesso);
            Assert.Equal(500, _compraService.CalcularCustoTotal(carrinho)); // Sem frete
        }

        [Fact]
        public async Task FinalizarCompraAsync_CarrinhoComPesoEntre5E10kgClienteBronze_FreteAplicado()
        {
            var carrinhoId = 1L;
            var cliente = CriarCliente(TipoCliente.BRONZE);
            var carrinho = CriarCarrinho(7, 500); // 7kg total, com frete de 2,00 por kg

            _carrinhoServiceMock
                .Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.Is<long>(id => id == carrinhoId), It.Is<Cliente>(c => c == cliente)));

            _clienteServiceMock
                .Setup(x => x.BuscarPorId(It.IsAny<long>()));

            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(carrinhoId, cliente.Id);

            Assert.True(compraDTO.Sucesso);
            Assert.Equal(514, _compraService.CalcularCustoTotal(carrinho)); // 500 + (7 * 2 = 14)
        }

        [Fact]
        public async Task FinalizarCompraAsync_CarrinhoComPesoEntre10E50kgClientePrata_DescontoFreteAplicado()
        {
            var carrinhoId = 1L;
            var cliente = CriarCliente(TipoCliente.PRATA);
            var carrinho = CriarCarrinho(20, 500); // 20kg total, com frete de 4,00 por kg, e 50% de desconto

            _carrinhoServiceMock
                .Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.Is<long>(id => id == carrinhoId), It.Is<Cliente>(c => c == cliente)));

            _clienteServiceMock
                .Setup(x => x.BuscarPorId(It.IsAny<long>()));

            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(carrinhoId, cliente.Id);

            Assert.True(compraDTO.Sucesso);
            Assert.Equal(540, _compraService.CalcularCustoTotal(carrinho)); // 500 + ((20 * 4) * 50% = 40)
        }

        [Fact]
        public async Task FinalizarCompraAsync_CarrinhoComPesoAcimaDe50kgClienteOuro_SemFrete()
        {
            var carrinhoId = 1L;
            var cliente = CriarCliente(TipoCliente.OURO);
            var carrinho = CriarCarrinho(60, 500); // 60kg total, frete grátis para cliente ouro

            _carrinhoServiceMock
                .Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.Is<long>(id => id == carrinhoId), It.Is<Cliente>(c => c == cliente)));

            _clienteServiceMock
                .Setup(x => x.BuscarPorId(It.IsAny<long>()));

            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(carrinhoId, cliente.Id);

            Assert.True(compraDTO.Sucesso);
            Assert.Equal(500, _compraService.CalcularCustoTotal(carrinho)); // Sem frete para cliente Ouro
        }

        [Fact]
        public async Task FinalizarCompraAsync_CarrinhoComValorAcimaDe500Desconto10Aplicado()
        {
            var carrinhoId = 1L;
            var cliente = CriarCliente(TipoCliente.BRONZE);
            var carrinho = CriarCarrinho(3, 600); // Valor do carrinho > 500

            _carrinhoServiceMock
                .Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.Is<long>(id => id == carrinhoId), It.Is<Cliente>(c => c == cliente)));

            _clienteServiceMock
                .Setup(x => x.BuscarPorId(It.IsAny<long>()));

            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(carrinhoId, cliente.Id);

            Assert.True(compraDTO.Sucesso);
            Assert.Equal(540, _compraService.CalcularCustoTotal(carrinho)); // 600 * 10% de desconto
        }

        [Fact]
        public async Task FinalizarCompraAsync_CarrinhoComValorAcimaDe1000Desconto20Aplicado()
        {
            var carrinhoId = 1L;
            var cliente = CriarCliente(TipoCliente.BRONZE);
            var carrinho = CriarCarrinho(3, 1200); // Valor do carrinho > 1000

            _carrinhoServiceMock
                .Setup(x => x.BuscarPorCarrinhoIdEClienteId(It.Is<long>(id => id == carrinhoId), It.Is<Cliente>(c => c == cliente)));

            _clienteServiceMock
                .Setup(x => x.BuscarPorId(It.IsAny<long>()));

            CompraDTO compraDTO = await _compraService.FinalizarCompraAsync(carrinhoId, cliente.Id);

            Assert.True(compraDTO.Sucesso);
            Assert.Equal(960, _compraService.CalcularCustoTotal(carrinho)); // 1200 * 20% de desconto
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
            return new Cliente { Id = 1, Tipo = tipo };
        }
    }
}