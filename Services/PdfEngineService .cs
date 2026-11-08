using loadpdf3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using PDFiumCore;
using System.Runtime.CompilerServices;

namespace loadpdf3.Services
{
    public class PdfEngineService : IPdfEngineService
    {
        private readonly IPdfMessenger _messenger;
        public PdfEngineService(IPdfMessenger messenger)
        {
            _messenger = messenger;
        }

        public async Task<List<PdfPageData>> LoadAllPagesAsync(string filePath, IProgress<int> progress = null)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);

            return await Task.Run(() =>
            {

                FpdfDocumentT doc = fpdfview.FPDF_LoadDocument(filePath, null);
                if (doc == null)
                {
                    throw new Exception($"Failed to load PDF document: {filePath}");
                }

                var pages = new List<PdfPageData>();

                try
                {
                    int pageCount = fpdfview.FPDF_GetPageCount(doc);
                    _messenger.Publish(new PdfProgressMessage { IsLoading = true, CurrentPage = 0, TotalPages = pageCount });

                    for (int i = 0; i < pageCount; i++)
                    {
                        var page = fpdfview.FPDF_LoadPage(doc, i);
                        if (page != null)
                        {
                            try
                            {
                                int width = (int)fpdfview.FPDF_GetPageWidth(page);
                                int height = (int)fpdfview.FPDF_GetPageHeight(page);
                                pages.Add(new PdfPageData
                                {
                                    Width = width,
                                    Height = height
                                });
                            }
                            finally
                            {
                                fpdfview.FPDF_ClosePage(page);
                            }
                        }

                        _messenger.Publish(new PdfProgressMessage { IsLoading = true, CurrentPage = i + 1, TotalPages = pageCount });
                    }

                }
                finally
                {
                    fpdfview.FPDF_CloseDocument(doc);
                    _messenger.Publish(new PdfProgressMessage { IsLoading = false });
                }

                return pages;
            });
        }
    }
}
