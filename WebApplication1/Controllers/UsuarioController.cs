using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // GET: api/<UsuarioController>
        [HttpGet]
        public IEnumerable<UsuarioModel> Get()
        {
            List<UsuarioModel> usuarioModels = new List<UsuarioModel>();

            usuarioModels.Add(new UsuarioModel() { Id = 1, Nome = "Lala", Email = "lala@gmail.com"});
        
            return usuarioModels;
        
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public UsuarioModel Get(int id)
        {
            UsuarioModel usuario = new UsuarioModel() { Id = 1, Nome = "Lala", Email = "lala@gmail.com" };

            return usuario;
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public void Post([FromBody] UsuarioModel usuario)
        {
            //[FromBody] é um termo que deixa claro que os dados
            //do usuario vão vir do corpo desta requisição

        }

        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UsuarioModel usuario)
        {
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
