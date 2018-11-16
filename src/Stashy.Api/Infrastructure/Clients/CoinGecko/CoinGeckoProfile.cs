using AutoMapper;
using Stashy.Api.Infrastructure.Clients.CoinGecko.Responses;
using Stashy.Api.Infrastructure.Models;

namespace Stashy.Api.Infrastructure.Clients.CoinGecko
{
    public class CoinGeckoProfile : Profile
    {
        public CoinGeckoProfile()
        {
            CreateMap<CoinItem, Coin>()
                .ForMember(dest => dest.Symbol, opt => opt.ResolveUsing(src => src.Symbol.ToUpperInvariant()))
                .ForMember(dest => dest.MarketCap, opt => opt.ResolveUsing<MarketCapValueResolver>())
                .ForMember(dest => dest.TotalVolume, opt => opt.ResolveUsing<TotalVolumeValueResolver>())
                .ForMember(dest => dest.CurrentPrice, opt => opt.ResolveUsing<CurrentPriceValueResolver>())
                .ForMember(dest => dest.PercentChange, opt => opt.ResolveUsing<PercentChangeValueResolver>());
        }
    }
}
