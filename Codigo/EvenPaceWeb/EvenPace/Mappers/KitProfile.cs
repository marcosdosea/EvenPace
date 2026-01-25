using AutoMapper;
using Models; 
using Core;

namespace EvenPace.Mappers;

public class KitProfile : Profile
{
    public KitProfile()
    { 
        CreateMap<KitViewModel, Kit>().ReverseMap();
    }
}