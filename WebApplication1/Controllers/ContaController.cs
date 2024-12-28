using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SistemaDeTarefas.Models;

namespace SistemaDeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if(login.Login == "admin" && login.Senha == "admin")
            {
                var token = GerarTokenJWT();
                return Ok(new { token });
            }

            return BadRequest(new { mensagem = "Credenciais inválidas. Verifique seu nome de usuário e senha"});
        }

        private string GerarTokenJWT()
        {
            //estrutura padrão Jwt: headers;payload;Assinatura
            //este token deve somente conter informações n sigilosas

            string chaveSecreta = "e029127a-ca2d-4f13-9509-1a99fe699751";

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));
            var credencial = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            //ex de lista de claims:
            var claims = new[]
            {
                new Claim("login", "admin"),
                new Claim("nome", "Adm do sistema")
            };

            var token = new JwtSecurityToken(
                issuer: "sua_empresa" ,
                audience: "sua_aplicacao",
                claims: claims, //lista de claims
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credencial
            );

            return  new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
