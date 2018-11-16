using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stashy.Api.Infrastructure.Clients.CoinGecko.Responses
{
    public class CoinItem
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("symbol")] public string Symbol { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("image")] public Image Image { get; set; }

        [JsonProperty("market_data")] public MarketData MarketData { get; set; }

        [JsonProperty("community_data")] public CommunityData CommunityData { get; set; }

        [JsonProperty("developer_data")] public DeveloperData DeveloperData { get; set; }

        [JsonProperty("public_interest_stats")]
        public PublicInterestStats PublicInterestStats { get; set; }

        [JsonProperty("last_updated")] public DateTimeOffset? LastUpdated { get; set; }

        [JsonProperty("localization")] public Dictionary<string, string> Localization { get; set; }
    }

    public class CommunityData
    {
        [JsonProperty("facebook_likes")] public long? FacebookLikes { get; set; }

        [JsonProperty("twitter_followers")] public long? TwitterFollowers { get; set; }

        [JsonProperty("reddit_average_posts_48h")]
        public double? RedditAveragePosts48H { get; set; }

        [JsonProperty("reddit_average_comments_48h")]
        public double? RedditAverageComments48H { get; set; }

        [JsonProperty("reddit_subscribers")] public long? RedditSubscribers { get; set; }

        [JsonProperty("reddit_accounts_active_48h")]
        public long? RedditAccountsActive48H { get; set; }
    }

    public class DeveloperData
    {
        [JsonProperty("forks")] public long? Forks { get; set; }

        [JsonProperty("stars")] public long? Stars { get; set; }

        [JsonProperty("subscribers")] public long? Subscribers { get; set; }

        [JsonProperty("total_issues")] public long? TotalIssues { get; set; }

        [JsonProperty("closed_issues")] public long? ClosedIssues { get; set; }

        [JsonProperty("pull_requests_merged")] public long? PullRequestsMerged { get; set; }

        [JsonProperty("pull_request_contributors")]
        public long? PullRequestContributors { get; set; }

        [JsonProperty("commit_count_4_weeks")] public long? CommitCount4Weeks { get; set; }
    }

    public class Image
    {
        [JsonProperty("thumb")] public Uri Thumb { get; set; }

        [JsonProperty("small")] public Uri Small { get; set; }

        [JsonProperty("large")] public Uri Large { get; set; }
    }

    public class MarketData
    {
        [JsonProperty("current_price")] public Dictionary<string, double?> CurrentPrice { get; set; }

        [JsonProperty("roi")] public object Roi { get; set; }

        [JsonProperty("market_cap")] public Dictionary<string, double?> MarketCap { get; set; }

        [JsonProperty("market_cap_rank")] public long? MarketCapRank { get; set; }

        [JsonProperty("total_volume")] public Dictionary<string, double?> TotalVolume { get; set; }

        [JsonProperty("high_24h")] public Dictionary<string, double?> High24H { get; set; }

        [JsonProperty("low_24h")] public Dictionary<string, double?> Low24H { get; set; }

        [JsonProperty("price_change_24h")] public string PriceChange24H { get; set; }

        [JsonProperty("price_change_percentage_24h")]
        public string PriceChangePercentage24H { get; set; }

        [JsonProperty("price_change_percentage_7d")]
        public string PriceChangePercentage7D { get; set; }

        [JsonProperty("price_change_percentage_14d")]
        public string PriceChangePercentage14D { get; set; }

        [JsonProperty("price_change_percentage_30d")]
        public string PriceChangePercentage30D { get; set; }

        [JsonProperty("price_change_percentage_60d")]
        public string PriceChangePercentage60D { get; set; }

        [JsonProperty("price_change_percentage_200d")]
        public string PriceChangePercentage200D { get; set; }

        [JsonProperty("price_change_percentage_1y")]
        public string PriceChangePercentage1Y { get; set; }

        [JsonProperty("market_cap_change_24h")]
        public string MarketCapChange24H { get; set; }

        [JsonProperty("market_cap_change_percentage_24h")]
        public string MarketCapChangePercentage24H { get; set; }

        [JsonProperty("price_change_24h_in_currency")]
        public Dictionary<string, double?> PriceChange24HInCurrency { get; set; }

        [JsonProperty("price_change_percentage_1h_in_currency")]
        public Dictionary<string, double?> PriceChangePercentage1HInCurrency { get; set; }

        [JsonProperty("price_change_percentage_24h_in_currency")]
        public Dictionary<string, double?> PriceChangePercentage24HInCurrency { get; set; }

        [JsonProperty("price_change_percentage_7d_in_currency")]
        public Dictionary<string, double?> PriceChangePercentage7DInCurrency { get; set; }

        [JsonProperty("price_change_percentage_14d_in_currency")]
        public Dictionary<string, double?> PriceChangePercentage14DInCurrency { get; set; }

        [JsonProperty("price_change_percentage_30d_in_currency")]
        public Dictionary<string, double?> PriceChangePercentage30DInCurrency { get; set; }

        [JsonProperty("price_change_percentage_60d_in_currency")]
        public Dictionary<string, double?> PriceChangePercentage60DInCurrency { get; set; }

        [JsonProperty("price_change_percentage_200d_in_currency")]
        public Dictionary<string, double?> PriceChangePercentage200DInCurrency { get; set; }

        [JsonProperty("price_change_percentage_1y_in_currency")]
        public Dictionary<string, double?> PriceChangePercentage1YInCurrency { get; set; }

        [JsonProperty("market_cap_change_24h_in_currency")]
        public Dictionary<string, double?> MarketCapChange24HInCurrency { get; set; }

        [JsonProperty("market_cap_change_percentage_24h_in_currency")]
        public Dictionary<string, double?> MarketCapChangePercentage24HInCurrency { get; set; }

        [JsonProperty("total_supply")] public long? TotalSupply { get; set; }

        [JsonProperty("circulating_supply")] public string CirculatingSupply { get; set; }
    }

    public class PublicInterestStats
    {
        [JsonProperty("alexa_rank")] public long? AlexaRank { get; set; }

        [JsonProperty("bing_matches")] public long? BingMatches { get; set; }
    }
}
