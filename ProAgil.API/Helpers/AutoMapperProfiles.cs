using System.Linq;
using AutoMapper;
using ProAgil.API.Dtos;
using ProAgil.Domain;

namespace ProAgil.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //
            CreateMap<Evento, EventoDto>()
            .ForMember(dest => dest.Palestrantes, opt => {
                opt.MapFrom(src => src.PalestrantesEventos.Select(x => x.Palestrante).ToList());
            }).ReverseMap();
            //CreateMap<EventoDto, Evento>(); mapeamente reverso (Palestrantae<->PalestranteEvento) mesmo igual usar ReverMap()

            CreateMap<Palestrante, PalestranteDto>()
            .ForMember(dest => dest.Eventos, opt => {
                opt.MapFrom(src => src.PalestrantesEventos.Select(x => x.Evento).ToList());
            });

            CreateMap<Lote, LoteDto>().ReverseMap();;

            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
        }
    }
}