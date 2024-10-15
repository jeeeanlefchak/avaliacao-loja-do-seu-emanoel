using PedidosAPI.Models;

namespace PedidosAPI.Models
{
    public class PedidoProcessado
    {
        public int pedido_id { get; set; }
        public List<Produto> produtos { get; set; }

        public PedidoProcessado(Pedido pedido)
        {
            pedido_id = pedido.pedido_id;
            produtos = pedido.produtos.Select(p => new Produto
            {
                produto_id = p.produto_id,
                dimensoes = new Dimensoes
                {
                    altura = p.dimensoes.altura,
                    largura = p.dimensoes.largura,
                    comprimento = p.dimensoes.comprimento
                }
            }).ToList();
        }
    }
}
