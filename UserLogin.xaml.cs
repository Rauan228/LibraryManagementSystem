using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LibraryManagementSystem
{
    public partial class UserLogin : Window
    {
        public static int userId;
        public static string userName;

        public UserLogin()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbUserEmail.Text) && !string.IsNullOrEmpty(tbUserPass.Password))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("UserLogin", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserEmail", tbUserEmail.Text);
                            cmd.Parameters.AddWithValue("@UserPass", tbUserPass.Password);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read()) // Если найден пользователь
                                {
                                    userId = Convert.ToInt32(reader["UserId"]);
                                    userName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : "Unknown";

                                    // Выводим сообщение об успешном входе
                                    alertUser.Text = "Вход выполнен успешно!";
                                    alertUser.Foreground = Brushes.LimeGreen;

                                    // Ждем 3 секунды перед переходом
                                    await Task.Delay(3000);

                                    // Открываем главное окно и закрываем текущее
                                    UserHome userHome = new UserHome(userId, userName);
                                    userHome.Show();
                                    this.Close();
                                }
                                else
                                {
                                    alertUser.Text = "Неверный email или пароль.";
                                    alertUser.Foreground = Brushes.Red;
                                    tbUserEmail.Clear();
                                    tbUserPass.Clear();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                alertUser.Text = "Введите email и пароль.";
                alertUser.Foreground = Brushes.Red;
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            UserRegister registerWindow = new UserRegister();
            registerWindow.Show();
            this.Close();
        }
    }
}
