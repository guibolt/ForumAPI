using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
   public class Base
    {
        public Guid Id { get;  set; } = Guid.NewGuid();
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime DataCadastro { get;   set; } = DateTime.Now;
    }
}
