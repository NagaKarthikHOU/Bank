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
        private readonly string _connectionString;
  
        public BankService(string bankId,string connectionString)
        {
            _connectionString = connectionString;
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
            }
        }
        
        public void CreateBankStaff(string name, string password,string bankId)
        {
            string staffId = bankId.Substring(0,3)+name.Substring(0,3) + DateTime.Now.ToString("ddMMyyyy");
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("INSERT INTO Staff (StaffId, Name, Password, BankId) VALUES (@StaffId, @Name, @Password, @BankId)", new { StaffId = staffId, Name = name, Password = password, BankId = bankId });
            }
        }
        public Staff AuthenticateStaff(string bankId,string name, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Staff>("SELECT * FROM Staff WHERE Name = @Name AND Password = @Password AND BankId = @BankId", new { Name = name, Password = password, BankId=bankId });
            }

        }
        public Account AuthenticateUser(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Account>("SELECT * FROM Account WHERE AccountHolderName = @AccountHolderName AND Password = @Password", new { AccountHolderName = username, Password = password });
            }      
        }
        public decimal ConvertCurrency(string currencyCode, decimal amount)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Currency currency = connection.QueryFirstOrDefault<Currency>("SELECT * FROM Currency WHERE CurrencyCode = @CurrencyCode", new { CurrencyCode = currencyCode });
                if (currency != null)
                {
                    amount *= currency.ExchangeRate;
                }
                return amount;
            }
        }
    }
}
