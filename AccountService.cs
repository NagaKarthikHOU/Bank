using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class AccountService
    {
        public Bank bank;
        public SqlConnection connection;
        public AccountService(string bankId, SqlConnection connect)
        {
            connection = connect;
            bank = connection.QueryFirstOrDefault<Bank>("SELECT * FROM BANK WHERE BankId = @BankId", new { BankId = bankId });
        }
        public void Deposite(string accountId, decimal amount)
        {
            decimal balance = GetAccountBalance(accountId);
            balance += amount;
            Account account = GetAccount(accountId);
            connection.Open();
            connection.Execute("UPDATE ACCOUNT SET Balance = @Balance WHERE AccountId = @AccountId", new { Balance = balance, AccountId=accountId});
            account.Balance += amount;
            DateTime dateTime = DateTime.Now;
            long time = dateTime.Ticks;
            string transactionId = "TXN" + bank.BankId + account.AccountId + time;

            connection.Open();
            connection.Execute("INSERT INTO Transaction (TransactionId, SourceAccountId, DestinationAccountId, Amount, Time) VALUES (@TransactionId, @SourceAccountId, @DestinationAccountId, @Amount, @Time)",
                new { TransactionId = transactionId, SourceAccountId = accountId, DestinationAccountId = accountId, Amount = amount, Time = dateTime });

            Console.WriteLine("Amount added Successfully");
            Console.WriteLine("Transaction Id : " + transactionId);
        }
        public void Withdraw(string accountId, decimal amount)
        {
            decimal balance = GetAccountBalance(accountId);
            if (balance >= amount)
            {
                balance -= amount;
                Account account = GetAccount(accountId);
                DateTime dateTime = DateTime.Now;
                long time = dateTime.Ticks;
                string transactionId = "TXN" + bank.BankId + account.AccountId + time;

                connection.Open();
                connection.Execute("INSERT INTO Transaction (TransactionId, SourceAccountId, DestinationAccountId, Amount, Time) VALUES (@TransactionId, @SourceAccountId, @DestinationAccountId, @Amount, @Time)",
                    new { TransactionId = transactionId, SourceAccountId = accountId, DestinationAccountId = accountId, Amount = amount, Time = dateTime });

                Console.WriteLine("Withdraw Successfull");
                Console.WriteLine("Transaction Id : " + transactionId);
            }
            else
            {
                Console.WriteLine("Insufficient funds");
            }
        }
        public void TransactionHistory(string accountId, TimeSpan timeRange)
        {
            //DateTime startDate = DateTime.Now - timeRange;

            //Console.WriteLine("Transaction history for " + account.AccountHolderName + "'s account " + account.AccountId + " in the last " + timeRange.TotalDays + " days:");

            //var filteredTransactions = account.transactions
            //    .Where(transaction => transaction.Time >= startDate)
            //    .OrderBy(transaction => transaction.Time);

            //if (filteredTransactions.Any())
            //{
            //    foreach (var transaction in filteredTransactions)
            //    {
            //        Console.WriteLine("\nTransaction Id : " + transaction.TransactionId);
            //        if (transaction.SourceAccountId == account.AccountId)
            //        {
            //            Console.WriteLine("Dear user " + transaction.SourceAccountId + "debited by " + transaction.Amount + "INR on " + transaction.Time + " to " + transaction.DestinationAccountId);
            //        }
            //        else if (transaction.DestinationAccountId == account.AccountId)
            //        {
            //            Console.WriteLine("Dear user " + transaction.SourceAccountId + "credited by " + transaction.Amount + "INR on " + transaction.Time + " by " + transaction.DestinationAccountId);
            //        }
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No transactions found in the specified time range.");
            //}
        }
        public Account GetAccount(string accountId)
        {
            connection.Open();
            return connection.QueryFirstOrDefault<Account>("SELECT * FROM ACCOUNT WHERE AccountId = @AccountId", new { AccountId = accountId });
        }
        public decimal GetAccountBalance(string accountId)
        {
            return connection.ExecuteScalar<decimal>("SELECT Balance FROM Account WHERE AccountId = @AccountId", new { AccountId = accountId });
        }
    }
}