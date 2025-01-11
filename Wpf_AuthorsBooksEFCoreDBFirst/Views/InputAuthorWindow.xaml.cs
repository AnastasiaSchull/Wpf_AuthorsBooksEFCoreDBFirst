using System.Windows;

namespace Wpf_AuthorsBooksEFCoreDBFirst.Views
{

    public partial class InputAuthorWindow : Window
    {
        public string AuthorName
        {
            get => AuthorNameTextBox.Text;
            set => AuthorNameTextBox.Text = value;
        }
        public InputAuthorWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
        }
        public InputAuthorWindow(string existingAuthorName) : this()
        {
            AuthorName = existingAuthorName;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorName = AuthorNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(AuthorName))
            {
                MessageBox.Show("Please enter a valid author name!");
                return;
            }
            DialogResult = true;  // закрываем окно с положительным результатом
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;  // закрываем окно с отрицательным результатом
        }
    }
}
