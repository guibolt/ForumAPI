using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
   public class Base
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime DataCadastro { get; private set; } = DateTime.Now;
    }
}
