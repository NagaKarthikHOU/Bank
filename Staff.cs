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
        public Staff(string name,string password)
        {
            Name = name;
            Password = password;
        }
    }
}
