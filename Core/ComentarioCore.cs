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
   public class ComentarioCore:AbstractValidator<Comment>
    {
        private Comment _comentario;
        private readonly IMapper _mapper;
        public ForumContext _dbcontext { get; set; }

        public ComentarioCore(ForumContext Contexto)
        {
            _dbcontext = Contexto;
            _dbcontext.Comentarios = _dbcontext.Set<Comment>();
        }
        public ComentarioCore(IMapper Mapper, ForumContext Contexto)
        {
            _mapper = Mapper;
            _dbcontext = Contexto;
                 _dbcontext.Comentarios = _dbcontext.Set<Comment>();

        }

        public ComentarioCore(ComentarioView Comentario, IMapper Mapper, ForumContext Contexto)
        {
            _dbcontext = Contexto;
            _mapper = Mapper;
            _dbcontext.Comentarios = _dbcontext.Set<Comment>();
            _comentario = _mapper.Map<ComentarioView, Comment>(Comentario);

            RuleFor(c => c.Msg).Length(10, 500).WithMessage("O comentario deve ficar entre 10 e 500 caracteres");
         
        }

        //Método para comentar
        public Retorno Comentar(string tokenAutor)
        {

            if (!Guid.TryParse(tokenAutor, out Guid token))
                return new Retorno { Status = false, Resultado = new List<string> { "Token inválido" } };

            //procuro pelo suuario que esta comentando
            var usuarioForum = _dbcontext.Usuarios.FirstOrDefault(c => c.Id == token);
            if (usuarioForum == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Usuario nao existe na base de dados" } };

            //busco a publicacao em questao
            var umPost = _dbcontext.Posts.FirstOrDefault(c => c.Id == _comentario.PublicacaoId);
           

            if (umPost == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Publicacao nao existe" } };


            _comentario.AutorId = usuarioForum.Id;
            
            _dbcontext.Comentarios.Add(_comentario);

            _dbcontext.SaveChanges();

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
            var usuarioForum = _dbcontext.Usuarios.FirstOrDefault(c => c.Id == token);
              if (usuarioForum == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Usuario nao existe na base de dados" } };

            var umComentario = _dbcontext.Comentarios.FirstOrDefault(c => c.Id == comentId);

            // validacao do autor 
            if(umComentario.AutorId != usuarioForum.Id)
                return new Retorno { Status = false, Resultado = new List<string> { "Autor do comentario inválido" } };

            //verifico a tamanho dos textos 
            if(comentarioAtt.Msg.Length <10 || comentarioAtt.Msg.Length >500)
                return new Retorno { Status = false, Resultado = new List<string> { "Tamanho da mensagem inválido, digite entre 10 e 500 caracteres" } };

            _mapper.Map(comentarioAtt, umComentario);

            _dbcontext.SaveChanges();
            return new Retorno { Status = true, Resultado = umComentario};

        }

        // método para buscar um comentrio por id
        public Retorno BuscarComentario(string tokenAutor, string idComent)
        {
            // conversoes dos guids
            if (!Guid.TryParse(tokenAutor, out Guid token) || !_dbcontext.Usuarios.Any(e => e.Id == token) || (!Guid.TryParse(idComent, out Guid comentarioId) ) )
                return new Retorno { Status = false, Resultado = new List<string> { "Dádos inválidos" } };

            // procuro um comentario e verifico se existe
            var umComentario = _dbcontext.Comentarios.FirstOrDefault(c => c.Id == comentarioId);

            if (umComentario == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario inválido" } };
            //AtualizaVoto(umComentario);

            return new Retorno { Status = true, Resultado = umComentario};
        }


        public Retorno  DeletarComentario(string tokenAutor, string idComent)
        {
            // conversoes dos guids
            if (!Guid.TryParse(tokenAutor, out Guid token) || !_dbcontext.Usuarios.Any(e => e.Id == token) || (!Guid.TryParse(idComent, out Guid comentarioId)))
                return new Retorno { Status = false, Resultado = new List<string> { "Dádos inválidos" } };

            // procuro um comentario e verifico se existe
            var umComentario = _dbcontext.Comentarios.FirstOrDefault(c => c.Id == comentarioId);

            // validacoes para a atualizacao do comentario.
            if (umComentario == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario inválido" } };

            if(umComentario.CitacaoId != null || umComentario.ComentarioId != null)
                return new Retorno { Status = false, Resultado = new List<string> { "Não é possivel deletar este comentario" } };

            _dbcontext.Comentarios.Remove(umComentario);
            _dbcontext.SaveChanges();
            return new Retorno { Status = true, Resultado = new List<string> { "Comentario deletado com sucesso!" } };

        }

        //public Retorno VotarComentario(string tokenAutor, VotoView voto)
        //{
        //    var votosPossiveis = new List<double> { 0,0.5, 1, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0 };

        //    if (!Guid.TryParse(tokenAutor, out Guid token) || !_dbcontext.Usuarios.Any(e => e.Id == token) )
        //        return new Retorno { Status = false, Resultado = new List<string> { "Dádos inválidos" } };


        //    if (_dbcontext.Comentarios.Any(p => p.Id == voto.ComentarioId))
        //        return new Retorno { Status = false, Resultado = new List<string> { "Comentario nao existe" } };

        //    var VotoMap =_mapper.Map<VotoView, Voto>(voto);

        //    var votosDoComentario = _dbcontext.Votos.Where(c => c.ComentarioId == VotoMap.ComentarioId).ToList();

        //    if(votosDoComentario.Any(c => c.UsuarioId == token))
        //        return new Retorno { Status = false, Resultado = new List<string> { "Não é possivel votar duas vezes no mesmo comentario!" } };

        //    VotoMap.UsuarioId = token;

        //    if(!Double.TryParse(voto.Nota, out double notaVot) || !votosPossiveis.Contains(notaVot) )
        //    return new Retorno { Status = false, Resultado = new List<string> { "Voto inváldo" } };

        //    _dbcontext.Votos.Add(VotoMap);

        //    Arquivo.Salvar(_dbcontext);

        //    return new Retorno { Status = true, Resultado = new List<string> { "Voto computado com sucesso!" } };
        //}

        //public void AtualizaVoto(Comment comentario)
        //{
        //    var votosDoComentario = _dbcontext.Votos.Where(c => c.ComentarioId == comentario.Id).ToList();

        //    double contador = 0;

        //    votosDoComentario.ForEach(c => contador = Convert.ToDouble(c.Nota));
         
        //    double mediaTotal = contador / votosDoComentario.Count();

        //    comentario.MediaVotos = mediaTotal.ToString();
        //}

        //public Retorno TodosComentarios()
        //{
        //    _dbcontext.Comentarios.ForEach(c => AtualizaVoto(c));
        //    return new Retorno { Status = true, Resultado = _dbcontext.Comentarios };
        //}


    }
}
