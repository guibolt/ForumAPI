using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model.Views.Exibir;
using Model.Views.Receber;

namespace ApiForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        // propriedade automapper
        private readonly IMapper _mapper;

        // construtor para a utilização do automapper por meio de injeçao de dependecia
        public ComentariosController(IMapper mapper) { _mapper = mapper; }

        //Chamando o metodo de cadastro da core 
        [HttpPost]
        public async Task<IActionResult> Comentar([FromHeader] string tokenAutor,[FromBody] ComentarioView comentario)
        {
            var Core = new ComentarioCore(comentario, _mapper).Comentar(tokenAutor);
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);

        }
        //Chamando o metodo de listar por id da core 
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromHeader]string tokenAutor,string id)
        {
            var Core = new ComentarioCore(_mapper).BuscarComentario(tokenAutor, id);
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }
     
        
        //Chamando o metodo de deletar por id da core 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar([FromHeader] string tokenAutor, string id)
        {
            var Core = new ComentarioCore().DeletarComentario(tokenAutor,id);
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }

        //Chamando o metodo de listar todos da core 
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromHeader] string  tokenAutor,[FromBody] ComentarioAtt comentario, string id)
        {
            var Core = new ComentarioCore(_mapper).EditarComentario(tokenAutor,comentario,id);
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }

        //Chamando o metodo de listar todos da core 
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var Core = new ComentarioCore().TodosComentarios();
            return Core.Status ? Ok(Core) : (IActionResult)BadRequest(Core);
        }

    }
}                                                    