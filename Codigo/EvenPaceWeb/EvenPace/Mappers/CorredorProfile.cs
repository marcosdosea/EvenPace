using AutoMapper;
using Models;
using Core;

namespace EvenPace.Mappers;

public class CorredorProfile : Profile
{
    public CorredorProfile()
    {
        CreateMap<CorredorViewModel,Corredor>().ReverseMap();
        CreateMap<Evento, HistoricoEventoViewModel>();
        CreateMap<AvaliacaoEvento, AvaliacaoEventoViewModel>().ReverseMap();
    }
}
