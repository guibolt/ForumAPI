using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Comment : Base
    {
        [ForeignKey("Posts")]
        public Guid PostId { get; set; }

        [ForeignKey("Comentarios")]
        public Guid? ComentarioId { get; set; }

        [ForeignKey("Comentarios")]
        public Guid? CitacaoId { get; set; }

        public Usuario Autor { get; set; }
        [ForeignKey("Usuarios")]
        public Guid AutorId { get; set; }

        public double? Nota { get; set; }

        [MaxLength(500)]
        public string Msg { get; set; }

        [NotMapped]
        public List<Comment> Replicas { get; set; }
    }
}
    