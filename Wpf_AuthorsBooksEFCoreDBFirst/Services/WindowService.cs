using System.Windows;
using Wpf_AuthorsBooksEFCoreDBFirst.Views;

namespace Wpf_AuthorsBooksEFCoreDBFirst.Services
{
    public class WindowService : IWindowService
    {
        public bool ShowAuthorDialog(ref string authorName)
        {
            var window = new InputAuthorWindow(authorName);
            bool? result = window.ShowDialog();
            if (result == true)
            {
                authorName = window.AuthorName;
                return true;
            }
            return false;
        }

        public bool ShowBookDialog(ref string bookTitle)
        {
            var window = new InputBookWindow();
            window.BookTitleTextBox.Text = bookTitle;
            bool? result = window.ShowDialog();
            if (result == true)
            {
                bookTitle = window.BookTitle;
                return true;
            }
            return false;
        }

        public bool ConfirmDelete(string itemType, string itemName)
        {
            return MessageBox.Show($"Are you sure you want to delete this {itemType}: '{itemName}'?",
                "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }
    }
}
