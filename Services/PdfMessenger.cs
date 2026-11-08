using loadpdf3.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace loadpdf3.Services
{
    public interface IPdfMessenger
    {
        event Action<PdfProgressMessage> ProgressChanged;
        void Publish(PdfProgressMessage message);
    }

    public class PdfMessenger : IPdfMessenger
    {
        public event Action<PdfProgressMessage> ProgressChanged;

        public void Publish(PdfProgressMessage message)
        {
            ProgressChanged?.Invoke(message);
        }
    }
}
