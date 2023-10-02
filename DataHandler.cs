
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BankApp
{
    public class DataHandler
    {   
        //This method is called automatically when the AccountsWindow is opened and gives all the accounts from the database
        public List<Account> GetAccounts()
        {   
            //Create a dictionary to store the accounts
            var accountsDictionary = new Dictionary<int, List<Account>>();

            // Load data from Konten.csv
            var accountsFile = "Konten.csv";
            using (var reader = new StreamReader(accountsFile))
            {
                var header = reader.ReadLine().Split(',');
                var idIndex = Array.IndexOf(header, "Kunden_ID");   //Find the index of the column "Kunden_ID"
                var kontonummerIndex = Array.IndexOf(header, "Kontonummer");  //Find the index of the column "Kontonummer"

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var id = int.Parse(values[idIndex]); 
                    var account = new Account
                    {
                        Id = id,
                        IBAN = values[kontonummerIndex],
                    };

                    if (accountsDictionary.ContainsKey(id)) //Check if the dictionary already contains the ID
                    {
                        accountsDictionary[id].Add(account);
                    }
                    else //If the dictionary doesn't contain the ID, add the ID and the account to the dictionary
                    {
                        accountsDictionary[id] = new List<Account> { account };
                    }
                }
            }

            var customersFile = "Kunden.csv";
            using (var reader = new StreamReader(customersFile))
            {
                var header = reader.ReadLine().Split(',');      //Create a header array with the column names splitted by comma
                var idIndex = Array.IndexOf(header, "Kunden_ID");       //Find the index of the column "Kunden_ID"
                var nameIndex = Array.IndexOf(header, "Name");          //Find the index of the column "Name"

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var id = int.Parse(values[idIndex]);                
                    var name = values[nameIndex];

                    if (accountsDictionary.ContainsKey(id))  //Check if the dictionary contains the ID then add the names to the accounts based on the ID
                    {
                        foreach (var account in accountsDictionary[id])
                        {
                            account.Name = name;
                        }
                    }
                }
            }

            // Convert the lists from the dictionary to a list
            var accounts = accountsDictionary.Values.SelectMany(acc => acc).ToList();  //Using the SelectMany method we can select all the accounts from the dictionary and convert them to a list
            return accounts;
        }

        //This method is called automatically when the Statements window is opened and gives all the transactions from the database
        public List<Transaction> GetTransactions(string IBAN)   
        {
            var transactions = new List<Transaction>();

            // Here we read the transactions from the "Buchungen.csv" file
            var transactionsFile = "Buchungen.csv";
            using (var reader = new StreamReader(transactionsFile))
            {
                var header = reader.ReadLine().Split(',');
                var senderIndex = Array.IndexOf(header, "Sender-KntNr");
                var empfangerIndex = Array.IndexOf(header, "Empfaenger-KntNr");
                var verwendungszweckIndex = Array.IndexOf(header, "Verwendungszweck");
                var betragIndex = Array.IndexOf(header, "Betrag");
                var buchungsdatumIndex = Array.IndexOf(header, "Buchungsdatum");

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    // Here we check if the sender or the receiver is the account we are looking for
                    if (values[senderIndex] == IBAN || values[empfangerIndex] == IBAN) //If the sender or the receiver is the account we are looking for, we create a new transaction object and add it to the list
                    {
                        var transaction = new Transaction
                        {
                            Sender = values[senderIndex],
                            Receiver = values[empfangerIndex],
                            Reason = values[verwendungszweckIndex],
                            Amount = decimal.Parse(values[betragIndex]),
                            DateOfTransaction = DateTime.ParseExact(values[buchungsdatumIndex], "dd.MM.yyyy", CultureInfo.InvariantCulture)  //Here we convert the string to a DateTime object
                        };
                        transactions.Add(transaction);
                    }
                }
            }

            return transactions;
        }
    }     
}
