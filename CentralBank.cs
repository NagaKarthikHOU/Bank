using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Data.SqlClient;
namespace Bank
{
    internal class CentralBank
    {
        public void SetUpBank(string connectionString)
        {
            Console.WriteLine("Enter Bank Name : ");
            string bankName = Helper.Input<string>().ToLower();
            string bankId = bankName.Substring(0, 3) + DateTime.Now.ToString("ddMMyyyy");
            using(var connection = new SqlConnection(connectionString)) {
                connection.Open();
                bool bankExists = connection.ExecuteScalar<bool>("SELECT 1 FROM Bank WHERE BankName = @BankName", new { BankName = bankName });
                if (bankExists)
                {
                    Console.WriteLine(bankName + " bank is already existed in the database.\n");
                }
                else
                {
                    connection.Execute("INSERT INTO Bank (BankName,BankId) VALUES (@BankName,@BankId)", new { BankName = bankName, BankId = bankId });
                    Helper.BankSimulation(bankId, connectionString);
                } 
            } 
        }
        public void ContinueWithExistingBank(string connectionString)
        {
            Console.WriteLine("Enter Bank Name : ");
            string bankName = Helper.Input<string>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Bank bank = connection.QuerySingle<Bank>("SELECT * FROM Bank WHERE BankName = @BankName", new { BankName = bankName });
                Helper.BankSimulation(bank.BankId, connectionString);
            }
            
        }
    }
}
