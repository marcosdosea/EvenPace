using AutoMapper;

namespace EvenPaceWeb.Mappers
{
    public class AvaliacaoEventoProfile : Profile
    {

        public AvaliacaoEventoProfile()
        {
            CreateMap<Core.AvaliacaoEvento, Models.AvaliacaoEventoViewModel>().ReverseMap();
        }
    }
}
