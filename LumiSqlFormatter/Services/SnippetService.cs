using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LumiSqlFormatter.Models;
using Newtonsoft.Json;

namespace LumiSqlFormatter.Services
{
    public class SnippetMatchResult
    {
        public SnippetItem Snippet { get; set; }
        public string MatchedShortcut { get; set; }
        public int ShortcutLength => MatchedShortcut?.Length ?? 0;
        public string ExpandedText { get; set; }
        public int CaretOffsetInExpansion { get; set; }
    }

    public class SnippetService
    {
        private static readonly Lazy<SnippetService> _instance =
            new Lazy<SnippetService>(() => new SnippetService());
        public static SnippetService Instance => _instance.Value;

        private readonly SettingsService _settingsService;
        private static readonly Regex ShortcutRegex = new Regex(@"^[a-zA-Z0-9_#@]+$", RegexOptions.Compiled);

        private SnippetService()
        {
            _settingsService = SettingsService.Instance;
        }

        public IReadOnlyList<SnippetItem> GetAllSnippets()
        {
            return _settingsService.Snippets.AsReadOnly();
        }

        public void ReloadSnippets()
        {
            _settingsService.LoadSnippets();
        }

        public IReadOnlyList<SnippetItem> GetSuggestions(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix) || !_settingsService.Settings.EnableSnippets || !_settingsService.Settings.EnableSnippetSuggestions)
                return new List<SnippetItem>();

            string p = prefix.Trim();
            var all = _settingsService.Snippets;

            // Prioritize starts-with, then contains
            return all
                .Where(s => s.Shortcut != null && s.Shortcut.StartsWith(p, StringComparison.OrdinalIgnoreCase))
                .OrderBy(s => s.Shortcut.Length)
                .ThenBy(s => s.Shortcut)
                .ToList();
        }

        public SnippetMatchResult TryMatchAndExpand(string textBeforeCaret)
        {
            if (string.IsNullOrEmpty(textBeforeCaret) || !_settingsService.Settings.EnableSnippets)
                return null;

            // Extract candidate shortcut immediately preceding caret
            // Search backwards for whitespace or punctuation
            int index = textBeforeCaret.Length - 1;
            while (index >= 0 && (char.IsLetterOrDigit(textBeforeCaret[index]) || textBeforeCaret[index] == '_' || textBeforeCaret[index] == '#'))
            {
                index--;
            }

            string candidate = textBeforeCaret.Substring(index + 1);
            if (string.IsNullOrEmpty(candidate))
                return null;

            var snippet = _settingsService.Snippets.FirstOrDefault(s =>
                string.Equals(s.Shortcut, candidate, StringComparison.OrdinalIgnoreCase));

            if (snippet == null)
                return null;

            return ExpandSnippet(snippet, candidate);
        }

        public SnippetMatchResult ExpandSnippet(SnippetItem snippet, string matchedShortcut)
        {
            if (snippet == null)
                return null;

            string template = snippet.Template ?? string.Empty;
            int caretOffset = -1;

            // Check for $CURSOR$ or $CARET$ placeholder
            string[] cursorTokens = new[] { "$CURSOR$", "$CARET$" };
            foreach (var token in cursorTokens)
            {
                int pos = template.IndexOf(token, StringComparison.OrdinalIgnoreCase);
                if (pos >= 0)
                {
                    caretOffset = pos;
                    template = template.Remove(pos, token.Length);
                    break;
                }
            }

            // If no explicit cursor token, place cursor at the end or at first variable placeholder
            if (caretOffset == -1)
            {
                var varMatch = Regex.Match(template, @"\$[A-Za-z0-9_]+\$");
                if (varMatch.Success)
                {
                    caretOffset = varMatch.Index;
                }
                else
                {
                    caretOffset = template.Length;
                }
            }

            return new SnippetMatchResult
            {
                Snippet = snippet,
                MatchedShortcut = matchedShortcut,
                ExpandedText = template,
                CaretOffsetInExpansion = caretOffset
            };
        }

        public void AddSnippet(SnippetItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Prefix))
                return;

            if (string.IsNullOrWhiteSpace(item.Id))
                item.Id = Guid.NewGuid().ToString();

            var list = new List<SnippetItem>(_settingsService.Snippets);
            list.RemoveAll(s => string.Equals(s.Prefix, item.Prefix, StringComparison.OrdinalIgnoreCase));
            list.Add(item);
            _settingsService.SaveSnippets(list);
        }

        public void UpdateSnippet(string originalPrefix, SnippetItem updatedItem)
        {
            if (updatedItem == null || string.IsNullOrWhiteSpace(updatedItem.Prefix))
                return;

            if (string.IsNullOrWhiteSpace(updatedItem.Id))
                updatedItem.Id = Guid.NewGuid().ToString();

            // If prefix changed, clean up previous file
            if (!string.IsNullOrWhiteSpace(originalPrefix) &&
                !string.Equals(originalPrefix, updatedItem.Prefix, StringComparison.OrdinalIgnoreCase))
            {
                _settingsService.DeleteSnippetFile(originalPrefix);
            }

            var list = new List<SnippetItem>(_settingsService.Snippets);
            int idx = list.FindIndex(s => string.Equals(s.Prefix, originalPrefix, StringComparison.OrdinalIgnoreCase) ||
                                          string.Equals(s.Id, updatedItem.Id, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0)
            {
                list[idx] = updatedItem;
            }
            else
            {
                list.Add(updatedItem);
            }
            _settingsService.SaveSnippets(list);
        }

        public void DeleteSnippet(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return;

            _settingsService.DeleteSnippetFile(prefix);

            var list = new List<SnippetItem>(_settingsService.Snippets);
            list.RemoveAll(s => string.Equals(s.Prefix, prefix, StringComparison.OrdinalIgnoreCase));
            _settingsService.SaveSnippets(list);
        }

        public void ExportToFile(string filePath)
        {
            string json = JsonConvert.SerializeObject(_settingsService.Snippets, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void ImportFromFile(string filePath, bool overwriteExisting = true)
        {
            if (!File.Exists(filePath))
                return;

            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
                return;

            string trimmed = json.Trim();
            List<SnippetItem> imported = null;

            if (trimmed.StartsWith("["))
            {
                imported = JsonConvert.DeserializeObject<List<SnippetItem>>(trimmed);
            }
            else
            {
                var single = JsonConvert.DeserializeObject<SnippetItem>(trimmed);
                if (single != null)
                {
                    imported = new List<SnippetItem> { single };
                }
            }

            if (imported == null || imported.Count == 0)
                return;

            var list = new List<SnippetItem>(_settingsService.Snippets);

            foreach (var item in imported)
            {
                if (string.IsNullOrWhiteSpace(item.Prefix))
                    continue;

                if (string.IsNullOrWhiteSpace(item.Id))
                    item.Id = Guid.NewGuid().ToString();

                int existingIdx = list.FindIndex(s => string.Equals(s.Prefix, item.Prefix, StringComparison.OrdinalIgnoreCase) ||
                                                      (!string.IsNullOrEmpty(item.Id) && string.Equals(s.Id, item.Id, StringComparison.OrdinalIgnoreCase)));
                if (existingIdx >= 0)
                {
                    if (overwriteExisting)
                    {
                        list[existingIdx] = item;
                    }
                }
                else
                {
                    list.Add(item);
                }
            }

            _settingsService.SaveSnippets(list);
        }
    }
}
