using AutoMapper;
using Core;
using Models;

namespace Mappers;

public class InscricaoProfile : Profile
{
    public InscricaoProfile()
    {
        CreateMap<InscricaoViewModel, Inscricao>().ReverseMap();
    }
}