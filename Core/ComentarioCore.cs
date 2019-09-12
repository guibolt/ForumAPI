using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using Model.Views.Exibir;
using System;
using System.Collections.Generic;
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
        public Retorno Comentar(Guid token)
        {
            //procuro pelo suuario que esta comentando
            var usuarioForum = _arm.Usuarios.Find(c => c.Id == token);

            //busco a publicacao em questao
            var umPost = _arm.Posts.Find(c => c.Id == _comentario.PublicacaoId);
            //atribuo o usuario ao autor do comentario
            _comentario.Autor = usuarioForum;

            if (umPost == null)
                return new Retorno { Status = false, Resultado = "Publicacao nao existe" };

            _arm.Comentarios.Add(_comentario);


            return new Retorno { Status = true, Resultado = "Comentário cadastrado com sucesso!" };
        }



    }
}
