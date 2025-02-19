using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryManagementSystem
{
    public partial class BooksPerUser : UserControl
    {
        public BooksPerUser()
        {
            InitializeComponent();
            LoadBooksPerUser();
        }

        private void LoadBooksPerUser()
        {
            string connectionString = "Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true";
            string query = @"
                SELECT 
                    UserId, 
                    UserName, 
                    COUNT(BookId) AS BooksCount
                FROM 
                    tblRecievedUsers
                GROUP BY 
                    UserId, 
                    UserName;
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgBooksPerUser.ItemsSource = dataTable.DefaultView;
            }
        }
    }
}
