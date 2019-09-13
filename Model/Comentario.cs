using Model.Views.Retornar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Comentario : Base
    {
        public Guid PublicacaoId { get; set; }
        public string ComentarioId { get; set; }
        public string CitacaoId { get; set; }
        public string AutorId { get; set; }
        public string Msg { get; set; }
        public List<Comentario> Replicas { get; set; }
    }
}
    