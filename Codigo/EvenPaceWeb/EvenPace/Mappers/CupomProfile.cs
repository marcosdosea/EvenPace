using AutoMapper;
using Core;
using EvenPace.Models;

public class CupomProfile : Profile
{
    public CupomProfile()
    {
        CreateMap<Cupom, CupomDto>();
        CreateMap<CupomDto, Cupom>();
    }
}
