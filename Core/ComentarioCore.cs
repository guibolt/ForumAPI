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

            //verifico a tamanho dos textos 
            if(comentarioAtt.Msg.Length <10 || comentarioAtt.Msg.Length >500)
                return new Retorno { Status = false, Resultado = new List<string> { "Tamanho da mensagem inválido, digite entre 10 e 500 caracteres" } };

            _mapper.Map(comentarioAtt, umComentario);

            Arquivo.Salvar(_arm);
            return new Retorno { Status = true, Resultado = _mapper.Map<ComentarioRetorno>(umComentario)};

        }

        // método para buscar um comentrio por id
        public Retorno BuscarComentario(string tokenAutor, string idComent)
        {
            // conversoes dos guids
            if (!Guid.TryParse(tokenAutor, out Guid token) || !_arm.Usuarios.Any(e => e.Id == token) || (!Guid.TryParse(idComent, out Guid comentarioId) ) )
                return new Retorno { Status = false, Resultado = new List<string> { "Dádos inválidos" } };

            // procuro um comentario e verifico se existe
            var umComentario = _arm.Comentarios.Find(c => c.Id == comentarioId);

            if (umComentario == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario inválido" } };
            AtualizaVoto(umComentario);

            return new Retorno { Status = true, Resultado = _mapper.Map<ComentarioRetorno>(umComentario)};
        }


        public Retorno  DeletarComentario(string tokenAutor, string idComent)
        {
            // conversoes dos guids
            if (!Guid.TryParse(tokenAutor, out Guid token) || !_arm.Usuarios.Any(e => e.Id == token) || (!Guid.TryParse(idComent, out Guid comentarioId)))
                return new Retorno { Status = false, Resultado = new List<string> { "Dádos inválidos" } };

            // procuro um comentario e verifico se existe
            var umComentario = _arm.Comentarios.Find(c => c.Id == comentarioId);

            // validacoes para a atualizacao do comentario.
            if (umComentario == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario inválido" } };

            if(umComentario.CitacaoId != null || umComentario.ComentarioId != null)
                return new Retorno { Status = false, Resultado = new List<string> { "Não é possivel deletar este comentario" } };

            _arm.Comentarios.Remove(umComentario);
            Arquivo.Salvar(_arm);
            return new Retorno { Status = true, Resultado = new List<string> { "Comentario deletado com sucesso!" } };

        }

        public Retorno VotarComentario(string tokenAutor, VotoView voto)
        {
            var votosPossiveis = new List<double> { 0.5, 1, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0 };

            if (!Guid.TryParse(tokenAutor, out Guid token) || !_arm.Usuarios.Any(e => e.Id == token) )
                return new Retorno { Status = false, Resultado = new List<string> { "Dádos inválidos" } };


            if (_arm.Comentarios.Any(p => p.Id == voto.ComentarioId))
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario nao existe" } };

            var VotoMap =_mapper.Map<VotoView, Voto>(voto);

            var votosDoComentario = _arm.Votos.Where(c => c.ComentarioId == VotoMap.ComentarioId).ToList();

            if(votosDoComentario.Any(c => c.UsuarioId == token))
                return new Retorno { Status = false, Resultado = new List<string> { "Não é possivel votar duas vezes no mesmo comentario!" } };

            VotoMap.UsuarioId = token;

            if(!Double.TryParse(voto.Nota, out double notaVot) || !votosPossiveis.Contains(notaVot) )
            return new Retorno { Status = false, Resultado = new List<string> { "Voto inváldo" } };

            _arm.Votos.Add(VotoMap);

            Arquivo.Salvar(_arm);

            return new Retorno { Status = true, Resultado = new List<string> { "Voto computado com sucesso!" } };
        }

        public void AtualizaVoto(Comentario comentario)
        {
            var votosDoComentario = _arm.Votos.Where(c => c.ComentarioId == comentario.Id).ToList();

            double contador = 0;

            votosDoComentario.ForEach(c => contador = Convert.ToDouble(c.Nota));
         
            double mediaTotal = contador / votosDoComentario.Count();

            comentario.MediaVotos = mediaTotal.ToString();
        }

        public Retorno TodosComentarios()
        {
            _arm.Comentarios.ForEach(c => AtualizaVoto(c));
            return new Retorno { Status = true, Resultado = _arm.Comentarios };
        }


    }
}
