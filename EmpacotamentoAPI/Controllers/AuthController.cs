using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmpacotamentoAPI.Models;

namespace EmpacotamentoAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest.Username == "usuario" && loginRequest.Password == "senha123")
            {
                var token = GerarJwtToken(loginRequest.Username);
                return Ok(new LoginResponse { Token = token });
            }
            else
            {
                return Unauthorized("Credenciais inválidas.");
            }
        }

        private string GerarJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("bTxt+/+Gm9ykc2WAwCSKvAbxp9AoOxOAyPnhzp7g7bI="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "empacotamento-api",
                audience: "empacotamento-api-client",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
