using Dapper;
using Microsoft.Data.SqlClient;
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
        void RevertTransaction(Account sourceAccount, string recipientBankName, string transactionId);
    }
    class BankStaffService:IBankStaffService
    {
        private Bank _bank;
        private AccountService accountService;
        public SqlConnection connection;
        public string bankId;
        public BankStaffService(string bankId,SqlConnection connect)
        {
            this.bankId = bankId;
            connection = connect;
            connection.Open();
            _bank = connection.QueryFirstOrDefault<Bank>("SELECT * FROM BANK WHERE BankId = @BankId", new { BankId = bankId });
            accountService = new AccountService(bankId, connection);
        }

        public void CreateAccount(string username, string password)
        {
            string accountId = username.Substring(0, 3) + DateTime.Now.ToString("ddMMyyyy");
            connection.Open();
            connection.Execute("INSERT INTO Account (AccountId, AccountHolderName, Password) VALUES (@AccountId, @AccountHolderName, @Password)",
                    new { AccountId = accountId, AccountHolderName = username, Password = password});
   
            Console.WriteLine("Account Created Successfully.\n"+"Account Name : "+username+"\nAccount Id : "+accountId+"\n");
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
                    connection.Execute("UPDATE Account SET AccountHolderName = @AccountHolderName WHERE AccountId = @AccountId",
                    new { AccountHolderName = username, AccountId = accountId });
                    Console.WriteLine("UserName Successfully Changed");
                }
                else if (option == 2)
                {
                    Console.Write("Enter New Password : ");
                    string password = Helper.Input<string>();
                    connection.Execute("UPDATE ACCOUNT SET Password = @Password WHERE AccountId = @Accountid", new {Password=password,AccountId=accountId});
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
            Account account = accountService.GetAccount(accountId);
            if(account is not null)
            {
                connection.Execute("DELETE FROM Account WHERE AccountId = @AccountId", new {AccountId=accountId});
                Console.WriteLine("Account Successfully Removed");
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
            connection.Open();
            connection.Execute("INSERT INTO Currency(CurrencyCode,ExchangeRate) VALUES (@CurrencyCode,@ExchangeRate)", new {CurrencyCode=currency,ExchangeRate=exchangeRate});
            Console.WriteLine("Currency Added Successfully");
        }
        public void AddServiceChargeSameBank(string serviceType, decimal charge)
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
        public void AddServiceChargeOtherBank(string serviceType, decimal charge)
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
        public void TransactionHistory(string accountId, TimeSpan timeRange)
        {
            accountService.TransactionHistory(accountId, timeRange);
        }
        public void RevertTransaction(Account sourceAccount,string recipientBankName,string transactionId)
        {

            //Transaction transaction = sourceAccount.transactions.Find(a => a.TransactionId == transactionId);
            //if (_bank.BankName == recipientBankName)
            //{
            //    var destinationAccount = _bank.accounts.Find(a => a.AccountId == transaction.DestinationAccountId);
            //    if(sourceAccount is not null && destinationAccount is not null)
            //    {
            //        PerformRevertTransaction(transaction,sourceAccount, destinationAccount,transactionId);
            //    }
            //}
            //else
            //{
            //    var anotherBank = CentralBank.banks.Find(a=>a.BankName == recipientBankName);
            //    var destinationAccount = anotherBank.accounts.Find(a => a.AccountId == transaction.DestinationAccountId);
            //    if (sourceAccount is not null && destinationAccount is not null)
            //    {
            //      PerformRevertTransaction(transaction, sourceAccount, destinationAccount, transactionId);
            //    }
            //}
        }
        private void PerformRevertTransaction(Transaction transaction,Account sourceAccount,Account destinationAccount,string transactionId)
        {
            //sourceAccount.Balance += transaction.Amount;
            //destinationAccount.Balance -= transaction.Amount;
            //DateTime dateTime = DateTime.Now;
            //long time = dateTime.Ticks;
            //string revertTransactionId = "TXN" + _bank.BankId + sourceAccount.AccountId + time;
            //Transaction revertTransaction = new Transaction(transactionId, sourceAccount.AccountId, destinationAccount.AccountId, transaction.Amount, dateTime);
            //sourceAccount.transactions.Add(revertTransaction);
            //destinationAccount.transactions.Add(revertTransaction);
            //Console.WriteLine("Transaction Reverted Successfully");
        }


    }
}
