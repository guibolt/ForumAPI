using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Views.Exibir
{
    public class ComentarioView
    {
        public Guid PostId { get; set; }
        public string ComentarioId { get; set; }
        public string CitacaoId { get; set; }
        public string Msg { get; set; }
    }
}
