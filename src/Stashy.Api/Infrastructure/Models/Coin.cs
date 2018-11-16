namespace Stashy.Api.Infrastructure.Models
{
    public class Coin
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal MarketCap { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PercentChange { get; set; }
    }
}
