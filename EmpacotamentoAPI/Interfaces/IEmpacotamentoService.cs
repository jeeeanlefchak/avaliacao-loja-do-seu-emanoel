
using EmpacotamentoAPI.Models;

namespace EmpacotamentoAPI.Interfaces
{
    public interface IEmpacotamentoService
    {
        ResultadoEmpacotamento Empacotar(PedidoRequest pedidos);
    }
}
