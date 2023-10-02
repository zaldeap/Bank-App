
using System.Collections.Generic;
using System.Linq;

using System.Windows;

namespace BankApp
{
    public partial class AccountsWindow : Window
    {   
        
        private List<Account> accounts;
        private DataHandler dataHandler;

        public AccountsWindow()
        {
            InitializeComponent();
            // Get the accounts from the database
            dataHandler = new DataHandler();
            accounts = dataHandler.GetAccounts();
            // Display the accounts
            DisplayAccounts();
        }

        private void DisplayAccounts()
        {
            // Clear the listbox before adding the accounts to avoid duplicates
            AccountsListBox.Items.Clear();
            foreach (var account in accounts)
            {
                AccountsListBox.Items.Add(account);
            }
        }

        // This method is called when the "Sort" button is clicked, it will sort the accounts by ID and display them
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            accounts = accounts.OrderBy(account => account.Id).ToList();
            DisplayAccounts();
        }

        // This method is called when the "Details" button is clicked, it will open the Statements window and close the Accounts window
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccount = AccountsListBox.SelectedItem as Account;
            // Check if an account is selected before opening the Statements window
            if (selectedAccount == null)
            {
                MessageBox.Show("Please select an account.");
                return;
            }

            Statements statements = new Statements(selectedAccount);
            statements.Show();
            this.Close();
        }
    }
}
