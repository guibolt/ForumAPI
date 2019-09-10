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
        public string Conteudo { get; set; }
        public string TipoTopico { get; set; }

    }
}
