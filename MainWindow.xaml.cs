using System.Windows;

namespace BankApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // This method is called when the "Accounts Manager" button is clicked, it will open the AccountsWindow and close the MainWindow
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AccountsWindow accountsWindow = new AccountsWindow();
            accountsWindow.Show();
            this.Close();
        }
    }
}
