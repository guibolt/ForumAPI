using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Armazenamento
    {
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public List<Voto> Votos { get; set; } = new List<Voto>();
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();

    }
}
