using AutoMapper;
using Core.Util;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Views;
using Model.Views.Exibir;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Core
{
    public class UsuarioCore : AbstractValidator<Usuario>
    {
        private Usuario _usuario;
        //private readonly IMapper _mapper;
        public ForumContext _dbcontext { get; set; }

        public UsuarioCore(ForumContext Context)
        {
            _dbcontext = Context;
            _dbcontext.Usuarios = _dbcontext.Set<Usuario>();
        }

        public UsuarioCore(Usuario Usuario, ForumContext Context)
        {


            _dbcontext = Context;
            _usuario = Usuario;

            _dbcontext.Usuarios = _dbcontext.Set<Usuario>();

            RuleFor(u => u.Nome).NotNull().MinimumLength(3).WithMessage("O Nome deve ter no minimo 3 letras!");
            RuleFor(u => u.Email).EmailAddress().NotNull().WithMessage("Email inválido.");
            RuleFor(u => u.Senha).NotNull().Length(8, 12).WithMessage("A senha deve ser entre 8 e 12 caracteres e nao pode ser nula");
            RuleFor(u => u.Senha).Matches(@"[a-z-A-Z].\d|\d.[a-z-A-Z]").WithMessage("A senha deve conter ao menos uma letra e um número");
            RuleFor(u => u.ConfirmaSenha).Equal(_usuario.Senha).WithMessage("As senhas devem ser iguais!");
        }


        //Método para cadastro de usuario
        public Retorno CadastrarUsuario()
        {
            var valida = Validate(_usuario);
            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado = valida.Errors.Select(a => a.ErrorMessage).ToList() };

            if (_dbcontext.Usuarios.Any(e => e.Email == _usuario.Email))
                return new Retorno { Status = false, Resultado = new List<string> { "Email ja cadastrado!" } };


            AtualizaUp(_usuario);
            _dbcontext.Usuarios.Add(_usuario);
            _dbcontext.SaveChanges();
            return new Retorno { Status = true, Resultado = new List<string> { "Usuário cadastrado com sucesso!" } };
        }

        //Método para logar o usuario na plataforma.
        public Retorno LogarUsuario()
        {
            var usuarioLogin = _dbcontext.Usuarios.FirstOrDefault(u => u.Email == _usuario.Email && u.Senha == _usuario.Senha);
            dynamic obj = new ExpandoObject();


            if (usuarioLogin == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Email ou senha inválidos!" } };

            obj.Status = true;
            obj.TokenUsuario = usuarioLogin.Id;
            obj.Nome = usuarioLogin.Nome;

            return new Retorno { Status = true, Resultado = obj };
        }

        public Retorno Listar() => new Retorno { Status = true, Resultado = _dbcontext.Usuarios.ToList() };

        public void AtualizaUp(Usuario user)
        {

            user.Nome = user.Nome.ToUpper();
            user.Email = user.Email.ToUpper();
        }
    }
}
