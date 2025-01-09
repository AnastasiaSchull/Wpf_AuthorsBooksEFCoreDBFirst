using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Wpf_AuthorsBooksEFCoreDBFirst.Models;

namespace Wpf_AuthorsBooksEFCoreDBFirst
{
  
    public partial class MainWindow : Window
    {
        private readonly BooksContext _context = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _context.Authors.Local.ToObservableCollection();
            _context.Authors.Load();
        }
    }
}