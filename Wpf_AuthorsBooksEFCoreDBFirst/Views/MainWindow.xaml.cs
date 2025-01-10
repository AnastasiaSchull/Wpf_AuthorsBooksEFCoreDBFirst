using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Wpf_AuthorsBooksEFCoreDBFirst.Models;


namespace Wpf_AuthorsBooksEFCoreDBFirst.Views
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly BooksContext _context = new();
        private Author? _selectedAuthor = new Author();

        public event PropertyChangedEventHandler? PropertyChanged;
       
        public Author? SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged(nameof(SelectedAuthor));
                OnPropertyChanged(nameof(SelectedAuthor.Books));

            }
        }

        private Book? _selectedBook;
        public Book? SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainWindow()
        {
            InitializeComponent();

            // загружаем данные и привязываем к интерфейсу
            _context.Authors.Load();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;

            DataContext = this;
            if (_context.Authors.Local.Any())
            {
                SelectedAuthor = _context.Authors.Local.First(); // устанавливаем 1го автора, если список не пустой
            }
            else
            {
                SelectedAuthor = new Author { Name = "No Author Available" }; 
            }
            AuthorsComboBox.ItemsSource = _context.Authors.Local.ToObservableCollection();
        }

        // обработчик для добавления автора
        private void MenuItem_AddAuthor_Click(object sender, RoutedEventArgs e)
        {         
            var inputWindow = new InputAuthorWindow();
            if (inputWindow.ShowDialog() == true) // если "Save"
            {
                var newAuthor = new Author { Name = inputWindow.AuthorName };
                _context.Authors.Add(newAuthor);
                _context.SaveChanges();
                // обновляем ComboBox 
                AuthorsComboBox.ItemsSource = null;
                AuthorsComboBox.ItemsSource = _context.Authors.Local.ToObservableCollection();
            }
        }

        // для удаления автора
        private void MenuItem_RemoveAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAuthor != null)
            {
                _context.Authors.Remove(SelectedAuthor);
                _context.SaveChanges();
                MessageBox.Show("Author removed successfully!");
                // обновляем ComboBox
                AuthorsComboBox.ItemsSource = null;
                AuthorsComboBox.ItemsSource = _context.Authors.Local.ToObservableCollection();
            }
            else
            {
                MessageBox.Show("No author selected");
            }
        }


        // для редактирования автора
        private void MenuItem_EditAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAuthor != null)
            {
                // откр окно
                var inputWindow = new InputAuthorWindow(SelectedAuthor.Name);

                if (inputWindow.ShowDialog() == true)
                {
                    SelectedAuthor.Name = inputWindow.AuthorName;
                    _context.SaveChanges();
                    // обновляем список выпадающий
                    AuthorsComboBox.ItemsSource = null;
                    AuthorsComboBox.ItemsSource = _context.Authors.Local.ToObservableCollection();
                }
            }
            else
            {
                MessageBox.Show("No author selected");
            }
        }


        private void MenuItem_AddBook_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAuthor != null)
            {
                // новое окно для ввода данных о книге
                var inputBookWindow = new InputBookWindow();
                //если нажали save
                if (inputBookWindow.ShowDialog() == true)
                {
                    // получение данных из нового окна
                    var newBook = new Book
                    {
                        Title = inputBookWindow.BookTitle,
                        Author = SelectedAuthor
                    };

                    // добавление книги в базу данных
                    _context.Books.Add(newBook);
                    _context.SaveChanges();

                    MessageBox.Show("Book added successfully!");
   
                    // обновляем список выпадающий
                    AuthorsComboBox.ItemsSource = null;
                    AuthorsComboBox.ItemsSource = _context.Authors.Local.ToObservableCollection();
            
                }
            }
            else
            {
                MessageBox.Show("No author selected");
            }
        }

        // для удаления книги
        private void MenuItem_RemoveBook_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBook != null)
            {
                _context.Books.Remove(SelectedBook);
                _context.SaveChanges();

                // обновляем список выпадающий
                AuthorsComboBox.ItemsSource = null;
                AuthorsComboBox.ItemsSource = _context.Authors.Local.ToObservableCollection();
            }
            else
            {
                MessageBox.Show("No book selected");
            }
        }

        // для редактирования книги
        private void MenuItem_EditBook_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBook != null)
            {
                var inputBookWindow = new InputBookWindow();
                inputBookWindow.BookTitleTextBox.Text = SelectedBook.Title;

                if (inputBookWindow.ShowDialog() == true)
                {
                    SelectedBook.Title = inputBookWindow.BookTitle;
                    _context.SaveChanges();

                    // обновляем список выпадающий
                    AuthorsComboBox.ItemsSource = null;
                    AuthorsComboBox.ItemsSource = _context.Authors.Local.ToObservableCollection();
                }
            }
            else
            {
                MessageBox.Show("No book selected");
            }
        }


        // для фильтра
        private void FilterCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectedBook != null)
            {
                var authorsSnapshot = _context.Authors.Local.ToList();
                var filteredAuthors = authorsSnapshot
                            .Where(a => a.Books.Any(b => b.Title == SelectedBook.Title))
                    .ToList();

                AuthorsComboBox.ItemsSource = null;
                AuthorsComboBox.ItemsSource = filteredAuthors;
            }
            else
            {
                MessageBox.Show("Please select a book to filter authors!");
            }
        }


        private void FilterCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AuthorsComboBox.ItemsSource = null;
            AuthorsComboBox.ItemsSource = _context.Authors.Local.ToObservableCollection();
        }

        // для выхода из приложения
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // для сохранения данных
        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            MessageBox.Show("Changes saved!");
        }    

        private void AuthorsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (AuthorsComboBox.SelectedItem != null)
            {
                SelectedAuthor = AuthorsComboBox.SelectedItem as Author;
            }
        }

    }
}
