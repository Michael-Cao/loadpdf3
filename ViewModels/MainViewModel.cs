using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace loadpdf3.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public WorkspaceViewModel Workspace { get; }

        public MainViewModel(WorkspaceViewModel workspaceViewModel)
        {
            Workspace = workspaceViewModel ?? throw new ArgumentNullException(nameof(workspaceViewModel));
        }


        public void HandleOpenFileExecution()
        {
            Application.Current.Dispatcher.Invoke(async() =>
            {
                try
                {
                    var openFileDialog = new OpenFileDialog
                    {
                        Filter = "PDF Documents (*.pdf)|*.pdf",
                        Title = "Select a PDF File to Edit"
                    };

                    Window ownerWindow = Application.Current.MainWindow;

                    if (openFileDialog.ShowDialog(ownerWindow) == true)
                    {
                        // Route the file string straight down to the workspace component safely
                        await Workspace.OpenAndLoadPdfAsync(openFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"文件读取中转出现未知异常:\n{ex.Message}\n\n堆栈跟踪:\n{ex.StackTrace}",
                        "运行时业务错误",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });
        }
    }
}
