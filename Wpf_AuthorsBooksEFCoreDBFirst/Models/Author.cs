using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_AuthorsBooksEFCoreDBFirst.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }

}
