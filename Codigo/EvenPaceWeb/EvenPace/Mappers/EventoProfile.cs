using AutoMapper;
using Core;
using Models;

namespace EvenPaceWeb.Mappers
{
    public class EventoProfile : Profile
    {
        public EventoProfile()
        {
            CreateMap<EventoViewModel, Evento>()
                .ReverseMap()
                // Quando carregar do Banco p/ Tela: Separa Data e Hora
                .ForMember(dest => dest.DataOnly, opt => opt.MapFrom(src => src.Data.Date))
                .ForMember(dest => dest.HoraOnly, opt => opt.MapFrom(src => src.Data.TimeOfDay));
        }
    }
}