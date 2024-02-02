using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class Currency
    {
        public string CurrencyName {  get; set; }
        public decimal ExchangeRate {  get; set; }
        public Currency(string currencyName, decimal exchangeRate)
        {
            CurrencyName = currencyName;
            ExchangeRate = exchangeRate;
        }   
    }
}
