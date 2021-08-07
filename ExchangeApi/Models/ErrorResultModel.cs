using Newtonsoft.Json;

namespace ExchangeApi.Models
{
    public class ErrorResultModel
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}