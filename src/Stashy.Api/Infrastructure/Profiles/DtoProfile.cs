using AutoMapper;
using Stashy.Api.Infrastructure.Dtos;
using Stashy.Api.Infrastructure.Models;

namespace Stashy.Api.Infrastructure.Profiles
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
