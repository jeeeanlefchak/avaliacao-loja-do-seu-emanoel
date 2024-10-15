using EmpacotamentoAPI.Interfaces;
using EmpacotamentoAPI.Models;
using static EmpacotamentoAPI.Models.CaixaResultado;

namespace EmpacotamentoAPI.Service
{
    public class EmpacotamentoService : IEmpacotamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly List<Caixa> _caixasDisponiveis = new List<Caixa>
        {
            new Caixa { Id = "Caixa 1", Altura = 30, Largura = 40, Comprimento = 80 },
            new Caixa { Id = "Caixa 2", Altura = 80, Largura = 50, Comprimento = 40 },
            new Caixa { Id = "Caixa 3", Altura = 50, Largura = 80, Comprimento = 60 }
        };

        public EmpacotamentoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public ResultadoEmpacotamento Empacotar(PedidoRequest pedidoRequest)
        {
            var resultados = new ResultadoEmpacotamento
            {
                pedidos = new List<ResultadoPedido>()
            };

            foreach (var pedido in pedidoRequest.pedidos)
            {
                var resultadoPedido = CriarResultadoPedido(pedido, pedidoRequest);
                resultados.pedidos.Add(resultadoPedido);
            }

            return resultados;
        }

        private ResultadoPedido CriarResultadoPedido(Pedido pedido, PedidoRequest pedidoRequest)
        {
            var resultadoPedido = new ResultadoPedido
            {
                pedido_id = pedido.pedido_id,
                caixas = new List<CaixaResultadoRecord>()
            };

            var produtosPorCaixa = new Dictionary<string, List<string>>();

            foreach (var produto in pedido.produtos)
            {
                var caixa = SelecionarCaixa(produto, produtosPorCaixa, pedidoRequest);
                if (caixa != null)
                {
                    AdicionarProdutoNaCaixa(produto, caixa, produtosPorCaixa);
                }
                else
                {
                    AdicionarProdutoNaoEmpacotado(resultadoPedido, produto);
                }
            }

            AdicionarCaixasComProdutos(resultadoPedido, produtosPorCaixa, pedidoRequest);
            return resultadoPedido;
        }

        private void AdicionarProdutoNaCaixa(Produto produto, Caixa caixa, Dictionary<string, List<string>> produtosPorCaixa)
        {
            if (!produtosPorCaixa.ContainsKey(caixa.Id))
            {
                produtosPorCaixa[caixa.Id] = new List<string>();
            }
            produtosPorCaixa[caixa.Id].Add(produto.produto_id);
        }

        private void AdicionarProdutoNaoEmpacotado(ResultadoPedido resultadoPedido, Produto produto)
        {
            var caixaResultado = new CaixaResultadoRecord(
                null,
                new List<string> { produto.produto_id },
                "Produto não cabe em nenhuma caixa disponível."
            );
            resultadoPedido.caixas.Add(caixaResultado);
        }

        private void AdicionarCaixasComProdutos(ResultadoPedido resultadoPedido, Dictionary<string, List<string>> produtosPorCaixa, PedidoRequest pedidoRequest)
        {
            foreach (var caixaId in produtosPorCaixa.Keys)
            {
                var caixaResultado = new CaixaResultadoRecord(caixaId, produtosPorCaixa[caixaId], observacao: null);
                resultadoPedido.caixas.Add(caixaResultado);
            }
        }

        private Caixa SelecionarCaixa(Produto produto, Dictionary<string, List<string>> produtosPorCaixa, PedidoRequest pedidoRequest)
        {
            foreach (var caixa in _caixasDisponiveis)
            {
                var volumeAtual = CalcularVolumeAtual(caixa, produtosPorCaixa, pedidoRequest);
                var volumeProduto = produto.GetProdutoVolume();

                if (volumeAtual + volumeProduto <= caixa.GetVolumeCaixa() && produto.CabeNaCaixa(caixa))
                {
                    double alturaTotal = volumeAtual == 0 ? produto.dimensoes.altura : GetAlturaTotal(produtosPorCaixa[caixa.Id], pedidoRequest) + produto.dimensoes.altura;

                    if (alturaTotal <= caixa.Altura || produto.dimensoes.altura <= caixa.Altura)
                    {
                        return caixa;
                    }
                }
            }
            return null;
        }

        private double CalcularVolumeAtual(Caixa caixa, Dictionary<string, List<string>> produtosPorCaixa, PedidoRequest pedidoRequest)
        {
            return produtosPorCaixa.ContainsKey(caixa.Id)
                ? produtosPorCaixa[caixa.Id].Sum(produtoId => pedidoRequest.pedidos
                    .SelectMany(p => p.produtos)
                    .FirstOrDefault(p => p.produto_id == produtoId)?.GetProdutoVolume() ?? 0)
                : 0;
        }

        private double GetAlturaTotal(List<string> produtosIds, PedidoRequest pedidoRequest)
        {
            return produtosIds.Sum(produtoId =>
            {
                var produto = pedidoRequest.pedidos.SelectMany(p => p.produtos).FirstOrDefault(p => p.produto_id == produtoId);
                return produto?.dimensoes.altura ?? 0;
            });
        }
    }
}
