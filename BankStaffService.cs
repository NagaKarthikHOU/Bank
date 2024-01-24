using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bank
{
    interface IBankStaffService
    {
        void CreateAccount(string username, string password);
        void UpdateAccount(string accountId);
        void DeleteAccount(string accountId);
        void AddCurrency(string currency, decimal exchangeRate);
        void AddServiceChargeSameBank(string serviceType, decimal charge);
        void AddServiceChargeOtherBank(string serviceType, decimal charge);
        void ViewTransactionHistory(string accountId);
        void RevertTransaction(string recipientBankName, string transactionId);
    }
    class BankStaffService:IBankStaffService
    {
        private Bank bank;
        public BankStaffService(Bank bank)
        {
            this.bank = bank;
        }
        
        public void CreateAccount(string username, string password)
        {
            string accountId = username.Substring(0, 3) + DateTime.Now.ToString("ddMMyyyy");
            Account account = new Account(accountId, username, password);
            bank.accounts.Add(account);
            Console.WriteLine("Account Created Successfully.\n"+"Account Name : "+username+"\nAccount Id : "+accountId+"\n");
        }
        public void UpdateAccount(string accountId)
        {
            Account account = bank.accounts.Find(a => a.AccountId == accountId);
            if(account != null)
            {
                Console.WriteLine("1. Change UserName\n2. Change password");
                int option = Helper.Input<int>();
                if (option == 1)
                {
                    Console.Write("Enter New UserName : ");
                    string username = Helper.Input<string>();
                    account.AccountHolderName = username;
                    Console.WriteLine("UserName Successfully Changed");
                }
                else if (option == 2)
                {
                    Console.Write("Enter New Password : ");
                    string password = Helper.Input<string>();
                    account.Password = password;
                    Console.Write("Password Successfully Changed");
                }
            }
            else
            {
                Console.WriteLine("Account not found");
            }
        }
        public void DeleteAccount(string accountId)
        {
            Account account = bank.accounts.Find(a => a.AccountId == accountId);
            if(account != null)
            {
                bank.accounts.Remove(account);
                Console.WriteLine("Account Successfully Removed");
            }
            else
            {
                Console.WriteLine("Account not found");
            }
        }
        public void AddCurrency(string currency, decimal exchangeRate)
        {
            bank.currency.Add(currency, exchangeRate);
        }
        public void AddServiceChargeSameBank(string serviceType, decimal charge)
        {
            if (serviceType == "RTGS")
            {
                bank.RTGSChargeSameBank = charge;
            }
            else if (serviceType == "IMPS")
            {
                bank.IMPSChargeSameBank = charge;
            }
            else
            {
                Console.WriteLine("Invalid service type.");
            }
        }
        public void AddServiceChargeOtherBank(string serviceType, decimal charge)
        {
            if (serviceType == "RTGS")
            {
                bank.RTGSChargeOtherBank = charge;
            }
            else if (serviceType == "IMPS")
            {
                bank.IMPSChargeOtherBank = charge;
            }
            else
            {
                Console.WriteLine("Invalid service type.");
            }
        }
        public void ViewTransactionHistory(string accountId)
        {
            foreach (var transaction in bank.transactions)
            {
                if (transaction.SourceAccountId == accountId || transaction.DestinationAccountId == accountId)
                {
                    Console.WriteLine("\nTransaction Id : " + transaction.TransactionId);
                    Console.WriteLine(transaction.Amount + " INR is debited in " + transaction.SourceAccountId);
                    Console.WriteLine(transaction.Amount + " INR is credited to " + transaction.DestinationAccountId);
                }
            }
        }
        public void RevertTransaction(string recipientBankName,string transactionId)
        {
            if(bank.BankName == recipientBankName)
            {
                Transaction transaction = bank.transactions.Find(a => a.TransactionId == transactionId);
                var sourceAccount = bank.accounts.Find(a => a.AccountId == transaction.SourceAccountId);
                var destinationAccount = bank.accounts.Find(a => a.AccountId == transaction.DestinationAccountId);
                if(sourceAccount != null && destinationAccount != null)
                {
                    sourceAccount.Balance += transaction.Amount;
                    destinationAccount.Balance -= transaction.Amount;
                    bank.transactions.Remove(transaction);
                    Console.WriteLine("Transaction Reverted Successfully");
                }
            }
            else
            {
                var anotherBank = StartUp.banks.Find(a=>a.BankName == recipientBankName);
                Transaction transaction = bank.transactions.Find(a => a.TransactionId == transactionId);
                var sourceAccount = bank.accounts.Find(a => a.AccountId == transaction.SourceAccountId);
                var destinationAccount = anotherBank.accounts.Find(a => a.AccountId == transaction.DestinationAccountId);
                if (sourceAccount != null && destinationAccount != null)
                {
                    sourceAccount.Balance += transaction.Amount;
                    destinationAccount.Balance -= transaction.Amount;
                    bank.transactions.Remove(transaction);
                    Console.WriteLine("Transaction Reverted Successfully");
                }
            }
        }
    }
}
