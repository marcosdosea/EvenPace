using AutoMapper;
using Core;
using Models;

namespace EvenPaceWeb.Mappers
{
    public class CartaoCreditoProfile : Profile
    {
        public CartaoCreditoProfile()
        {
            CreateMap<CartaoCredito, CartaoCreditoViewViewModel>()
                .ReverseMap();
        }
    }
}

