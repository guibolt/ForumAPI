using Microsoft.EntityFrameworkCore;
using Model.Views.Receber;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ForumContext : DbContext
    {
        public ForumContext(DbContextOptions options) : base(options ) {}
        public DbSet<Post> Posts { get; set; } 
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Comment> Comentarios { get; set; }
 
    }
}
