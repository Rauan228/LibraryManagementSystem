using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryManagementSystem
{
    public partial class BooksOnHand : UserControl
    {
        public BooksOnHand()
        {
            InitializeComponent();
            LoadBooksOnHand();
        }

        private void LoadBooksOnHand()
        {
            string connectionString = "Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true";
            string query = @"
                SELECT 
                    tblRecievedUsers.BookId, 
                    tblRecievedUsers.BookName, 
                    tblRecievedUsers.UserId, 
                    tblRecievedUsers.UserName, 
                    tblRecievedUsers.DateRecieved AS DueDate
                FROM 
                    tblRecievedUsers
                LEFT JOIN 
                    tblReturnedUsers ON tblRecievedUsers.BookId = tblReturnedUsers.BookId
                    AND tblRecievedUsers.UserId = tblReturnedUsers.UserId
                WHERE 
                    tblReturnedUsers.BookId IS NULL;
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgBooksOnHand.ItemsSource = dataTable.DefaultView;
            }
        }

        private void TxtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Search...")
            {
                txtSearch.Text = string.Empty;
                txtSearch.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TxtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search...";
                txtSearch.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Добавьте здесь логику для обработки изменений текста поиска
        }
    }
}
