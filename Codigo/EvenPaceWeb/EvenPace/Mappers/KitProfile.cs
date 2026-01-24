using AutoMapper;
using EvenPace.Models;
using Core;

namespace EvenPace.Mappers;

public class KitProfile : Profile
{
    public KitProfile()
    {
        // CreateMap<KitModel, Kit>().ReverseMap(); TODO: Retira o comentario quando o Modelo Kit for feito
    }
}