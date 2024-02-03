using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Bank
{
    class BankService
    {
        public Bank bank;
        public SqlConnection connection;
        public BankService(string bankId,SqlConnection connect)
        {
            connection = connect;
            connection.Open();
            bank = connection.QueryFirstOrDefault<Bank>("SELECT * FROM BANK WHERE BankId = @BankId", new { BankId = bankId });
        }
        
        public void CreateBankStaff(string name, string password)
        {

            connection.Open();
            connection.Execute("INSERT INTO Staff (Name, Password, BankId) VALUES (@Name, @Password, @BankId)", new { Name = name, Password = password, BankId = bank.BankId });
        }
        public Staff AuthenticateStaff(string name, string password)
        {
            connection.Open();
            return connection.QueryFirstOrDefault<Staff>("SELECT * FROM Staff WHERE Name = @Name AND Password = @Password AND BankId = @BankId", new { Name = name, Password = password, BankId = bank.BankId });
        }
        public Account AuthenticateUser(string username, string password)
        {
            connection.Open();
            return connection.QueryFirstOrDefault<Account>("SELECT * FROM Account WHERE AccountHolderName = @AccountHolderName AND Password = @Password", new { AccountHolderName = username, Password = password});
        }
        public decimal ConvertCurrency(string currencyName, decimal amount)
        {
            connection.Open();
            Currency currency = connection.QueryFirstOrDefault<Currency>("SELECT * FROM Currency WHERE CurrencyName = @CurrencyName", new { CurrencyName = currencyName });
            if (currency != null)
            {
                amount *= currency.ExchangeRate;
            }
            return amount;
        }
    }
}
