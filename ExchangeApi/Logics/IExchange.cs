using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeApi.Logics
{
    public interface IExchange
    {
        Task<IActionResult> Convert(string sourceCurrencyParam, string targetCurrencyParam, decimal sourceAmountParam);
    }
}