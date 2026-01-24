using AutoMapper;
using Core;
using EvenPace.Models;

public class OrganizacaoProfile : Profile
{
    public OrganizacaoProfile()
    {
        CreateMap<Organizacao, OrganizacaoModel>().ReverseMap();
    }
}
