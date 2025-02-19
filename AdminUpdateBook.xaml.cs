using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace LibraryManagementSystem
{
    public partial class AdminUpdateBook : Window
    {
        private readonly AdminBooks _parent;
        private int bookId;

        public AdminUpdateBook(AdminBooks parent)
        {
            InitializeComponent();
            _parent = parent;

            if (AdminBooks.updateBook != null)
            {
                this.bookId = AdminBooks.updateBook.BookId;
                tbBName.Text = AdminBooks.updateBook.BookName;
                tbBAuthor.Text = AdminBooks.updateBook.BookAuthor;
                tbBISBN.Text = AdminBooks.updateBook.BookISBN;
                tbBPrice.Text = AdminBooks.updateBook.BookPrice.ToString();
                tbBCopy.Text = AdminBooks.updateBook.BookCopies.ToString();
            }
            else
            {
                MessageBox.Show("Ошибка: книга для обновления не выбрана!");
                this.Close();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbBName.Text) ||
                string.IsNullOrWhiteSpace(tbBAuthor.Text) ||
                string.IsNullOrWhiteSpace(tbBISBN.Text) ||
                string.IsNullOrWhiteSpace(tbBPrice.Text) ||
                string.IsNullOrWhiteSpace(tbBCopy.Text))
            {
                MessageBox.Show("Введите все поля!");
                return;
            }

            if (!double.TryParse(tbBPrice.Text, out double price))
            {
                MessageBox.Show("Ошибка: Некорректная цена!");
                return;
            }

            if (!int.TryParse(tbBCopy.Text, out int copies))
            {
                MessageBox.Show("Ошибка: Некорректное количество копий!");
                return;
            }

            using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
            {
                try
                {
                    conn.Open();

                    // Проверяем, существует ли книга
                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tblBooks WHERE BookId = @BookId", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@BookId", this.bookId);
                        int bookExists = (int)checkCmd.ExecuteScalar();

                        if (bookExists == 0)
                        {
                            MessageBox.Show($"Ошибка: Книга с ID {this.bookId} не найдена в базе данных.");
                            return;
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("UpdateBook", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BookId", this.bookId);
                        cmd.Parameters.AddWithValue("@BookName", tbBName.Text);
                        cmd.Parameters.AddWithValue("@BookAuthor", tbBAuthor.Text);
                        cmd.Parameters.AddWithValue("@BookISBN", tbBISBN.Text);
                        cmd.Parameters.AddWithValue("@BookPrice", price);
                        cmd.Parameters.AddWithValue("@BookCopies", copies);

                        //// Логируем параметры запроса
                        //string debugMessage = $"Обновление книги: ID={this.bookId}, Name={tbBName.Text}, Author={tbBAuthor.Text}, ISBN={tbBISBN.Text}, Price={price}, Copies={copies}";
                        //MessageBox.Show(debugMessage);

                        // Используем ExecuteNonQuery() для возврата количества измененных строк
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine($"Rows affected: {rowsAffected}");

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Книга успешно обновлена!");

                            // Обновляем список книг в родительском окне
                            _parent.InitializeAdminBooks();

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка обновления! Проверьте введенные данные. Измененных строк: {rowsAffected}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении: " + ex.Message);
                }
            }
        }

    }
}
