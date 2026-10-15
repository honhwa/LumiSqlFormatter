using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using LumiSqlFormatter.UI.ViewModels;

namespace LumiSqlFormatter.UI.Views
{
    public partial class SettingsView : UserControl
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetFocus();

        private HwndSource _hwndSource;

        public SettingsView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            DataContextChanged += OnDataContextChanged;
            SubscribeViewModel(DataContext as SettingsViewModel);
        }

        private HwndSource GetHwndSource()
        {
            if (_hwndSource != null && _hwndSource.Handle != IntPtr.Zero)
            {
                return _hwndSource;
            }

            _hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (_hwndSource != null)
            {
                _hwndSource.RemoveHook(HwndSourceHook);
                _hwndSource.AddHook(HwndSourceHook);
            }
            return _hwndSource;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            GetHwndSource();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is SettingsViewModel oldVm)
            {
                oldVm.SnippetAdded -= OnSnippetAdded;
            }
            if (e.NewValue is SettingsViewModel newVm)
            {
                SubscribeViewModel(newVm);
            }
        }

        private void SubscribeViewModel(SettingsViewModel vm)
        {
            if (vm != null)
            {
                vm.SnippetAdded -= OnSnippetAdded;
                vm.SnippetAdded += OnSnippetAdded;
            }
        }

        private void EnsureWin32Focus()
        {
            try
            {
                var src = GetHwndSource();
                if (src != null && src.Handle != IntPtr.Zero)
                {
                    if (GetFocus() != src.Handle)
                    {
                        SetFocus(src.Handle);
                    }
                }
            }
            catch
            {
            }
        }

        private void OnSnippetAdded()
        {
#pragma warning disable VSTHRD001, VSTHRD110
            Action focusAction = () =>
            {
                try
                {
                    if (SnippetsGrid != null && SnippetsGrid.SelectedItem != null)
                    {
                        SnippetsGrid.ScrollIntoView(SnippetsGrid.SelectedItem);
                    }

                    EnsureWin32Focus();

                    if (PrefixTextBox != null)
                    {
                        PrefixTextBox.Focus();
                        Keyboard.Focus(PrefixTextBox);
                        PrefixTextBox.SelectAll();
                    }
                }
                catch
                {
                }
            };

            // Dispatch at Input priority first, then ContextIdle to ensure focus stays after layout/rendering
            _ = Dispatcher.BeginInvoke(focusAction, DispatcherPriority.Input);
            _ = Dispatcher.BeginInvoke(focusAction, DispatcherPriority.ContextIdle);
#pragma warning restore VSTHRD001, VSTHRD110
        }

        public void OnUserControlPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            EnsureWin32Focus();
        }

        public void OnTextBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EnsureWin32Focus();
            if (sender is TextBox textBox)
            {
                if (!textBox.IsKeyboardFocusWithin)
                {
                    textBox.Focus();
                    Keyboard.Focus(textBox);
                }
            }
        }

        public void OnTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            EnsureWin32Focus();
        }

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_GETDLGCODE = 0x0087;
            const uint DLGC_WANTARROWS = 0x0001;
            const uint DLGC_WANTTAB = 0x0002;
            const uint DLGC_WANTALLKEYS = 0x0004;
            const uint DLGC_HASSETSEL = 0x0008;
            const uint DLGC_WANTCHARS = 0x0080;

            if (msg == WM_GETDLGCODE)
            {
                var focused = Keyboard.FocusedElement as DependencyObject;
                if (focused is TextBox tb)
                {
                    // For TextBoxes: request characters, selection, and arrow keys.
                    // CRITICAL: Do NOT return DLGC_WANTALLKEYS unconditionally!
                    // Returning DLGC_WANTALLKEYS causes IsDialogMessage to consume character
                    // keystrokes without calling TranslateMessage, suppressing WM_CHAR and breaking typing!
                    uint flags = DLGC_WANTCHARS | DLGC_HASSETSEL | DLGC_WANTARROWS;

                    if (tb.AcceptsTab)
                    {
                        flags |= DLGC_WANTTAB;
                    }

                    // Multiline textbox wants Return key so Enter doesn't trigger the dialog's default button
                    if (tb.AcceptsReturn && lParam != IntPtr.Zero)
                    {
                        try
                        {
                            // In Win32, lParam of WM_GETDLGCODE points to MSG struct when sent by IsDialogMessage
                            // Check if the current message key is VK_RETURN (0x0D)
                            int wParamOffset = IntPtr.Size == 8 ? 16 : IntPtr.Size + 4;
                            int vk = Marshal.ReadInt32(lParam, wParamOffset);
                            if (vk == 0x0D)
                            {
                                flags |= DLGC_WANTALLKEYS;
                            }
                        }
                        catch
                        {
                            flags |= DLGC_WANTALLKEYS;
                        }
                    }

                    handled = true;
                    return new IntPtr(flags);
                }
            }
            return IntPtr.Zero;
        }
    }
}
