using System;
using System.Windows.Input;
using LumiSqlFormatter.Models;
using LumiSqlFormatter.Services;

namespace LumiSqlFormatter.UI.ViewModels
{
    public class QuickSnippetViewModel : ViewModelBase
    {
        private readonly SnippetService _snippetService;
        private string _shortcut = string.Empty;
        private string _description = string.Empty;
        private string _template = string.Empty;
        private string _statusMessage = string.Empty;

        public Action<bool> CloseAction { get; set; }

        public string Shortcut
        {
            get => _shortcut;
            set
            {
                SetProperty(ref _shortcut, value);
                OnPropertyChanged(nameof(Prefix));
            }
        }

        public string Prefix
        {
            get => Shortcut;
            set => Shortcut = value;
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Template
        {
            get => _template;
            set
            {
                SetProperty(ref _template, value);
                OnPropertyChanged(nameof(Body));
            }
        }

        public string Body
        {
            get => Template;
            set => Template = value;
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public string EditorBackground => (SettingsService.Instance?.Settings?.SnippetTheme ?? SnippetTheme.Dark) == SnippetTheme.Light ? "#FFFFFF" : "#1E1E1E";
        public string EditorForeground => (SettingsService.Instance?.Settings?.SnippetTheme ?? SnippetTheme.Dark) == SnippetTheme.Light ? "#212121" : "#D4D4D4";

        public ICommand InsertCursorCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public QuickSnippetViewModel(string initialTemplate = "")
        {
            _snippetService = SnippetService.Instance;
            Template = initialTemplate ?? string.Empty;

            InsertCursorCommand = new RelayCommand(InsertCursor);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void InsertCursor()
        {
            Template = (Template ?? string.Empty) + "$CURSOR$";
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Shortcut))
            {
                StatusMessage = "Vui lòng nhập Shortcut (Mã gợi nhớ)!";
                return;
            }

            if (string.IsNullOrWhiteSpace(Template))
            {
                StatusMessage = "Template không được để trống!";
                return;
            }

            var newItem = new SnippetItem(Shortcut.Trim(), Description?.Trim() ?? string.Empty, Template);
            _snippetService.AddSnippet(newItem);

            CloseAction?.Invoke(true);
        }

        private void Cancel()
        {
            CloseAction?.Invoke(false);
        }
    }
}
