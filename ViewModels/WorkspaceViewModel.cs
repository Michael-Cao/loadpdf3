using loadpdf3.Models;
using loadpdf3.Services;
using PDFiumCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace loadpdf3.ViewModels
{
    public class WorkspaceViewModel : ViewModelBase
    {
        private readonly IPdfEngineService _pdfEngineService;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private int _currentProgress;
        public int CurrentProgress
        {
            get => _currentProgress;
            set { _currentProgress = value; OnPropertyChanged();  }
        }

        public Dictionary<int, FontMetadata> DocumentFonts { get; private set; }

        private ObservableCollection<PdfPageData> _pages = new();
        public ObservableCollection<PdfPageData> Pages
        {
            get => _pages;
            set { _pages = value; OnPropertyChanged(); }
        }

        public WorkspaceViewModel(IPdfEngineService pdfEngineService)
        {
            this._pdfEngineService = pdfEngineService;
            // Initialize any necessary properties or commands here
        }


        public async Task OpenAndLoadPdfAsync(string filePath)
        {
            try
            {
                IsLoading = true;

                var progressHandler = new Progress<int>(currentPage =>
                {
                    CurrentProgress = currentPage;
                });

                var doc = await _pdfEngineService.LoadAllPagesAsync(filePath, progressHandler);

                Pages = new ObservableCollection<PdfPageData>(doc.Pages);
                DocumentFonts = doc.Fonts;
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading PDF: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
