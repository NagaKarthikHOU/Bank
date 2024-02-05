using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Helper
    {
        public static T Input<T>()
        {
            string n = "";
            try
            {
                n = Console.ReadLine();
                if (typeof(T)==typeof(string))
                {
                    return (T)Convert.ChangeType(n, typeof(T));
                }
                else
                {
                    T a = (T)Convert.ChangeType(n, typeof(T));
                    if (Comparer<T>.Default.Compare(a, default(T)) < 0)
                    {
                        throw new Exception();
                    }
                    return (T)Convert.ChangeType(n, typeof(T));
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Please Enter Number");
                return Input<T>();
            }
            catch (OverflowException)
            {
                Console.WriteLine("Please Enter smaller Number");
                return Input<T>();
            }
            catch (Exception)
            {
                Console.WriteLine("Please Enter Positive Number");
                return Input<T>();
            }
            
        }
        public static void StaffActions(IBankStaffService staffService, BankService bankService)
        {
            while (true)
            {
                Console.WriteLine("\nChoose your option");
                Console.WriteLine("1. Create new Account for Account Holder");
                Console.WriteLine("2. Update Account\n3. Delete Account");
                Console.WriteLine("4. Deposite Amount\n5. Withdraw Amount");
                Console.WriteLine("6. Add new Accepted currency with exchange rate");
                Console.WriteLine("7. Add service charge for same bank account");
                Console.WriteLine("8. Add service charge for other bank account");
                Console.WriteLine("9. View account transaction history");
                Console.WriteLine("10. Revert transaction\n11. Back");
                int option = Helper.Input<int>();
                string accountId = "";
                switch (option)
                {
                    case 1:
                        staffService.CreateAccount(Helper.AccountHolderName(), Helper.AccountHolderPassword());
                        break;
                    case 2:
                        Console.Write("Enter AccountId");
                        accountId = Input<string>();
                        staffService.UpdateAccount(accountId);
                        break;
                    case 3:
                        Console.Write("Enter AccountId");
                        accountId = Input<string>();
                        staffService.DeleteAccount(accountId);
                        break;
                    case 4:
                        Console.WriteLine("Enter AccountId : ");
                        accountId = Input<string>();
                        Console.WriteLine("Enter Deposit Amount");
                        decimal depositeAmount = Input<decimal>();
                        Console.WriteLine("Enter Currency : ");
                        string enterCurrency = Input<string>();
                        decimal convertedAmount = bankService.ConvertCurrency(enterCurrency, depositeAmount);
                        staffService.Deposite(accountId, convertedAmount);
                        break;
                    case 5:
                        Console.WriteLine("Enter AccountId : ");
                        accountId = Input<string>();
                        Console.WriteLine("Enter Withdraw Amount");
                        decimal withdrawAmount = Input<decimal>();
                        staffService.Withdraw(accountId, withdrawAmount);
                        break;
                    case 6:
                        Console.Write("Enter Currency : ");
                        string currency = Input<string>();
                        Console.Write("Enter Exchange Rate : ");
                        decimal exchangeRate = Input<decimal>();
                        staffService.AddCurrency(currency, exchangeRate);
                        break;
                    case 7:
                        Console.Write("Enter Service Type : ");
                        string serviceType = Input<string>();
                        Console.Write("Enter Charge : ");
                        decimal charge = Input<decimal>();
                        staffService.AddServiceChargeSameBank(serviceType, charge);
                        break;
                    case 8:
                        Console.Write("Enter Service Type : ");
                        string serviceType1 = Input<string>();
                        Console.Write("Enter Charge : ");
                        decimal charge1 = Input<decimal>();
                        staffService.AddServiceChargeOtherBank(serviceType1, charge1);
                        break;
                    case 9:
                        Console.WriteLine("Enter AccountId : ");
                        accountId = Input<string>();
                        Console.WriteLine("Enter the time range for transaction history (e.g., '7 days', '1 month', '2 years'): ");
                        string userInput = Input<string>();

                        if (TryParseTimeSpan(userInput, out TimeSpan userTimeRange))
                        {
                            staffService.TransactionHistory(accountId, userTimeRange);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid time range.");
                        }
                        break;
                    case 10:
                        Console.Write("Enter AccountId : ");
                        accountId = Input<string>();
                        Console.Write("Enter Recipient Account Id : ");
                        string recipientAccountId = Input<string>();
                        Console.Write("Enter Transaction Id : ");
                        string transactionId = Input<string>();
                        staffService.RevertTransaction(accountId, recipientAccountId, transactionId);
                        break;
                    case 11:
                        return;
                }
            }
        }
        public static void BankSimulation(string bankId,string connectionString)
        {
            BankService bankService = new BankService(bankId,connectionString);
            IBankStaffService staffService = new BankStaffService(bankId,connectionString);
            IAccountHolderService userService = new AccountHolderService(bankId,connectionString);
            while (true)
            {
                Console.WriteLine("\n1. Create Bank Staff \n2. Login as bank staff \n3. Login as Account holder\n4. Back");
                int choice = Input<int>();
                switch (choice)
                {
                    case 1:
                        bankService.CreateBankStaff(StaffMemberName(), StaffMemberPassword(),bankId);
                        break;
                    case 2:
                        Console.WriteLine("Enter Bank Id : ");
                        string bank_Id = Input<string>();    
                        Console.Write("Enter Bank Staff Username : ");
                        string staffUsername = Input<string>();
                        Console.Write("Enter Bank Staff Password : ");
                        string staffPassword = Input<string>();
                        Staff authenticateStaff = bankService.AuthenticateStaff(bank_Id,staffUsername, staffPassword);
                        if (authenticateStaff != null)
                        {
                            StaffActions(staffService, bankService);
                        }
                        else
                        {
                            Console.WriteLine("Staff Member not found");
                        }
                        break;
                    case 3:
                        Account account = bankService.AuthenticateUser(Helper.AccountHolderName(), Helper.AccountHolderPassword());
                        if (account != null)
                        {
                            PerformTransactions(account,userService, bankService);
                        }
                        else
                        {
                            Console.WriteLine("Account Not found");
                        }
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Enter valid Number");
                        break;
                }
            }
        }
        public static string StaffMemberName()
        {
            Console.WriteLine("Enter Name : ");
            return Input<string>();
        }
        public static string StaffMemberPassword()
        {
            Console.WriteLine("Enter Password : ");
            return Input<string>();
        }
        public static string AccountHolderName()
        {
            Console.WriteLine("Enter Account Holder Username : ");
            return Input<string>();
        }
        public static string AccountHolderPassword()
        {
            Console.WriteLine("Enter Account Holder Password : ");
            return Input<string>();
        }
        private static bool TryParseTimeSpan(string input, out TimeSpan result)
        {
            result = default(TimeSpan);

            string[] parts = input.Split(' ');
            if (parts.Length != 2)
            {
                return false;
            }

            if (int.TryParse(parts[0], out int value))
            {
                string unit = parts[1].ToLower();
                switch (unit)
                {
                    case "days":
                        result = TimeSpan.FromDays(value);
                        break;
                    case "months":
                        result = TimeSpan.FromDays(value * 30);
                        break;
                    case "years":
                        result = TimeSpan.FromDays(value * 365);
                        break;
                    default:
                        return false;
                }

                return true;
            }

            return false;
        }
        public static void PerformTransactions(Account account,IAccountHolderService userService,BankService bankService)
        {
            while (true)
            {
                Console.WriteLine("\nChoose your option");
                Console.WriteLine("1. Deposit Amount\n2. WithDraw Amount\n3. Transfer Funds\n4. Check Balance\n5. Transaction History\n6. Back");
                int op = Input<int>();
                switch (op)
                {
                    case 1:
                        Console.WriteLine("Enter Deposit Amount");
                        decimal depositeAmount = Input<decimal>();
                        Console.WriteLine("Enter Currency : ");
                        string currency = Input<string>();
                        decimal convertedAmount = bankService.ConvertCurrency(currency,depositeAmount);
                        userService.Deposite(account.AccountId, convertedAmount);
                        break;
                    case 2:
                        Console.WriteLine("Enter Withdraw Amount");
                        decimal withdrawAmount = Input<decimal>();
                        userService.Withdraw(account.AccountId, withdrawAmount);
                        break;
                    case 3:
                        Console.WriteLine("Enter Recipient Bank Id : ");
                        string bankId = Input<string>();
                        Console.WriteLine("Enter Recipient AccountId : ");
                        string accountId = Input<string>();
                        Console.WriteLine("Enter Amount to Transfer : ");
                        decimal amount = Input<decimal>();
                        Console.WriteLine("Enter Service Type : ");
                        string serviceType = Input<string>();
                        userService.TransferFunds(bankId,accountId, account, amount,serviceType);
                        break;
                    case 4:
                        Console.WriteLine(account.Balance + " INR ");
                        break;
                    case 5:
                        Console.WriteLine("Enter the time range for transaction history (e.g., '7 days', '1 month', '2 years'): ");
                        string userInput = Input<string>();

                        if (TryParseTimeSpan(userInput, out TimeSpan userTimeRange))
                        {
                           userService.TransactionHistory(account.AccountId,userTimeRange);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid time range.");
                        }
                        break;
                    case 6:
                        return;
                }
            }
           
        }
    }
    
}
