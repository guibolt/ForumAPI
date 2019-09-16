﻿using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        // propriedade automapper
        private readonly IMapper _mapper;
        //Construtor contendo o contexto.
        private ForumContext _contexto { get; set; }
    
        // construtor para a utilização do automapper por meio de injeçao de dependecia
        public UsuariosController(IMapper mapper, ForumContext contexto)
        {  _mapper  = mapper;
           _contexto = contexto;
        }

        //Chamando o metodo de cadastar usurario da core 
        [HttpPost]
        public async Task<IActionResult> Cadastro([FromBody] Usuario Usuario)
        {
            var Core = new UsuarioCore(Usuario, _contexto).CadastrarUsuario();
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }
        //Chamando o metodo de logar usurario da core 
        [HttpPost("Autenticar")]
        public async Task<IActionResult> Logar([FromBody] Usuario usuario)
        {
            var Core = new UsuarioCore(usuario, _contexto).LogarUsuario();
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }

        //Chamando o metodo de listar todos da core 
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var Core = new UsuarioCore(_contexto).Listar();
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }
    }
}