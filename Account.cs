using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bank
{
    class Account
    {
        public string AccountId { get; set; }
        public string AccountHolderName {  get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; } = 0;
        public string BankId { get; set; }
        public Account()
        {

        }

    }
}
