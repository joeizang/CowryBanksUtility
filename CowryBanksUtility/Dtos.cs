using System.Text.Json.Serialization;

namespace CowryBanksUtility
{
    public class DtoBase
    {
        [JsonPropertyName("errors")]
        public object Errors { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class Bank
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("acronym")]
        public string Acronym { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("nip_bank_code")]
        public object NipBankCode { get; set; }
    }

    public class BankResponse : DtoBase
    {
        [JsonPropertyName("data")]
        public List<Bank> Data { get; set; }
    }

    public class GetPaginatedResponseInputModel
    {
        [JsonPropertyName("page_size")]
        public string PageSize { get; set; } = 20.ToString();

        [JsonPropertyName("page")]
        public string Page { get; set; } = 1.ToString();
    }

    public class AssetPrice
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("buy_price")]
        public double BuyPrice { get; set; }

        [JsonPropertyName("sell_price")]
        public double SellPrice { get; set; }

        [JsonPropertyName("annual_returns")]
        public double AnnualReturns { get; set; }
    }

    public class Meta
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("fund_manager")]
        public string FundManager { get; set; }

        [JsonPropertyName("is_eurobond")]
        public bool IsEurobond { get; set; }

        [JsonPropertyName("is_bond")]
        public bool IsBond { get; set; }

        [JsonPropertyName("is_money_market_fund")]
        public bool IsMoneyMarketFund { get; set; }

        [JsonPropertyName("is_equity_fund")]
        public bool IsEquityFund { get; set; }

        [JsonPropertyName("risk_class")]
        public string RiskClass { get; set; }

        [JsonPropertyName("price")]
        public AssetPrice Price { get; set; }
    }

    public class SingleAssetData
    {
        [JsonPropertyName("asset_id")]
        public string AssetId { get; set; }

        [JsonPropertyName("asset_code")]
        public string AssetCode { get; set; }

        [JsonPropertyName("asset_type")]
        public string AssetType { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("is_indexable")]
        public bool IsIndexable { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("asset_class")]
        public string AssetClass { get; set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }
    }

    public class Pagination
    {
        [JsonPropertyName("next")]
        public string Next { get; set; }

        [JsonPropertyName("previous")]
        public object Previous { get; set; }

        [JsonPropertyName("current_page")]
        public string CurrentPage { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    public class AssetResponse
    {
        [JsonPropertyName("data")]
        public List<SingleAssetData> Assets { get; set; } = new();

        [JsonPropertyName("pagination")]
        public Pagination Pagination { get; set; }

        [JsonPropertyName("errors")]
        public object Errors { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
