using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using FireAnt.Windows.Forms.Util;
using System.Collections.Generic;
using System.Linq;

namespace WindowThing
{
    public partial class MainForm : Form
    {
        private double _xratio;
        private readonly List<Window> _windows = new List<Window>();
        private readonly List<string> _ignore = new List<string>();
        private readonly NotifyIcon  _trayIcon;
        private readonly ContextMenu _trayMenu;
        private IntPtr foreground;

        public delegate bool CallBackPtr(int hwnd, int lParam);

        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
        private static extern bool IsWindowVisible(IntPtr hWnd);


        public MainForm()
        {
            InitializeComponent();
            try
            {
                hotKeyLeft.RegisterHotkey(Modifiers.Win | Modifiers.Control, Keys.J); //grab current window and move to left column
                hotKeyRight.RegisterHotkey(Modifiers.Win | Modifiers.Control, Keys.L); //grab current window and move to right column
                hotKeyMakeMain.RegisterHotkey(Modifiers.Win | Modifiers.Control, Keys.I); //grab current window and move to left column and move all others to right
                hotKeyUnGet.RegisterHotkey(Modifiers.Win | Modifiers.Control, Keys.K); //stop controlling current window
            }
            catch (Win32Exception e)
            {
                if (e.Message == "Hot key is already registered")
                {
                    MessageBox.Show("Hot key is already registered, exiting.");
                    Application.Exit();
                }
                else
                {
                    throw;
                }
            }
            
            _xratio = (double)ratio.Value / 100;
                // Create a simple tray menu with only one item.
            _trayMenu = new ContextMenu();
            _trayMenu.MenuItems.Add("Show", ShowWindow);
            _trayMenu.MenuItems.Add("Exit", OnExit);

            _trayIcon = new NotifyIcon { Text = "WindowThing", Icon = new Icon("WindowThing.ico", 40, 40), ContextMenu = _trayMenu, Visible = true };

            _ignore.Add("WindowThing");
            _ignore.Add("VirtuaWinMainClass");
            _ignore.Add("Program Manager");
            _ignore.Add("Tiny Time Tracker");
        }
        
        private void HotKeyLeftHotKeyPressed(object sender, EventArgs e)
        {
            var hotKey = sender as HotKey;
            LeftOrRightHotKeyPressed(hotKey, true);
        }

        private void LeftOrRightHotKeyPressed(HotKey hotKey, bool isLeft)
        {
            if (hotKey != null)
            {
                foreground = GetForegroundWindow();
                if (_windows.Exists(w => w.hwnd == foreground))
                    _windows.Single(w => w.hwnd == foreground).isLeft = isLeft;
                else
                    _windows.Add(new Window(foreground, isLeft));
                MoveAllControlledWindows();
            }
        }

        private void HotKeyRightHotKeyPressed(object sender, EventArgs e)
        {
            var hotKey = sender as HotKey;
            LeftOrRightHotKeyPressed(hotKey, false);
        }

        private void HotKeyMakeMainHotKeyPressed(object sender, EventArgs e)
        {
            var hotKey = sender as HotKey;
            if (hotKey != null)
            {
                foreground = GetForegroundWindow();
                if (!_windows.Exists(w => w.hwnd == foreground)) _windows.Add(new Window(foreground, true));
                MakeActiveOnlyLeftWindow();
                MoveAllControlledWindows();
            }
        }

        private void HotKeyUnGetHotKeyPressed(object sender, EventArgs e)
        {
            var hotKey = sender as HotKey;
            if (hotKey != null)
            {
                foreground = GetForegroundWindow();
                RemoveActiveFromControl();
                MoveAllControlledWindows();
            }
        }

        private void MakeActiveOnlyLeftWindow()
        {
            foreach (var w in _windows.Where(w => w.IsOnSameScreen(foreground)))
            {
                w.isLeft = w.hwnd == foreground;
            }
        }

        public bool AddAllToRight(int hwnd, int lParam)
        {
            var w = new Window((IntPtr)hwnd, false);
            //if (!w.IsWindowVisible()) return true; //doesn't work, no idea why
            if (!IsWindowVisible(w.hwnd)) return true;
            if (w.GetWindowTextLength() == 0) return true;
            if (_ignore.Contains(w.GetWindowText())) return true;
            if (!w.IsOnSameScreen(foreground)) return true;
            if (!_windows.Exists(wi => wi.hwnd == w.hwnd)) _windows.Add(w);
            return true;
        }

        private void MoveAllControlledWindows()
        {
            var wa = Screen.FromHandle(GetForegroundWindow()).WorkingArea;
            CleanWindowList(_windows);
            var split = (int)(wa.Width * _xratio);
            MoveControlledWindows(_windows.Where(w => w.isLeft && w.IsOnSameScreen(foreground)), wa.Left, split, wa.Height);
            MoveControlledWindows(_windows.Where(w => !w.isLeft && w.IsOnSameScreen(foreground)), wa.Left + split, wa.Width - split, wa.Height);
        }
        
        private static void MoveControlledWindows(IEnumerable<Window> l, int left, int width, int height)
        {
            var windowList = l.Where(w => !w.IsIconic()).ToList();
            var h = (windowList.Count == 0) ? height : height / windowList.Count;
            var top = 0;
            foreach (var window in windowList)
            {
                window.SetWindowPos(left, top, width, h);
                top += h;
            }
        }

        private static void CleanWindowList(ICollection<Window> l)
        {
            var dirtyWindows = l.Where(w => !w.IsWindow()).ToList();
            foreach (var w in dirtyWindows) l.Remove(w);
        }

        private void RemoveActiveFromControl()
        {
            _windows.RemoveAll(w => w.hwnd == foreground);
        }

        private void RatioValueChanged(object sender, EventArgs e)
        {
            _xratio = (double)ratio.Value / 100;
            MoveAllControlledWindows();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
            base.OnLoad(e);
        }

        private static void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ShowWindow(object sender, EventArgs e)
        {
            Show();
            BringToFront();
            var screen = Screen.PrimaryScreen.WorkingArea;
            Location = new Point(screen.Width - Width, screen.Height - Height);            
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void MainFormClosing(object sender,  System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        public static bool AreOnSameScreen(IntPtr a, IntPtr b)
        {
            return Screen.FromHandle(a).DeviceName.Substring(0, 20)
                   == Screen.FromHandle(b).DeviceName.Substring(0, 20);
        }
    }
}


