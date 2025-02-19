using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementSystem
{
    public partial class UserTransaction : UserControl
    {
        private int userId;
        private string userName;
        private readonly string connectionString = "Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true";

        public UserTransaction()
        {
            InitializeComponent();
            InitializeUserTransaction();
        }
        private void InitializeUserTransaction()
        {
            try
            {
                userId = UserLogin.userId;
                userName = GetUserName(userId); // <-- добавляем получение имени пользователя
                dgRequest.ItemsSource = GetRequestedBooks(userId);
                dgReturn.ItemsSource = GetReceivedBooks(userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private string GetUserName(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT UserName FROM tblUsers WHERE UserId = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
        }


        private ObservableCollection<RequestedBook> GetRequestedBooks(int userId)
        {
            var lstRequest = new ObservableCollection<RequestedBook>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllRequestUser", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@userId", userId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    lstRequest.Add(new RequestedBook
                    {
                        BookName = Convert.ToString(dr["BookName"]),
                        BookId = Convert.ToInt32(dr["BookId"]),
                        DateRequested = Convert.ToDateTime(dr["DateRequested"]).ToShortDateString(),
                    });
                }
            }
            return lstRequest;
        }

        private ObservableCollection<AcceptedBook> GetReceivedBooks(int userId)
        {
            var lstReceived = new ObservableCollection<AcceptedBook>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllRecieveUser", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@userId", userId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    lstReceived.Add(new AcceptedBook
                    {
                        BookName = Convert.ToString(dr["BookName"]),
                        BookId = Convert.ToInt32(dr["BookId"]),
                        DateRecieved = Convert.ToDateTime(dr["DateRecieved"]).ToShortDateString(),
                    });
                }
            }
            return lstReceived;
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AcceptedBook accepted = dgReturn.SelectedItem as AcceptedBook;
                if (accepted != null)
                {
                    if (ReturnBook(accepted.BookId, userId, accepted.BookName, userName))

                    {
                        MessageBox.Show("Book returned successfully...");
                        InitializeUserTransaction();
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
            catch (Exception)
            {
                MessageBox.Show("Some unknown exception occurred! Try again.");
            }
        }

        private bool ReturnBook(int bookId, int userId, string bookName, string userName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmdReturn = new SqlCommand("AddReturn", conn, transaction) { CommandType = CommandType.StoredProcedure };
                        cmdReturn.Parameters.AddWithValue("@bId", bookId);
                        cmdReturn.Parameters.AddWithValue("@uId", userId);
                        cmdReturn.Parameters.AddWithValue("@bName", bookName);
                        cmdReturn.Parameters.AddWithValue("@uName", userName); // Добавляем @uName
                        cmdReturn.Parameters.AddWithValue("@date", DateTime.Now); // Добавляем дату возврата
                        cmdReturn.ExecuteNonQuery();

                        SqlCommand cmdDelete = new SqlCommand("DeleteRecieve", conn, transaction) { CommandType = CommandType.StoredProcedure };
                        cmdDelete.Parameters.AddWithValue("@bId", bookId);
                        cmdDelete.Parameters.AddWithValue("@uId", userId);
                        cmdDelete.ExecuteNonQuery();

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка в ReturnBook: {ex.Message}");
                        return false;
                    }
                }
            }
        }



    }
}