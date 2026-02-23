using AutoMapper;
using Models;
using Core;

namespace Mappers
{
    public class AvaliacaoEventoProfile : Profile
    {
        public AvaliacaoEventoProfile()
        {
            CreateMap<AvaliacaoViewModel, AvaliacaoEvento>()
                .ForMember(dest => dest.Inscricaos, opt => opt.Ignore());
        }
    }
}
