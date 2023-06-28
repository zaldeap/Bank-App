using System;

namespace BankApp
{
    public class Transaction
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateOfTransaction { get; set; }

        public override string ToString()
        {
            return $"Sender: {Sender}, Receiver: {Receiver}, Reason: {Reason}, Amount: {Amount}, Date: {DateOfTransaction.ToString("dd.MM.yyyy")}";
        }
    }
}
