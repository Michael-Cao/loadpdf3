using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace loadpdf3.Native
{
    public class PdfiumNative
    {
        [DllImport("fpdfview.dll", EntryPoint = "FPDF_LoadDocument", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr FPDF_LoadDocument(string file_path, string password);

        [DllImport("fpdfview.dll", EntryPoint = "FPDF_GetPageCount", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FPDF_GetPageCount(IntPtr document);

        [DllImport("fpdfview.dll", EntryPoint = "FPDF_CloseDocument", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FPDF_CloseDocument(IntPtr document);

        [DllImport("fpdfview.dll", EntryPoint = "FPDF_LoadPage", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr FPDF_LoadPage(IntPtr document, int page_index);

        [DllImport("fpdfview.dll", EntryPoint = "FPDF_ClosePage", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FPDF_ClosePage(IntPtr page);
    }
}
