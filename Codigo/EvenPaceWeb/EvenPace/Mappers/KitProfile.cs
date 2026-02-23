using AutoMapper;
using Models; 

namespace Mappers;

public class KitProfile : Profile
{
    public KitProfile()
    { 
        CreateMap<KitViewModel, Kit>().ReverseMap();
    }
}