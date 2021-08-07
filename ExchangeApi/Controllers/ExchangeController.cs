using System.Threading.Tasks;
using ExchangeApi.Logics;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeApi.Controllers
{
    [Route("api/exchange")]
    public class ExchangeController: ControllerBase
    {
        private readonly IExchange _exchange;

        public ExchangeController(IExchange exchange)
        {
            _exchange = exchange;
        }

        [HttpGet("convert")]
        public async Task<IActionResult> Convert(string sourceCurrency, string targetCurrency, decimal sourceAmount)
        {
            return await _exchange.Convert(sourceCurrency, targetCurrency, sourceAmount);
        }
    }
}