
using PedidosAPI.Models;

namespace PedidosAPI.Interfaces
{
    public interface IEmpacotamentoService
    {
        Task<ResultadoEmpacotamento> EmpacotarPedidos(PedidoRequest pedidoRequest);
    }
}
