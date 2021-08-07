using System.Collections.Generic;
using System.Linq;

namespace ExchangeApi.Logics.Extensions
{
    public static class CurrencyExtensions
    {
        private const string DefaultCurrency = "USD";
        public static bool HasCurrency(this IEnumerable<KeyValuePair<string, decimal>> currencies, string currency)
        {
            if (IsDefaultCurrency(currency))
            {
                return true;
            }
            var currencyKey = GetKey(currency);
            return currencies.Any(i => i.Key == currencyKey);
        }

        public static decimal GetCurrencyRate(this IEnumerable<KeyValuePair<string, decimal>> currencies,
            string currency)
        {
            if (IsDefaultCurrency(currency))
            {
                return 1;
            }
            var currencyKey = GetKey(currency);
            return currencies.FirstOrDefault(i => i.Key == currencyKey).Value;
        }

        private static string GetKey(string currency)
        {
            return DefaultCurrency + currency;
        }

        private static bool IsDefaultCurrency(string currency)
        {
            return currency == DefaultCurrency;
        }
    }
}