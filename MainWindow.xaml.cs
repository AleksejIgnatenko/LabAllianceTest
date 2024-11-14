using System.Windows;

namespace LabAllianceTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonRegistrationClick(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }

        private void ButtonLoginClick(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void ButtonGetAllUsersClick(object sender, RoutedEventArgs e)
        {
            AllUsersWindow allUsersWindow = new AllUsersWindow();
            allUsersWindow.Show();
            this.Close();
        }
    }
}