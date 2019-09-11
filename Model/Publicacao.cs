using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Publicacao : Base
    {
        public Usuario Autor { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Tipo{ get; set; }
        public bool Finalizada { get; set; }
        public double MediaVotos { get; private set; }
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
