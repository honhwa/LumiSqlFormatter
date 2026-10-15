using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using LumiSqlFormatter.UI.Views;

namespace LumiSqlFormatter.UI
{
    [Guid("984A2292-6FE7-4029-BA91-62885918A01A")]
    public class SettingsToolWindow : ToolWindowPane
    {
        public SettingsToolWindow() : base(null)
        {
            this.Caption = "Lumi SQL Formatter & Snippet Manager (PHONGNT)";
            this.Content = new SettingsView();
        }
    }
}
