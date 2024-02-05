using System.Runtime.CompilerServices;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Dapper;
using Microsoft.Data.SqlClient;
namespace Bank
{
    class StartUp
    {
        static void Main()
        {
            string connectionString = "Data Source = (LocalDb)\\MSSQLLocalDB;Initial Catalog = BankDB; Integrated Security = True";
            CentralBank centralBank = new CentralBank();    
            while (true)
            {
                Console.WriteLine("1. Set Up new Bank");
                Console.WriteLine("2. Continue With Excisting Bank");
                Console.WriteLine("3. Exit");
                int a = Helper.Input<int>();
                if (a == 1)
                {
                    centralBank.SetUpBank(connectionString);
                }
                else if (a == 2)
                {
                    centralBank.ContinueWithExistingBank(connectionString);
                }
                else if (a == 3)
                {
                    return;
                }
            }
            
        }
    }
}