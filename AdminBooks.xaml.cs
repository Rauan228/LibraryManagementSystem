using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryManagementSystem
{
    public partial class AdminBooks : UserControl
    {
        public static Book updateBook = new Book();
        private ObservableCollection<Book> allBooks = new ObservableCollection<Book>(); // Все книги
        private ObservableCollection<Book> filteredBooks = new ObservableCollection<Book>(); // Отфильтрованные книги

        public AdminBooks()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                InitializeAdminBooks(); // Загрузка данных при старте
                FilterBooks();
            };
        }



        public void InitializeAdminBooks()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    SqlCommand cmd = new SqlCommand("GetAllBooks", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    allBooks.Clear();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        allBooks.Add(new Book
                        {
                            BookName = Convert.ToString(dr["BookName"]),
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookAuthor = Convert.ToString(dr["BookAuthor"]),
                            BookISBN = Convert.ToString(dr["BookISBN"]),
                            BookCopies = Convert.ToInt32(dr["BookCopies"]),
                            BookPrice = Convert.ToInt32(dr["BookPrice"]),
                            BookGenre = Convert.ToString(dr["BookGenre"]),
                        });
                    }

                    //MessageBox.Show($"Загружено книг: {allBooks.Count}"); // Проверяем количество книг
                    dgBooks.ItemsSource = null; // Обновляем привязку
                    dgBooks.ItemsSource = allBooks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке книг: " + ex.Message);
            }
        }

        private void FilterBooks()
        {
            if (dgBooks == null) return; // Если таблицы нет — просто выйти

            string searchText = txtSearch?.Text?.Trim()?.ToLower();

            if (string.IsNullOrEmpty(searchText) || searchText == "search...") // Проверяем плейсхолдер
            {
                dgBooks.ItemsSource = allBooks;
            }
            else
            {
                var filtered = allBooks.Where(b =>
                    (b.BookName?.ToLower().Contains(searchText) ?? false) ||
                    (b.BookAuthor?.ToLower().Contains(searchText) ?? false) ||
                    (b.BookISBN?.ToLower().Contains(searchText) ?? false) ||
                    b.BookPrice.ToString().Contains(searchText)
                ).ToList();

                dgBooks.ItemsSource = filtered;
            }
        }






        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterBooks();
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


        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Book book = dgBooks.SelectedItem as Book;
            if (book != null)
            {
                updateBook = book;
                AdminUpdateBook adminUpdateBook = new AdminUpdateBook(this);
                adminUpdateBook.Show();
            }
            else
            {
                MessageBox.Show("Select a book to update...");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book book = dgBooks.SelectedItem as Book;
                if (book == null)
                {
                    MessageBox.Show("Выберите книгу для удаления!");
                    return;
                }

                if (MessageBox.Show("Вы уверены, что хотите удалить эту книгу?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }

                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    SqlCommand cmd = new SqlCommand("DeleteBook", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@BookId", book.BookId));

                    conn.Open();
                    int deleted = (int)cmd.ExecuteScalar();
                    conn.Close();

                    if (deleted > 0)
                    {
                        MessageBox.Show("Книга успешно удалена!");
                        InitializeAdminBooks();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: книга не найдена в базе данных.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка: " + ex.Message);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AdminAddBook adminAddBook = new AdminAddBook(this);
            adminAddBook.Show();
        }
    }
}
