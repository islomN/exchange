using Newtonsoft.Json;

namespace ExchangeApi.Models
{
    public class ExchangeResponse
    {
        [JsonProperty("sourceCurrency")]
        public string SourceCurrency { get; set; }

        [JsonProperty("targetCurrency")]
        public string TargetCurrency { get; set; }

        [JsonProperty("sourceAmount")]
        public decimal SourceAmount { get; set; }

        [JsonProperty("targetAmount")]
        public decimal TargetAmount { get; set; }
    }
}