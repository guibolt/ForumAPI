using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Cadastro([FromBody] Usuario usuario)
        {
            var Core = new UsuarioCore(usuario).CadastrarUsuario();
            return Core.Item1 ? Ok(Core.Item2) : BadRequest(Core.Item2);
         
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new UsuarioCore().BuscarUsuario(id);
            return Core.Item1 ? Ok(Core.Item2) : BadRequest(Core.Item2);
        }

    }
}