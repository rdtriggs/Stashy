using AutoMapper;
using Stashy.Api.Infrastructure.Models;
using Stashy.Api.V2.Dtos;

namespace Stashy.Api.V2
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<Coin, CoinDto>();
            CreateMap<Coin, PriceDto>();
        }
    }
}
