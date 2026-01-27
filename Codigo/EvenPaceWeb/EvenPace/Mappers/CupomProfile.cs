using AutoMapper;
using Core;
using Models;

namespace EvenPaceWeb.Mappers
{
    public class CupomProfile : Profile
    {
        public CupomProfile() 
        {
           CreateMap<Cupom, CupomViewModel>().ReverseMap(); 
        }

    }
}
