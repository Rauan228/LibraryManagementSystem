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

namespace LibraryManagementSystem
{
    /// <summary>
    /// Interaction logic for AdminHome.xaml
    /// </summary>
    public partial class AdminHome : Window
    {
        //SHOW BOOK MENU USER CONTROLLER ONLY =>PL
        public AdminHome()
        {
            InitializeComponent();
            AdminBooks adminBooks = new AdminBooks();
            adminStackPanel.Children.Clear();
            adminStackPanel.Children.Add(adminBooks);
        }

        //SHOW BOOK MENU USER CONTROLLER ONLY =>PL
        private void BtnBooks_Click(object sender, RoutedEventArgs e)
        {
            AdminBooks adminBooks = new AdminBooks();
            adminStackPanel.Children.Clear();
            adminStackPanel.Children.Add(adminBooks);
        }

        //SHOW USER MENU USER CONTROLLER ONLY =>PL
        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            AdminUsers adminUsers = new AdminUsers();
            adminStackPanel.Children.Clear();
            adminStackPanel.Children.Add(adminUsers);
        }

        //SHOW REQUEST MENU USER CONTROLLER ONLY =>PL
        private void BtnRequests_Click(object sender, RoutedEventArgs e)
        {
            AdminRequests adminRequests = new AdminRequests();
            adminStackPanel.Children.Clear();
            adminStackPanel.Children.Add(adminRequests);
        }

        //SHOW ACCEPTED MENU USER CONTROLLER ONLY =>PL
        private void BtnAccepted_Click(object sender, RoutedEventArgs e)
        {
            AdminAccepted adminAccepted = new AdminAccepted();
            adminStackPanel.Children.Clear();
            adminStackPanel.Children.Add(adminAccepted);
        }

        //LOGOUT ADMIN HOME =>PL
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //SHOW RETURN MENU USER CONTROLLER ONLY =>PL
        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            AdminReturn adminReturn = new AdminReturn();
            adminStackPanel.Children.Clear();
            adminStackPanel.Children.Add(adminReturn);
        }

        private void BtnBooksOnHand_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу "Books on Hand"
            BooksOnHand booksOnHand = new BooksOnHand();
            adminStackPanel.Children.Clear();
            adminStackPanel.Children.Add(booksOnHand);
        }

        private void BtnBooksPerUser_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу "Books per User"
            BooksPerUser booksPerUser = new BooksPerUser();
            adminStackPanel.Children.Clear();
            adminStackPanel.Children.Add(booksPerUser);
        }

        private void BtnOverdueBooks_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу "Overdue Books"
            OverdueBooks overdueBooks = new OverdueBooks();
            adminStackPanel.Children.Clear();
            adminStackPanel.Children.Add(overdueBooks);
        }

        private void BtnBooksByGenre_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу "Books by Genre"
            BooksByGenre booksByGenre = new BooksByGenre();
            adminStackPanel.Children.Clear();
            adminStackPanel.Children.Add(booksByGenre);
        }
    }
}