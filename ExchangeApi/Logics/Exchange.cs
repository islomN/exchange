using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExchangeApi.ActionResults;
using ExchangeApi.CurrencyLayer;
using ExchangeApi.Logics.Extensions;
using ExchangeApi.Logics.Helpers;
using ExchangeApi.Models;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using WCardAPI.Infrastructure.ActionResults;

namespace ExchangeApi.Logics
{
    public class Exchange: IExchange
    {
        private readonly IAppCache _cache;

        private const string CurrencyCacheName = "currencylayer";

        private string _sourceCurrency;
        private string _targetCurrency;
        private decimal _sourceAmount;
        
        private IEnumerable<KeyValuePair<string, decimal>> Currencies { get; set; }

        public Exchange(IAppCache cache)
        {
            _cache = cache;
        }

        public async Task<IActionResult> Convert(string sourceCurrencyParam, string targetCurrencyParam, decimal sourceAmountParam)
        {
            try
            {
                InitParams(sourceCurrencyParam, targetCurrencyParam, sourceAmountParam);
                var sourceAmountResult = CheckSourceAmount();
                if (!sourceAmountResult.Success)
                {
                    return new ErrorActionResult(sourceAmountResult.Message, HttpStatusCode.BadRequest);
                }
                
                var currenciesResult = await GetCurrencies();
                if (!currenciesResult.Success)
                {
                    return new ErrorActionResult(currenciesResult.Message);
                }

                Currencies = currenciesResult.Result;

                var currencyResult = CheckCurrency();
                if (!currencyResult.Success )
                {
                    return new ErrorActionResult(currencyResult.Message, HttpStatusCode.BadRequest);
                }

                var targetAmount = Calculate();
                return new SuccessActionResult(new ExchangeResponse
                {
                    SourceCurrency = _sourceCurrency,
                    TargetCurrency = _targetCurrency,
                    SourceAmount = _sourceAmount,
                    TargetAmount = targetAmount
                });
            }
            catch (Exception ex)
            {
                return new ErrorActionResult(ex.Message + "::"+"Service not available, try again later!");
            }
        }

        private void InitParams(string sourceCurrencyParam, string targetCurrencyParam, decimal sourceAmountParam)
        {
            _sourceCurrency = sourceCurrencyParam;
            _sourceAmount = sourceAmountParam;
            _targetCurrency = targetCurrencyParam;
        }

        private decimal Calculate()
        {
            if (_sourceCurrency == _targetCurrency)
            {
                return _sourceAmount;
            }
                        
            var sourceCurrencyRate = Currencies.GetCurrencyRate(_sourceCurrency);
            var targetCurrencyRate = Currencies.GetCurrencyRate(_targetCurrency);
            return Math.Round(targetCurrencyRate / sourceCurrencyRate * _sourceAmount, 4);
        }

        private TBaseResult<bool> CheckCurrency()
        {
            if (_sourceCurrency == null)
            {
                return new TBaseResult<bool>("Source currency cannot be null");
            }

            if (_targetCurrency == null)
            {
                return new TBaseResult<bool>("Target currency cannot be null");
            }

            _sourceCurrency = _sourceCurrency.ToUpper();
            _targetCurrency = _targetCurrency.ToUpper();
            
            if (!Currencies.HasCurrency(_sourceCurrency))
            {
                return new TBaseResult<bool>("Source currency not found");
            }
            
            if (!Currencies.HasCurrency(_targetCurrency))
            {
                return new TBaseResult<bool>("Target currency not found");
            }

            return new TBaseResult<bool>();
        }

        private TBaseResult<bool> CheckSourceAmount()
        {
            return _sourceAmount < 0 
                ? new TBaseResult<bool>("source amount must not be below zero") 
                : new TBaseResult<bool>();
        }
        
        private async Task<TBaseResult<IEnumerable<KeyValuePair<string, decimal>>>> GetCurrencies()
        {
            var currenciesResult = await _cache.GetAsync<IEnumerable<KeyValuePair<string, decimal>>>($"_{CurrencyCacheName}");
            if (currenciesResult != null)
            {
                return new TBaseResult<IEnumerable<KeyValuePair<string, decimal>>>(currenciesResult);
            }

            var currenciesInServerResult =  await CurrencyServer.GetCurrenciesInServer();
            if (!currenciesInServerResult.Success)
            {
                return new TBaseResult<IEnumerable<KeyValuePair<string, decimal>>>("Service not available, try again later!", false);
            }
            
            if (currenciesInServerResult.Result.Quotes.Any())
            {
                _cache.Add($"_{CurrencyCacheName}", currenciesInServerResult.Result.Quotes, TimeSpanHelper.GetTimeUntilEndOfHour());
            }
            return new TBaseResult<IEnumerable<KeyValuePair<string, decimal>>>(currenciesInServerResult.Result.Quotes);
        }
    }
}