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
             DateTime time = DateTime.Now;
             string transactionId = "TXN" + bank.BankId + account.AccountId + time;
             Transaction transaction = new Transaction(transactionId,account.AccountId,account.AccountId,amount,time);
             bank.transactions.Add(transaction);
             Console.WriteLine("Amount added Successfully");
             Console.WriteLine("Transaction Id : " + transactionId);
        }
        public void Withdraw(Account account, decimal amount)
        {
            if (account.Balance >= amount)
            {
                account.Balance -= amount;
                DateTime time = DateTime.Now;
                string transactionId = "TXN" + bank.BankId + account.AccountId + time;
                Transaction transaction = new Transaction(transactionId, account.AccountId, account.AccountId, amount,time);
                bank.transactions.Add(transaction);
                Console.WriteLine("Withdraw Successfull");
                Console.WriteLine("Transaction Id : " + transactionId);
            }
            else
            {
                Console.WriteLine("Insufficient funds");
            }
        }
        public void TransferFunds(string bankName,string recipientAccountId,Account account,decimal amount,string serviceType)
        {
            decimal taxAmount = 0;
            if(bank.BankName ==  bankName)
            {
                var recipientAccount = bank.accounts.Find(a => a.AccountId == recipientAccountId);
                if (recipientAccount != null)
                {
                    if (account.Balance >= amount)
                    {
                        if (serviceType == "RTGS")
                        {
                            taxAmount = amount * bank.RTGSChargeSameBank;
                        }
                        else if (serviceType == "IMPS")
                        {
                            taxAmount = amount * bank.IMPSChargeSameBank;
                        }
                        account.Balance = account.Balance - amount - taxAmount;
                        recipientAccount.Balance += amount;
                        DateTime time = DateTime.Now;
                        string transactionId = "TXN" + bank.BankId + account.AccountId + time;
                        Transaction transaction = new Transaction(transactionId, account.AccountId, recipientAccountId, amount, time);
                        bank.transactions.Add(transaction);
                        Console.WriteLine("Transfer successfull");
                        Console.WriteLine("Transaction Id : " + transactionId);
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
            else
            {
                Bank recipientBank = SetUpBank.banks.Find(a=>a.BankName == bankName);
                var recipientAccount = recipientBank.accounts.Find(a => a.AccountId == recipientAccountId);
                if (recipientAccount != null)
                {
                    if (account.Balance >= amount)
                    {
                        if (serviceType == "RTGS")
                        {
                            taxAmount = amount * bank.RTGSChargeOtherBank;
                        }
                        else if (serviceType == "IMPS")
                        {
                            taxAmount = amount * bank.IMPSChargeOtherBank;
                        }
                        account.Balance = account.Balance - amount - taxAmount;
                        recipientAccount.Balance += amount;
                        DateTime time = DateTime.Now;
                        string transactionId = "TXN" + recipientBank.BankId + account.AccountId + time;
                        Transaction transaction = new Transaction(transactionId, account.AccountId, recipientAccountId, amount, time);
                        bank.transactions.Add(transaction);
                        Console.WriteLine("Transfer successfull");
                        Console.WriteLine("Transaction Id : " + transactionId);
                    }
                    else
                    {
                        Console.WriteLine("Insufficient funds");
                    }
                }
            }
        }
        public decimal ConvertCurrency(string currency,decimal amount)
        {
            amount = amount * bank.currency[currency];
            return amount;
        }
        public void TransactionHistory(string accountId)
        {
            foreach(var transaction in bank.transactions)
            {
                if(transaction.SourceAccountId == accountId)
                {
                    Console.WriteLine("\nTransaction Id : "+ transaction.TransactionId);
                    Console.WriteLine("Source AccountId : " + transaction.SourceAccountId);
                    Console.WriteLine("Amount Transfered : " + transaction.Amount);
                    Console.WriteLine("Destination AccountId : " + transaction.DestinationAccountId);
                    Console.WriteLine("Transaction Time : " + transaction.Time+"\n");
                }
            }
        }
    }
}
