using System.Runtime.CompilerServices;

namespace Bank
{
    class StartUp
    {
       
        public void BankSimulation(Bank bank)
        {
            BankService bankService = new BankService(bank);
            IBankStaffService staffService = new BankStaffService(bank);
            IAccountHolderService userService = new AccountHolderService(bank);
            while (true)
            {
                Console.WriteLine("\n1. Create Bank Staff \n2. Login as bank staff \n3. Login as Account holder\n4. Exit");
                int choice = Helper.Input<int>();
                switch (choice)
                {
                    case 1:
                        bankService.CreateBankStaff(Helper.StaffMemberName(), Helper.StaffMemberPassword());
                        break;
                    case 2:
                        Console.Write("Enter Bank Staff Username : ");
                        string staffUsername = Helper.Input<string>();
                        Console.Write("Enter Bank Staff Password : ");
                        string staffPassword = Helper.Input<string>();
                        Staff authenticateStaff = bankService.AuthenticateStaff(staffUsername, staffPassword);
                        if (authenticateStaff != null)
                        {
                            Helper.StaffActions(staffService);
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
                            Helper.PerformTransactions(account, userService,bankService);
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
        public static List<Bank> banks = new List<Bank>();
        static void Main()
        {
            StartUp startUp = new StartUp();
            while (true)
            {
                Console.WriteLine("1. Set Up new Bank");
                Console.WriteLine("2. Continue With Excisting Bank");
                Console.WriteLine("3. Exit");
                int a = Helper.Input<int>();
                if (a == 1)
                {
                    Console.WriteLine("Enter Bank Name : ");
                    string bankName = Helper.Input<string>();
                    Bank bank = new Bank(bankName);
                    banks.Add(bank);
                    startUp.BankSimulation(bank);
                }
                else if (a == 2)
                {
                    Console.WriteLine("Enter Bank Name : ");
                    string bankName = Helper.Input<string>();
                    Bank bank = banks.Find(a=>a.BankName == bankName);
                    startUp.BankSimulation(bank);
                }
                else if (a == 3)
                {
                    return;
                }
                
            }    
        }
    }
}