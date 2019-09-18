using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Afins;
using Model.Views;
using Model.Views.Exibir;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class PostCore : AbstractValidator<Post>
    {
        private Post _publicacao;
        private readonly IMapper _mapper;
        public ForumContext _dbcontext { get; set; }

        public PostCore(ForumContext Context) => _dbcontext = Context;
     
        public PostCore(IMapper Mapper, ForumContext Context)
        {
            _dbcontext = Context;
            _mapper = Mapper;
        }

        public PostCore(PostView Publicacao, IMapper Mapper, ForumContext Context)
        {
            _dbcontext = Context;
            _mapper = Mapper;
            _publicacao = _mapper.Map<PostView, Post>(Publicacao);

            RuleFor(p => p.Titulo).NotNull().Length(8, 250).WithMessage("O título deve ter entre 8 e 250 caracteres");
            RuleFor(p => p.Texto).NotNull().MinimumLength(50).WithMessage("O texto deve ter no mínimo 50 caracteres");
        }

        //Método para cadastro de uma publicacao
        public Retorno CadastrarPost(string tokenAutor)
        {
            try
            {
                //Validação do tipo da publicação
                if (_publicacao.Tipo.ToUpper() == "TUTORIAL" || _publicacao.Tipo.ToUpper() == "DUVIDA" || _publicacao.Tipo.ToUpper() == "DÚVIDA")
                {
                    //Validacao para o usuari, se o token é valido ou se ele esta logado
                    if (!Autorizacao.ValidarUsuario(tokenAutor, _dbcontext))
                        return new Retorno { Status = false, Resultado = new List<string> { "Autorização negada!" } };

                    // validacao para a publicação
                    var valido = Validate(_publicacao);
                    if (!valido.IsValid)
                        return new Retorno { Status = false, Resultado = valido.Errors.Select(e => e.ErrorMessage).ToList() };

                    //Busco pelo autor do post
                    var oAutor = _dbcontext.Usuarios.FirstOrDefault(c => c.Id == Guid.Parse(tokenAutor));

                    //atribuo o autor ao post
                    _publicacao.AutorID = Guid.Parse(tokenAutor);

                    // Setando valor do topico tipo duvida para berto
                    if (_publicacao.Tipo.ToUpper() == "DUVIDA" || _publicacao.Tipo.ToUpper() == "DÚVIDA")
                        _publicacao.Status = "aberta";

                    //adiciono o salvo na lista 
                    _dbcontext.Posts.Add(_publicacao);
                    _dbcontext.SaveChanges();
                    return new Retorno { Status = true, Resultado = new List<string> { "Publicacão registrada com sucesso!" } };
                }
            }
            catch (Exception) { return new Retorno { Status = false, Resultado = new List<string> { "O tipo da publicaçao nao pode ser nula." } }; ; }

            return new Retorno { Status = false, Resultado = new List<string> { "Tipo da publicacao deve ser tutorial ou duvida!" } };
        }
        ///
        //Método para efetuar a editaçao de uma publicacao
        public Retorno EditarPost(string id, PostAtt postatt, string tokenAutor)
        {
            //tento realizar a conversao do guid
            if (!Guid.TryParse(id, out Guid ident)) return new Retorno { Status = false, Resultado = new List<string> { "Id do post inválido" } };

            //Validação para o usuario, se o token é valido ou se ele esta logado.
            if (!Autorizacao.ValidarUsuario(tokenAutor, _dbcontext))
                return new Retorno { Status = false, Resultado = new List<string> { "Autorização negada!" } };

            //Validações internas para a edição do titulo e do texto.

            if (postatt.Titulo == null)
                postatt.Titulo = "";

            if (postatt.Titulo.Length < 8 || postatt.Texto.Length > 250)
                return new Retorno { Status = false, Resultado = new List<string> { "Tamanho do titulo inválido, minimo de 5 e maximo de 250 caracteres" } };

            if (postatt.Texto == null)
                postatt.Texto = "";

            if (postatt.Texto.Length < 50)
                return new Retorno { Status = false, Resultado = new List<string> { "Tamanho do texto inválido, minimo de 50 caracteres" } };

            //checko se o post realmente existe
            var umPost = _dbcontext.Posts.FirstOrDefault(c => c.Id == ident); if (umPost == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Esse post não existe!" } };

            // Vejo se o usuario tem autorização para editar o post
            if (umPost.Autor.Id != Guid.Parse(tokenAutor))
                return new Retorno { Status = false, Resultado = new List<string> { "Autorização para editar esse post negada" } };

            //mapeamento
            _mapper.Map(postatt, umPost);

            //Validação para limitar a mudança de status de aberta nos post do tipo tutorial
            if (umPost.Tipo.ToUpper() == "TUTORIAL")
                umPost.Status = null;

            //verifico se o status fornecido é valido
            if (umPost.Status.ToUpper() != "FECHADA" && umPost.Status.ToUpper() != "ABERTA")
                return new Retorno { Status = false, Resultado = new List<string> { "Para fechar uma publicacao , é necessario mudar o status para fechada" } };

            _dbcontext.SaveChanges();
            return new Retorno { Status = true, Resultado = umPost };
        }

        //Método para deletar uma publicacao se baseando no id
        public Retorno DeletarPost(string tokenAutor, string id)
        {
            if (!Guid.TryParse(id, out Guid ident))
                return new Retorno { Status = false, Resultado = new List<string> { "Id do post inválido" } };

            // Verifico se o token é valido e se o usuario esta logado
            if (!Autorizacao.ValidarUsuario(tokenAutor, _dbcontext))
                return new Retorno { Status = false, Resultado = new List<string> { "Autorização negada!" } };
             
            //Procuro o post e vejo se ele existe.
            var umPost = _dbcontext.Posts.FirstOrDefault(p => p.Id == ident);

            if (umPost == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Publicação nao existe na base de dados" } };

            //checko se o usuario tem autorização para deletar o post
            if (umPost.Autor.Id != Guid.Parse(tokenAutor))
                return new Retorno { Status = false, Resultado = new List<string> { "Autorização para editar esse post negada" } };

            _dbcontext.Posts.Remove(umPost);
            _dbcontext.SaveChanges();

            return new Retorno { Status = true, Resultado = new List<string> { "Publicação deletada com sucesso!" } };
        }
        //método para listar uma publicacao por id
        public Retorno ListarPorId(string id, string tokenAutor)
        {
            // Verifico se o token é valido e se o usuario esta logado
            if (!Autorizacao.ValidarUsuario(tokenAutor, _dbcontext))
                return new Retorno { Status = false, Resultado = new List<string> { "Autorização negada!" } };

            if (!Guid.TryParse(id, out Guid ident))
                return new Retorno { Status = false, Resultado = new List<string> { "Id do post inválido" } };

            // verifico se o post existe
            var umPost = _dbcontext.Posts.Include(c => c.Autor).FirstOrDefault(p => p.Id == ident);
            if (umPost == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Publicação nao existe na base de dados" } };

            return new Retorno { Status = true, Resultado = umPost };
        }
        //método para listar todas as publicacoes
        public Retorno ListarTodos(string tokenAutor)
        {
            if (!Autorizacao.ValidarUsuario(tokenAutor, _dbcontext))
                return new Retorno { Status = false, Resultado = new List<string> { "Autorização negada!" } };

            var todosPosts = _dbcontext.Posts.Include(c => c.Autor).ToList();

            return todosPosts.Count() == 0 ? new Retorno { Status = false, Resultado = new List<string> { "Não existe registros na base de dados" } } : new Retorno { Status = true, Resultado = todosPosts };
        }
    }
}