﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Voto
    {
        public Guid PostId { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid ComentarioId { get; set; }
        public string Nota { get; set; }
    }
}
