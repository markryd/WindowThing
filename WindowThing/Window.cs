using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WindowThing
{
    class Window
    {
        private IntPtr _hwnd;
        private String _screenname;
        private Boolean _isLeft;

        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 0;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", EntryPoint = "IsWindow")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "IsIconic")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public Window(IntPtr h, Boolean left)
        {
            _hwnd = h;
            _screenname = System.Windows.Forms.Screen.FromHandle(h).DeviceName.Substring(0, 12);
            _isLeft = left;
        }
        
        public IntPtr hwnd
        {
            get { return _hwnd; }
        }

        public Boolean isLeft
        {
            get { return _isLeft; }
            set { _isLeft = value; }
        }

        public bool IsWindow()
        {
            return IsWindow(_hwnd);
        }

        public bool IsIconic()
        {
            return IsIconic(_hwnd);
        }

        public bool IsWindowVisible()
        {
            return IsWindowVisible(_hwnd);
        }

        public bool IsOnSameScreen(IntPtr h)
        {
            return (_screenname == System.Windows.Forms.Screen.FromHandle(h).DeviceName.Substring(0, 12));
        }

        public bool IsOnActiveScreen()
        {
            return _screenname == System.Windows.Forms.Screen.FromHandle(MainForm.GetForegroundWindow()).DeviceName.Substring(0, 12);
        }

        public int GetWindowTextLength()
        {
            return GetWindowTextLength(_hwnd);
        }

        public string GetWindowText()
        {
            StringBuilder sb = new StringBuilder(GetWindowTextLength() + 1);
            GetWindowText(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }

        public void SetWindowPos(int left, int top, int width, int height)
        {
            SetWindowPos(_hwnd, 0, left, top, width, height, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);    
        }
        
    }
}
