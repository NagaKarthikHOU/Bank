using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
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
        void TransactionHistory(string accountId, TimeSpan timeRange);
        void TransferFunds(string bankName, string recipientAccountId, Account account, decimal amount, string serviceType);
    }
    internal class AccountHolderService : IAccountHolderService
    {
        public string bankId;
        public AccountService accountService;
        private readonly string _connectionString;

        public AccountHolderService(string bankId,string connectionString)
        {
            this.bankId = bankId;
            _connectionString = connectionString;
            accountService = new AccountService(bankId, _connectionString);
        }

        public void Deposite(string accountId,decimal amount)
        {
             accountService.Deposite(accountId, amount);
        }
        public void Withdraw(string accountId, decimal amount)
        {
            accountService.Withdraw(accountId, amount);
        }
        public void TransferFunds(string recipientBankId,string recipientAccountId,Account account,decimal amount,string serviceType)
        {
            if (bankId == recipientBankId)
            {
                var recipientAccount = accountService.GetAccount(recipientAccountId);   
                if (recipientAccount is not null)
                {
                    decimal taxAmount = FindSameBankTaxAmount(serviceType, amount);
                    if (account.Balance + taxAmount >= amount)
                    {
                        PerformTransaction(account, recipientAccountId, amount, taxAmount);
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
                using (var connection = new SqlConnection(_connectionString))
                {
                    var recipientAccount = connection.QueryFirstOrDefault<Account>("Select * from Account where AccountId=@AccountId and BankId=@BankId", new {AccountId = recipientAccountId,BankId= recipientBankId });
                    if (recipientAccount is not null)
                    {
                        decimal taxAmount = FindOtherBankTaxAmount(serviceType, amount);
                        if (account.Balance + taxAmount >= amount)
                        {
                            PerformTransaction(account, recipientAccountId, amount, taxAmount);
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds");
                        }
                    }
                }
                
            }
        }

        public void TransactionHistory(string accountId, TimeSpan timeRange)
        {
           accountService.TransactionHistory(accountId, timeRange);
        }

        private decimal FindOtherBankTaxAmount(string serviceType, decimal amount)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Bank bank = connection.QueryFirstOrDefault<Bank>("Select * from Bank where BankId=@BankId", new {BankId=this.bankId});
                decimal taxAmount = 0;
                if (serviceType == "RTGS")
                {
                    taxAmount = amount * bank.RTGSChargeOtherBank;
                }
                else if (serviceType == "IMPS")
                {
                    taxAmount = amount * bank.IMPSChargeOtherBank;
                }
                else
                {
                    Console.WriteLine("Enter Valid Service Type");
                }
                return taxAmount;

            }
            
        }
        private decimal FindSameBankTaxAmount(string serviceType, decimal amount)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Bank bank = connection.QueryFirstOrDefault<Bank>("Select * from Bank where BankId=@BankId", new { BankId = this.bankId });
                decimal taxAmount = 0;
                if (serviceType == "RTGS")
                {
                    taxAmount = amount * bank.RTGSChargeSameBank;
                }
                else if (serviceType == "IMPS")
                {
                    taxAmount = amount * bank.IMPSChargeSameBank;
                }
                else
                {
                    Console.WriteLine("Enter Valid Service Type");
                }
                return taxAmount;

            }
        }

        private void PerformTransaction(Account account,string recipientAccountId,decimal amount,decimal taxAmount)
        {
            decimal sourceBalance = account.Balance - amount - taxAmount;
            decimal destinationBalance = account.Balance + amount;
            accountService.UpdateBalance(account.AccountId, sourceBalance);
            accountService.UpdateBalance(recipientAccountId, destinationBalance);
            DateTime dateTime = DateTime.Now;
            long time = dateTime.Ticks;
            string transactionId = "TXN" + account.BankId + account.AccountId + time;
            accountService.AddTransaction(transactionId, account.AccountId, recipientAccountId, amount, dateTime);
            Console.WriteLine("Transfer successfull");
            Console.WriteLine("Transaction Id : " + transactionId);
        }
    }
}
