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
    public class PostsController : ControllerBase
    {
        // propriedade automapper
        private readonly IMapper _mapper;

        // construtor para a utilização do automapper por meio de injeçao de dependecia
        public PostsController(IMapper mapper) { _mapper = mapper; }

        //Chamando o metodo de cadastro da core 
        [HttpPost]
        public async Task<IActionResult> Cadastro([FromHeader] Guid tokenAutor,[FromBody] PostView publicacao)
        {
            var Core = new PostCore(publicacao, _mapper).CadastrarPost(tokenAutor);
            return Core.Status ? Ok(Core) : BadRequest(Core.Resultado);

        }
        //Chamando o metodo de listar por id da core 
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new PostCore().ListarPorId(id);
            return Core.Status ? Ok(Core) : BadRequest(Core.Resultado);
        }
        //Chamando o metodo de listar todos da core 
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var Core = new PostCore().ListarTodos();
            return Core.Status ? Ok(Core) : BadRequest(Core.Resultado);
        }

        //Chamando o metodo de deletar por id da core 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar([FromHeader] Guid tokenAutor, string id)
        {
            var Core = new PostCore().DeletarPost(tokenAutor,id);
            return Core.Status ? Ok(Core) : BadRequest(Core.Resultado);
        }

        //Chamando o metodo de listar todos da core 
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromHeader] Guid tokenAutor,[FromBody] PostAtt postAtt, string id)
        {
            var Core = new PostCore(_mapper).EditarPost(id,postAtt, tokenAutor);
            return Core.Status ? Ok(Core) : BadRequest(Core.Resultado);
        }

    }
}                                                    