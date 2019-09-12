using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Views;
using Model.Views.Exibir;

namespace ApiForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        // propriedade automapper
        private readonly IMapper _mapper;

        // construtor para a utilização do automapper por meio de injeçao de dependecia
        public UsuariosController(IMapper mapper) {  _mapper = mapper; }

        //Chamando o metodo de cadastar usurario da core 
        [HttpPost]
        public async Task<IActionResult> Cadastro([FromBody] UsuarioView  Usuario)
        {
            var Core = new UsuarioCore(Usuario,_mapper).CadastrarUsuario();
            return Core.Status ? Ok(Core) : BadRequest(Core.Resultado);
         
        }

        //Chamando o metodo de logar usurario da core 
        [HttpPost("Autenticar")]
        public async Task<IActionResult> Logar([FromBody] LoginUserView usuario)
        {
            var Core = new UsuarioCore(usuario,_mapper).LogarUsuario();
            return Core.Status ? Ok(Core) : BadRequest(Core.Resultado);

        }

  
        //Chamando o metodo de listar todos da core 
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var Core = new UsuarioCore().Listar();
            return Core.Status ? Ok(Core) : BadRequest(Core.Resultado);
        }
    }
}