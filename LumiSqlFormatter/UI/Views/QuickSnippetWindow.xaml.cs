using System;
using System.Windows;
using System.Windows.Input;
using LumiSqlFormatter.UI.ViewModels;

namespace LumiSqlFormatter.UI.Views
{
    public partial class QuickSnippetWindow : Window
    {
        public QuickSnippetWindow(string initialTemplate = "")
        {
            InitializeComponent();

            var vm = new QuickSnippetViewModel(initialTemplate);
            vm.CloseAction = (dialogResult) =>
            {
                this.DialogResult = dialogResult;
                this.Close();
            };
            this.DataContext = vm;

            this.Loaded += (s, e) =>
            {
                ShortcutBox.Focus();
            };

            this.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    this.Close();
                    e.Handled = true;
                }
            };
        }
    }
}
