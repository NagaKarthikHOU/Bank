using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class BankStaffService
    {
        private Bank bank;
        public BankStaffService(Bank bank)
        {
            this.bank = bank;
        }
        public void StaffDetails()
        {
            bank.staff.Add(new Staff("Karthik", "Karthik123"));
            bank.staff.Add(new Staff("Sampath", "Sampath456"));
            bank.staff.Add(new Staff("Hemanth", "Hemanth789"));
        }
        public Staff AuthenticateStaff(string name, string password)
        {
            return bank.staff.Find(u => u.Name == name && u.Password == password);
        }
        public void CreateAccount(string username, string password)
        {
            string accountId = username.Substring(0, 3) + DateTime.Now.ToString("M/d/yyyy");
            Account account = new Account(accountId, username, password);
            bank.accounts.Add(account);
            Console.WriteLine("Account Created Successfully.\n"+"Account Name : "+username+"\nAccount Id : "+accountId+"\n");
        }
        public void UpdateAccount(string accountId)
        {
            Account account = bank.accounts.Find(a => a.AccountId == accountId);
            if(account != null)
            {
                Console.WriteLine("1. Change UserName\n2. Change password");
                int option = Helper.Input<int>();
                if (option == 1)
                {
                    Console.WriteLine("Enter New UserName : ");
                    string username = Helper.Input<string>();
                    account.AccountHolderName = username;
                }
                else if (option == 2)
                {
                    Console.WriteLine("Enter New Password : ");
                    string password = Helper.Input<string>();
                    account.Password = password;
                }
            }
            else
            {
                Console.WriteLine("Account not found");
            }
        }
        public void DeleteAccount(string accountId)
        {
            Account account = bank.accounts.Find(a => a.AccountId == accountId);
            if(account != null)
            {
                bank.accounts.Remove(account);
            }
            else
            {
                Console.WriteLine("Account not found");
            }
        }
    }
}
