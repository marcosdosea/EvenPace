using AutoMapper;

namespace EvenPaceWeb.Mappers
{
    public class AvaliacaoEventoProfile : Profile
    {

        public AvaliacaoEventoProfile()
        {
            CreateMap<Core.Avaliacaoevento, Models.AvaliacaoEventoViewModel>().ReverseMap();
        }
    }
}
