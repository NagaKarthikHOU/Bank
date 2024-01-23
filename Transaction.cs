using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Transaction
    {
        public string TransactionId { get; set; }
        public string SourceAccountId { get; set; }
        public string DestinationAccountId { get; set; }
        public decimal Amount { get; set; } 

        public DateTime Time { get; set; }
        public Transaction(string transactionId,string sourceAccountId,string destinationAccountId,decimal amount,DateTime time)
        {
            TransactionId = transactionId;
            SourceAccountId = sourceAccountId;
            DestinationAccountId = destinationAccountId;
            Amount = amount;
            Time = time;
        }
    }
}
