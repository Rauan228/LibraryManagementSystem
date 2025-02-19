using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for AdminAddBook.xaml
    /// </summary>
    public partial class AdminAddBook : Window
    {
        private AdminBooks _adminBooks;

        public AdminAddBook(AdminBooks adminBooks)
        {
            InitializeComponent();
            _adminBooks = adminBooks;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
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

            if (!decimal.TryParse(tbBPrice.Text, out decimal price))
            {
                MessageBox.Show("Ошибка: Некорректная цена!");
                return;
            }

            if (!int.TryParse(tbBCopy.Text, out int copies))
            {
                MessageBox.Show("Ошибка: Некорректное количество копий!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    SqlCommand cmd = new SqlCommand("AddBook", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@BookName", tbBName.Text));
                    cmd.Parameters.Add(new SqlParameter("@BookAuthor", tbBAuthor.Text));
                    cmd.Parameters.Add(new SqlParameter("@BookISBN", tbBISBN.Text));
                    cmd.Parameters.Add(new SqlParameter("@BookPrice", price));
                    cmd.Parameters.Add(new SqlParameter("@BookCopies", copies));

                    // Добавляем OUTPUT параметр
                    SqlParameter outputParam = new SqlParameter("@Result", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    int rowsAffected = (int)cmd.Parameters["@Result"].Value;
                    conn.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Книга успешно добавлена!");

                        // Обновляем список книг
                        _adminBooks.InitializeAdminBooks();

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления! Проверьте введенные данные.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка: " + ex.Message);
            }
        }
    }

}
