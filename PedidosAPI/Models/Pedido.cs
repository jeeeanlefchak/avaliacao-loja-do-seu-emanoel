namespace PedidosAPI.Models
{
    public class Pedido
    {
        public int pedido_id { get; set; }
        public List<Produto> produtos { get; set; }
    }
}
