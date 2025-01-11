using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Wpf_AuthorsBooksEFCoreDBFirst.Models;
using Wpf_AuthorsBooksEFCoreDBFirst.Services;

namespace Wpf_AuthorsBooksEFCoreDBFirst.ViewModels

{
    public class MainViewModel : BaseViewModel
    {
        private readonly BooksContext _context = new();
        private Author? _selectedAuthor;
        private Book? _selectedBook;
        private readonly IWindowService _windowService;
        private bool _isFilterEnabled;
        public ObservableCollection<Author> Authors { get; set; }

        public Author? SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged(nameof(SelectedAuthor));
                UpdateSelectedAuthorBooks();
               //System.Windows.MessageBox.Show($"Selected Author: {_selectedAuthor?.Name}");
            }
        }

        public Book? SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
            }
        }
        private ObservableCollection<Book> _selectedAuthorBooks = new();
        public ObservableCollection<Book> SelectedAuthorBooks
        {
            get => _selectedAuthorBooks;
            private set
            {
                _selectedAuthorBooks = value;
                OnPropertyChanged(nameof(SelectedAuthorBooks));
            }
        }
        public bool IsFilterEnabled
        {
            get => _isFilterEnabled;
            set
            {
                _isFilterEnabled = value;
                OnPropertyChanged(nameof(IsFilterEnabled));

                if (_isFilterEnabled)
                {
                    ApplyBookFilter();
                }
                else
                {
                    ResetAuthorFilter();
                }
            }
        }
        public ICommand AddAuthorCommand { get; }
        public ICommand RemoveAuthorCommand { get; }
        public ICommand EditAuthorCommand { get; }
        public ICommand AddBookCommand { get; }
        public ICommand RemoveBookCommand { get; }
        public ICommand EditBookCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ExitCommand { get; }

        public MainViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            Authors = new ObservableCollection<Author>(_context.Authors.Include(a => a.Books).ToList());

            AddAuthorCommand = new RelayCommand(AddAuthor);
            RemoveAuthorCommand = new RelayCommand(RemoveAuthor);
            EditAuthorCommand = new RelayCommand(EditAuthor);

            AddBookCommand = new RelayCommand(AddBook);
            RemoveBookCommand = new RelayCommand(RemoveBook);
            EditBookCommand = new RelayCommand(EditBook);

            SaveCommand = new RelayCommand(SaveChanges);
            ExitCommand = new RelayCommand(ExitApplication);

            if (Authors.Any())
                SelectedAuthor = Authors.First();
        }

        private void UpdateSelectedAuthorBooks()
        {
            if (SelectedAuthor != null)
            {
                //загружаем книги из бд
                var books = _context.Books
                                    .Where(b => b.AuthorId == SelectedAuthor.Id)
                                    .ToList(); 

                SelectedAuthorBooks.Clear(); // Clear перед добавлением новых
                foreach (var book in books)
                {
                    SelectedAuthorBooks.Add(book); // добавляем в ObservableCollection
                }

               // System.Windows.MessageBox.Show($"Books Count: {SelectedAuthorBooks.Count}");
            }
            else
            {
                SelectedAuthorBooks.Clear();
            }
        }

        private void UpdateAuthors()
        {
            // принудительно загружаем авторов из бд
            var authors = _context.Authors.Include(a => a.Books).ToList();

            Authors.Clear();

            foreach (var author in authors)
            {
                Authors.Add(author);
            }
        }


        private void AddAuthor()
        {
            string newAuthorName = "New Author";
            if (_windowService.ShowAuthorDialog(ref newAuthorName))
            {
                var newAuthor = new Author { Name = newAuthorName };
                _context.Authors.Add(newAuthor);
                Authors.Add(newAuthor);
                SaveChanges();
                SelectedAuthor = newAuthor;
            }
        }
        private void RemoveAuthor()
        {
            if (SelectedAuthor != null)
            {
                _context.Authors.Remove(SelectedAuthor);
                Authors.Remove(SelectedAuthor);
                SaveChanges();
            }
        }

        private void EditAuthor()
        {
            if (SelectedAuthor != null)
            {
                string editedName = SelectedAuthor.Name;
                int selectedAuthorId = SelectedAuthor.Id;  

                if (_windowService.ShowAuthorDialog(ref editedName))
                {
                    SelectedAuthor.Name = editedName;
                    SaveChanges();
                    UpdateAuthors();
                    SelectedAuthor = Authors.FirstOrDefault(a => a.Name == editedName);                   
                    SelectedAuthor = Authors.FirstOrDefault(a => a.Id == selectedAuthorId);
                }
            }
        }
  
        private void AddBook()
        {
            if (SelectedAuthor != null)
            {
                string bookTitle = "New Book";
                if (_windowService.ShowBookDialog(ref bookTitle))
                {
                    var newBook = new Book { Title = bookTitle, Author = SelectedAuthor };
                    _context.Books.Add(newBook);
                    SelectedAuthor.Books.Add(newBook);
                    SaveChanges();
                    UpdateSelectedAuthorBooks();
                }
            }
        }

        private void RemoveBook()
        {
            if (SelectedBook != null)
            {
                _context.Books.Remove(SelectedBook);
                SelectedAuthor?.Books.Remove(SelectedBook);
                OnPropertyChanged(nameof(SelectedAuthor.Books));
                SaveChanges();
                UpdateSelectedAuthorBooks();
            }
        }

        private void EditBook()
        {
            if (SelectedBook != null)
            {
                string editedTitle = SelectedBook.Title;
                if (_windowService.ShowBookDialog(ref editedTitle))
                {
                    SelectedBook.Title = editedTitle;
                    SaveChanges();
                    UpdateSelectedAuthorBooks();
                }
            }
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
            MessageBox.Show("Changes saved!");
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private void ApplyBookFilter()
        {
            if (SelectedBook != null)
            {
                var filteredAuthors = _context.Authors
                    .Include(a => a.Books)
                    .Where(a => a.Books.Any(b => b.Title == SelectedBook.Title))
                    .ToList();

                Authors.Clear();
                foreach (var author in filteredAuthors)
                {
                    Authors.Add(author);
                }
            }
            else
            {
                MessageBox.Show("Please select a book to filter authors!");
            }
        }

        private void ResetAuthorFilter()
        {
            var allAuthors = _context.Authors.Include(a => a.Books).ToList();
            Authors.Clear();
            foreach (var author in allAuthors)
            {
                Authors.Add(author);
            }
        }     

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

