using Core.Interface;
using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Linq;

namespace Core
{
    public class PublicacaoCore : AbstractValidator<Publicacao>, ICore<Retorno>
    {
        private Publicacao _publicacao;
        public Armazenamento _arm { get; set; }

        public PublicacaoCore()
        {
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }

        public PublicacaoCore(Publicacao Publicacao)
        {
            _publicacao = Publicacao;

            RuleFor(p => p.Titulo).NotNull().Length(5, 250).WithMessage("O título deve ter entre 8 e 250 caracteres");
            RuleFor(p => p.Conteudo).NotNull().MinimumLength(50).WithMessage("O texto deve ter no mínimo 50 caracteres");
            RuleFor(p => p.Tipo.ToUpper()).NotNull().Must(e => e == "TUTORIAL" || e == "DUVIDA").WithMessage("O tipo deve ser ou tutorial ou duvida");

            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }

        //Método para cadastro de uma publicacao
        public Retorno Cadastrar()
        {
            var valida = Validate(_publicacao);

            if (!valida.IsValid)
                return new Retorno { Status = false, Msg = valida.Errors.Select(e => e.ErrorMessage).ToList() };

            _arm.Posts.Add(_publicacao);


            return new Retorno { Status = true, Msg = "Publicacão registrada com sucesso!" };

        }

        //Método para efetuar a editaçao de uma publicacao
        public Retorno Editar()
        {
            throw new NotImplementedException();
        }

        //Método para deletar uma publicacao se baseando no id
        public Retorno Deletar(string id)
        {
            if (!Guid.TryParse(id, out Guid ident))
                return new Retorno { Status = false, Msg = "Id inválido" };

            var umaPublicacao = _arm.Posts.FirstOrDefault(p => p.Id == ident);

            return umaPublicacao == null ? new Retorno { Status = false, Msg = "Publicação nao existe na base de dados" } : new Retorno { Status = true, Msg = "Publicação deletada com sucesso!" };

        }
        //método para listar uma publicacao por id
        public Retorno ListarPorId(string id)
        {
            if (!Guid.TryParse(id, out Guid ident))
                return new Retorno { Status = false, Msg = "Id inválido" };

            var umaPublicacao = _arm.Posts.FirstOrDefault(p => p.Id == ident);

            return umaPublicacao == null ? new Retorno { Status = false, Msg = "Publicação nao existe na base de dados" } : new Retorno { Status = true, Msg = umaPublicacao };
        }
        //método para listar todas as publicacoes
        public Retorno ListarTodos()
        {
            var todasPublicacoes = _arm.Posts;
            return todasPublicacoes.Count == 0 ? new Retorno { Status = false, Msg = "Não existe registros na base de dados" } : new Retorno { Status = true, Msg = todasPublicacoes };
        }
    }
}
