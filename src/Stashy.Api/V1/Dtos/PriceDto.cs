namespace Stashy.Api.V1.Dtos
{
    public class PriceDto
    {
        public string Id { get; set; }
        public decimal MarketCap { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PercentChange { get; set; }
    }
}
