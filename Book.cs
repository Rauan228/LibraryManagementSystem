using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string BookISBN { get; set; }
        public int BookPrice { get; set; }
        public int BookCopies { get; set; }
        public string BookGenre { get; set; } // Добавляем свойство жанра
    }

}
