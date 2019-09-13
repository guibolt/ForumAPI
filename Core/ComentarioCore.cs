using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using Model.Views.Exibir;
using Model.Views.Receber;
using Model.Views.Retornar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
   public class ComentarioCore:AbstractValidator<Comentario>
    {
        private Comentario _comentario;
        private readonly IMapper _mapper;
        public Armazenamento _arm { get; set; }

        public ComentarioCore()
        {
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }
        public ComentarioCore(IMapper Mapper)
        {
            _mapper = Mapper;
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }

        public ComentarioCore(ComentarioView Comentario, IMapper Mapper)
        {
            _mapper = Mapper;
            _comentario = _mapper.Map<ComentarioView, Comentario>(Comentario);

            RuleFor(c => c.Msg).Length(10, 500).WithMessage("O comentario deve ficar entre 10 e 500 caracteres");
            _arm = Arquivo.Recuperar(_arm);
            _arm = _arm ?? new Armazenamento();
        }

        //Método para comentar
        public Retorno Comentar(string tokenAutor)
        {

            if (!Guid.TryParse(tokenAutor, out Guid token))
                return new Retorno { Status = false, Resultado = new List<string> { "Token inválido" } };

            //procuro pelo suuario que esta comentando
            var usuarioForum = _arm.Usuarios.Find(c => c.Id == token);
            if (usuarioForum == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Usuario nao existe na base de dados" } };

            //busco a publicacao em questao
            var umPost = _arm.Posts.Find(c => c.Id == _comentario.PublicacaoId);
           

            if (umPost == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Publicacao nao existe" } };



            _comentario.AutorId = usuarioForum.Id.ToString();
            
            _arm.Comentarios.Add(_comentario);

            Arquivo.Salvar(_arm);

            return new Retorno { Status = true, Resultado = new List<string> { "Comentário cadastrado com sucesso!" } };
        }

        public Retorno EditarComentario(string tokenAutor, ComentarioAtt comentarioAtt, string comentarioId)
        {
            // conversoes dos guids
            if (!Guid.TryParse(tokenAutor, out Guid token))
                return new Retorno { Status = false, Resultado = new List<string> { "Token inválido" } };

            if (!Guid.TryParse(comentarioId, out Guid comentId))
                return new Retorno { Status = false, Resultado = new List<string> { "Comentário inválido" } };


            //procuro pelo suuario que esta comentando
            var usuarioForum = _arm.Usuarios.Find(c => c.Id == token);
              if (usuarioForum == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Usuario nao existe na base de dados" } };

            var umComentario = _arm.Comentarios.Find(c => c.Id == comentId);

            // validacao do autor 
            if(umComentario.AutorId != usuarioForum.Id.ToString())
                return new Retorno { Status = false, Resultado = new List<string> { "Autor do comentario inválido" } };

            if(comentarioAtt.Msg.Length <10 || comentarioAtt.Msg.Length >500)
                return new Retorno { Status = false, Resultado = new List<string> { "Tamanho da mensagem inválido, digite entre 10 e 500 caracteres" } };

            _mapper.Map(comentarioAtt, umComentario);

            Arquivo.Salvar(_arm);
            return new Retorno { Status = true, Resultado = _mapper.Map<ComentarioRetorno>(umComentario)};

        }


        public Retorno BuscarComentario(string tokenAutor, string idComent)
        {
            // conversoes dos guids
            if (!Guid.TryParse(tokenAutor, out Guid token) || !_arm.Usuarios.Any(e => e.Id == token) || (!Guid.TryParse(idComent, out Guid comentarioId) ) )
                return new Retorno { Status = false, Resultado = new List<string> { "Dádos inválidos" } };


            var umComentario = _arm.Comentarios.Find(c => c.Id == comentarioId);

            if (umComentario == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario inválido" } };

            return new Retorno { Status = true, Resultado = _mapper.Map<ComentarioRetorno>(umComentario)};
        }


        public Retorno  DeletarComentario(string tokenAutor, string idComent)
        {
            // conversoes dos guids
            if (!Guid.TryParse(tokenAutor, out Guid token) || !_arm.Usuarios.Any(e => e.Id == token) || (!Guid.TryParse(idComent, out Guid comentarioId)))
                return new Retorno { Status = false, Resultado = new List<string> { "Dádos inválidos" } };


            var umComentario = _arm.Comentarios.Find(c => c.Id == comentarioId);

            if (umComentario == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario inválido" } };

            if(umComentario.CitacaoId != null || umComentario.ComentarioId != null)
                return new Retorno { Status = false, Resultado = new List<string> { "Não é possivel deletar este comentario" } };


            return new Retorno { Status = true, Resultado = new List<string> { "Comentario deletado com sucesso!" } };

        }


        public Retorno TodosComentarios() => new Retorno { Status = true, Resultado = _arm.Comentarios };
    }
}
