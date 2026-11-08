using System;
using System.Collections.Generic;
using System.Text;

namespace loadpdf3.Models
{
    public class PdfPageData
    {
        private int _width;
        public int Width
        {
            get => _width;
            set => _width = value;
        }

        private int _height;
        public int Height
        {
            get => _height;
            set => _height = value;
        }
    }
}
