using System;
using System.Collections.Generic;
using System.Text;

namespace loadpdf3.Models
{
    public class PdfPageData
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public List<PdfTextData> Texts { get; set; } = new();
        public List<PdfObjectData> VisualObjects { get; set; } = new();
    }

    public class PdfTextData
    {
        public string Text { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class PdfObjectData
    {
        public string Type { get; set; } // "Image", "Path", etc.
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
