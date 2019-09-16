using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Base
    {
        [Key]
        public Guid Id { get;  set; } = Guid.NewGuid();
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        [JsonIgnore]
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
