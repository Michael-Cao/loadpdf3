using loadpdf3.Services;
using loadpdf3.ViewModels;
using loadpdf3.Views;
using Microsoft.Extensions.DependencyInjection;
using PDFiumCore;
using System.Configuration;
using System.Data;
using System.Windows;

namespace loadpdf3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            fpdfview.FPDF_InitLibrary();

            // 1. Initialize our service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // 2. Build the IoC container
            ServiceProvider = serviceCollection.BuildServiceProvider();


            // 3. Resolve the Main View, set its DataContext, and display it
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            fpdfview.FPDF_DestroyLibrary();
            base.OnExit(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPdfMessenger, PdfMessenger>();
            services.AddSingleton<IPdfEngineService>(provider => 
                new PdfEngineService(
                    provider.GetRequiredService<IPdfMessenger>()
                )
            );

            services.AddSingleton<WorkspaceViewModel>();
            services.AddSingleton<StatusBarViewModel>(provider => 
                new StatusBarViewModel(
                    provider.GetRequiredService<IPdfMessenger>()
                )
            );
            services.AddSingleton<MainViewModel>(
                provider => new MainViewModel(
                    provider.GetRequiredService<WorkspaceViewModel>(),
                    provider.GetRequiredService<StatusBarViewModel>()
                )
            );

            services.AddSingleton<MainWindow>(provider =>
                new MainWindow(
                    provider.GetRequiredService<MainViewModel>()
                )
            );
        }
    }
}