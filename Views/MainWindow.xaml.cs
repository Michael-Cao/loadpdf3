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
            try
            {
                if (this.DataContext == null)
                {
                    MessageBox.Show("错误：MainWindow 的 DataContext 为空（Null），请检查绑定链路。", "架构错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (this.DataContext is MainViewModel mainVm)
                {
                    mainVm.HandleOpenFileExecution();
                }
                else
                {
                    MessageBox.Show($"错误：DataContext 的类型不是 MainViewModel，而是 {this.DataContext.GetType().Name}", "架构错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"在 UI 层点击触发时发生致命错误:\n{ex.Message}\n\n{ex.StackTrace}", "点击异常", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void ExitApp_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}