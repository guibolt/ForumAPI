using System.ComponentModel.DataAnnotations;
namespace Model
{
    public class Usuario : Base
    {
        [Display(Name = "Nome do usuário")]
        public string Nome { get; set; }
        public string Email { get; set; }
        [Display(Name = "Senha do usuário")]
        public string Senha { get; set; }
        public string ConfirmaSenha { get; set; }
        

    }
}
