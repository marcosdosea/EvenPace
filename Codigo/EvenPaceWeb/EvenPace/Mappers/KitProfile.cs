using AutoMapper;
using Models; 
using Core;

namespace Mappers;

public class KitProfile : Profile
{
    public KitProfile()
    { 
        CreateMap<KitViewModel, Kit>().ReverseMap();
    }
}