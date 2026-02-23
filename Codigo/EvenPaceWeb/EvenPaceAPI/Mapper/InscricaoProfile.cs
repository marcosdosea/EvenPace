using AutoMapper;
using Core;
using EvenPaceAPI.Models;
using Models;

namespace Mappers
{
    public class InscricaoProfile : Profile
    {
        public InscricaoProfile()
        {
            CreateMap<InscricaoViewModel, Inscricao>().ReverseMap();
        }
    }
}