using loadpdf3.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace loadpdf3.Services
{
    public interface IPdfEngineService
    {
        Task<PdfDocumentData> LoadAllPagesAsync(string filePath, IProgress<int> progress = null);
    }
}
