using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_DOTNETCORE
{
    public class Transaction
    {
        public int id { get; set; }
        public string Time { get; set; }
        public string SenderWalletNumber { get; set; }
        public string ReceiverWalletNumber { get; set; }
        public string Category { get; set; }
        public decimal Sum { get; set; }
        public Transaction() { }

        public Transaction(string time, string senderWalletNumber, string receiverWalletNumber, string category, decimal sum)
        {
            this.Time = time;
            this.SenderWalletNumber = senderWalletNumber;
            this.ReceiverWalletNumber = receiverWalletNumber;
            this.Category = category;
            this.Sum = sum;
        }
        public override string ToString()
        {
            return $"Id - {id}: Category - {Category}, Sender - {SenderWalletNumber}, Receiver - {ReceiverWalletNumber}, Transaction performed at {Time}. Sum - {Sum}";
        }
    }
}
