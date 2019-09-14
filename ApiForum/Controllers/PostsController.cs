using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model.Views;
using Model.Views.Exibir;
using Model.Views.Receber;

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
        public async Task<IActionResult> Cadastro([FromHeader] string tokenAutor,[FromBody] PostView publicacao)
        {
            var Core = new PostCore(publicacao, _mapper).CadastrarPost(tokenAutor);
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);

        }
        //Chamando o metodo de listar por id da core 
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromHeader]string tokenAutor,string id)
        {
            var Core = new PostCore().ListarPorId(id, tokenAutor);
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }
        //Chamando o metodo de listar todos da core 
        [HttpGet]
        public async Task<IActionResult> ListarTodos([FromHeader]string tokenAutor)
        {
            var Core = new PostCore().ListarTodos(tokenAutor);
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }
        //Chamando o metodo de deletar por id da core 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar([FromHeader] string tokenAutor, string id)
        {
            var Core = new PostCore().DeletarPost(tokenAutor,id);
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }
        //Chamando o metodo de listar todos da core 
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromHeader] string  tokenAutor,[FromBody] PostAtt postAtt, string id)
        {
            var Core = new PostCore(_mapper).EditarPost(id,postAtt, tokenAutor);
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }
        //Chamando o metodo de cadastro da core 
        [HttpPost("Votar")]
        public async Task<IActionResult> VotarComentario([FromHeader] string tokenAutor, [FromBody] VotoPostView Voto)
        {
            var Core = new PostCore(_mapper).VotarPost(tokenAutor, Voto);
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }
    }
}                                                    