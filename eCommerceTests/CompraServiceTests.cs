using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.Services;
using eCommerce.Domain.DTO;
using eCommerce.Domain.Entity;
using eCommerce.External;
using eCommerce.Services;
using Ecommerce.Entity;
using Moq;

namespace eCommerce.Tests
{
    public class CompraServiceTests
    {
        private readonly Mock<CarrinhoDeComprasService> _carrinhoServiceMock;
        private readonly Mock<ClienteService> _clienteServiceMock;
        private readonly Mock<IEstoqueExternal> _estoqueExternalMock;
        private readonly Mock<IPagamentoExternal> _pagamentoExternalMock;
        private readonly CompraService _compraService;
        public CompraServiceTests()
        {
            _carrinhoServiceMock = new Mock<CarrinhoDeComprasService>();
            _clienteServiceMock = new Mock<ClienteService>();
            _estoqueExternalMock = new Mock<IEstoqueExternal>();
            _pagamentoExternalMock = new Mock<IPagamentoExternal>();

            _compraService = new CompraService(
                _carrinhoServiceMock.Object,
                _clienteServiceMock.Object,
                _estoqueExternalMock.Object,
                _pagamentoExternalMock.Object
            );
        }

        [Fact]
        public async Task FinalizarCompra_Sucesso()
        {
            // Arrange
            var clienteId = 1L;
            var carrinhoId = 1L;
            var cliente = new Cliente { Id = clienteId };
            var carrinho = new CarrinhoDeCompras
            {
                Id = carrinhoId,
                Cliente = cliente,
                Itens = new List<ItemCompra>
                {
                    new ItemCompra { Produto = new Produto { Id = 1, Preco = 10.0m }, Quantidade = 2 }
                }
            };

            var disponibilidade = new DisponibilidadeDTO(true, new List<long>());
            var pagamento = new PagamentoDTO(true, 12345);

            _clienteServiceMock.Setup(x => x.BuscarPorId(clienteId)).Returns(cliente);
            _carrinhoServiceMock.Setup(x => x.BuscarPorCarrinhoIdEClienteId(carrinhoId, cliente)).Returns(carrinho);
            _estoqueExternalMock.Setup(x => x.VerificarDisponibilidade(It.IsAny<List<long>>(), It.IsAny<List<long>>())).Returns(disponibilidade);
            _pagamentoExternalMock.Setup(x => x.AutorizarPagamento(clienteId, It.IsAny<double>())).Returns(pagamento);
            _estoqueExternalMock.Setup(x => x.DarBaixa(It.IsAny<List<long>>(), It.IsAny<List<long>>())).Returns(new EstoqueBaixaDTO(true));

            // Act
            var result = await _compraService.FinalizarCompraAsync(carrinhoId, clienteId);

            // Assert
            Assert.True(result.Sucesso);
            Assert.Equal("Compra finalizada com sucesso.", result.Mensagem);
        }

        [Fact]
        public async Task FinalizarCompra_FalhaEstoque()
        {
            // Arrange
            var clienteId = 1L;
            var carrinhoId = 1L;
            var cliente = new Cliente { Id = clienteId };
            var carrinho = new CarrinhoDeCompras
            {
                Id = carrinhoId,
                Cliente = cliente,
                Itens = new List<ItemCompra>
                {
                    new ItemCompra { Produto = new Produto { Id = 1, Preco = 10.0m }, Quantidade = 2 }
                }
            };

            var disponibilidade = new DisponibilidadeDTO(false, new List<long> { 1 });

            _clienteServiceMock.Setup(x => x.BuscarPorId(clienteId)).Returns(cliente);
            _carrinhoServiceMock.Setup(x => x.BuscarPorCarrinhoIdEClienteId(carrinhoId, cliente)).Returns(carrinho);
            _estoqueExternalMock.Setup(x => x.VerificarDisponibilidade(It.IsAny<List<long>>(), It.IsAny<List<long>>())).Returns(disponibilidade);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _compraService.FinalizarCompraAsync(carrinhoId, clienteId));
        }
       
    }
}