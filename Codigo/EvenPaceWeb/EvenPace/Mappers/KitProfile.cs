using AutoMapper;
using EvenPaceWeb.Models; 
using Core;

namespace EvenPace.Mappers;

public class KitProfile : Profile
{
    public KitProfile()
    { 
        CreateMap<KitViewModel, Kit>().ReverseMap();
    }
}