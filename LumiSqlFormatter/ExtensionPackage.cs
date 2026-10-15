using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using LumiSqlFormatter.UI;

namespace LumiSqlFormatter
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("Lumi SQL Formatter (PHONGNT)", "Real-time SQL Auto-Capitalization & Smart Snippets for SSMS 22", "1.1.3")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideToolWindow(typeof(SettingsToolWindow), Style = VsDockStyle.Tabbed, Window = ToolWindowGuids.Outputwindow)]
    [ProvideOptionPage(typeof(OptionsPage), "Lumi SQL Formatter", "General", 0, 0, true)]
    [Guid(GuidList.guidLumiSqlPackageString)]
    public sealed class ExtensionPackage : AsyncPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            // Register OleMenuCommandService
            var commandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(GuidList.guidLumiSqlCmdSet, (int)PkgCmdIDList.cmdidOpenSettings);
                var menuItem = new MenuCommand(ShowSettingsToolWindow, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        private void ShowSettingsToolWindow(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            ToolWindowPane window = this.FindToolWindow(typeof(SettingsToolWindow), 0, true);
            if (window?.Frame == null)
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            var windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
