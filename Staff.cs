using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Staff
    {
        public string Name {  get; set; }
        public string Password { get; set; }
        public string BankId { get; set; }  
        public Staff(string name,string password,string bankId)
        {
            Name = name;
            Password = password;
            BankId = bankId;
        }
    }
}
