using LabAllianceTest.Abstractions;
using LabAllianceTest.Exceptions;
using LabAllianceTest.Models;
using LabAllianceTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LabAllianceTest
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        IUserService _userService;
        public LoginWindow()
        {
            _userService = new UserService();
            InitializeComponent();
        }

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = textBoxLogin.Text;
                string password = textBoxPassword.Text;

                var user = UserModel.Create(login, password, false).user;

                labelErrorLogin.Content = string.Empty;
                labelErrorPassword.Content = string.Empty;

                var (message, statusCode) = await _userService.LoginUserAsync(user);

                if (statusCode == 200)
                {
                    labelSuccessMessage.Content = message;
                    labelErrorMessage.Content = string.Empty;
                }
                else if (statusCode == 401)
                {
                    labelSuccessMessage.Content = string.Empty;
                    labelErrorMessage.Content = message;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("В результате работы возникла непредвиденная ошибка.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBackClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
