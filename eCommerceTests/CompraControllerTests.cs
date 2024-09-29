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
        private readonly Mock<ICompraService> _compraServiceMock;
        private readonly CompraController _controller;
        public CompraControllerTests()
        {
            // Configurando o mock do serviço de compra
            _compraServiceMock = new Mock<ICompraService>();

            // Criando a instância do controller, injetando o serviço de compra mockado
            _controller = new CompraController(_compraServiceMock.Object);
        }
        [Fact(DisplayName = "Controller: Finalizar compra deve retornar Ok quando compra bem sucedida")]
        public async Task FinalizarCompra_DeveRetornarOk_QuandoCompraBemSucedida()
        {
            // Arrange: Configurando o mock para retornar um objeto CompraDTO com sucesso
            var compraEsperada = new CompraDTO(true, 12345, "Compra realizada com sucesso.");
            _compraServiceMock.Setup(x => x.FinalizarCompraAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(compraEsperada);

            // Act: Chamando o método do controller
            var result = await _controller.FinalizarCompra(1, 1);

            // Assert: Verifica se o retorno foi Ok com o objeto correto
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var compraRetornada = Assert.IsType<CompraDTO>(okResult.Value);
            Assert.True(compraRetornada.Sucesso);
            Assert.Equal(12345, compraRetornada.TransacaoPagamentoId);
            Assert.Equal("Compra realizada com sucesso.", compraRetornada.Mensagem);
        }

        [Fact(DisplayName = "Controller: Finalizar compra deve retornar BadRequest quando tem argumento invalido")]
        public async Task FinalizarCompra_DeveRetornarBadRequest_QuandoArgumentoInvalido()
        {
            // Arrange
            var carrinhoId = 1;
            var clienteId = 1;

            // Simula o serviço lançando uma exceção de ArgumentException
            _compraServiceMock.Setup(service => service.FinalizarCompraAsync(carrinhoId, clienteId))
                            .ThrowsAsync(new ArgumentException("Argumento inválido"));

            // Act
            var result = await _controller.FinalizarCompra(carrinhoId, clienteId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CompraDTO>>(result); // Confirma que é do tipo ActionResult<CompraDTO>
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result); // Acessa o 'Result' e verifica se é BadRequestObjectResult
            Assert.Equal(400, badRequestResult.StatusCode); // Verifica o status code
        }

        [Fact(DisplayName = "Controller: Finalizar compra deve retornar Conflict quando tem erro de estado")]
        public async Task FinalizarCompra_DeveRetornarConflict_QuandoErroDeEstado()
        {
            // Arrange
            var carrinhoId = 1;
            var clienteId = 1;
            
            // Simula o serviço lançando uma exceção de InvalidOperationException
            _compraServiceMock.Setup(service => service.FinalizarCompraAsync(carrinhoId, clienteId))
                            .ThrowsAsync(new InvalidOperationException("Erro de estado"));
            // Act: Chama o endpoint
            var result = await _controller.FinalizarCompra(carrinhoId, clienteId);

            // Assert: Verifica se o retorno é Conflict (409) com a mensagem correta
            var actionResult = Assert.IsType<ActionResult<CompraDTO>>(result); // Confirma que é do tipo ActionResult<CompraDTO>
            var conflictResult = Assert.IsType<ConflictObjectResult>(actionResult.Result); // Acessa o 'Result' e verifica se é ConflictObjectResult
            Assert.Equal(409, conflictResult.StatusCode); // Verifica o status code
        }

        [Fact(DisplayName = "Controller: Finalizar compra deve retornar InternalServerError quando tem erro desconhecido")]
        public async Task FinalizarCompra_DeveRetornarInternalServerError_QuandoErroDesconhecido()
        {
            // Arrange: Simula um erro inesperado (genérico)
            _compraServiceMock
                .Setup(x => x.FinalizarCompraAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ThrowsAsync(new Exception());

            // Act: Chama o endpoint
            var result = await _controller.FinalizarCompra(1, 1);

            // Assert: Verifica se o retorno é InternalServerError (500)

            // Assert
            var actionResult = Assert.IsType<ActionResult<CompraDTO>>(result); // Confirma que é do tipo ActionResult<CompraDTO>
            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);

            // var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            var returnedDTO = Assert.IsType<CompraDTO>(objectResult.Value);
            Assert.False(returnedDTO.Sucesso);
            Assert.Equal("Erro ao processar compra.", returnedDTO.Mensagem);
        }

    }
}