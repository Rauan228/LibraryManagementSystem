using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AdminUpdateUser.xaml
    /// </summary>
    public partial class AdminUpdateUser : Window
    {
        private AdminUsers _adminUsersWindow; // Ссылка на AdminUsers
        private int userId;

        // Инициализация формы с текущими данными пользователя
        public AdminUpdateUser(AdminUsers adminUsersWindow) // Исправленный конструктор
        {
            InitializeComponent();
            _adminUsersWindow = adminUsersWindow;
            userId = AdminUsers.updateUser.UserId;
            tbUName.Text = AdminUsers.updateUser.UserName;
            tbUAdNo.Text = AdminUsers.updateUser.UserAdNo.ToString();
            tbUEmail.Text = AdminUsers.updateUser.UserEmail;
            tbUPass.Password = AdminUsers.updateUser.UserPass;
        }

        // Обновление пользователя напрямую через SQL-процедуру
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUName.Text) ||
                string.IsNullOrWhiteSpace(tbUAdNo.Text) ||
                string.IsNullOrWhiteSpace(tbUEmail.Text) ||
                string.IsNullOrWhiteSpace(tbUPass.Password))
            {
                MessageBox.Show("Заполните все поля! Каждое поле обязательно.");
                return;
            }

            try
            {
                int userAdNo = int.Parse(tbUAdNo.Text);

                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("UpdateUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", userId));
                        cmd.Parameters.Add(new SqlParameter("@name", tbUName.Text));
                        cmd.Parameters.Add(new SqlParameter("@adno", userAdNo));
                        cmd.Parameters.Add(new SqlParameter("@email", tbUEmail.Text));
                        cmd.Parameters.Add(new SqlParameter("@pass", tbUPass.Password));

                        object result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int updateResult) && updateResult == 1)
                        {
                            MessageBox.Show("Пользователь успешно обновлён!");
                            this.Close();

                            _adminUsersWindow?.InitializeAdminUsers(); // Обновляем список пользователей
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при обновлении пользователя! Пользователь не найден.");
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный номер допуска! Он должен быть числом.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

    }
}
