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
        public void SetUpBank(SqlConnection connection)
        {
            Console.WriteLine("Enter Bank Name : ");
            string bankName = Helper.Input<string>().ToLower();
            string bankId = bankName.Substring(0, 3) + DateTime.Now.ToString("ddMMyyyy");
            connection.Open();
            connection.Execute("INSERT INTO Bank (BankName,BankId) VALUES (@BankName,@BankId)", new { BankName = bankName,BankId=bankId });
            connection.Close();
            Helper.BankSimulation(bankId,connection);
        }
        public void ContinueWithExistingBank(SqlConnection connection)
        {
            Console.WriteLine("Enter Bank Name : ");
            string bankName = Helper.Input<string>();
            connection.Open();
            Bank bank = connection.QuerySingle<Bank>("SELECT * FROM Bank WHERE BankName = @BankName", new { BankName = bankName });
            Helper.BankSimulation(bank.BankId,connection);
        }
    }
}
