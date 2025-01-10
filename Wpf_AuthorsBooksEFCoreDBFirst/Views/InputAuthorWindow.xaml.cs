using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wpf_AuthorsBooksEFCoreDBFirst.Views
{
    /// <summary>
    /// Interaction logic for InputAuthorWindow.xaml
    /// </summary>
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
