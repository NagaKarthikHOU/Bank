using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class CentralBank
    {
        public static List<Bank> banks = new List<Bank>();
        public void SetUpBank()
        {
            Console.WriteLine("Enter Bank Name : ");
            string bankName = Helper.Input<string>().ToLower();
            Bank bank = new Bank(bankName);
            banks.Add(bank);
            Helper.BankSimulation(bank.BankId);
        }
        public void ContinueWithExistingBank()
        {
            Console.WriteLine("Enter Bank Name : ");
            string bankName = Helper.Input<string>();
            Bank bank = banks.FirstOrDefault(a => a.BankName == bankName);
            Helper.BankSimulation(bank.BankId);
        }
    }
}
