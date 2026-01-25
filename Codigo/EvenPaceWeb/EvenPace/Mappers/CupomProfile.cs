using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EvenPaceWeb.Mappers
{
    public class CupomProfile : Profile
    {
        public CupomProfile() 
        {
            CreateMap<Core.Cupom, Models.CupomViewModel>().ReverseMap();
        }

    }
}
