using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class RequestedBook
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DateRequested { get; set; }
    }
}
