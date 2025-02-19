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
    public partial class AdminRequests : UserControl
    {
        private ObservableCollection<Request> allRequests = new ObservableCollection<Request>(); // Все заявки
        private ObservableCollection<Request> filteredRequests = new ObservableCollection<Request>(); // Отфильтрованные

        public AdminRequests()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                InitializeRequests();
                //FilterRequests();
            };
        }

        public void InitializeRequests()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    SqlCommand cmd = new SqlCommand("GetAllRequest", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    allRequests.Clear();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        allRequests.Add(new Request
                        {
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookName = Convert.ToString(dr["BookName"]),
                            DateRequested = Convert.ToDateTime(dr["DateRequested"]),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            UserName = Convert.ToString(dr["UserName"])
                        });
                    }

                    dgRequests.ItemsSource = null; // Обновляем привязку
                    dgRequests.ItemsSource = allRequests;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заявок: " + ex.Message);
            }
        }

        private void FilterRequests()
        {
            if (dgRequests == null) return;

            string searchText = txtSearch?.Text?.Trim()?.ToLower();

            if (string.IsNullOrEmpty(searchText) || searchText == "search...")
            {
                dgRequests.ItemsSource = allRequests;
            }
            else
            {
                var filtered = allRequests.Where(r =>
                    (r.BookName?.ToLower().Contains(searchText) ?? false) ||
                    (r.UserName?.ToLower().Contains(searchText) ?? false) ||
                    r.UserId.ToString().Contains(searchText) ||
                    r.BookId.ToString().Contains(searchText)
                ).ToList();

                dgRequests.ItemsSource = filtered;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterRequests();
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



        //ACCEPT THE REQUESTED BOOK =>PL
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Request request = dgRequests.SelectedItem as Request;
                if (request == null)
                {
                    MessageBox.Show("Выберите книгу перед принятием запроса.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection("Server=localhost; Database=LibraryMSWF; Integrated Security=true"))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // Вывод передаваемых параметров
                        //string debugInfo = $"Параметры запроса: BookId={request.BookId}, BookName={request.BookName}, " +
                        //                   $"UserId={request.UserId}, UserName={request.UserName}, Date={DateTime.Now}";
                        //MessageBox.Show(debugInfo);

                        using (SqlCommand cmd = new SqlCommand("AddRecieve", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@bId", request.BookId));
                            cmd.Parameters.Add(new SqlParameter("@bName", request.BookName));
                            cmd.Parameters.Add(new SqlParameter("@uId", request.UserId));
                            cmd.Parameters.Add(new SqlParameter("@uName", request.UserName));
                            cmd.Parameters.Add(new SqlParameter("@date", DateTime.Now));

                            int result = Convert.ToInt32(cmd.ExecuteScalar()); // Получаем результат

                            if (result == 1)
                            {
                                // Успех
                            }
                            else if (result == 0)
                            {
                                throw new Exception("Ошибка при добавлении книги в список полученных.");
                            }
                            else if (result == -1)
                            {
                                throw new Exception("Ошибка: книга или пользователь не существуют.");
                            }
                        }


                        using (SqlCommand cmd = new SqlCommand("DeleteRequest", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@bId", request.BookId);
                            cmd.Parameters.AddWithValue("@uId", request.UserId);

                            SqlParameter outputParam = new SqlParameter("@Deleted", SqlDbType.Bit);
                            outputParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(outputParam);

                            cmd.ExecuteNonQuery();

                            bool isDeleted = Convert.ToBoolean(outputParam.Value);
                            if (!isDeleted)
                            {
                                throw new Exception("Ошибка: запрос не найден или уже удалён.");
                            }
                        }


                        transaction.Commit();
                        MessageBox.Show("Запрос успешно принят!");
                        InitializeRequests();
                    }
                    catch (SqlException sqlEx)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"SQL Ошибка {sqlEx.Number}: {sqlEx.Message}\nСтрока: {sqlEx.LineNumber}\nСтек вызовов:\n{sqlEx.StackTrace}");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка: {ex.Message}\nСтек вызовов:\n{ex.StackTrace}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}\nСтек вызовов:\n{ex.StackTrace}");
            }
        }



        //REJECT THE REQUESTED BOOK =>PL
        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Request request = dgRequests.SelectedItem as Request;
                if (request == null)
                {
                    MessageBox.Show("Выберите книгу перед отклонением запроса.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction(); // Используем транзакцию

                    try
                    {
                        // Увеличение количества копий книги
                        using (SqlCommand cmd = new SqlCommand("IncBookCopy", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@BookId", request.BookId);

                            // Добавляем выходной параметр для возврата значения
                            SqlParameter returnParameter = new SqlParameter();
                            returnParameter.Direction = ParameterDirection.ReturnValue;
                            cmd.Parameters.Add(returnParameter);

                            cmd.ExecuteNonQuery();

                            int rowsAffected1 = Convert.ToInt32(returnParameter.Value);

                            if (rowsAffected1 <= 0)
                            {
                                throw new Exception("Ошибка: книга с таким BookId не найдена.");
                            }
                        }




                        // Удаление запроса на книгу
                        using (SqlCommand cmd = new SqlCommand("DeleteRequest", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@bId", request.BookId));
                            cmd.Parameters.Add(new SqlParameter("@uId", request.UserId));

                            // Добавляем выходной параметр
                            SqlParameter deletedParam = new SqlParameter("@Deleted", SqlDbType.Bit);
                            deletedParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(deletedParam);

                            cmd.ExecuteNonQuery();

                            bool isDeleted = Convert.ToBoolean(deletedParam.Value);

                            if (!isDeleted)
                            {
                                throw new Exception("Ошибка: запрос не найден.");
                            }
                        }


                        transaction.Commit(); // Подтверждаем транзакцию
                        MessageBox.Show("Запрос успешно отклонён!");
                        InitializeRequests(); // Обновляем список запросов
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Откат транзакции при ошибке
                        MessageBox.Show("Ошибка при отклонении запроса: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла неизвестная ошибка: " + ex.Message);
            }
        }

    }

    public class Request
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public DateTime DateRequested { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
