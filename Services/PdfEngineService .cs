using loadpdf3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace loadpdf3.Services
{
    public class PdfEngineService : IPdfEngineService
    {
        public async Task<List<PdfPageData>> LoadAllPagesAsync(string filePath, IProgress<int> progress = null)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);

            return await Task.Run(() =>
            {
                var pages = new List<PdfPageData>();



                return pages;
            });
        }
    }
}
