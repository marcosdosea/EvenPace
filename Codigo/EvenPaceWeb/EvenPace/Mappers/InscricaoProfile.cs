using AutoMapper;
using Core;
using Models;

namespace EvenPace.Mappers;

public class InscricaoProfile : Profile
{
    public InscricaoProfile()
    {
        CreateMap<InscricaoViewModel, Inscricao>().ReverseMap();
    }
}