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


        public Bank(string bankName,string bankId,decimal rtgsSame,decimal impsSame,decimal rtgsDifferent,decimal impsdifferent)
        {
            BankName = bankName;
            BankId = bankId;
            RTGSChargeSameBank = rtgsSame;
            IMPSChargeSameBank = impsSame;
            RTGSChargeOtherBank = rtgsDifferent;
            RTGSChargeSameBank= impsdifferent;
            //currency.Add(new Currency("INR",1));
        }
    }
}
