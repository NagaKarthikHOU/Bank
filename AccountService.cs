using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bank
{
    class AccountService
    {
        private readonly string _connectionString;
        public string BankId { get; set; }
        public AccountService(string bankId, string connectionString)
        {
            _connectionString = connectionString;
            BankId = bankId;
        }
        public void Deposite(string accountId, decimal amount)
        {
            decimal balance = GetAccountBalance(accountId);
            balance += amount;
            UpdateBalance(accountId, balance); 
            DateTime dateTime = DateTime.Now;
            long time = dateTime.Ticks;
            string transactionId = "TXN" + BankId + accountId + time;
            AddTransaction(transactionId, accountId, accountId, amount, dateTime);
            Console.WriteLine("Amount added Successfully");
            Console.WriteLine("Transaction Id : " + transactionId);
        }
        public void Withdraw(string accountId, decimal amount)
        {
            decimal balance = GetAccountBalance(accountId);
            if (balance >= amount)
            {
                balance -= amount;
                UpdateBalance(accountId, balance);
                DateTime dateTime = DateTime.Now;
                long time = dateTime.Ticks;
                string transactionId = "TXN" + BankId + accountId + time;
                AddTransaction(transactionId, accountId, accountId, amount, dateTime);
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
            DateTime startDate = DateTime.Now - timeRange;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Account account = GetAccount(accountId);
                if (account != null)
                {
                    Console.WriteLine("Transaction history for "+ account.AccountHolderName + " account " + account.AccountId+" in the last "+ timeRange.TotalDays+" days:");
                    var filteredTransactions = connection.Query<TransactionHistory>("SELECT * FROM TransactionHistory WHERE (SourceAccountId = @SourceAccountId Or DestinationAccountId=@DestinationAccountId) AND Time >= @Time ORDER BY Time",
                                                                                     new { SourceAccountId = accountId, DestinationAccountId=accountId, Time = startDate });

                    if (filteredTransactions.Any())
                    {
                        foreach (var transaction in filteredTransactions)
                        {
                            Console.WriteLine("\nTransaction Id : " + transaction.TransactionId);

                            if (transaction.SourceAccountId == accountId)
                            {
                                Console.WriteLine("Dear user " + transaction.SourceAccountId + " debited by " + transaction.Amount + "INR on " + transaction.Time + " to " + transaction.DestinationAccountId);
                            }
                            else if (transaction.DestinationAccountId == accountId)
                            {
                                Console.WriteLine("Dear user " + transaction.SourceAccountId + " credited by " + transaction.Amount + "INR on " + transaction.Time + " by " + transaction.DestinationAccountId);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No transactions found in the specified time range.");
                    }
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
        }

        public Account GetAccount(string accountId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Account>("SELECT * FROM ACCOUNT WHERE AccountId = @AccountId", new { AccountId = accountId });
            }     
        }
        public decimal GetAccountBalance(string accountId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.ExecuteScalar<decimal>("SELECT Balance FROM Account WHERE AccountId = @AccountId", new { AccountId = accountId });
            }    
        }

        public Bank GetBank(string bankId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Bank>("SELECT * FROM BANK WHERE BankId = @BankId", new { BankId = bankId });
            }
        }
        public void AddTransaction(string transactionId,string sourceAccountId,string destinationAccountId,decimal amount,DateTime time)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("INSERT INTO TransactionHistory(TransactionId, SourceAccountId, DestinationAccountId, Amount, Time) VALUES (@TransactionId, @SourceAccountId, @DestinationAccountId, @Amount, @Time)",
                        new { TransactionId = transactionId, SourceAccountId = sourceAccountId, DestinationAccountId = destinationAccountId, Amount = amount, Time = time });
            }
        }
        public void UpdateBalance(string accountId,decimal balance) 
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("UPDATE ACCOUNT SET Balance = @Balance WHERE AccountId = @AccountId", new { Balance = balance, AccountId = accountId });
            }
        }
    }
}