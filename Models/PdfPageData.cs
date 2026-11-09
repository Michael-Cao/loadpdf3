using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace loadpdf3.Models
{
    public class PdfDocumentData
    {
        public Dictionary<int, FontMetadata> Fonts { get; set; } = new();
        public List<PdfPageData> Pages { get; set; } = new();
    }

    public class FontMetadata
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
    }

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

        public int FontId { get; set; }
        public double FontSize { get; set; }
    }

    public class PdfObjectData
    {
        public string Type { get; set; } // "Image", "Path", etc.
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class PdfFontRegistry
    {
        private readonly ConcurrentDictionary<string, FontMetadata> _fontsCache = new();
        private int _nextFontId = 0;

        public int GetOrRegisterFont(string name, int weight, bool isBold, bool isItalic)
        {
            string key = $"{name}_{weight}_{(isBold ? "B" : "N")}_{(isItalic ? "I" : "N")}";

            var metadata = _fontsCache.GetOrAdd(key, _ => new FontMetadata
            {
                Id = System.Threading.Interlocked.Increment(ref _nextFontId),
                Name = name,
                Weight = weight,
                IsBold = isBold,
                IsItalic = isItalic
            });

            return metadata.Id;
        }

        public Dictionary<int, FontMetadata> GetAllFonts()
        {
            var dict = new Dictionary<int, FontMetadata>();
            foreach (var kvp in _fontsCache.Values)
            {
                dict[kvp.Id] = kvp;
            }
            return dict;
        }

        public void Clear()
        {
            _fontsCache.Clear();
            _nextFontId = 0;
        }
    }
}
