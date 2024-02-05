using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
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
        void Deposite(string accountId, decimal amount);
        void Withdraw(string accountId, decimal amount);
        void AddCurrency(string currency, decimal exchangeRate);
        void AddServiceChargeSameBank(string serviceType, decimal charge);
        void AddServiceChargeOtherBank(string serviceType, decimal charge);
        void TransactionHistory(string accountId, TimeSpan timeRange);
        void RevertTransaction(string accountId, string recipientAccountId, string transactionId);
    }
    class BankStaffService:IBankStaffService
    {
        private AccountService accountService;
        private readonly string _connectionString;
        public string bankId;
        public BankStaffService(string bankId,string connectionString)
        {
            this.bankId = bankId;
            _connectionString = connectionString;  
            accountService = new AccountService(bankId, _connectionString);
        }

        public void CreateAccount(string username, string password)
        {
            string accountId = username.Substring(0, 3) + DateTime.Now.ToString("ddMMyyyy");
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("INSERT INTO Account (AccountId, AccountHolderName, Password, BankId) VALUES (@AccountId, @AccountHolderName, @Password, @BankId)",
                        new { AccountId = accountId, AccountHolderName = username, Password = password, BankId = bankId });
                Console.WriteLine("Account Created Successfully.\n" + "Account Name : " + username + "\nAccount Id : " + accountId + "\n");
            }
        }
        public void UpdateAccount(string accountId)
        {
            Account account = accountService.GetAccount(accountId);
            if(account is not null)
            {
                Console.WriteLine("1. Change UserName\n2. Change password");
                int option = Helper.Input<int>();
                if (option == 1)
                {
                    Console.Write("Enter New UserName : ");
                    string username = Helper.Input<string>();
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        connection.Execute("UPDATE Account SET AccountHolderName = @AccountHolderName WHERE AccountId = @AccountId",
                             new { AccountHolderName = username, AccountId = accountId });
                        Console.WriteLine("UserName Successfully Changed");
                    }    
                }
                else if (option == 2)
                {
                    Console.Write("Enter New Password : ");
                    string password = Helper.Input<string>();
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        connection.Execute("UPDATE ACCOUNT SET Password = @Password WHERE AccountId = @Accountid", new { Password = password, AccountId = accountId });
                        Console.Write("Password Successfully Changed");
                    }      
                }
            }
            else
            {
                Console.WriteLine("Account not found");
            }
        }
        public void DeleteAccount(string accountId)
        {
            Account account = accountService.GetAccount(accountId);
            if(account is not null)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    connection.Execute("DELETE FROM Account WHERE AccountId = @AccountId", new { AccountId = accountId });
                    Console.WriteLine("Account Successfully Removed");
                }    
            }
            else
            {
                Console.WriteLine("Account not found");
            }
        }
        public void Deposite(string accountId, decimal amount)
        {
            accountService.Deposite(accountId, amount);
        }
        public void Withdraw(string accountId, decimal amount)
        {
            accountService.Withdraw(accountId, amount);
        }
        public void AddCurrency(string currency, decimal exchangeRate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                bool currencyExists = connection.ExecuteScalar<bool>("SELECT 1 FROM Currency WHERE CurrencyCode = @CurrencyCode", new { CurrencyCode = currency });
                if (currencyExists)
                {
                    Console.WriteLine(currency + " is already exists in the database.");
                }
                else
                {
                    connection.Execute("INSERT INTO Currency(CurrencyCode,ExchangeRate) VALUES (@CurrencyCode,@ExchangeRate)", new { CurrencyCode = currency, ExchangeRate = exchangeRate });
                    Console.WriteLine("Currency Added Successfully");
                }  
            }   
        }
        public void AddServiceChargeSameBank(string serviceType, decimal charge)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                if (serviceType == "RTGS")
                {
                    connection.Execute("UPDATE BANK SET RTGSChargeSameBank=@RTGSChargeSameBank WHERE BankId=@BankId", new { RTGSChargeSameBank = charge, BankId = bankId });
                }
                else if (serviceType == "IMPS")
                {
                    connection.Execute("UPDATE BANK SET IMPSChargeSameBank=@IMPSChargeSameBank WHERE BankId=@BankId", new { IMPSChargeSameBank = charge, BankId = bankId });
                }
                else
                {
                    Console.WriteLine("Invalid service type.");
                }
            }
                
        }
        public void AddServiceChargeOtherBank(string serviceType, decimal charge)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                if (serviceType == "RTGS")
                {
                    connection.Execute("UPDATE BANK SET RTGSChargeOtherBank=@RTGSChargeOtherBank WHERE BankId=@BankId", new { RTGSChargeOtherBank = charge, BankId = bankId });
                }
                else if (serviceType == "IMPS")
                {
                    connection.Execute("UPDATE BANK SET IMPSChargeOtherBank=@IMPSChargeOtherBank WHERE BankId=@BankId", new { IMPSChargeOtherBank = charge, BankId = bankId });
                }
                else
                {
                    Console.WriteLine("Invalid service type.");
                }
            }
        }
        public void TransactionHistory(string accountId, TimeSpan timeRange)
        {
            accountService.TransactionHistory(accountId, timeRange);
        }
        public void RevertTransaction(string sourceAccountId,string recipientAccountId,string transactionId)
        {
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var transaction = connection.QueryFirstOrDefault<TransactionHistory>("Select * from TransactionHistory where TransactionId = @TransactionId", new {TransactionId = transactionId});
                var sourceAccount = accountService.GetAccount(sourceAccountId);
                var destinationAccount = accountService.GetAccount(recipientAccountId);
                if (sourceAccount is not null && destinationAccount is not null)
                {
                    decimal sourceBalance = sourceAccount.Balance + transaction.Amount;
                    decimal destinationBalance = destinationAccount.Balance - transaction.Amount;
                    accountService.UpdateBalance(sourceAccountId, sourceBalance);
                    accountService.UpdateBalance(recipientAccountId, destinationBalance);
                    DateTime dateTime = DateTime.Now;
                    long time = dateTime.Ticks;
                    string revertTransactionId = "TXN" + bankId + sourceAccountId + time;
                    accountService.AddTransaction(revertTransactionId, sourceAccountId, recipientAccountId, transaction.Amount, dateTime);
                    Console.WriteLine("Transaction Reverted Successfully");
                }
            }         
        }
    }
}
