namespace EmpacotamentoAPI.Models
{
    public class CaixaResultado
    {
        public string caixa_id { get; set; }
        public List<string> produtos { get; set; }
        public string observacao { get; set; }

        public record CaixaResultadoRecord(string caixa_id, List<string> produtos, string observacao);
        
    }
}
