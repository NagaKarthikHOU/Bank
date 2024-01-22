using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Bank
    {
        public string BankName { get; set; }
        public string BankId { get; set; }
        public List<Staff> staff = new List<Staff>();
        public List<Account> accounts = new List<Account>();

        public Bank(string bankName)
        {
            BankName = bankName;
            BankId = bankName.Substring(0,3) + DateTime.Now.ToString("M/d/yyyy");
        }
    }
}
