using LabAllianceTest.Abstractions;
using LabAllianceTest.Exceptions;
using LabAllianceTest.Helpers;
using LabAllianceTest.Models;
using LabAllianceTest.Services;
using System.Windows;

namespace LabAllianceTest
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        string defaultTextTextBoxLogin = "Login";
        string defaultTextTextBoxPassword = "Пароль";

        IUserService _userService;
        public RegistrationWindow()
        {
            _userService = new UserService();
            InitializeComponent();
        }

        private async void RegistrationClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = textBoxLogin.Text;
                string password = textBoxPassword.Text;

                labelErrorLogin.Content = string.Empty;
                labelErrorPassword.Content = string.Empty;
                labelErrorLogin.Content = string.Empty;
                labelErrorPassword.Content = string.Empty;

                var (errors, user) = UserModel.Create(login, password);
                if (errors.Count > 0)
                {
                    if (errors.TryGetValue("Login", out string? loginError))
                    {
                        labelErrorLogin.Content = loginError;
                    }
                    else
                    {
                        labelErrorLogin.Content = string.Empty;
                    }

                    if (errors.TryGetValue("Password", out string? passwordError))
                    {
                        labelErrorPassword.Content = passwordError;
                    }
                    else
                    {
                        labelErrorPassword.Content = string.Empty;
                    }
                }
                else
                {
                    labelErrorLogin.Content = string.Empty;
                    labelErrorPassword.Content = string.Empty;

                    var (message, statusCode) = await _userService.RegistrationUserAsync(user);

                    if (statusCode == 200)
                    {
                        labelSuccessMessage.Content = message;
                        labelErrorMessage.Content = string.Empty;
                    }
                    else if (statusCode == 400)
                    {
                        labelSuccessMessage.Content = message;
                    }
                    else if (statusCode == 409)
                    {
                        labelSuccessMessage.Content = string.Empty;
                        labelErrorMessage.Content = message;
                    }
                }
            }
            catch (UserValidationException ex)
            {
                if (ex.Errors.TryGetValue("Login", out string? loginError))
                {
                    labelErrorLogin.Content = loginError;
                }
                else
                {
                    labelErrorLogin.Content = "";
                }

                if (ex.Errors.TryGetValue("Password", out string? passwordError))
                {
                    labelErrorPassword.Content = passwordError;
                }
                else
                {
                    labelErrorPassword.Content = "";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBackClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void TextBoxLoginGotFocus(object sender, RoutedEventArgs e)
        {
            UIHelper.TextBox_Style_0(textBoxLogin, defaultTextTextBoxLogin);
            UIHelper.TextBox_Style_1(textBoxPassword, defaultTextTextBoxPassword);

            UIHelper.Label_Style_1(labelLogin, textBoxLogin, defaultTextTextBoxLogin);
            UIHelper.Label_Style_1(labelPassword, textBoxPassword, defaultTextTextBoxPassword);
        }

        private void TextBoxPasswordGotFocus(object sender, RoutedEventArgs e)
        {
            UIHelper.TextBox_Style_1(textBoxLogin, defaultTextTextBoxLogin);
            UIHelper.TextBox_Style_0(textBoxPassword, defaultTextTextBoxPassword);

            UIHelper.Label_Style_1(labelLogin, textBoxLogin, defaultTextTextBoxLogin);
            UIHelper.Label_Style_1(labelPassword, textBoxPassword, defaultTextTextBoxPassword);
        }
    }
}
