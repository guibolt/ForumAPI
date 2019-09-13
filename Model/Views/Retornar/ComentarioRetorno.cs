using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Views.Retornar
{
    public class ComentarioRetorno
    {
        public Guid Id { get; set; }
        public Guid PublicacaoId { get; set; }
        public string ComentarioId { get; set; }
        public string CitacaoId { get; set; }
        public string Msg { get; set; }
    }
}
