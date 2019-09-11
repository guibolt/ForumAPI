using Core.Interface;
using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Linq;

namespace Core
{
    public class UsuarioCore : AbstractValidator<Usuario>
    {
        private Usuario _usuario;
        public Armazenamento _arm { get; set; }

        public UsuarioCore()
        {
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();

        }

        public UsuarioCore(Usuario Usuario)
        {
           _usuario = Usuario;
            RuleFor(u => u.Nome).NotNull().MinimumLength(3).WithMessage("O Nome deve ter no minimo 3 letras!");
            RuleFor(u => u.Email).EmailAddress().NotNull().WithMessage("Email inválido.");
            RuleFor(u => u.Senha).NotNull().Length(8, 12).WithMessage("A senha deve ser entre 8 e 12 caracteres");
            RuleFor(u => u.ConfirmaSenha).Equal(_usuario.Senha).WithMessage("As senhas devem ser iguais!");
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();

        }

        //Método para cadastro de usuario
        public Retorno CadastrarUsuario()
        {
            var valida = Validate(_usuario);

            if (!valida.IsValid)
                return new Retorno { Status = false, Msg = valida.Errors.Select(a => a.ErrorMessage).ToList() };

            _arm.Usuarios.Add(_usuario);

            Arquivo.Salvar(_arm);
            return new Retorno { Status = true, Msg = "Usuário cadastrado com sucesso!" };

        }

        //Método para logar o usuario na plataforma.
        public Retorno LogarUsuario()
        {
            return null;
        }

        //Método para buscar um usuario se baseando no id fornecido.
        public Retorno BuscarUsuario(string id)
        {
            if (!Guid.TryParse(id, out Guid ident))
                return new Retorno { Status = false, Msg = "Id incorreto!" };

            var umUsuario = _arm.Usuarios.FirstOrDefault(u => u.Id == ident);

            if (umUsuario == null) return new Retorno { Status = false, Msg = "Esse usuario nao existe na base de dados" };

            return new Retorno { Status = true, Msg = umUsuario };

        }
    }
}
