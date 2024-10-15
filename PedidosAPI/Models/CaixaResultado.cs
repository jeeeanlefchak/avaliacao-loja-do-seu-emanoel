namespace PedidosAPI.Models
{
    public class CaixaResultado
    {
        public string caixa_id { get; set; }
        public List<string> produtos { get; set; }
        public string observacao { get; set; }
    }
}
