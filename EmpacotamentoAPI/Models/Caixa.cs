namespace EmpacotamentoAPI.Models
{
    public class Caixa
    {
        public string Id { get; set; }
        public int Altura { get; set; }
        public int Largura { get; set; }
        public int Comprimento { get; set; }
        public int? Capacidade { get; set; }

        public double GetVolumeCaixa()
        {
            return Altura * Largura * Comprimento;
        }
    }
}
