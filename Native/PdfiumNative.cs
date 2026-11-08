using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace loadpdf3.Native
{
    public class PdfiumNative
    {
        private const string DllName = "pdfium";

        [DllImport(DllName, EntryPoint = "FPDF_LoadDocument", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr FPDF_LoadDocument(string file_path, string password);

        [DllImport(DllName, EntryPoint = "FPDF_GetPageCount", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FPDF_GetPageCount(IntPtr document);

        [DllImport(DllName, EntryPoint = "FPDF_CloseDocument", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FPDF_CloseDocument(IntPtr document);

        [DllImport(DllName, EntryPoint = "FPDF_LoadPage", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr FPDF_LoadPage(IntPtr document, int page_index);

        [DllImport(DllName, EntryPoint = "FPDF_ClosePage", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FPDF_ClosePage(IntPtr page);

        [DllImport(DllName, EntryPoint = "FPDF_GetPageWidth", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FPDF_GetPageWidth(IntPtr page);

        [DllImport(DllName, EntryPoint = "FPDF_GetPageHeight", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FPDF_GetPageHeight(IntPtr page);

        [DllImport(DllName, EntryPoint = "FPDFText_LoadPage", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr FPDFText_LoadPage(IntPtr page);

        [DllImport(DllName, EntryPoint = "FPDFText_ClosePage", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FPDFText_ClosePage(IntPtr text_page);

        [DllImport(DllName, EntryPoint = "FPDFText_CountChars", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FPDFText_CountChars(IntPtr text_page);

        [DllImport(DllName, EntryPoint = "FPDFText_CountRects", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FPDFText_CountRects(IntPtr text_page, int start_index, int count);

        [DllImport(DllName, EntryPoint = "FPDFText_GetRect", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool FPDFText_GetRect(IntPtr text_page, int rect_index, ref double left, ref double top, ref double right, ref double bottom);

        [DllImport(DllName, EntryPoint = "FPDFText_GetBoundedText", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FPDFText_GetBoundedText(IntPtr text_page, double left, double top, double right, double bottom, ushort[] buffer, int buflen);

        [DllImport(DllName, EntryPoint = "FPDFPageObj_GetBounds", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool FPDFPageObj_GetBounds(IntPtr page_object, ref float left, ref float bottom, ref float right, ref float top);

        [DllImport(DllName, EntryPoint = "FPDFPageObj_GetType", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FPDFPageObj_GetType(IntPtr page_object);

        [DllImport("pdfium", EntryPoint = "FPDFPage_GetObject", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr FPDFPage_GetObject(IntPtr page, int index);

        [DllImport(DllName, EntryPoint = "FPDFPage_CountObjects", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FPDFPage_CountObjects(IntPtr page);
    }
}
