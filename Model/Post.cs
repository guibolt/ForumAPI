using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Post : Base
    {
        public Usuario Autor { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string Tipo{ get; set; }
        public string Aberta { get; set; } 
        public string MediaVotos { get; private set; }
        public List<Comentario> Comentarios { get; set; } 
    }
}
