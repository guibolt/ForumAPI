using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using Model.Views;
using Model.Views.Exibir;
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
            RuleFor(p => p.Tipo.ToUpper()).NotNull().Must(e => e == "TUTORIAL" || e == "DUVIDA").WithMessage("O tipo deve ser ou tutorial ou duvida");
     
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }

        //Método para cadastro de uma publicacao
        public Retorno Cadastrar(Guid token)
        {
            var valida = Validate(_publicacao);

            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado = valida.Errors.Select(e => e.ErrorMessage).ToList() };

            var oAutor = _arm.Usuarios.Find(c => c.Id == token);

            //Achando o autor;  

            var outroAutor = new Usuario();

            outroAutor.Nome = oAutor.Nome;
            outroAutor.Email = oAutor.Email;

            _publicacao.Autor = outroAutor;
        
         

            // Setando valor do topico tipo duvida para berto
            if (_publicacao.Tipo.ToUpper() == "DUVIDA")
                _publicacao.Aberta = "Aberta";
               




            _arm.Posts.Add(_publicacao);
            Arquivo.Salvar(_arm);
            return new Retorno { Status = true, Resultado = "Publicacão registrada com sucesso!" };

        }

        //Método para efetuar a editaçao de uma publicacao
        public Retorno Editar(string id ,PostAtt postatt)
        {

            if (!Guid.TryParse(id, out Guid ident))
                return new Retorno { Status = false, Resultado = "Id inválido" };

            var umPost = _arm.Posts.FirstOrDefault(c => c.Id == ident);

            if (umPost == null) return new Retorno { Status = false, Resultado = "Esse post não existe!" };

            _mapper.Map(postatt, umPost);

            Arquivo.Salvar(_arm);
            return new Retorno { Status = true, Resultado = umPost };
        }

        //Método para deletar uma publicacao se baseando no id
        public Retorno Deletar(string id)
        {
            if (!Guid.TryParse(id, out Guid ident))
                return new Retorno { Status = false, Resultado = "Id inválido" };

            var umaPublicacao = _arm.Posts.FirstOrDefault(p => p.Id == ident);
            return umaPublicacao == null ? new Retorno { Status = false, Resultado = "Publicação nao existe na base de dados" } : new Retorno { Status = true, Resultado = "Publicação deletada com sucesso!" };

        }
        //método para listar uma publicacao por id
        public Retorno ListarPorId(string id)
        {
            if (!Guid.TryParse(id, out Guid ident))
                return new Retorno { Status = false, Resultado = "Id inválido" };

            var umaPublicacao = _arm.Posts.FirstOrDefault(p => p.Id == ident);
            return umaPublicacao == null ? new Retorno { Status = false, Resultado = "Publicação nao existe na base de dados" } : new Retorno { Status = true, Resultado = umaPublicacao };
        }
        //método para listar todas as publicacoes
        public Retorno ListarTodos()
        {
            var todasPublicacoes = _arm.Posts;
            return todasPublicacoes.Count == 0 ? new Retorno { Status = false, Resultado = "Não existe registros na base de dados" } : new Retorno { Status = true, Resultado = todasPublicacoes };
        }
    }
}
