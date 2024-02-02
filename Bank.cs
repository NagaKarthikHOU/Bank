using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class Bank
    {
        public string BankName { get; set; }
        public string BankId { get; set; }
        public decimal RTGSChargeSameBank { get; set; } = 0;
        public decimal IMPSChargeSameBank { get; set; } = (decimal)0.05;
        public decimal RTGSChargeOtherBank { get; set; } = (decimal)0.02;
        public decimal IMPSChargeOtherBank { get; set; } = (decimal)0.06;

        public List<Staff> staff = new List<Staff>();
        public List<Account> accounts = new List<Account>();
        public List<Currency> currency = new List<Currency>();

        public Bank(string bankName)
        {
            BankName = bankName;
            BankId = bankName.Substring(0,3) + DateTime.Now.ToString("ddMMyyyy");
            currency.Add(new Currency("INR",1));
        }
    }
}
