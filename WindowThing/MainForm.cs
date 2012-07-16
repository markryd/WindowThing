using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using FireAnt.Windows.Forms.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WindowThing
{
    public partial class MainForm : Form
    {
        private double xratio;
        //private List<IntPtr> leftWindows = new List<IntPtr>();
        //private List<IntPtr> rightWindows = new List<IntPtr>();
        private List<Window> windows = new List<Window>();
        private List<string> ignore = new List<string>();
        private NotifyIcon  trayIcon;
        private ContextMenu trayMenu;
        private IntPtr foreground;

        public delegate bool CallBackPtr(int hwnd, int lParam);
        private CallBackPtr callBackPtr;

        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int EnumWindows(CallBackPtr callPtr, int lPar);

        [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
        private static extern bool IsWindowVisible(IntPtr hWnd);


        public MainForm()
        {
            InitializeComponent();
            this.hotKeyLeft.RegisterHotkey(Modifiers.Win | Modifiers.Control, Keys.Left);
            this.hotKeyRight.RegisterHotkey(Modifiers.Win | Modifiers.Control, Keys.Right);
            this.hotKeyMakeMain.RegisterHotkey(Modifiers.Win | Modifiers.Control, Keys.Up);
            this.hotKeyUnGet.RegisterHotkey(Modifiers.Win | Modifiers.Control, Keys.Down);
            
            xratio = (double)this.ratio.Value / 100;
	            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Show", ShowWindow);
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon      = new NotifyIcon();
            trayIcon.Text = "WindowThing";
            trayIcon.Icon = new Icon("WindowThing.ico", 40, 40);

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible     = true;

            ignore.Add("WindowThing");
            ignore.Add("VirtuaWinMainClass");
            ignore.Add("Program Manager");
            ignore.Add("Tiny Time Tracker");
        }
        
        private void hotKeyLeft_HotKeyPressed(object sender, EventArgs e)
        {
            // get the hotkey reference
            HotKey hotKey = sender as HotKey;
            if (hotKey != null)
            {
                foreground = GetForegroundWindow();
                if (windows.Exists(w => w.hwnd == foreground))
                    windows.Single(w => w.hwnd == foreground).isLeft = true;
                else
                    windows.Add(new Window(foreground, true));
                MoveAllControlledWindows();
            }
        }

        private void hotKeyRight_HotKeyPressed(object sender, EventArgs e)
        {
            // get the hotkey reference
            HotKey hotKey = sender as HotKey;
            if (hotKey != null)
            {
                foreground = GetForegroundWindow();
                if (windows.Exists(w => w.hwnd == foreground))
                    windows.Single(w => w.hwnd == foreground).isLeft = false;
                else
                    windows.Add(new Window(foreground, false));
                MoveAllControlledWindows();
            }
        }

        private void hotKeyMakeMain_HotKeyPressed(object sender, EventArgs e)
        {
            HotKey hotKey = sender as HotKey;
            if (hotKey != null)
            {
                foreground = GetForegroundWindow();
                if (!windows.Exists(w => w.hwnd == foreground)) windows.Add(new Window(foreground, true));
                MakeActiveOnlyLeftWindow();
                MoveAllControlledWindows();
            }
        }

        private void hotKeyUnGet_HotKeyPressed(object sender, EventArgs e)
        {
            HotKey hotKey = sender as HotKey;
            if (hotKey != null)
            {
                foreground = GetForegroundWindow();
                RemoveActiveFromControl();
                //callBackPtr = new CallBackPtr(AddAllToRight);
                //EnumWindows(callBackPtr, 0);
                //MakeActiveOnlyLeftWindow();
                MoveAllControlledWindows();
            }
        }

        private void MakeActiveOnlyLeftWindow()
        {
            foreach (var w in windows.Where(w => w.IsOnSameScreen(foreground)))
            {
                if (w.hwnd == foreground)
                    w.isLeft = true;
                else
                    w.isLeft = false;
            }
        }

        public bool AddAllToRight(int hwnd, int lParam)
        {
            Window w = new Window((IntPtr)hwnd, false);
            //if (!w.IsWindowVisible()) return true; //doesn't work, no idea why
            if (!IsWindowVisible(w.hwnd)) return true;
            if (w.GetWindowTextLength() == 0) return true;
            if (ignore.Contains(w.GetWindowText())) return true;
            if (!w.IsOnSameScreen(foreground)) return true;
            if (!windows.Exists(wi => wi.hwnd == w.hwnd)) windows.Add(w);
            return true;
        }

        private void MoveAllControlledWindows()
        {
            Rectangle wa = System.Windows.Forms.Screen.FromHandle(GetForegroundWindow()).WorkingArea;
            CleanWindowList(windows);
            int split = (int)(wa.Width * xratio);
            MoveControlledWindows(windows.Where(w => w.isLeft && w.IsOnSameScreen(foreground)), wa.Left, split, wa.Height);
            MoveControlledWindows(windows.Where(w => !w.isLeft && w.IsOnSameScreen(foreground)), wa.Left + split, wa.Width - split, wa.Height);
        }
        
        private void MoveControlledWindows(IEnumerable<Window> l, int left, int width, int height)
        {
            var windowList = l.Where(w => !w.IsIconic()).ToList();
            int h = (windowList.Count == 0) ? height : (int)(height / windowList.Count);
            int top = 0;
            foreach (var window in windowList)
            {
                window.SetWindowPos(left, top, width, h);
                top += h;
            }
        }

        private void CleanWindowList(List<Window> l)
        {
            var dirtyWindows = l.Where(w => !w.IsWindow()).ToList();
            foreach (var w in dirtyWindows) l.Remove(w);
        }

        private void RemoveActiveFromControl()
        {
            windows.RemoveAll(w => w.hwnd == foreground);
        }

        private void ratio_ValueChanged(object sender, EventArgs e)
        {
            xratio = (double)this.ratio.Value / 100;
            MoveAllControlledWindows();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ShowWindow(object sender, EventArgs e)
        {
            this.Show();
            this.BringToFront();
            Rectangle screen = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(screen.Width - this.Width, screen.Height - this.Height);            
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void MainForm_Closing(object sender,  System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public static bool AreOnSameScreen(IntPtr a, IntPtr b)
        {
            if (System.Windows.Forms.Screen.FromHandle(a).DeviceName.Substring(0, 20)
                == System.Windows.Forms.Screen.FromHandle(b).DeviceName.Substring(0, 20)) return true;
            return false;
        }

    }
}


