using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            UserLogin userLoginWindow = new UserLogin();
            userLoginWindow.Show();
            this.Close(); // Закрываем текущее окно (если не нужно — удали эту строку)
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminLogin adminLoginWindow = new AdminLogin();
            adminLoginWindow.Show();
            this.Close(); // Закрываем текущее окно (если не нужно — удали эту строку)
        }

    }
}
