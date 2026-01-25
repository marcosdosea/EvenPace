
namespace EvenPaceWeb.Mappers
{
    public class AdministradorProfile : Profile
    {
        public AdministradorProfile()
        {
            CreateMap<Administrador, AdministradorViewModel>()
                .ReverseMap();
        }
    }
}

