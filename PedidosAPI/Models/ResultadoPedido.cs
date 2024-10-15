
namespace PedidosAPI.Models
{
    public class ResultadoPedido
    {
        public int pedido_id { get; set; }
        public List<CaixaResultado> caixas { get; set; }
    }
}
