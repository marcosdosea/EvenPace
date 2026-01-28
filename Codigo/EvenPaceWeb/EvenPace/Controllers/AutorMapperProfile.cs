using AutoMapper;
using Core;
using Models; 

namespace EvenPaceWeb.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            
            CreateMap<Kit, KitViewModel>().ReverseMap();

        
            CreateMap<Evento, EventoViewModel>().ReverseMap();
            CreateMap<Inscricao, InscricaoViewModel>().ReverseMap();
        }
    }
}
