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
                    //if (n.Length <= 12)
                    //{
                    //    return (T)Convert.ChangeType(n, typeof(T));
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Enter 6 to 12-digit vehicle Number");
                    //    return Input<T>();  
                    //} 
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
                switch (option)
                {
                    case 1:
                        staffService.CreateAccount(Helper.AccountHolderName(), Helper.AccountHolderPassword());
                        break;
                    case 2:
                        string accountId = Input<string>();
                        staffService.UpdateAccount(accountId);
                        break;
                    case 9:
                        return;
                }
            }
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
                        userService.Deposite(account, depositeAmount);
                        break;
                    case 2:
                        Console.WriteLine("Enter Withdraw Amount");
                        decimal withdrawAmount = Helper.Input<decimal>();
                        userService.Withdraw(account, withdrawAmount);
                        break;
                    case 3:
                        Console.WriteLine("Enter Recipient Name : ");
                        string accountId = Helper.Input<string>();
                        Console.WriteLine("Enter Amount to Transfer : ");
                        decimal amount = Helper.Input<decimal>();
                        userService.TransferFunds(accountId, account, amount);
                        break;
                    case 4:
                        Console.WriteLine(account.Balance);
                        break;
                    case 5:
                        break;
                    case 6:
                        return;
                }
            }
           
        }
    }
    
}
