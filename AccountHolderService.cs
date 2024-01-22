using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class AccountHolderService
    {
        private Bank bank;
        public AccountHolderService(Bank bank)
        {
            this.bank = bank;
        }
        public Account AuthenticateUser(string username, string password)
        {
            return bank.accounts.Find(u => u.AccountHolderName == username && u.Password == password);
        }
        public void Deposite(Account account,decimal amount)
        {
             account.Balance += amount;
             Console.WriteLine("Amount added Successfully"); 
        }
        public void Withdraw(Account account, decimal amount)
        {
            if (account.Balance >= amount)
            {
                account.Balance -= amount;
                Console.WriteLine("Withdraw Successfull");
            }
            else
            {
                Console.WriteLine("Insufficient funds");
            }
        }
        public void TransferFunds(string recipientAccountId,Account account,decimal amount)
        {
            var recipientAccount = bank.accounts.Find(a => a.AccountId == recipientAccountId);
            if (recipientAccount != null)
            {
                if (account.Balance >= amount)
                {
                    account.Balance -= amount;
                    recipientAccount.Balance += amount;
                    Console.WriteLine("Transfer successfull");
                }
                else
                {
                    Console.WriteLine("Insufficient funds");
                }
            }
            else
            {
                Console.WriteLine("Recipient Account Not Found");
            }
        }
    }
}
