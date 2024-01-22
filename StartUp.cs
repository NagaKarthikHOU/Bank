using System.Runtime.CompilerServices;

namespace Bank
{
    class StartUp
    {
        static void Main()
        {
            Bank bank = new Bank("SBI");
            BankStaffService staffService = new BankStaffService(bank);
            AccountHolderService userService = new AccountHolderService(bank);
            while (true)
            {
                Console.WriteLine("1. Login as bank staff \n2. Login as Account holder\n3. Exit");
                int choice = Helper.Input<int>();
                switch(choice)
                {
                    case 1:
                        staffService.StaffDetails();
                        Console.Write("Enter Bank Staff Username : ");
                        string staffUsername = Helper.Input<string>();
                        Console.Write("Enter Bank Staff Password : ");
                        string staffPassword = Helper.Input<string>();
                        Staff authenticateStaff = staffService.AuthenticateStaff(staffUsername, staffPassword);
                        if(authenticateStaff != null)
                        {
                            Helper.StaffActions(staffService);  
                        }
                        else
                        {
                            Console.WriteLine("Staff Member not found");
                        }
                        break;
                    case 2:
                        Account account = userService.AuthenticateUser(Helper.AccountHolderName(), Helper.AccountHolderPassword());
                        if (account != null)
                        {
                            Helper.PerformTransactions(account,userService);
                        }
                        else
                        {
                            Console.WriteLine("Account Not found");
                        }
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Enter valid Number");
                        break;
                }
            }
        }
    }
}