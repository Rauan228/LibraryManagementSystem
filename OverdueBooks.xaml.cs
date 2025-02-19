using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryManagementSystem
{
    public partial class OverdueBooks : UserControl
    {
        public OverdueBooks()
        {
            InitializeComponent();
            LoadOverdueBooks();
        }

        private void LoadOverdueBooks()
        {
            string connectionString = "Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true";
            string query = @"
                SELECT r.UserId, r.UserName, r.BookId, r.BookName, r.DateRecieved AS DueDate
                FROM tblRecievedUsers r
                LEFT JOIN tblReturnedUsers ret ON r.BookId = ret.BookId AND r.UserId = ret.UserId
                WHERE ret.BookId IS NULL AND DATEDIFF(DAY, r.DateRecieved, GETDATE()) > 30;
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgOverdueBooks.ItemsSource = dataTable.DefaultView;
            }
        }
    }
}
