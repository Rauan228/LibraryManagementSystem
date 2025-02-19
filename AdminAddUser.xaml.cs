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
    /// Interaction logic for AdminAddUser.xaml
    /// </summary>
    public partial class AdminAddUser : Window
    {
        private AdminUsers _adminUsersWindow; // Ссылка на AdminUsers

        public AdminAddUser(AdminUsers adminUsersWindow)
        {
            InitializeComponent();
            _adminUsersWindow = adminUsersWindow; // Сохраняем ссылку
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
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

                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP ; Database=LibraryMSWF; Integrated Security=true"))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("AddUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@name", tbUName.Text));
                        cmd.Parameters.Add(new SqlParameter("@adno", userAdNo));
                        cmd.Parameters.Add(new SqlParameter("@email", tbUEmail.Text));
                        cmd.Parameters.Add(new SqlParameter("@pass", tbUPass.Password));

                        object result = cmd.ExecuteScalar();

                        if (result != null && result.ToString() == "true")
                        {
                            MessageBox.Show("Пользователь успешно добавлен!");

                            _adminUsersWindow?.InitializeAdminUsers(); // Обновляем список пользователей

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(result + " Попробуйте снова.");
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
