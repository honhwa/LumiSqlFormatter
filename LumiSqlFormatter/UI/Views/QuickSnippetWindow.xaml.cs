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

        private void InsertCursor_Click(object sender, RoutedEventArgs e)
        {
            if (BodyTextBox != null)
            {
                int insertIndex = BodyTextBox.SelectionStart;
                string text = BodyTextBox.Text ?? string.Empty;
                if (insertIndex < 0) insertIndex = 0;
                if (insertIndex > text.Length) insertIndex = text.Length;

                const string cursorTag = "$CURSOR$";
                BodyTextBox.SelectedText = cursorTag;
                BodyTextBox.SelectionLength = 0;
                BodyTextBox.SelectionStart = insertIndex + cursorTag.Length;

                BodyTextBox.Focus();
                Keyboard.Focus(BodyTextBox);
            }
        }
    }
}
