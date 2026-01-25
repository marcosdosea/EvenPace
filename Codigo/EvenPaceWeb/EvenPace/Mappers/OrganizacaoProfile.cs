using AutoMapper;

namespace EvenPaceWeb.Mappers
{
    public class OrganizacaoProfile : Profile
    {
        public OrganizacaoProfile()
        {
            CreateMap<Core.Organizacao, Models.OrganizacaoViewModel>().ReverseMap();
        }
        
    }
}
