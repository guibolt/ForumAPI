using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Afins;
using Model.Views.Exibir;
using Model.Views.Receber;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class ComentarioCore : AbstractValidator<Comment>
    {
        private Comment _comentario;
        private readonly IMapper _mapper;
        public ForumContext _dbcontext { get; set; }
        public ComentarioCore(ForumContext Contexto) => _dbcontext = Contexto;
          
        public ComentarioCore(IMapper Mapper, ForumContext Contexto)
        {
            _mapper = Mapper;
            _dbcontext = Contexto;
        }

        public ComentarioCore(ComentarioView Comentario, IMapper Mapper, ForumContext Contexto)
        {
            _dbcontext = Contexto;
            _mapper = Mapper;
            _comentario = _mapper.Map<ComentarioView, Comment>(Comentario);

            RuleFor(c => c.Msg).Length(10, 500).WithMessage("O comentario deve ficar entre 10 e 500 caracteres");
        }

        //Método para comentar
        public Retorno Comentar(string tokenAutor)
        {
            // Verifico se o token do usuario é valido e se ele esta logado
            if (!Autorizacao.ValidarUsuario(tokenAutor, _dbcontext))
                return new Retorno { Status = false, Resultado = new List<string> { "Autorização negada" } };

            //busco a publicacao em questao
            var umPost = _dbcontext.Posts.FirstOrDefault(c => c.Id == _comentario.PostId);

            if (umPost == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Publicacao nao existe" } };

            _comentario.AutorId = Guid.Parse(tokenAutor);

            _dbcontext.Comentarios.Add(_comentario);

            _dbcontext.SaveChanges();

            return new Retorno { Status = true, Resultado = new List<string> { "Comentário cadastrado com sucesso!" } };
        }
        public Retorno EditarComentario(string tokenAutor, ComentarioAtt comentarioAtt, string comentarioId)
        {
            // Verifico se o token do usuario é valido e se ele esta logado
            if (!Autorizacao.ValidarUsuario(tokenAutor, _dbcontext))
                return new Retorno { Status = false, Resultado = new List<string> { "Autorização negada" } };

            if (!Guid.TryParse(comentarioId, out Guid comentId))
                return new Retorno { Status = false, Resultado = new List<string> { "Id do Comentário inválido" } };

            var umComentario = _dbcontext.Comentarios.Include(c => c.Autor).FirstOrDefault(c => c.Id == comentId);

            if(umComentario == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Comentário nao existe" } };
            // validacao do autor 
            if (umComentario.AutorId != Guid.Parse(tokenAutor)) 
                return new Retorno { Status = false, Resultado = new List<string> { "Autor do comentario inválido" } };

            //verifico a tamanho dos textos 
            if (comentarioAtt.Msg.Length < 10 || comentarioAtt.Msg.Length > 500)
                return new Retorno { Status = false, Resultado = new List<string> { "Tamanho da mensagem inválido, digite entre 10 e 500 caracteres" } };

            _mapper.Map(comentarioAtt, umComentario);

            _dbcontext.SaveChanges();
            return new Retorno { Status = true, Resultado = umComentario };
        }

        // método para buscar um comentrio por id
        public Retorno BuscarComentario(string tokenAutor, string idComent)
        {
            // conversoes dos guids
            if (!Autorizacao.ValidarUsuario(tokenAutor, _dbcontext) || (!Guid.TryParse(idComent, out Guid comentarioId)))
                return new Retorno { Status = false, Resultado = new List<string> { "Dádos inválidos" } };

            // procuro um comentario e verifico se existe
            var umComentario = _dbcontext.Comentarios.Include(c => c.Autor).FirstOrDefault(c => c.Id == comentarioId);

            if (umComentario == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario inválido" } };
            //AtualizaVoto(umComentario);

            return new Retorno { Status = true, Resultado = umComentario };
        }
        public Retorno DeletarComentario(string tokenAutor, string idComent)
        {
            // conversoes dos guids
            if (!Autorizacao.ValidarUsuario(tokenAutor, _dbcontext) || (!Guid.TryParse(idComent, out Guid comentarioId)))
                return new Retorno { Status = false, Resultado = new List<string> { "Dádos inválidos" } };

            // procuro um comentario e verifico se existe
            var umComentario = _dbcontext.Comentarios.FirstOrDefault(c => c.Id == comentarioId);

            // validacoes para a atualizacao do comentario.
            if (umComentario == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario inválido" } };

            if (umComentario.CitacaoId != null || umComentario.ComentarioId != null)
                return new Retorno { Status = false, Resultado = new List<string> { "Não é possivel deletar este comentario" } };

            //Tento realizar a remoção, se nao acontecer retorno uma mensagem para o usuario.
            try
            {
                _dbcontext.Comentarios.Remove(umComentario);
                _dbcontext.SaveChanges();
            }
            catch (Exception){   return new Retorno { Status = false, Resultado = new List<string> { "Não é possivel deletar esse comentario, pois ele esta relacionado com outro!" } }; }
        
            return new Retorno { Status = true, Resultado = new List<string> { "Comentario deletado com sucesso!" } };
        }
    }
}
