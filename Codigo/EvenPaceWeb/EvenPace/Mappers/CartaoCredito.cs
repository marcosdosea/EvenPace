namespace EvenPaceWeb.Mappers
{
    public class CartaoCreditoProfile : Profile
    {
        public CartaoCreditoProfile()
        {
            CreateMap<Cartaocredito, CartaoCreditoViewModel>()
                .ReverseMap();
        }
    }
}

