using AutoMapper;
using Model;
using Model.Views;
using Model.Views.Exibir;
using Model.Views.Receber;
using Model.Views.Retornar;
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
            CreateMap<ComentarioView, Comment>();


            CreateMap<PostAtt, Post>()
                                   .ForMember(dest => dest.Titulo, opt => opt.Condition(src => src.Titulo != null))
                                   .ForMember(dest => dest.Texto, opt => opt.Condition(src => src.Texto != null))
                                   .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status != null));


            CreateMap<ComentarioAtt, Comment>()
                                    .ForMember(dest => dest.CitacaoId, opt => opt.Condition(src => src.CitacaoId != null))
                                    .ForMember(dest => dest.Msg, opt => opt.Condition(src => src.Msg != null));

        }
    }
}
