using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Comentario : Base
    {
        public string Texto { get; set; }
        public Guid PublicacaoId { get; set; }
        public Guid AutorId { get; set; }
        public Guid CitacaoId { get; set; }
        public List<Comentario> Replicas { get; set; }
    }
}
