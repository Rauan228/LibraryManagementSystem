using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagementSystem
{
    public partial class UserRegister : Window
    {
        public UserRegister()
        {
            InitializeComponent();
        }

        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = tbFullName.Text.Trim();
            string email = tbEmail.Text.Trim();
            string password = tbPassword.Password.Trim();
            string userAdNo = tbUserAdNo.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(userAdNo))
            {
                alertUser.Text = "Все поля обязательны!";
                alertUser.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(*) FROM tblUsers WHERE UserEmail = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userExists > 0)
                        {
                            alertUser.Text = "Email уже зарегистрирован!";
                            alertUser.Visibility = Visibility.Visible;
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO tblUsers (UserName, UserEmail, UserPass, UserAdNo) OUTPUT INSERTED.UserId VALUES (@Name, @Email, @Pass, @AdNo)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Pass", password);
                        cmd.Parameters.AddWithValue("@AdNo", userAdNo);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int userId = Convert.ToInt32(result);

                            // Скрываем возможные сообщения об ошибках
                            alertUser.Visibility = Visibility.Collapsed;

                            // Показываем сообщение об успешной регистрации
                            successMessage.Text = "Регистрация успешна! Переход на страницу входа...";
                            successMessage.Visibility = Visibility.Visible;

                            // Задержка 2 секунды перед переходом
                            await Task.Delay(2000);

                            // Переход на страницу входа
                            UserLogin loginWindow = new UserLogin();
                            loginWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            alertUser.Text = "Ошибка регистрации! Попробуйте снова.";
                            alertUser.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            UserLogin loginWindow = new UserLogin();
            loginWindow.Show();
            this.Close();
        }
    }
}
