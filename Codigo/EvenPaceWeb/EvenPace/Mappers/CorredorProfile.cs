using AutoMapper;
using Models;
using Core;

namespace EvenPace.Mappers;

public class CorredorProfile : Profile
{
    public CorredorProfile()
    {
        CreateMap<CorredorModel,Corredor>().ReverseMap();
    }
}