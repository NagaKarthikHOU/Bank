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
        public AccountService(string bankId)
        {
            bank = CentralBank.banks.Find(x => x.BankId == bankId);
        }
        public void Deposite(Account account, decimal amount)
        {
            account.Balance += amount;
            DateTime dateTime = DateTime.Now;
            long time = dateTime.Ticks;
            string transactionId = "TXN" + bank.BankId + account.AccountId + time;
            Transaction transaction = new Transaction(transactionId, account.AccountId, account.AccountId, amount, dateTime);
            account.transactions.Add(transaction);
            Console.WriteLine("Amount added Successfully");
            Console.WriteLine("Transaction Id : " + transactionId);
        }
        public void Withdraw(Account account, decimal amount)
        {
            if (account.Balance >= amount)
            {
                account.Balance -= amount;
                DateTime dateTime = DateTime.Now;
                long time = dateTime.Ticks;
                string transactionId = "TXN" + bank.BankId + account.AccountId + time;
                Transaction transaction = new Transaction(transactionId, account.AccountId, account.AccountId, amount, dateTime);
                account.transactions.Add(transaction);
                Console.WriteLine("Withdraw Successfull");
                Console.WriteLine("Transaction Id : " + transactionId);
            }
            else
            {
                Console.WriteLine("Insufficient funds");
            }
        }
        public void TransactionHistory(Account account, TimeSpan timeRange)
        {
            DateTime startDate = DateTime.Now - timeRange;

            Console.WriteLine("Transaction history for " + account.AccountHolderName + "'s account " + account.AccountId + " in the last " + timeRange.TotalDays + " days:");

            var filteredTransactions = account.transactions
                .Where(transaction => transaction.Time >= startDate)
                .OrderBy(transaction => transaction.Time);

            if (filteredTransactions.Any())
            {
                foreach (var transaction in filteredTransactions)
                {
                    Console.WriteLine("\nTransaction Id : " + transaction.TransactionId);
                    if (transaction.SourceAccountId == account.AccountId)
                    {
                        Console.WriteLine("Dear user " + transaction.SourceAccountId + "debited by " + transaction.Amount + "INR on " + transaction.Time + " to " + transaction.DestinationAccountId);
                    }
                    else if (transaction.DestinationAccountId == account.AccountId)
                    {
                        Console.WriteLine("Dear user " + transaction.SourceAccountId + "credited by " + transaction.Amount + "INR on " + transaction.Time + " by " + transaction.DestinationAccountId);
                    }
                }
            }
            else
            {
                Console.WriteLine("No transactions found in the specified time range.");
            }
        }
    }
}
