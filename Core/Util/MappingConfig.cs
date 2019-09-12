using AutoMapper;
using Model;
using Model.Views;
using Model.Views.Exibir;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Util
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<PostView, Post>();
            CreateMap<LoginUserView, Usuario>();
            CreateMap<UsuarioView, Usuario>();

            CreateMap<PostAtt, Post>()
                                   .ForMember(dest => dest.Titulo, opt => opt.Condition(src => src.Titulo != null))
                                   .ForMember(dest => dest.Texto, opt => opt.Condition(src => src.Texto != null))
                                   .ForMember(dest => dest.Aberta, opt => opt.Condition(src => src.Aberta != null));

            CreateMap<Post, Post>().ForMember(dest => dest.Id, opt => opt.Ignore())
                                   .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
                                   .ForMember(dest => dest.Autor, opt => opt.Ignore())
                                   .ForMember(dest => dest.Comentarios, opt => opt.Ignore())
                                   .ForMember(dest => dest.Titulo, opt => opt.Condition(src => src.Titulo != null))
                                   .ForMember(dest => dest.Texto, opt => opt.Condition(src => src.Texto != null))
                                   .ForMember(dest => dest.Aberta, opt => opt.Condition(src => src.Aberta != null));

        }
    }
}
