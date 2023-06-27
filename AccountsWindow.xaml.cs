using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace BankApp
{   
    // Create a new window called AccountsWindow
    public partial class AccountsWindow : Window
    {
        private List<Account> accounts;
        
        // This is the logic for the AccountsWindow, it will load the data from the CSV files and display it in the AccountsListBox
        public AccountsWindow()
        {
            InitializeComponent();
            accounts = new List<Account>();
            LoadDataFromMultipleFiles();
            DisplayAccounts();
        }

        // This method will load the data from the CSV files
        private void LoadDataFromMultipleFiles()
        {
            List<string> fileNames = new List<string>();
            bool fileChosen = false;
            while (fileChosen == false)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Select CSV files";
                openFileDialog.Filter = "CSV files|*.csv";                                      // Only show CSV files
                openFileDialog.Multiselect = true;                                              // Allow the user to select multiple files using CTRL+Click or Strg+Click
                if (openFileDialog.ShowDialog() == true)                                        // Show the dialog and check if the user clicked "Open" or double clicked a file.
                {
                    // If the user clicked "Open" or double clicked a file, add the file names to the list
                    fileChosen = true;

                    foreach (String fileName in openFileDialog.FileNames)
                    {
                        fileNames.Add(fileName);
                    }
                }
                // If the user clicked "Cancel" or X out of the dialog, show a message box and ask the user to select a file
                else
                {
                    MessageBox.Show("Please select a file to access the accounts.");
                }
            }

            foreach (String fileName in fileNames)
            {
                using (var reader = new StreamReader(fileName))
                {

                    var header = reader.ReadLine().Split(',');                              // Read the first line of the CSV file and split it into an array
                    var kontonummerIndex = Array.IndexOf(header, "Kontonummer");            // The index of the "Kontonummer" column
                    var nameIndex = Array.IndexOf(header, "Name");                          // The index of the "Name" column

                    // Read the rest of the CSV file and add the data to the accounts list
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();                                       // Read the next line of the CSV file
                        var values = line.Split(',');                                       // Split the line where a comma is found and add the values to an array
                        var account = new Account { Id = int.Parse(values[0]) };            // Create a new account object and set the Id property to the first value in the array

                        if (nameIndex != -1) account.Name = values[nameIndex];               // If the nameIndex is not -1, set the Name property to the value at the nameIndex
                        if (kontonummerIndex != -1) account.Kontonummer = values[kontonummerIndex]; // If the kontonummerIndex is not -1, set the Kontonummer property to the value at the kontonummerIndex

                        var existingAccount = accounts.FirstOrDefault(a => a.Id == account.Id && a.Name == account.Name && a.Kontonummer == account.Kontonummer); // Check if the account already exists in the accounts list

                        if (existingAccount == null)
                        {  // If the account does not exist, add it
                            accounts.Add(account);
                        }
                    }
                }
            }
        }




        // This method will display the accounts in the AccountsListBox
        private void DisplayAccounts()
        {
            AccountsListBox.Items.Clear();
            foreach (var account in accounts)
            {   
                var description = account.Name + " " + account.Kontonummer;
                AccountsListBox.Items.Add($"{account.Id} - {description}");
            }
        }

        // This method will sort the accounts by Id and display them in the AccountsListBox
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            accounts = accounts.OrderBy(a => a.Id).ToList();
            DisplayAccounts();
        }

        // This method will open the Statements window and close the AccountsWindow
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            Statements statements = new Statements();
            statements.Show();
            this.Close();
        }
    }

    // Create a new class called Account
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Kontonummer { get; set; }
    }
}
