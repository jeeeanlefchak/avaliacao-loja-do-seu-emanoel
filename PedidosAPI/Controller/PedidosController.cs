using Microsoft.AspNetCore.Mvc;
using PedidosAPI.Interfaces;
using PedidosAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IEmpacotamentoService _empacotamentoService;

    public PedidosController(IEmpacotamentoService empacotamentoService)
    {
        _empacotamentoService = empacotamentoService;
    }

    [HttpPost]
    [Route("empacotar")]
    public async Task<IActionResult> EmpacotarPedidos([FromBody] PedidoRequest request)
    {
        var resultado = await _empacotamentoService.EmpacotarPedidos(request);
        return Ok(resultado);
    }
}
