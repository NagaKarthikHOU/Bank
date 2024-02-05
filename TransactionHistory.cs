using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class TransactionHistory
    {
        public string TransactionId { get; set; }
        public string SourceAccountId { get; set; }
        public string DestinationAccountId { get; set; }
        public decimal Amount { get; set; } 
        public DateTime Time { get; }

        public TransactionHistory() {

        }
    }
}
