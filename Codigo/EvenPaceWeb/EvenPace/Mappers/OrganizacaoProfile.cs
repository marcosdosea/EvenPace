using AutoMapper;
using Core;
using EvenPace.Models;

public class OrganizacaoProfile : Profile
{
    public OrganizacaoProfile()
    {
        CreateMap<Organizacao, OrganizacaoDto>();
        CreateMap<OrganizacaoDto, Organizacao>();
    }
}
