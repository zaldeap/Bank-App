using System.Collections.Generic;
using System.Windows;

namespace BankApp
{

    public partial class Statements : Window
    {   
        // Launches the Statements window
        public Statements()
        {
            InitializeComponent();
        }
        
        // This method is called when the "Return" button is clicked, it will open the AccountsWindow and close the Statements window
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            AccountsWindow accountsWindow = new AccountsWindow();
            accountsWindow.Show();
            this.Close();
        }

        // This method is called when the "Print" button is clicked, it will show a message box until more code is added and then print the statements in csv files
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Printed");
        }
    }
}
