using System.Windows;
using Wpf_AuthorsBooksEFCoreDBFirst.ViewModels;
using Wpf_AuthorsBooksEFCoreDBFirst.Services;

namespace Wpf_AuthorsBooksEFCoreDBFirst.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow() : this(new MainViewModel(new WindowService())) { }

        public MainWindow(BaseViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
           
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
        }
    }
}
