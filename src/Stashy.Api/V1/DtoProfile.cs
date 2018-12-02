using AutoMapper;
using Stashy.Api.Infrastructure.Models;
using Stashy.Api.V1.Dtos;

namespace Stashy.Api.V1
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
