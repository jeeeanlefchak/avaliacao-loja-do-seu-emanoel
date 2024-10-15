namespace EmpacotamentoAPI.Models
{
    public class Produto
    {
        public string produto_id { get; set; }
        public Dimensoes dimensoes { get; set; }

        public double GetProdutoVolume()
        {
            return dimensoes.altura * dimensoes.largura * dimensoes.comprimento;
        }

        public bool CabeNaCaixa(Caixa caixa)
        {
            return (dimensoes.altura <= caixa.Altura && dimensoes.largura <= caixa.Largura && dimensoes.comprimento <= caixa.Comprimento) ||
                        (dimensoes.altura <= caixa.Altura && dimensoes.comprimento <= caixa.Largura && dimensoes.largura <= caixa.Comprimento) ||
                        (dimensoes.largura <= caixa.Altura && dimensoes.altura <= caixa.Largura && dimensoes.comprimento <= caixa.Comprimento) ||
                        (dimensoes.largura <= caixa.Altura && dimensoes.comprimento <= caixa.Largura && dimensoes.altura <= caixa.Comprimento) ||
                        (dimensoes.comprimento <= caixa.Altura && dimensoes.altura <= caixa.Largura && dimensoes.largura <= caixa.Comprimento) ||
                        (dimensoes.comprimento <= caixa.Altura && dimensoes.largura <= caixa.Largura && dimensoes.altura <= caixa.Comprimento);
        }

    }
}
