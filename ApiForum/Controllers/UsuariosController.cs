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
        //Chamando o metodo de cadastar usurario da core 
        [HttpPost]
        public async Task<IActionResult> Cadastro([FromBody] Usuario usuario)
        {
            var Core = new UsuarioCore(usuario).CadastrarUsuario();
            return Core.Status ? Ok(Core.Msg) : BadRequest(Core.Msg);
         
        }
        //Chamando o metodo de buscar por id usurario da core 
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new UsuarioCore().BuscarUsuario(id);
            return Core.Status ? Ok(Core.Msg) : BadRequest(Core.Msg);
        }

    }
}