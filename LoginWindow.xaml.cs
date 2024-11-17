using LabAllianceTest.Abstractions;
using LabAllianceTest.Helpers;
using LabAllianceTest.Models;
using LabAllianceTest.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LabAllianceTest
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        string defaultTextTextBoxLogin = "Login";
        string defaultTextTextBoxPassword = "Пароль";

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
