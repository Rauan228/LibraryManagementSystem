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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace LibraryManagementSystem
{
    /// <summary>
    /// Interaction logic for AdminAccepted.xaml
    /// </summary>
    public partial class AdminAccepted : UserControl
    {
        //INITIALIZE THE ACCEPTED GV =>PL
        private ObservableCollection<AcceptedBook> allAcceptedBooks = new ObservableCollection<AcceptedBook>();

        public AdminAccepted()
        {
            InitializeComponent();
            InitializeAdminAccepted();
        }

        public void InitializeAdminAccepted()
        {
            try
            {
                string connectionString = "Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT BookId, BookName, UserId, UserName, DateRecieved FROM tblRecievedUsers";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        allAcceptedBooks.Clear();
                        while (reader.Read())
                        {
                            allAcceptedBooks.Add(new AcceptedBook
                            {
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                BookName = reader.GetString(reader.GetOrdinal("BookName")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                DateRecieved = reader.GetDateTime(reader.GetOrdinal("DateRecieved")).ToShortDateString()
                            });
                        }

                        dgAccepted.ItemsSource = allAcceptedBooks;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void FilterAcceptedBooks()
        {
            if (dgAccepted == null) return;

            string searchText = txtSearch?.Text?.Trim()?.ToLower();

            if (string.IsNullOrEmpty(searchText) || searchText == "search...")
            {
                dgAccepted.ItemsSource = allAcceptedBooks;
            }
            else
            {
                var filtered = allAcceptedBooks.Where(book =>
                    (book.BookName?.ToLower().Contains(searchText) ?? false) ||
                    (book.UserName?.ToLower().Contains(searchText) ?? false) ||
                    (book.DateRecieved?.ToLower().Contains(searchText) ?? false)
                ).ToList();

                dgAccepted.ItemsSource = filtered;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAcceptedBooks();
        }

        private void TxtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Search...")
            {
                txtSearch.Text = "";
                txtSearch.Foreground = Brushes.Black;
            }
        }

        private void TxtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search...";
                txtSearch.Foreground = Brushes.Gray;
            }
        }


        // DELETE THE RECIEVED BOOK => PL
        private void btnTake_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AcceptedBook accept = dgAccepted.SelectedItem as AcceptedBook;
                if (accept != null)
                {
                    string connectionString = "Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlTransaction transaction = conn.BeginTransaction()) // Начинаем транзакцию
                        {
                            try
                            {
                                // Удаляем запись из tblRecievedUsers
                                string deleteQuery = "DELETE FROM tblRecievedUsers WHERE BookId = @BookId AND UserId = @UserId";
                                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction))
                                {
                                    deleteCmd.Parameters.AddWithValue("@BookId", accept.BookId);
                                    deleteCmd.Parameters.AddWithValue("@UserId", accept.UserId);
                                    deleteCmd.ExecuteNonQuery();
                                }

                                // Увеличиваем количество копий книги
                                string updateQuery = "UPDATE tblBooks SET BookCopies = BookCopies + 1 WHERE BookId = @BookId";
                                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction))
                                {
                                    updateCmd.Parameters.AddWithValue("@BookId", accept.BookId);
                                    updateCmd.ExecuteNonQuery();
                                }

                                transaction.Commit(); // Фиксируем изменения в базе данных
                                MessageBox.Show("Book taken back successfully...");
                                InitializeAdminAccepted();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback(); // Откатываем изменения при ошибке
                                MessageBox.Show($"Ошибка: {ex.Message}");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select a book properly...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

    }

    public class AcceptedBook
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DateRecieved { get; set; }
    }

}
