using AutoMapper;
using Core;
using Models;

public class EventoProfile : Profile
{
    public EventoProfile()
    {
        CreateMap<Evento, EventoViewModel>().ReverseMap(); 
    }
}
