using AutoMapper;
using Core;
using EvenPaceWeb.Models;

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

