using System;

namespace BankApp
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IBAN { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Name} {IBAN}";
        }
    }


}