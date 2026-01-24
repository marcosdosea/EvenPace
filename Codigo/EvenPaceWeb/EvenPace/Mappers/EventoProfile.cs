using AutoMapper;
using Core;
using EvenPace.Models;

public class EventoProfile : Profile
{
    public EventoProfile()
    {
        CreateMap<Evento, EventoDto>();
        CreateMap<EventoDto, Evento>();
    }
}
