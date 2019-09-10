using System.ComponentModel.DataAnnotations;
namespace Model
{
    public class Usuario : Base
    {
        [Display(Name = "Nome do Usuário")]
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmaSenha { get; set; }
    }
}
