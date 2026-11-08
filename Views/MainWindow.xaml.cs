using loadpdf3.ViewModels;
using PDFiumCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace loadpdf3.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainViewModel ViewModel) : this()
        {
            this.DataContext = ViewModel;
        }

        private void OpenPdf_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MainViewModel mainVm)
            {
                mainVm.HandleOpenFileExecution();
            }
        }

        private void ExitApp_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}