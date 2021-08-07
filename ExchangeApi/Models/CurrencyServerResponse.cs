using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeApi.Models
{
    public class CurrencyServerResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("terms")]
        public Uri Terms { get; set; }

        [JsonProperty("privacy")]
        public string Privacy { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("quotes")]
        public Dictionary<string, decimal> Quotes { get; set; }
    }

}