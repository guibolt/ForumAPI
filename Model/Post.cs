using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Post : Base
    {
      
        public Usuario Autor { get; set; }
        [JsonIgnore]
        [ForeignKey("Usuarios")]
        public Guid AutorID { get; set; }

        [MinLength(8), MaxLength(250)]
        public string Titulo { get; set; }
        [MinLength(50)]
        public string Texto { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public double ? MediaVotos { get; set; }
        [NotMapped]
        public List<Comment> Comentarios { get; set; }
    }
}
