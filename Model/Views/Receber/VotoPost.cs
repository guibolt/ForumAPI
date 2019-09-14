using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Views.Receber
{
   public class VotoPost
    {
        public Guid PostId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Nota { get; set; }
    }
}
