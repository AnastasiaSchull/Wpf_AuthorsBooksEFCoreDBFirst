using System.Windows;

namespace Wpf_AuthorsBooksEFCoreDBFirst.Views
{ 
    public partial class InputBookWindow : Window
    {
        public string BookTitle { get; private set; }

        public InputBookWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            BookTitle = BookTitleTextBox.Text;
            DialogResult = true;  
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; 
            Close();
        }
    }
}
