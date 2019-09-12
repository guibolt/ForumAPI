using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Views.Exibir
{
    public class ComentarioView
    {
        public Guid PublicacaoId { get; set; }
        public Guid ComentarioId { get; set; }
        public Guid CitacaoId { get; set; }
        public string Msg { get; set; }
    }
}
