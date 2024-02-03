using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bank
{
    interface IAccountHolderService
    {
        void Deposite(string accountIdt, decimal amount);
        void Withdraw(string accountId, decimal amount);
        void TransactionHistory(Account account, TimeSpan timeRange);
        void TransferFunds(string bankName, string recipientAccountId, Account account, decimal amount, string serviceType);
    }
    internal class AccountHolderService : IAccountHolderService
    {
        public string bankId;
        private Bank _bank;
        public AccountService accountService;
        public SqlConnection connection;
        public AccountHolderService(string bankId,SqlConnection connect)
        {
            this.bankId = bankId;
            connection = connect;
            connection.Open();
            _bank = connection.QueryFirstOrDefault<Bank>("SELECT * FROM BANK WHERE BankId = @BankId", new { BankId = bankId });
            accountService = new AccountService(bankId, connection);
        }

        public void Deposite(string accountId,decimal amount)
        {
             accountService.Deposite(accountId, amount);
        }
        public void Withdraw(string accountId, decimal amount)
        {
            accountService.Withdraw(accountId, amount);
        }
        public void TransferFunds(string bankName,string recipientAccountId,Account account,decimal amount,string serviceType)
        {
            //if(_bank.BankName ==  bankName)
            //{
            //    var recipientAccount = _bank.accounts.Find(a => a.AccountId == recipientAccountId);
            //    if (recipientAccount is not null)
            //    {
            //        decimal taxAmount = FindSameBankTaxAmount(serviceType, amount);
            //        if (account.Balance + taxAmount >= amount)
            //        {
            //            PerformTransaction(account,recipientAccount,amount,taxAmount,_bank);
            //        }
            //        else
            //        {
            //            Console.WriteLine("Insufficient funds");
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("Recipient Account Not Found");
            //    }
            //}
            //else
            //{
            //    Bank recipientBank = CentralBank.banks.Find(a=>a.BankName == bankName);
            //    var recipientAccount = recipientBank.accounts.Find(a => a.AccountId == recipientAccountId);
            //    if (recipientAccount is not null)
            //    {
            //        decimal taxAmount = FindOtherBankTaxAmount(serviceType, amount);
            //        if (account.Balance + taxAmount >= amount)
            //        {
            //            PerformTransaction(account, recipientAccount, amount, taxAmount, recipientBank);
            //        }
            //        else
            //        {
            //            Console.WriteLine("Insufficient funds");
            //        }
            //    }
            //}
        }

        public void TransactionHistory(Account account, TimeSpan timeRange)
        {
           // accountService.TransactionHistory(account, timeRange);
        }

        private decimal FindOtherBankTaxAmount(string serviceType, decimal amount)
        {
            decimal taxAmount = 0;
            if (serviceType == "RTGS")
            {
                taxAmount = amount * _bank.RTGSChargeOtherBank;
            }
            else if (serviceType == "IMPS")
            {
                taxAmount = amount * _bank.IMPSChargeOtherBank;
            }
            else
            {
                Console.WriteLine("Enter Valid Service Type");
            }
            return taxAmount;
        }
        private decimal FindSameBankTaxAmount(string serviceType, decimal amount)
        {
            decimal taxAmount = 0;
            if (serviceType == "RTGS")
            {
                taxAmount = amount * _bank.RTGSChargeOtherBank;
            }
            else if (serviceType == "IMPS")
            {
                taxAmount = amount * _bank.IMPSChargeOtherBank;
            }
            else
            {
                Console.WriteLine("Enter Valid Service Type");
            }
            return taxAmount;
        }

        private void PerformTransaction(Account account,Account recipientAccount,decimal amount,decimal taxAmount,Bank myBank)
        {
            //account.Balance = account.Balance - amount - taxAmount;
            //recipientAccount.Balance += amount;
            //DateTime dateTime = DateTime.Now;
            //long time = dateTime.Ticks;
            //string transactionId = "TXN" + myBank.BankId + account.AccountId + time;
            //Transaction transaction = new Transaction(transactionId, account.AccountId, recipientAccount.AccountId, amount, dateTime);
            //account.transactions.Add(transaction);
            //recipientAccount.transactions.Add(transaction);
            //Console.WriteLine("Transfer successfull");
            //Console.WriteLine("Transaction Id : " + transactionId);
        }
    }
}
