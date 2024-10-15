using static EmpacotamentoAPI.Models.CaixaResultado;

namespace EmpacotamentoAPI.Models
{
    public class ResultadoPedido
    {
        public int pedido_id { get; set; }
        public List<CaixaResultadoRecord> caixas { get; set; }
    }
}
