using System;
using System.Windows;

namespace LibraryManagementSystem
{
    public partial class UserHome : Window
    {
        private int userId;
        private string userName;

        public UserHome(int userId, string userName)
        {
            InitializeComponent();
            this.userId = userId;
            this.userName = userName;

            UserBorrow userBorrow = new UserBorrow(userId, userName);
            userStackPanel.Children.Clear();
            userStackPanel.Children.Add(userBorrow);
        }

        // SHOW BOOK BORROW MENU
        private void BtnBorrow_Click(object sender, RoutedEventArgs e)
        {
            UserBorrow userBorrow = new UserBorrow(userId, userName); // Передаём параметры
            userStackPanel.Children.Clear();
            userStackPanel.Children.Add(userBorrow);
        }

        // SHOW BOOK REQUEST AND BOOK RECEIVED MENU
        private void BtnTransaction_Click(object sender, RoutedEventArgs e)
        {
            UserTransaction userTransaction = new UserTransaction();
            userStackPanel.Children.Clear();
            userStackPanel.Children.Add(userTransaction);
        }

        // LOGOUT USER HOME
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}