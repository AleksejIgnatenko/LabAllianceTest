﻿using LabAllianceTest.Abstractions;
using LabAllianceTest.Exceptions;
using LabAllianceTest.Services;
using System.Windows;

namespace LabAllianceTest
{
    /// <summary>
    /// Логика взаимодействия для AllUsersWindow.xaml
    /// </summary>
    public partial class AllUsersWindow : Window
    {
        private readonly IUserService _userService;
        public AllUsersWindow()
        {
            _userService = new UserService();
            InitializeComponent();
            this.Loaded += AllUsersWindowLoaded; 
        }

        private async void AllUsersWindowLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                allUsersDataGrid.ItemsSource = users;
            }
            catch(AuthenticationFailedException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private async void RefreshTokenButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var (message, statusCode) = await _userService.RefreshToken();

                var users = await _userService.GetAllUsersAsync();

                allUsersDataGrid.ItemsSource = users;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
