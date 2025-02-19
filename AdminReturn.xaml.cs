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
    /// Interaction logic for AdminReturn.xaml
    /// </summary>
    public partial class AdminReturn : UserControl
    {
        //INITIALIZE THE RETURN GV =>PL
        private ObservableCollection<ReturnedBook> allReturnedBooks = new ObservableCollection<ReturnedBook>();

        public AdminReturn()
        {
            InitializeComponent();
            Loaded += AdminReturn_Loaded; // Добавляем обработчик Loaded
        }

        private void AdminReturn_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeAdminReturn();
        }


        public void InitializeAdminReturn()
        {

            try
            {
                string connectionString = "Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true";
                
                allReturnedBooks.Clear(); // Очищаем список перед загрузкой новых данных


                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT BookName, BookId, UserId, UserName, DateReturned FROM tblReturnedUsers";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allReturnedBooks.Add(new ReturnedBook
                            {
                                BookName = reader["BookName"].ToString(),
                                BookId = Convert.ToInt32(reader["BookId"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                UserName = reader["UserName"].ToString(),
                                DateReturned = Convert.ToDateTime(reader["DateReturned"]).ToShortDateString(),
                            });
                        }
                    }
                }

                dgReturn.ItemsSource = allReturnedBooks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgReturn == null) return; // Проверяем, чтобы избежать NullReferenceException

            string searchText = txtSearch.Text.ToLower();

            var filteredList = allReturnedBooks.Where(book =>
                book.BookName.ToLower().Contains(searchText) ||
                book.UserName.ToLower().Contains(searchText) ||
                book.DateReturned.ToLower().Contains(searchText)).ToList();

            dgReturn.ItemsSource = filteredList;
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

        //DELETE THE RETURN BOOK =>PL
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReturnedBook returned = dgReturn.SelectedItem as ReturnedBook;
                if (returned != null)
                {
                    string connectionString = "Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true";
                    bool isDone1 = false;
                    bool isDone2 = false;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Удаляем запись из таблицы возвратов
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM tblReturnedUsers WHERE BookId = @bId AND UserId = @uId", conn))
                        {
                            cmd.Parameters.AddWithValue("@bId", returned.BookId);
                            cmd.Parameters.AddWithValue("@uId", returned.UserId);
                            isDone1 = cmd.ExecuteNonQuery() > 0;
                        }

                        // Увеличиваем количество копий книги
                        using (SqlCommand cmd = new SqlCommand("UPDATE tblBooks SET BookCopies = BookCopies + 1 WHERE BookId = @bId", conn))
                        {
                            cmd.Parameters.AddWithValue("@bId", returned.BookId);
                            isDone2 = cmd.ExecuteNonQuery() > 0;
                        }
                    }

                    if (isDone1 && isDone2)
                    {
                        MessageBox.Show("Book taken back successfully...");
                        InitializeAdminReturn();
                    }
                    else
                    {
                        MessageBox.Show("Try again...");
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
}
