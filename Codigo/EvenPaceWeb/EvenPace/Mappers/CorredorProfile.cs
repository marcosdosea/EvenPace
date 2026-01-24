using AutoMapper;
using EvenPace.Models;
using Core;

namespace EvenPace.Mappers;

public class CorredorProfile : Profile
{
    public CorredorProfile()
    {
        // CreateMap<CorredorModel,Corredor>().ReverseMap(); TODO: Retira o comentario quando o Modelo Corredor for feito
    }
}