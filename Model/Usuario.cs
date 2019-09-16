using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
   public class Usuario: Base
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmaSenha { get; set; }
    }
}
