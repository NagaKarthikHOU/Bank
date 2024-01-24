using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class BankService
    {
        public Bank bank;
        public BankService(Bank bank)
        {
            this.bank = bank;
        }
        public void CreateBankStaff(string name, string password)
        {
            Staff staff = new Staff(name, password);
            bank.staff.Add(staff);
        }
        public Staff AuthenticateStaff(string name, string password)
        {
            return bank.staff.Find(u => u.Name == name && u.Password == password);
        }
        public Account AuthenticateUser(string username, string password)
        {
            return bank.accounts.Find(u => u.AccountHolderName == username && u.Password == password);
        }
        public decimal ConvertCurrency(string currency, decimal amount)
        {
            amount = amount * bank.currency[currency];
            return amount;
        }
    }
}
