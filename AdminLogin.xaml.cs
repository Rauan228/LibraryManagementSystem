using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagementSystem
{
    public partial class AdminLogin : Window
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        // Делаем метод асинхронным
        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = tbAdminEmail.Text.Trim();
            string password = tbAdminPass.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                alertAdmin.Text = "Введите email и пароль.";
                successAdmin.Text = ""; // Очищаем сообщение об успехе
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("AdminLogin", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@adminEmail", System.Data.SqlDbType.NVarChar, 100)).Value = email;
                        cmd.Parameters.Add(new SqlParameter("@adminPass", System.Data.SqlDbType.NVarChar, 100)).Value = password;

                        object result = cmd.ExecuteScalar();
                        bool isAuthenticated = result != null && Convert.ToInt32(result) > 0;

                        if (isAuthenticated)
                        {
                            alertAdmin.Text = ""; // Очищаем сообщение об ошибке
                            successAdmin.Text = "Успешный вход!"; // Показываем сообщение об успехе

                            // Ждем 3 секунды
                            await Task.Delay(3000);

                            // Открываем главное окно администратора
                            AdminHome adminHome = new AdminHome();
                            adminHome.Show();

                            // Очищаем поля и закрываем текущее окно
                            tbAdminEmail.Clear();
                            tbAdminPass.Clear();
                            this.Close();
                        }
                        else
                        {
                            alertAdmin.Text = "Неверный email или пароль.";
                            successAdmin.Text = ""; // Очищаем сообщение об успехе
                            tbAdminPass.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\nДетали:\n{ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}