using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using LumiSqlFormatter.Models;
using LumiSqlFormatter.Services;

namespace LumiSqlFormatter.UI.Views
{
    public partial class SnippetSuggestionPopup : UserControl
    {
        public event Action<SnippetItem> SnippetSelected;

        public SnippetItem SelectedItem
        {
            get => SuggestionsList.SelectedItem as SnippetItem;
            set
            {
                SuggestionsList.SelectedItem = value;
                if (value != null)
                {
                    SuggestionsList.ScrollIntoView(value);
                }
            }
        }

        public SnippetSuggestionPopup()
        {
            InitializeComponent();

            SuggestionsList.MouseDoubleClick += (s, e) =>
            {
                if (SelectedItem != null)
                {
                    SnippetSelected?.Invoke(SelectedItem);
                }
            };
        }

        public void SetSuggestions(IReadOnlyList<SnippetItem> items)
        {
            ApplyTheme(SettingsService.Instance?.Settings?.SnippetTheme ?? SnippetTheme.Dark);
            SuggestionsList.ItemsSource = items;
            if (items != null && items.Count > 0)
            {
                SuggestionsList.SelectedIndex = 0;
            }
            else
            {
                PreviewBox.Text = string.Empty;
            }
        }

        public void ApplyTheme(SnippetTheme theme)
        {
            if (theme == SnippetTheme.Light)
            {
                MainBorder.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                MainBorder.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 120, 215));
                HeaderBorder.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 120, 215));
                HeaderTitle.Foreground = System.Windows.Media.Brushes.White;
                HeaderHint.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));

                SuggestionsList.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                SuggestionsList.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(17, 17, 17));
                SuggestionsList.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(204, 204, 204));
                SuggestionsList.Tag = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(40, 40, 40));

                PreviewContainer.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(250, 250, 250));
                PreviewLabel.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 30));
                PreviewBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(250, 250, 250));
                PreviewBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(17, 17, 17));
            }
            else
            {
                MainBorder.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(37, 37, 38));
                MainBorder.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 136, 229));
                HeaderBorder.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 136, 229));
                HeaderTitle.Foreground = System.Windows.Media.Brushes.White;
                HeaderHint.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(227, 242, 253));

                SuggestionsList.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(45, 45, 48));
                SuggestionsList.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(241, 241, 241));
                SuggestionsList.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(63, 63, 70));
                SuggestionsList.Tag = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(204, 204, 204));

                PreviewContainer.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 30));
                PreviewLabel.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(133, 133, 133));
                PreviewBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 30));
                PreviewBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(220, 220, 220));
            }
        }

        public void SelectNext()
        {
            if (SuggestionsList.Items.Count == 0)
                return;

            int next = (SuggestionsList.SelectedIndex + 1) % SuggestionsList.Items.Count;
            SuggestionsList.SelectedIndex = next;
            SuggestionsList.ScrollIntoView(SuggestionsList.SelectedItem);
        }

        public void SelectPrevious()
        {
            if (SuggestionsList.Items.Count == 0)
                return;

            int prev = SuggestionsList.SelectedIndex - 1;
            if (prev < 0)
                prev = SuggestionsList.Items.Count - 1;

            SuggestionsList.SelectedIndex = prev;
            SuggestionsList.ScrollIntoView(SuggestionsList.SelectedItem);
        }

        private void SuggestionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItem != null)
            {
                PreviewBox.Text = SelectedItem.Template ?? string.Empty;
            }
            else
            {
                PreviewBox.Text = string.Empty;
            }
        }
    }
}
