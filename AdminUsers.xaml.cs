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
    /// Interaction logic for AdminUsers.xaml
    /// </summary>
    public partial class AdminUsers : UserControl
    {
        public static User updateUser = new User();
        //INITIALIZE THE USERS GV =>PL
        public AdminUsers()
        {
            InitializeComponent();
            this.Loaded += AdminUsers_Loaded;
        }

        private void AdminUsers_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeAdminUsers(); // ✅ dgUsers уже доступен
        }


        private ObservableCollection<User> allUsers = new ObservableCollection<User>();
        private ObservableCollection<User> filteredUsers = new ObservableCollection<User>();

        public void InitializeAdminUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllUsers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        allUsers.Clear();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            allUsers.Add(new User
                            {
                                UserName = Convert.ToString(dr["UserName"]),
                                UserId = Convert.ToInt32(dr["UserId"]),
                                UserEmail = Convert.ToString(dr["UserEmail"]),
                                UserPass = Convert.ToString(dr["UserPass"]),
                                UserAdNo = Convert.ToString(dr["UserAdNo"]), // ✅ Исправлено
                            });
                        }


                        // ✅ Присваиваем список пользователей в DataGrid сразу после загрузки
                        dgUsers.ItemsSource = allUsers;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        // Метод фильтрации пользователей
        private void FilterUsers()
        {
            if (dgUsers == null) return; // Защита от null

            string searchText = txtSearch?.Text?.Trim()?.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                dgUsers.ItemsSource = allUsers;
            }
            else
            {
                filteredUsers.Clear();
                foreach (var user in allUsers.Where(u =>
                    u.UserName.ToLower().Contains(searchText) ||
                    u.UserAdNo.ToString().Contains(searchText) ||
                    u.UserEmail.ToLower().Contains(searchText)))
                {
                    filteredUsers.Add(user);
                }
                dgUsers.ItemsSource = filteredUsers;
            }
        }


        // Обработчик события изменения текста в поиске
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterUsers();
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



        //OPEN UPDATE USER WINDOW
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = dgUsers.SelectedItem as User;
                if (user != null)
                {
                    updateUser = user;
                    AdminUpdateUser adminUpdateUser = new AdminUpdateUser(this);
                    adminUpdateUser.Show();
                }
                else
                {
                    MessageBox.Show("Select a user to update...");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some unknown exception is occured!!!, Try again..");
            }
        }

        //DELTE USER FROM USER TABLE
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = dgUsers.SelectedItem as User;
                if (user == null)
                {
                    MessageBox.Show("Выберите пользователя для удаления!");
                    return;
                }

                if (MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }

                // Подключение к базе данных и выполнение хранимой процедуры
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true"))
                {
                    conn.Open();

                    //MessageBox.Show("Удаляется пользователь с ID: " + user.UserId);

                    using (SqlCommand cmd = new SqlCommand("DeleteUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserId", user.UserId));

                        SqlParameter outputParam = new SqlParameter("@Deleted", SqlDbType.Int);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        cmd.ExecuteNonQuery();

                        int deleted = (outputParam.Value != DBNull.Value) ? Convert.ToInt32(outputParam.Value) : 0;

                        if (deleted > 0)
                        {
                            MessageBox.Show("Пользователь успешно удалён!");
                            InitializeAdminUsers(); // Обновляем список пользователей
                        }
                        else
                        {
                            MessageBox.Show("Ошибка: пользователь не найден или уже удалён.");
                        }
                    }



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }


        //OPEN ADD USER WINDOW =>PL
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AdminAddUser adminAddUser = new AdminAddUser(this);
            adminAddUser.Show();
        }


    }
    
}
