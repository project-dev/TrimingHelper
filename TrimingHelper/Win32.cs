using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TrimmingHelper
{
    public class Win32
    {
        public delegate bool EnumWindowsProc( IntPtr hwnd, IntPtr lParam );


        [DllImport( "user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static extern bool EnumWindows(
            [MarshalAs( UnmanagedType.FunctionPtr )] EnumWindowsProc enumProc,
            IntPtr lParam );        
        
        [DllImport( "user32.dll" )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static extern bool EnumChildWindows( IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam );

        public enum TernaryRasterOperations : uint
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062,
            CAPTUREBLT = 0x40000000
        }

        [DllImport( "gdi32.dll" )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static extern bool BitBlt( IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight,
          IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop );

        [DllImport( "user32.dll" )]
        public static extern IntPtr GetWindowDC( IntPtr hWnd );

        [DllImport( "user32.dll" )]
        private static extern IntPtr GetDC( IntPtr hwnd );

        [DllImport( "gdi32.dll", ExactSpelling = true, SetLastError = true )]
        public static extern IntPtr CreateCompatibleDC( IntPtr hdc );

        [DllImport( "gdi32.dll", ExactSpelling = true, SetLastError = true )]
        public static extern bool DeleteDC( IntPtr hdc );

        [DllImport( "gdi32.dll", ExactSpelling = true, SetLastError = true )]
        public static extern IntPtr SelectObject( IntPtr hdc, IntPtr hgdiobj );

        [DllImport( "user32.dll" )]
        public static extern int GetWindowRect( IntPtr hwnd,
            ref  RECT lpRect );

        [DllImport( "user32.dll" )]
        public static extern int GetClientRect( IntPtr hwnd,
            ref  RECT lpRect );

        [DllImport( "user32.dll" )]
        public static extern int ScreenToClient( IntPtr hwnd,
            ref  POINT lpPoint );

        [DllImport( "user32.dll" )]
        public static extern int ClientToScreen( IntPtr hwnd,
            ref  POINT lpPoint );

        [DllImport( "user32.dll" )]
        public static extern bool IsZoomed( IntPtr hwnd );

        [DllImport( "user32.dll" )]
        public static extern bool IsIconic( IntPtr hwnd );

        [DllImport( "user32.dll", SetLastError = true )]
        public static extern uint GetWindowThreadProcessId( IntPtr hWnd, out uint lpdwProcessId );

        [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
        public static extern int GetWindowText( IntPtr hWnd,
            StringBuilder lpString, int nMaxCount );

        [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
        public static extern int GetClassName( IntPtr hWnd,
            StringBuilder lpString, int nMaxCount );

        [DllImport( "user32.dll", ExactSpelling = true, SetLastError = true )]
        public static extern bool IsWindowVisible( IntPtr hwnd );

        public enum WindowLongIndex : int
        {
            GWL_WNDPROC = -1,
            GWL_HINSTANCE = -6,
            GWL_HWNDPARENT = -8,
            GWL_STYLE = -16,
            GWL_EXSTYLE = -20,
            GWL_USERDATA = -21,
            GWL_ID = -12
        }

        public enum GWL_STYLE : long
        {
            WS_OVERLAPPED = 0x00000000,
            WS_TILED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_CHILDWINDOW = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_ICONIC = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_CAPTION = 0x00C00000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_SIZEBOX = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_OVERLAPPEDWINDOW = 0x00CF0000,
            WS_POPUPWINDOW = 0x80880000
        }


        [DllImport( "user32.dll", ExactSpelling = true, SetLastError = true )]
        public static extern long GetWindowLong( IntPtr hwnd, int index );

        [DllImport( "User32.dll" )]
        public extern static bool PrintWindow( IntPtr hwnd, IntPtr hDC, uint nFlags );
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct POINT
    {
        public int x;
        public int y;
    }
}
