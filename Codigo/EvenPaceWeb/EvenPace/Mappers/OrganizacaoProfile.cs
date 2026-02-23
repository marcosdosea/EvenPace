using AutoMapper;
using Core;
using Models;

namespace Mappers
{
    public class OrganizacaoProfile : Profile
    {
        public OrganizacaoProfile()
        {
            CreateMap<Organizacao, OrganizacaoViewModel>().ReverseMap();
        }
        
    }
}
