using AutoMapper;
using Models;
using Core;

namespace Mappers;

public class CorredorProfile : Profile
{
    public CorredorProfile()
    {
        CreateMap<CorredorViewModel,Corredor>().ReverseMap();
    }
}
