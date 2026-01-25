using AutoMapper;
using Core;
using EvenPace.Models;
using Models;

namespace EvenPaceWeb.Mappers
{
    public class OrganizacaoProfile : Profile
    {
        public OrganizacaoProfile()
        {
            CreateMap<Organizacao, OrganizacaoViewModel>().ReverseMap();
        }
        
    }
}
