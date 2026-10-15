using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Microsoft.VisualStudio.Shell;
using LumiSqlFormatter.UI.Views;
using LumiSqlFormatter.UI.ViewModels;

namespace LumiSqlFormatter.UI
{
    [Guid("A198B94C-3B47-49F1-8BDE-2D8E09B1F54B")]
    public class OptionsPage : UIElementDialogPage
    {
        private SettingsView _settingsView;

        #region Win32 Constants and P/Invoke
        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x00010000;
        private const int WS_THICKFRAME = 0x00040000;
        private const int SW_MAXIMIZE = 3;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint GA_ROOT = 2;
        private const uint GA_ROOTOWNER = 3;

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern IntPtr GetAncestor(IntPtr hwnd, uint flags);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        private static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return (int)GetWindowLongPtr64(hWnd, nIndex);
            return GetWindowLong32(hWnd, nIndex);
        }

        private static void SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 8)
                SetWindowLongPtr64(hWnd, nIndex, new IntPtr(dwNewLong));
            else
                SetWindowLong32(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr SetFocus(IntPtr hWnd);
        #endregion

        protected override UIElement Child
        {
            get
            {
                if (_settingsView == null)
                {
                    _settingsView = new SettingsView();
                }
                return _settingsView;
            }
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);
            TriggerFullScreen();
            if (_settingsView != null)
            {
#pragma warning disable VSTHRD001, VSTHRD110
                _ = _settingsView.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        var source = PresentationSource.FromVisual(_settingsView) as HwndSource;
                        if (source != null && source.Handle != IntPtr.Zero)
                        {
                            SetFocus(source.Handle);
                        }
                    }
                    catch
                    {
                    }
                }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
#pragma warning restore VSTHRD001, VSTHRD110
            }
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);
            try
            {
                if (_settingsView?.DataContext is SettingsViewModel vm)
                {
                    vm.SaveSettings();
                }
            }
            catch
            {
            }
        }

        private void TriggerFullScreen()
        {
            try
            {
                IntPtr handle = IntPtr.Zero;
                if (this.Window != null)
                {
                    handle = this.Window.Handle;
                }
                if (handle == IntPtr.Zero && _settingsView != null)
                {
                    var source = PresentationSource.FromVisual(_settingsView) as HwndSource;
                    if (source != null)
                        handle = source.Handle;
                }

                if (handle != IntPtr.Zero)
                {
                    MaximizeParentDialog(handle);
                }
                else if (_settingsView != null)
                {
                    _settingsView.Loaded += (s, e) =>
                    {
                        var src = PresentationSource.FromVisual(_settingsView) as HwndSource;
                        if (src != null && src.Handle != IntPtr.Zero)
                        {
                            MaximizeParentDialog(src.Handle);
                        }
                    };
                }
            }
            catch
            {
            }
        }

        private void MaximizeParentDialog(IntPtr childHwnd)
        {
            try
            {
                if (childHwnd == IntPtr.Zero)
                    return;

                IntPtr rootHwnd = GetAncestor(childHwnd, GA_ROOT);
                if (rootHwnd == IntPtr.Zero)
                    rootHwnd = GetAncestor(childHwnd, GA_ROOTOWNER);

                if (rootHwnd != IntPtr.Zero)
                {
                    // Add Maximize Box and resizable border styles if not present
                    int style = GetWindowLong(rootHwnd, GWL_STYLE);
                    SetWindowLong(rootHwnd, GWL_STYLE, style | WS_MAXIMIZEBOX | WS_THICKFRAME);

                    // Get screen working area (excluding taskbar)
                    Screen screen = Screen.FromHandle(rootHwnd);
                    Rectangle workArea = screen.WorkingArea;

                    // Expand to full screen working area
                    SetWindowPos(rootHwnd, IntPtr.Zero, workArea.X, workArea.Y, workArea.Width, workArea.Height,
                        SWP_NOZORDER | SWP_FRAMECHANGED | SWP_SHOWWINDOW);

                    // Send Maximize window command
                    ShowWindow(rootHwnd, SW_MAXIMIZE);
                }
            }
            catch
            {
                // Fail-safe: do not disturb SSMS if maximization fails
            }
        }
    }
}
