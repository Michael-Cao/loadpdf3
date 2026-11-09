using loadpdf3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using loadpdf3.Native;

namespace loadpdf3.Services
{
    public class PdfEngineService : IPdfEngineService
    {
        private readonly IPdfMessenger _messenger;
        public PdfEngineService(IPdfMessenger messenger)
        {
            _messenger = messenger;
        }

        public async Task<PdfDocumentData> LoadAllPagesAsync(string filePath, IProgress<int> progress = null)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);

            return await Task.Run(() =>
            {

                IntPtr doc = PdfiumNative.FPDF_LoadDocument(filePath, null);
                if (doc == null)
                {
                    throw new Exception($"Failed to load PDF document: {filePath}");
                }

                var fontRegistry = new PdfFontRegistry();
                var docData = new PdfDocumentData();

                try
                {
                    int pageCount = PdfiumNative.FPDF_GetPageCount(doc);
                    _messenger.Publish(new PdfProgressMessage { IsLoading = true, CurrentPage = 0, TotalPages = pageCount });

                    for (int i = 0; i < pageCount; i++)
                    {
                        var page = PdfiumNative.FPDF_LoadPage(doc, i);
                        if (page != null)
                        {
                            try
                            {
                                int width = (int)PdfiumNative.FPDF_GetPageWidth(page);
                                int height = (int)PdfiumNative.FPDF_GetPageHeight(page);

                                var textSegments = ExtractTextSegments(page, fontRegistry);
                                var visualObjects = ExtractVisualObjects(page);

                                docData.Pages.Add(new PdfPageData
                                {
                                    Width = width,
                                    Height = height,
                                    Texts = textSegments,
                                    VisualObjects = visualObjects
                                });
                            }
                            finally
                            {
                                PdfiumNative.FPDF_ClosePage(page);
                            }
                        }

                        _messenger.Publish(new PdfProgressMessage { IsLoading = true, CurrentPage = i + 1, TotalPages = pageCount });
                    }

                    docData.Fonts = fontRegistry.GetAllFonts();
                }
                finally
                {
                    PdfiumNative.FPDF_CloseDocument(doc);
                    _messenger.Publish(new PdfProgressMessage { IsLoading = false });
                }

                return docData;
            });
        }


        private List<PdfTextData> ExtractTextSegments(IntPtr page, PdfFontRegistry fontRegistry)
        {
            var list = new List<PdfTextData>();

            IntPtr textPage = PdfiumNative.FPDFText_LoadPage(page);
            if (textPage == null) return list;

            try
            {
                int charCount = PdfiumNative.FPDFText_CountChars(textPage);
                if (charCount <= 0) return list;

                int rectCount = PdfiumNative.FPDFText_CountRects(textPage, 0, charCount);

                ushort[] textBuffer = new ushort[1024];
                byte[] fontBuffer = new byte[256];

                for (int i = 0; i < rectCount; i++)
                {
                    double left = 0, top = 0, right = 0, bottom = 0;
                    if (PdfiumNative.FPDFText_GetRect(textPage, i, ref left, ref top, ref right, ref bottom))
                    {
                        int len = PdfiumNative.FPDFText_GetBoundedText(
                            textPage, left, top, right, bottom, textBuffer, textBuffer.Length);

                        string text = string.Empty;
                        if (len > 0)
                        {
                            byte[] bytes = new byte[len * 2];
                            Buffer.BlockCopy(textBuffer, 0, bytes, 0, bytes.Length);
                            text = System.Text.Encoding.Unicode.GetString(bytes).TrimEnd('\0');
                        }

                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            int firstCharIndex = PdfiumNative.FPDFText_GetCharIndexAtPos(textPage, left + 1.0, top - 1.0, 5.0, 5.0);

                            double fontSize = PdfiumNative.FPDFText_GetFontSize(textPage, firstCharIndex);

                            int fontFlags = 0;
                            int fontNameLen = PdfiumNative.FPDFText_GetFontInfo(textPage, firstCharIndex, fontBuffer, fontBuffer.Length, ref fontFlags);

                            string fontName = "Unknown";
                            if (fontNameLen > 0)
                            {
                                // 确保截取长度不超过 Buffer
                                int charsToRead = Math.Min(fontNameLen, fontBuffer.Length);
                                fontName = System.Text.Encoding.ASCII.GetString(fontBuffer, 0, charsToRead).TrimEnd('\0');
                            }

                            int fontWeight = PdfiumNative.FPDFText_GetFontWeight(textPage, firstCharIndex);

                            bool isBold = fontWeight > 500 || (fontFlags & 0x40000) != 0;
                            bool isItalic = (fontFlags & 64) != 0;
                            
                            int fontId = fontRegistry.GetOrRegisterFont(fontName, fontWeight, isBold, isItalic);


                            list.Add(new PdfTextData
                            {
                                Text = text.Trim(),
                                X = left,
                                Y = bottom,
                                Width = right - left,
                                Height = top - bottom,

                                FontId = fontId,
                                FontSize = fontSize
                            });
                        }
                    }
                }
            }
            finally
            {
                PdfiumNative.FPDFText_ClosePage(textPage);
            }

            return list;
        }


        private List<PdfObjectData> ExtractVisualObjects(IntPtr page)
        {
            var objects = new List<PdfObjectData>();

            int objCount = PdfiumNative.FPDFPage_CountObjects(page);

            for (int i = 0; i < objCount; i++)
            {
                IntPtr pageObj = PdfiumNative.FPDFPage_GetObject(page, i);
                if (pageObj == null) continue;

                int objType = PdfiumNative.FPDFPageObj_GetType(pageObj);

                float left = 0, bottom = 0, right = 0, top = 0;
                bool hasBounds = PdfiumNative.FPDFPageObj_GetBounds(pageObj, ref left, ref bottom, ref right, ref top);

                if (hasBounds)
                {
                    var objData = new PdfObjectData
                    {
                        Type = GetObjectTypeString(objType),
                        X = left,
                        Y = bottom,
                        Width = right - left,
                        Height = top - bottom
                    };

                    objects.Add(objData);

                }
            }

            return objects;
        }

        private string GetObjectTypeString(int type)
        {
            return type switch
            {
                1 => "TextObject",
                2 => "Path",
                3 => "Image",
                4 => "Shading",
                5 => "Form",
                _ => "Unknown"
            };
        }
    }
}