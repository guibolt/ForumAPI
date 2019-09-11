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
    public class PublicacoesController : ControllerBase
    {
        //Chamando o metodo de cadastro da core 
        [HttpPost]
        public async Task<IActionResult> Cadastro([FromBody] Publicacao publicacao)
        {
            var Core = new PublicacaoCore(publicacao).Cadastrar();
            return Core.Status ? Ok(Core.Msg) : BadRequest(Core.Msg);
         
        }
        //Chamando o metodo de listar por id da core 
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new PublicacaoCore().ListarPorId(id);
            return Core.Status ? Ok(Core.Msg) : BadRequest(Core.Msg);
        }
        //Chamando o metodo de listar todos da core 
        [HttpGet]
        public async Task<IActionResult> ListarTodos(string id)
        {
            var Core = new PublicacaoCore().ListarTodos();
            return Core.Status ? Ok(Core.Msg) : BadRequest(Core.Msg);
        }

        //Chamando o metodo de deletar por id da core 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(string id)
        {
            var Core = new PublicacaoCore().Deletar(id);
            return Core.Status ? Ok(Core.Msg) : BadRequest(Core.Msg);
        }
    }
}