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
        void Deposite(Account account, decimal amount);
        void Withdraw(Account account, decimal amount);
        void AddCurrency(string currency, decimal exchangeRate);
        void AddServiceChargeSameBank(string serviceType, decimal charge);
        void AddServiceChargeOtherBank(string serviceType, decimal charge);
        void TransactionHistory(Account account, TimeSpan timeRange);
        void RevertTransaction(Account sourceAccount, string recipientBankName, string transactionId);
    }
    class BankStaffService:IBankStaffService
    {
        private Bank _bank;
        private AccountService accountService;
        public BankStaffService(string bankId)
        {
            _bank = CentralBank.banks.Find(x => x.BankId == bankId);
            accountService = new AccountService(bankId);
        }

        public void CreateAccount(string username, string password)
        {
            string accountId = username.Substring(0, 3) + DateTime.Now.ToString("ddMMyyyy");
            Account account = new Account(accountId, username, password);
            _bank.accounts.Add(account);
            Console.WriteLine("Account Created Successfully.\n"+"Account Name : "+username+"\nAccount Id : "+accountId+"\n");
        }
        public void UpdateAccount(string accountId)
        {
            Account account = _bank.accounts.Find(a => a.AccountId == accountId);
            if(account is not null)
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
            Account account = _bank.accounts.Find(a => a.AccountId == accountId);
            if(account is not null)
            {
                _bank.accounts.Remove(account);
                Console.WriteLine("Account Successfully Removed");
            }
            else
            {
                Console.WriteLine("Account not found");
            }
        }
        public void Deposite(Account account, decimal amount)
        {
            accountService.Deposite(account, amount);
        }
        public void Withdraw(Account account, decimal amount)
        {
            accountService.Withdraw(account, amount);
        }
        public void AddCurrency(string currency, decimal exchangeRate)
        {
            if (!_bank.currency.ContainsKey(currency))
            {
                _bank.currency.Add(currency, exchangeRate);  
            }
            else
            {
                Console.WriteLine("Currency already existed");
            }
        }
        public void AddServiceChargeSameBank(string serviceType, decimal charge)
        {
            SetServiceCharge(serviceType, charge); 
        }
        public void AddServiceChargeOtherBank(string serviceType, decimal charge)
        {
            SetServiceCharge(serviceType, charge);
        }
        public void TransactionHistory(Account account, TimeSpan timeRange)
        {
            accountService.TransactionHistory(account, timeRange);
        }
        public void RevertTransaction(Account sourceAccount,string recipientBankName,string transactionId)
        {

            Transaction transaction = sourceAccount.transactions.Find(a => a.TransactionId == transactionId);
            if (_bank.BankName == recipientBankName)
            {
                var destinationAccount = _bank.accounts.Find(a => a.AccountId == transaction.DestinationAccountId);
                if(sourceAccount is not null && destinationAccount is not null)
                {
                    PerformRevertTransaction(transaction,sourceAccount, destinationAccount,transactionId);
                }
            }
            else
            {
                var anotherBank = CentralBank.banks.Find(a=>a.BankName == recipientBankName);
                var destinationAccount = anotherBank.accounts.Find(a => a.AccountId == transaction.DestinationAccountId);
                if (sourceAccount is not null && destinationAccount is not null)
                {
                  PerformRevertTransaction(transaction, sourceAccount, destinationAccount, transactionId);
                }
            }
        }
        private void SetServiceCharge(string serviceType,decimal charge)
        {
            if (serviceType == "RTGS")
            {
                _bank.RTGSChargeOtherBank = charge;
            }
            else if (serviceType == "IMPS")
            {
                _bank.IMPSChargeOtherBank = charge;
            }
            else
            {
                Console.WriteLine("Invalid service type.");
            }
        }
        private void PerformRevertTransaction(Transaction transaction,Account sourceAccount,Account destinationAccount,string transactionId)
        {
            sourceAccount.Balance += transaction.Amount;
            destinationAccount.Balance -= transaction.Amount;
            DateTime dateTime = DateTime.Now;
            long time = dateTime.Ticks;
            string revertTransactionId = "TXN" + _bank.BankId + sourceAccount.AccountId + time;
            Transaction revertTransaction = new Transaction(transactionId, sourceAccount.AccountId, destinationAccount.AccountId, transaction.Amount, dateTime);
            sourceAccount.transactions.Add(revertTransaction);
            destinationAccount.transactions.Add(revertTransaction);
            Console.WriteLine("Transaction Reverted Successfully");
        }
    }
}
