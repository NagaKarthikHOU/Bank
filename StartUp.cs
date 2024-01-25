using System.Runtime.CompilerServices;

namespace Bank
{
    class StartUp
    {
        static void Main()
        {
            CentralBank centralBank = new CentralBank();    
            while (true)
            {
                Console.WriteLine("1. Set Up new Bank");
                Console.WriteLine("2. Continue With Excisting Bank");
                Console.WriteLine("3. Exit");
                int a = Helper.Input<int>();
                if (a == 1)
                {
                    centralBank.SetUpBank();
                }
                else if (a == 2)
                {
                    centralBank.ContinueWithExistingBank();
                }
                else if (a == 3)
                {
                    return;
                }
                
            }    
        }
    }
}