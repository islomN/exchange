using System;
using System.Net;
using System.Threading.Tasks;
using ExchangeApi.Logics;
using ExchangeApi.Models;
using Newtonsoft.Json;
using RestSharp;

namespace ExchangeApi.CurrencyLayer
{
    public static class CurrencyLayerClient
    {
        public static async Task<TBaseResult<CurrencyServerResponse>> GetCurrenciesInServer()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                var client = new RestClient(CurrencyLayerConfiguration.Url)
                {
                    Timeout = 150000,
                    UseSynchronizationContext = true,
                    ReadWriteTimeout = 15000
                };

                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                var response = await client.ExecuteGetTaskAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<CurrencyServerResponse>(response.Content);
                    if (result == null)
                    {
                        return new TBaseResult<CurrencyServerResponse>("Service returned empty result");
                    }

                    if (!result.Success)
                    {
                        return new TBaseResult<CurrencyServerResponse>("Has error in service");
                    }
                    
                    return new TBaseResult<CurrencyServerResponse>(result);
                }

                return new TBaseResult<CurrencyServerResponse>("Service invalid", false);
            }
            catch (Exception ex)
            {
                return new TBaseResult<CurrencyServerResponse>("Service invalid");
            }
            
        }
    }
}