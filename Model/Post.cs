using Model.Views.Retornar;
using System.Collections.Generic;

namespace Model
{
    public class Post : Base
    {
        public UsuarioRetorno Autor { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string Tipo{ get; set; }
        public string Status { get; set; } 
        public string MediaVotos { get;  set; }
        public List<Comentario> Comentarios { get; set; } 
    }
}
