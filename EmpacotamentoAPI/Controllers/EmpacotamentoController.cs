using EmpacotamentoAPI.Interfaces;
using EmpacotamentoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class EmpacotamentoController : ControllerBase
{
    private readonly IEmpacotamentoService _empacotamentoService;

    public EmpacotamentoController(IEmpacotamentoService empacotamentoService)
    {
        _empacotamentoService = empacotamentoService;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("empacotar")]
    public IActionResult Empacotar([FromBody] PedidoRequest request)
    {
        var resultado = _empacotamentoService.Empacotar(request);
        return Ok(resultado);
    }
}
