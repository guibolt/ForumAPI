using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
   public class Base
    {
        [JsonIgnore]
        public Guid Id { get;  set; } = Guid.NewGuid();
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        [JsonIgnore]
        public DateTime DataCadastro { get;  set; } = DateTime.Now;
    }
}
