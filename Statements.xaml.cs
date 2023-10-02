﻿using KompCheck_Zaldea;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace BankApp
{
    public partial class Statements : Window
    {
        private Account account;
        private DataHandler dataHandler;
        private List<Transaction> transactions;

        // This constructor is called when the Statements window is opened from the Accounts window and it will display the transactions for the selected account
        public Statements(Account account)
        {
            InitializeComponent();
            this.account = account;
            dataHandler = new DataHandler();
            transactions = dataHandler.GetTransactions(account.IBAN);
            DisplayTransactions(transactions);
        }


        private void DisplayTransactions(List<Transaction> validTransactions)
        {   
            //We work with decimals because we are dealing with money
            decimal balance = 0;
            decimal totalIncome = 0;
            decimal totalOutcome = 0;

            //Here we check if the selected account is sender or receiver for each transaction and we add the transaction to the corresponding listbox
            foreach (var transaction in validTransactions)
            {
                if (transaction.Sender == account.IBAN)
                {   
                    AccountOutcome.Items.Add(transaction);
                    balance -= transaction.Amount;
                    totalOutcome += transaction.Amount;
                }
                else if (transaction.Receiver == account.IBAN)
                {
                    AccountIncome.Items.Add(transaction);
                    balance += transaction.Amount;
                    totalIncome += transaction.Amount;
                }
            }
            // Display total income and outcome
            TotalIncomeTextBlock.Text = $"Total Income: {totalIncome:C}";
            TotalOutcomeTextBlock.Text = $"Total Outcome: {totalOutcome:C}";

            // Display the balance
            BalanceTextBlock.Text = $"Balance: {balance:C}";
        }
            
        // This method is called when the "Return" button is clicked, it will open the Accounts window and close the Statements window
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            AccountsWindow accountsWindow = new AccountsWindow();
            accountsWindow.Show();
            this.Close();
        }

        //This method is called when a sorting option is selected from the combobox
        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            //Here we check which sorting option was selected and we display a GUI option for the user to select the date or the reason
            switch (ComboBoxSort.SelectedItem)
            {
                case ComboBoxItem item when item.Content.ToString() == "Date":
                    DatePickerSort.Visibility = Visibility.Visible;
                    ComboBoxVerwendungszweck.Visibility = Visibility.Collapsed;
                    break;
                case ComboBoxItem item when item.Content.ToString() == "Verwendungszweck":
                    ComboBoxVerwendungszweck.Visibility = Visibility.Visible;
                    DatePickerSort.Visibility = Visibility.Collapsed;
                    ComboBoxVerwendungszweck.ItemsSource = transactions.Select(t => t.Reason).Distinct().ToList();
                    break;
            }
        }

        //This method is called when a date is selected from the datepicker GUI object and it will display the transactions for the selected date, nothing will change
        private void DatePickerSort_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var chosenDate = DatePickerSort.SelectedDate;
            if (chosenDate.HasValue)
            {
                AccountIncome.Items.Clear();
                AccountOutcome.Items.Clear();
                //Here we filter the transactions list and we display the transactions for the selected date in the corresponding listbox, if the transaction date matches the chosen date
                var filteredTransactions = transactions.Where(transaction => transaction.DateOfTransaction.Date == chosenDate.Value.Date).ToList();
                DisplayTransactions(filteredTransactions);
            }
        }

        //This method is called when a reason is selected from the combobox GUI object and it will display the transactions for the chosen reason
        private void ComboBoxVerwendungszweck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var chosenReason = (string)ComboBoxVerwendungszweck.SelectedItem;
            if (!string.IsNullOrEmpty(chosenReason))
            {
                AccountIncome.Items.Clear();
                AccountOutcome.Items.Clear();
                //Filer the transactions to only display the transactions where the reason is the same as the chosen reason
                var filteredTransactions = transactions.Where(transaction => transaction.Reason == chosenReason).ToList();
                DisplayTransactions(filteredTransactions);
            }
        }

        //This method is called when the user clicks the print button, 
        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();      //This will open a save file dialog called sfd
            sfd.Filter = "CSV Files(*.csv)|*.csv";                                          //The user will see that the only extension available is csv, in my opinion is the best option
            sfd.DefaultExt = ".csv";                                                        //Here we set a default extension as .csv so we can use later
            sfd.AddExtension = true;                                                        //This makes sure the file will be automatically given an extension
            Nullable<bool> result = sfd.ShowDialog();
            if (result == true)
            {
                string filename = sfd.FileName;                                             //The name of the file is give by the user in the dialog
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("Sender, Receiver, Reason, Amount, Date"); //Here we write the first row then print each transaction from the transactions list one by one
                    foreach (var transaction in transactions)
                    {
                        writer.WriteLine($"{transaction.Sender}, {transaction.Receiver}, {transaction.Reason}, {transaction.Amount}, {transaction.DateOfTransaction}");
                    }
                }
            }
        }


    }
}