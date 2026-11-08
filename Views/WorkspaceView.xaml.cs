using loadpdf3.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for PdfWorkspaceView.xaml
    /// </summary>
    public partial class WorkspaceView : UserControl
    {
        private WorkspaceViewModel ViewModel => DataContext as WorkspaceViewModel;

        public WorkspaceView()
        {
            InitializeComponent();
        }

        public WorkspaceView(WorkspaceViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
