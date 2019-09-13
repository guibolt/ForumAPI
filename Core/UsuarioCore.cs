using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using Model.Views;
using Model.Views.Exibir;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class UsuarioCore : AbstractValidator<Usuario>
    {
        private Usuario _usuario;
        private readonly IMapper _mapper;
        public Armazenamento _arm { get; set; }

        public UsuarioCore()
        {
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }

        public UsuarioCore(UsuarioView Usuario, IMapper Mapper)
        {
            _mapper = Mapper;

            _usuario = _mapper.Map<UsuarioView, Usuario>(Usuario);

            RuleFor(u => u.Nome).NotNull().MinimumLength(3).WithMessage("O Nome deve ter no minimo 3 letras!");
            RuleFor(u => u.Email).EmailAddress().NotNull().WithMessage("Email inválido.");
            RuleFor(u => u.Senha).NotNull().Length(8, 12).WithMessage("A senha deve ser entre 8 e 12 caracteres e nao pode ser nula");
            RuleFor(u => u.Senha).Matches(@"[a-z-A-Z].\d|\d.[a-z-A-Z]").WithMessage("A senha deve conter ao menos uma letra e um número");
            RuleFor(u => u.ConfirmaSenha).Equal(_usuario.Senha).WithMessage("As senhas devem ser iguais!");

         
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }

        public UsuarioCore(LoginUserView Usuario, IMapper Mapper)
        {
            _mapper = Mapper;
            _usuario = _mapper.Map<LoginUserView, Usuario>(Usuario);
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }

     
        //Método para cadastro de usuario
        public Retorno CadastrarUsuario()
        {
            var valida = Validate(_usuario);
            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado = valida.Errors.Select(a => a.ErrorMessage).ToList() };

            if (_arm.Usuarios.Any(e => e.Email == _usuario.Email))
                return new Retorno { Status = false, Resultado = new List<string> { "Email ja cadastrado!" } };

            _arm.Usuarios.Add(_usuario);
            Arquivo.Salvar(_arm);
            return new Retorno { Status = true, Resultado = new List<string> { "Usuário cadastrado com sucesso!" } };
        }

        //Método para logar o usuario na plataforma.
        public Retorno LogarUsuario()
        {
            var usuarioLogin = _arm.Usuarios.Find(u => u.Email == _usuario.Email && u.Senha == _usuario.Senha);

            return usuarioLogin == null ? new Retorno { Status = false, Resultado = new List<string> { "Email ou senha inválidos!" } }
            : new Retorno { Status = true, Resultado = new LoginRetorno { Status = true, TokenUsuario = usuarioLogin.Id, Nome = usuarioLogin.Nome } };

        }

        public Retorno Listar() => new Retorno { Status = true, Resultado = _arm.Usuarios };
    }
}
