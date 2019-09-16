using Model.Views.Retornar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    public class Comment : Base
    {
        [ForeignKey("PublicacaoId")]
        public Post Publicacao { get; set; }
        public Guid PublicacaoId { get; set; }
      
        [ForeignKey("ComentarioId")]
        public Comment Comentario { get; set; }
        public Guid? ComentarioId { get; set; }

        [ForeignKey("CitacaoId")]
        public Comment Citacao { get; set; }
        public Guid? CitacaoId { get; set; }

        [ForeignKey("AutorId")]
        public Usuario Autor { get; set; }
        public Guid AutorId { get; set; }

        public double? Nota { get; set; }

        [ MinLength(10),MaxLength(500)]
        public string Msg { get; set; }

        [NotMapped]
        public List<Comment> Replicas { get; set; }
    }
}
    