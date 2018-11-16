using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Stashy.Api.Infrastructure.Clients.CoinGecko.Responses;
using Stashy.Api.Infrastructure.Models;

namespace Stashy.Api.Infrastructure.Clients.CoinGecko
{
    public abstract class BaseValueResolver : IValueResolver<CoinItem, Coin, decimal>
    {
        public abstract decimal Resolve(CoinItem source, Coin destination, decimal destMember,
            ResolutionContext context);

        protected decimal Resolve(Dictionary<string, double?> dictionary)
        {
            double result = dictionary.FirstOrDefault(item => item.Key == "usd").Value ?? 0;

            return Math.Round((decimal) result, 2, MidpointRounding.ToEven);
        }
    }

    public class MarketCapValueResolver : BaseValueResolver
    {
        public override decimal Resolve(CoinItem source, Coin destination, decimal destMember,
            ResolutionContext context)
        {
            return Resolve(source.MarketData?.MarketCap);
        }
    }

    public class TotalVolumeValueResolver : BaseValueResolver
    {
        public override decimal Resolve(CoinItem source, Coin destination, decimal destMember,
            ResolutionContext context)
        {
            return Resolve(source?.MarketData.TotalVolume);
        }
    }

    public class CurrentPriceValueResolver : BaseValueResolver
    {
        public override decimal Resolve(CoinItem source, Coin destination, decimal destMember,
            ResolutionContext context)
        {
            return Resolve(source?.MarketData.CurrentPrice);
        }
    }

    public class PercentChangeValueResolver : BaseValueResolver
    {
        public override decimal Resolve(CoinItem source, Coin destination, decimal destMember,
            ResolutionContext context)
        {
            return Resolve(source?.MarketData.PriceChangePercentage24HInCurrency);
        }
    }
}
