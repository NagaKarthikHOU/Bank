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
        public static void StaffActions(BankStaffService staffService)
        {
            while (true)
            {
                Console.WriteLine("\nChoose your option");
                Console.WriteLine("1. Create new Account for Account Holder");
                Console.WriteLine("2. Update Account\n3. Delete Account");
                Console.WriteLine("4. Add new Accepted currency with exchange rate");
                Console.WriteLine("5. Add service charge for same bank account");
                Console.WriteLine("6. Add service charge for other bank account");
                Console.WriteLine("7. View account transaction history");
                Console.WriteLine("8. Revert transaction\n9. Exit");
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
                        Console.Write("Enter Currency : ");
                        string currency = Input<string>();
                        Console.Write("Enter Exchange Rate : ");
                        decimal exchangeRate = Input<decimal>();
                        staffService.AddCurrency(currency, exchangeRate);
                        break;
                    case 5:
                        Console.Write("Enter Service Type : ");
                        string serviceType = Input<string>();
                        Console.Write("Enter Charge : ");
                        decimal charge = Input<decimal>();
                        staffService.AddServiceChargeSameBank(serviceType, charge);
                        break;
                    case 6:
                        Console.Write("Enter Service Type : ");
                        string serviceType1 = Input<string>();
                        Console.Write("Enter Charge : ");
                        decimal charge1 = Input<decimal>();
                        staffService.AddServiceChargeOtherBank(serviceType1, charge1);
                        break;
                    case 7:
                        Console.Write("Enter AccountId : ");
                        accountId = Input<string>();
                        staffService.ViewTransactionHistory(accountId);
                        break;
                    case 8:
                        Console.Write("Enter Transaction Id : ");
                        string transactionId = Input<string>();
                        staffService.RevertTransaction(transactionId);
                        break;
                    case 9:
                        return;
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

        public static void PerformTransactions(Account account,AccountHolderService userService)
        {
            while (true)
            {
                Console.WriteLine("\nChoose your option");
                Console.WriteLine("1. Deposit Amount\n2. WithDraw Amount\n3. Transfer Funds\n4. Check Balance\n5. Transaction History\n6. Exit");
                int op = Helper.Input<int>();
                switch (op)
                {
                    case 1:
                        Console.WriteLine("Enter Deposit Amount");
                        decimal depositeAmount = Helper.Input<decimal>();
                        Console.WriteLine("Enter Currency : ");
                        string currency = Helper.Input<string>();
                        decimal convertedAmount = userService.ConvertCurrency(currency,depositeAmount);
                        userService.Deposite(account, convertedAmount);
                        break;
                    case 2:
                        Console.WriteLine("Enter Withdraw Amount");
                        decimal withdrawAmount = Helper.Input<decimal>();
                        userService.Withdraw(account, withdrawAmount);
                        break;
                    case 3:
                        Console.WriteLine("Enter Bank Name : ");
                        string bankName = Helper.Input<string>();
                        Console.WriteLine("Enter Recipient AccountId : ");
                        string accountId = Helper.Input<string>();
                        Console.WriteLine("Enter Amount to Transfer : ");
                        decimal amount = Helper.Input<decimal>();
                        Console.WriteLine("Enter Service Type : ");
                        string serviceType = Helper.Input<string>();
                        userService.TransferFunds(bankName,accountId, account, amount,serviceType);
                        break;
                    case 4:
                        Console.WriteLine(account.Balance + " INR ");
                        break;
                    case 5:
                        userService.TransactionHistory(account.AccountId);
                        break;
                    case 6:
                        return;
                }
            }
           
        }
    }
    
}
