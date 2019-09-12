using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using Model.Views;
using Model.Views.Exibir;
using Model.Views.Retornar;
using System;
using System.Linq;

namespace Core
{
    public class PostCore : AbstractValidator<Post>
    {
        private Post _publicacao;
        private readonly IMapper _mapper;
        public Armazenamento _arm { get; set; }

        public PostCore()
        {
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }
        public PostCore(IMapper Mapper)
        {
            _mapper = Mapper;
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }

        public PostCore(PostView Publicacao, IMapper Mapper)
        {
            _mapper = Mapper;
            _publicacao = _mapper.Map<PostView, Post>(Publicacao);

            RuleFor(p => p.Titulo).NotNull().Length(5, 250).WithMessage("O título deve ter entre 8 e 250 caracteres");
            RuleFor(p => p.Texto).NotNull().MinimumLength(50).WithMessage("O texto deve ter no mínimo 50 caracteres");
     
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }

        //Método para cadastro de uma publicacao
        public Retorno CadastrarPost(Guid token)
        {
            try
            {
                //Validação do tipo da publicação
                if (_publicacao.Tipo.ToUpper() == "TUTORIAL" || _publicacao.Tipo.ToUpper() == "DUVIDA")
                {
                    var valido = Validate(_publicacao);
                    if (!valido.IsValid)
                        return new Retorno { Status = false, Resultado = valido.Errors.Select(e => e.ErrorMessage).ToList() };

                    //Busco pelo autor do post
                    var oAutor = _arm.Usuarios.Find(c => c.Id == token);
                    if (oAutor == null)
                        return new Retorno { Status = false, Resultado = "Usuario inválido." };

                    //atribuo o autor ao post
                    _publicacao.Autor = new UsuarioRetorno { Nome = oAutor.Nome, Email = oAutor.Email, };

                    // Setando valor do topico tipo duvida para berto
                    if (_publicacao.Tipo.ToUpper() == "DUVIDA")
                        _publicacao.Status = "aberta";

                    //adiciono o salvo na lista 
                    _arm.Posts.Add(_publicacao);
                    Arquivo.Salvar(_arm);
                    return new Retorno { Status = true, Resultado = "Publicacão registrada com sucesso!" };
                }
            }
            catch (Exception) {  return new Retorno { Status = false, Resultado = "O tipo da publicaçao nao pode ser nula." }; ;}
           
            return new Retorno { Status = false, Resultado = "Tipo da publicacao deve ser tutorial ou duvida!" };
        }

        //Método para efetuar a editaçao de uma publicacao
        public Retorno EditarPost(string id ,PostAtt postatt, Guid tokenAutor)
        {
            //tento realizar a conversao do guid
            if (!Guid.TryParse(id, out Guid ident)) return new Retorno { Status = false, Resultado = "Id inválido" };

            //checko se o post realmente existe
            var umPost = _arm.Posts.Find(c => c.Id == ident); if (umPost == null)
                return new Retorno { Status = false, Resultado = "Esse post não existe!" };


            //checko se o usuario tem autorização para editar o post
            var usuarioForum = _arm.Usuarios.Find(e => e.Id == tokenAutor);
            if (usuarioForum == null)
                return new Retorno { Status = false, Resultado = "Usuario nao existe na base de dados" };

            if (umPost.Autor.Email != usuarioForum.Email)
                return new Retorno { Status = false, Resultado = "Autorização para editar esse post negada" };

            //variavel auxiliar para ter o status original antes do mapeamento.
            var statuspost = umPost.Status;
            //mapeamento
            _mapper.Map(postatt, umPost );

            //Validação para limitar a mudança de status de aberta nos post do tipo tutorial
            if (statuspost == null)
            {
                umPost.Status = null;

                Arquivo.Salvar(_arm);
                return new Retorno { Status = true, Resultado = umPost };
            }

            //verifico se o status fornecido é valido
            if (umPost.Status.ToUpper() != "FECHADA")
                return new Retorno { Status = false, Resultado = "Para fechar uma publicacao , é necessario mudar o status para fechada" };
           
            Arquivo.Salvar(_arm);
            return new Retorno { Status = true, Resultado = umPost };
        }

        //Método para deletar uma publicacao se baseando no id
        public Retorno DeletarPost(Guid tokenAutor, string id)
                                  {
            if (!Guid.TryParse(id, out Guid ident))
                return new Retorno { Status = false, Resultado = "Id inválido" };

            //Procuro o post e vejo se ele existe.
            var umPost = _arm.Posts.Find(p => p.Id == ident);

            if (umPost == null)
                return new Retorno { Status = false, Resultado = "Publicação nao existe na base de dados" };

            //checko se o usuario tem autorização para editar o post
            var usuarioForum = _arm.Usuarios.Find(e => e.Id == tokenAutor);
            if (usuarioForum == null)
                return new Retorno { Status = false, Resultado = "Usuario nao existe na base de dados" };

            if (umPost.Autor.Email != usuarioForum.Email)
                return new Retorno { Status = false, Resultado = "Autorização para editar esse post negada" };

            _arm.Posts.Remove(umPost);

            return new Retorno { Status = true, Resultado = "Publicação deletada com sucesso!" };
        }
        //método para listar uma publicacao por id
        public Retorno ListarPorId(string id)
        {
            if (!Guid.TryParse(id, out Guid ident))
                return new Retorno { Status = false, Resultado = "Id inválido" };

            var umPost = _arm.Posts.Find(p => p.Id == ident);
            return umPost == null ? new Retorno { Status = false, Resultado = "Publicação nao existe na base de dados" } : new Retorno { Status = true, Resultado = umPost };
        }
        //método para listar todas as publicacoes
        public Retorno ListarTodos()
        {
            var todosPosts = _arm.Posts;
            return todosPosts.Count == 0 ? new Retorno { Status = false, Resultado = "Não existe registros na base de dados" } : new Retorno { Status = true, Resultado = todosPosts };
        }

        public void AtualizarComentarios(Post post)
        {

            var listaTotal = _arm.Comentarios.Where(e => e.PublicacaoId == post.Id).ToList();


            //listaTotal.ForEach(c => c.Replicas.Add())

            //post.Comentarios =
        }



    }
}
