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

                var (errors, user) = UserModel.Create(login, password);
                if (errors.Count > 0)
                {
                    if (errors.TryGetValue("Login", out string? loginError))
                    {
                        labelErrorLogin.Content = loginError;
                    }
                    else
                    {
                        labelErrorLogin.Content = "";
                    }

                    if (errors.TryGetValue("Password", out string? passwordError))
                    {
                        labelErrorPassword.Content = passwordError;
                    }
                    else
                    {
                        labelErrorPassword.Content = "";
                    }
                }
                else
                {
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
                        labelErrorMessage.Content = message;
                        labelSuccessMessage.Content = message;
                    }
                }
            }
            catch (UserValidationException ex)
            {
                if (ex.Errors.TryGetValue("login", out string? loginError))
                {
                    labelErrorLogin.Content = loginError;
                }
                else
                {
                    labelErrorLogin.Content = "";
                }

                if (ex.Errors.TryGetValue("password", out string? passwordError))
                {
                    labelErrorPassword.Content = passwordError;
                }
                else
                {
                    labelErrorPassword.Content = "";
                }

                foreach (var key in ex.Errors.Keys)
                {
                    labelErrorPassword.Content += key;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("В результате работы возникла непредвиденная ошибка.");
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
