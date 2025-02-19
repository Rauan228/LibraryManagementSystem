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
    /// Interaction logic for UserBorrow.xaml
    /// </summary>
    public partial class UserBorrow : UserControl
    {
        private int userId;
        private string userName;

        public UserBorrow(int userId, string userName)
        {
            InitializeComponent();
            this.userId = userId;
            this.userName = userName;
            InitializeUserBorrow();
        }

        public void InitializeUserBorrow()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    SqlCommand cmd = new SqlCommand("GetAllBooks", conn); // Используем ту же хранимую процедуру
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    ObservableCollection<Book> lst = new ObservableCollection<Book>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lst.Add(new Book
                        {
                            BookName = Convert.ToString(dr["BookName"]),
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookAuthor = Convert.ToString(dr["BookAuthor"]),
                            BookISBN = Convert.ToString(dr["BookISBN"]),
                            BookCopies = Convert.ToInt32(dr["BookCopies"]),
                            BookPrice = Convert.ToInt32(dr["BookPrice"]),
                        });
                    }

                    dgBorrow.ItemsSource = lst;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке книг: " + ex.Message);
            }
        }

        //REQUEST TO BORROW A BOOK FROM BORROW BOOK TABLE =>PL
        private void BtnRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book book = dgBorrow.SelectedItem as Book;
                if (book != null)
                {
                    if (book.BookCopies == 0)
                    {
                        MessageBox.Show("Book is not available...");
                        return;
                    }

                    using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                    {
                        conn.Open();
                        SqlTransaction transaction = conn.BeginTransaction();

                        try
                        {
                            // Уменьшаем количество копий книги
                            SqlCommand updateCmd = new SqlCommand("UPDATE tblBooks SET BookCopies = BookCopies - 1 WHERE BookId = @BookId", conn, transaction);
                            updateCmd.Parameters.AddWithValue("@BookId", book.BookId);
                            int rowsAffected1 = updateCmd.ExecuteNonQuery();

                            // Добавляем запрос пользователя на книгу
                            SqlCommand insertCmd = new SqlCommand(
                                "INSERT INTO tblRequestedUsers (BookId, BookName, UserId, UserName, DateRequested) VALUES (@BookId, @BookName, @UserId, @UserName, @DateRequested)",
                                conn, transaction);
                            insertCmd.Parameters.AddWithValue("@BookId", book.BookId);
                            insertCmd.Parameters.AddWithValue("@BookName", book.BookName);
                            insertCmd.Parameters.AddWithValue("@UserId", this.userId);
                            insertCmd.Parameters.AddWithValue("@UserName", this.userName);
                            insertCmd.Parameters.AddWithValue("@DateRequested", DateTime.Now); // Добавляем дату

                            int rowsAffected2 = insertCmd.ExecuteNonQuery();

                            // Если оба запроса выполнились успешно, фиксируем изменения
                            if (rowsAffected1 > 0 && rowsAffected2 > 0)
                            {
                                transaction.Commit();
                                MessageBox.Show("Requested successfully.");
                                InitializeUserBorrow();
                            }
                            else
                            {
                                transaction.Rollback();
                                MessageBox.Show("Request failed. Try again.");
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select a book to request...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

    }
}
