using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Domain.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace eCommerce.Tests
{
    public class CompraControllerIntegrationTests
    {
        // private readonly HttpClient _client;
        // internal CompraControllerIntegrationTests(WebApplicationFactory<Program> factory)
        // {
        //     _client = factory.CreateClient();
        // }

        // [Fact(DisplayName = "FinalizarCompra - Deve retornar sucesso quando compra é bem-sucedida")]
        // public async Task FinalizarCompra_DeveRetornarSucesso_QuandoCompraBemSucedida()
        // {
        //     // Arrange
        //     var compraDTO = new CompraDTO (true, 123,"Compra finalizada com sucesso.");

        //     // Serializando o objeto para JSON
        //     var content = new StringContent(JsonConvert.SerializeObject(compraDTO), Encoding.UTF8, "application/json");

        //     // Act
        //     var response = await _client.PostAsync("/api/compra/finalizar", content); // Substituir com o caminho correto do endpoint

        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //     var responseContent = await response.Content.ReadAsStringAsync();
        //     var result = JsonConvert.DeserializeObject<CompraDTO>(responseContent);

        //     Assert.NotNull(result);
        //     Assert.True(result.Sucesso);
        //     Assert.Equal(123, result.TransacaoPagamentoId);
        //     Assert.Equal("Compra finalizada com sucesso.", result.Mensagem);
        // }

        // [Fact(DisplayName = "FinalizarCompra - Deve retornar erro interno quando há falha no pagamento")]
        // public async Task FinalizarCompra_DeveRetornarErroInterno_QuandoFalhaNoPagamento()
        // {
        //     // Arrange
        //     var compraDTO = new CompraDTO (false, null, "Erro ao processar pagamento.");

        //     var content = new StringContent(JsonConvert.SerializeObject(compraDTO), Encoding.UTF8, "application/json");

        //     // Act
        //     var response = await _client.PostAsync("/api/compra/finalizar", content);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        //     var responseContent = await response.Content.ReadAsStringAsync();
        //     var result = JsonConvert.DeserializeObject<CompraDTO>(responseContent);

        //     Assert.False(result.Sucesso);
        //     Assert.Null(result.TransacaoPagamentoId);
        //     Assert.Equal("Erro ao processar pagamento.", result.Mensagem);
        // }
    }
}