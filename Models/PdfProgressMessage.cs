using System;
using System.Collections.Generic;
using System.Text;

namespace loadpdf3.Models
{
    public class PdfProgressMessage
    {
        public bool IsLoading { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
