using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryManagementSystem
{
    public partial class BooksByGenre : UserControl
    {
        public BooksByGenre()
        {
            InitializeComponent();
            LoadBooksByGenre();
        }

        private void LoadBooksByGenre()
        {
            string connectionString = "Server=DESKTOP-KJNEIGP; Database=LibraryMSWF; Integrated Security=true";
            string query = @"
                SELECT b.BookGenre, COUNT(r.BookId) AS BooksIssued
                FROM tblRecievedUsers r
                JOIN tblBooks b ON r.BookId = b.BookId
                GROUP BY b.BookGenre;
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgBooksByGenre.ItemsSource = dataTable.DefaultView;
            }
        }
    }
}