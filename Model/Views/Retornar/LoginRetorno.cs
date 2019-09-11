using System;

namespace Model.Views
{
    public class LoginRetorno
    {
        public bool Status { get; set; }
        public Guid TokenUsuario { get; set; }
        public string Nome { get; set; }
    }
}
