using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDeTarefas.Integracao.Interfaces;
using SistemaDeTarefas.Integracao.Response;

namespace SistemaDeTarefas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CepController : ControllerBase
    {
        private readonly IViaCepIntegracao _viaCepIntegracao;
        public CepController(IViaCepIntegracao viaCepIntegracao) 
        {
             _viaCepIntegracao = viaCepIntegracao;
        }
         
        [HttpGet]
        public async Task<ActionResult<ViaCepResponse>> ListarDadosEndereco(string cep)
        {
            var responseData = await _viaCepIntegracao.ObterDadosViaCep(cep);
            if (responseData == null)
            {
                return BadRequest("Cep não encontrado");
            }
            return Ok(responseData);
        }
    }
}
