using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ecommerce.Dto;
using ecommerce.Entity;
using ecommerce.External;
using ecommerce.Services;
using Microsoft.EntityFrameworkCore;
using eCommerce.Domain.DTO;
using eCommerce.Domain.Entity;

namespace ecommerce.Services
{
    public class CompraService
    {
        private readonly CarrinhoDeComprasService _carrinhoService;
        private readonly ClienteService _clienteService;
        private readonly IEstoqueExternal _estoqueExternal;
        private readonly IPagamentoExternal _pagamentoExternal;

        public CompraService(CarrinhoDeComprasService carrinhoService, 
                             ClienteService clienteService,
                             IEstoqueExternal estoqueExternal, 
                             IPagamentoExternal pagamentoExternal)
        {
            _carrinhoService = carrinhoService;
            _clienteService = clienteService;
            _estoqueExternal = estoqueExternal;
            _pagamentoExternal = pagamentoExternal;
        }

        public async Task<CompraDTO> FinalizarCompraAsync(long carrinhoId, long clienteId)
        {
            var cliente = await _clienteService.BuscarPorIdAsync(clienteId);
            var carrinho = await _carrinhoService.BuscarPorCarrinhoIdEClienteIdAsync(carrinhoId, cliente);

            var produtosIds = carrinho.Itens.Select(i => i.Produto.Id).ToList();
            var produtosQtds = carrinho.Itens.Select(i => i.Quantidade).ToList();

            var disponibilidade = await _estoqueExternal.VerificarDisponibilidadeAsync(produtosIds, produtosQtds);

            if (!disponibilidade.Disponivel)
            {
                throw new InvalidOperationException("Itens fora de estoque.");
            }

            decimal custoTotal = CalcularCustoTotal(carrinho);

            var pagamento = await _pagamentoExternal.AutorizarPagamentoAsync(cliente.Id, custoTotal);

            if (!pagamento.Autorizado)
            {
                throw new InvalidOperationException("Pagamento não autorizado.");
            }

            var baixaDTO = await _estoqueExternal.DarBaixaAsync(produtosIds, produtosQtds);

            if (!baixaDTO.Sucesso)
            {
                await _pagamentoExternal.CancelarPagamentoAsync(cliente.Id, pagamento.TransacaoId);
                throw new InvalidOperationException("Erro ao dar baixa no estoque.");
            }

            var compraDTO = new CompraDTO
            {
                Sucesso = true,
                TransacaoId = pagamento.TransacaoId,
                Mensagem = "Compra finalizada com sucesso."
            };

            return compraDTO;
        }

        public decimal CalcularCustoTotal(CarrinhoDeCompras carrinho)
        {
            // Implementação do cálculo do custo total
            return carrinho.Itens.Sum(i => i.Quantidade * i.Produto.Preco);
        }
    }
}
