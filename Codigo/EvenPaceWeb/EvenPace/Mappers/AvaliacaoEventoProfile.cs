using AutoMapper;
using EvenPace.Models;
using Models;

namespace EvenPaceWeb.Mappers
{
    public class AvaliacaoEventoProfile : Profile
    {

        public AvaliacaoEventoProfile()
        {
            CreateMap<Core.AvaliacaoEvento, AvaliacaoEventoViewModel>().ReverseMap();
        }
    }
}
