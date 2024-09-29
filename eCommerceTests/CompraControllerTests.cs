using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.Services;
using eCommerce.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace eCommerce.Services
{
    public class CompraControllerTests
    {
        private readonly Mock<CompraService> _compraServiceMock;
        private readonly CompraController _controller;
        public CompraControllerTests()
        {
            // Configurando o mock do serviço de compra
            _compraServiceMock = new Mock<CompraService>();

            // Criando a instância do controller, injetando o serviço de compra mockado
            _controller = new CompraController(_compraServiceMock.Object);
        }
        [Fact]
        public async Task FinalizarCompra_DeveRetornarOk_QuandoCompraBemSucedida()
        {
            // Arrange: Configura o comportamento esperado do serviço de compra
            var compraDTO = new CompraDTO(true, 12345, "Compra finalizada com sucesso.");
            _compraServiceMock
                .Setup(x => x.FinalizarCompraAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(compraDTO);

            // Act: Chama o endpoint com parâmetros válidos
            var result = await _controller.FinalizarCompra(1, 1);

            // Assert: Verifica se o retorno é Ok (200) e contém o DTO esperado
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDTO = Assert.IsType<CompraDTO>(okResult.Value);
            Assert.True(returnedDTO.Sucesso);
        }

        [Fact]
        public async Task FinalizarCompra_DeveRetornarBadRequest_QuandoArgumentoInvalido()
        {
            // Arrange: Simula que o serviço de compra lança uma exceção de argumento inválido
            _compraServiceMock
                .Setup(x => x.FinalizarCompraAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ThrowsAsync(new ArgumentException("Carrinho inválido"));

            // Act: Chama o endpoint
            var result = await _controller.FinalizarCompra(1, 1);

            // Assert: Verifica se o retorno é BadRequest (400) com a mensagem correta
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedDTO = Assert.IsType<CompraDTO>(badRequestResult.Value);
            Assert.False(returnedDTO.Sucesso);
            Assert.Equal("Carrinho inválido", returnedDTO.Mensagem);
        }

        [Fact]
        public async Task FinalizarCompra_DeveRetornarConflict_QuandoErroDeEstado()
        {
            // Arrange: Simula que o serviço de compra lança uma exceção de estado inválido
            _compraServiceMock
                .Setup(x => x.FinalizarCompraAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ThrowsAsync(new InvalidOperationException("Itens fora de estoque"));

            // Act: Chama o endpoint
            var result = await _controller.FinalizarCompra(1, 1);

            // Assert: Verifica se o retorno é Conflict (409) com a mensagem correta
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var returnedDTO = Assert.IsType<CompraDTO>(conflictResult.Value);
            Assert.False(returnedDTO.Sucesso);
            Assert.Equal("Itens fora de estoque", returnedDTO.Mensagem);
        }

        [Fact]
        public async Task FinalizarCompra_DeveRetornarInternalServerError_QuandoErroDesconhecido()
        {
            // Arrange: Simula um erro inesperado (genérico)
            _compraServiceMock
                .Setup(x => x.FinalizarCompraAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ThrowsAsync(new Exception());

            // Act: Chama o endpoint
            var result = await _controller.FinalizarCompra(1, 1);

            // Assert: Verifica se o retorno é InternalServerError (500)
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
            var returnedDTO = Assert.IsType<CompraDTO>(serverErrorResult.Value);
            Assert.False(returnedDTO.Sucesso);
            Assert.Equal("Erro ao processar compra.", returnedDTO.Mensagem);
        }

    }
}