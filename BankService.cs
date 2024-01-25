using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class BankService
    {
        public string bankId;
        public Bank bank;
        public BankService(string bankId)
        {
            this.bankId = bankId;
            Bank bank = CentralBank.banks.Find(x => x.BankId == bankId);
        }
        public static void GetBank(string bankId)
        {
            Bank bank = CentralBank.banks.Find(x => x.BankId == bankId);
        }
        
        public void CreateBankStaff(string name, string password)
        {
            
            Staff staff = new Staff(name, password);
            bank.staff.Add(staff);
        }
        public Staff AuthenticateStaff(string name, string password)
        {
            Bank bank = CentralBank.banks.Find(x => x.BankId == bankId);
            return bank.staff.Find(u => u.Name == name && u.Password == password);
        }
        public Account AuthenticateUser(string username, string password)
        {
            Bank bank = CentralBank.banks.Find(x => x.BankId == bankId);
            return bank.accounts.Find(u => u.AccountHolderName == username && u.Password == password);
        }
        public decimal ConvertCurrency(string currency, decimal amount)
        {
            Bank bank = CentralBank.banks.Find(x => x.BankId == bankId);
            amount *=  bank.currency[currency];
            return amount;
        }
    }
}
