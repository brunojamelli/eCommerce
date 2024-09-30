using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Domain.DTO;
using eCommerce.External;
using Moq;

namespace eCommerceTests
{
    public class PagamentoServiceTests
    {
        [Fact(DisplayName = "Pagamento: Processar Com Sucesso")]
        public void ProcessarPagamento_PagamentoComSucesso()
        {
            // Arrange
            var mockPagamentoService = new Mock<IPagamentoExternal>();
            _ = mockPagamentoService
                .Setup(x => x.AutorizarPagamento(It.IsAny<long>(), It.IsAny<double>()))
                .Returns(new PagamentoDTO(true, 123456));

            // Act
            var result = mockPagamentoService.Object.AutorizarPagamento(4111111111111111, 100);

            // Assert
            Assert.True(result.Autorizado);
            Assert.Equal(123456, result.TransacaoId);
        }

        [Fact(DisplayName = "Pagamento: Autorização do pagamento falhou")]
        public void AutorizarPagamento_PagamentoFalhou()
        {
            // Arrange
            var mockPagamentoService = new Mock<IPagamentoExternal>();
            mockPagamentoService
                .Setup(x => x.AutorizarPagamento(It.IsAny<long>(), It.IsAny<double>()))
                .Returns(new PagamentoDTO(false, 12345));

            // Act
            var result = mockPagamentoService.Object.AutorizarPagamento(1234567890123456,100.00);

            // Assert
            mockPagamentoService.Object.CancelarPagamento(12345,1234567890123456);
            Assert.False(result.Autorizado);
        }

        [Fact(DisplayName ="Pagamento: Autorização Lanca Excecao")]
        public void AutorizarPagamento_LancaExcecao()
        {
            // Arrange
            var mockPagamentoService = new Mock<IPagamentoExternal>();
            mockPagamentoService
                .Setup(x => x.AutorizarPagamento(It.IsAny<long>(), It.IsAny<double>()))
                .Throws(new Exception("Erro de conexão com o gateway de pagamento"));

            // Act & Assert
            Assert.Throws<Exception>(() => 
                mockPagamentoService.Object.AutorizarPagamento(4111111111111111, 100.00));
        }


    }
}