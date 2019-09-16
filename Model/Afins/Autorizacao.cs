using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Afins
{
    public static class Autorizacao
    {
        public static bool ValidarUsuario(string Usertoken, ForumContext forumContext)
        {

            if (!Guid.TryParse(Usertoken, out Guid token) || !forumContext.Usuarios.Any(e => e.Id == token))
                return false;

            return true;
        }
    }
}
