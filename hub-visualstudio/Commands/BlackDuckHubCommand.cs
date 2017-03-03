using System;
using System.ComponentModel.Design;
using BlackDuckHub.VisualStudio.Properties;
using BlackDuckHub.VisualStudio.Settings;
using BlackDuckHub.VisualStudio.UI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace BlackDuckHub.VisualStudio.Commands
{
    internal sealed class BlackDuckHubCommand
    {
        public const int CommandId = 0x0100;
        private readonly Package _package;

        private BlackDuckHubCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("Black Duck Hub package not found");
            }

            this._package = package;

            var commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService == null) return;
            var menuCommandId = new CommandID(GuidStrings.GuidCmdSet, CommandId);
            var menuItem = new MenuCommand(this.ShowToolWindow, menuCommandId);
            commandService.AddCommand(menuItem);
        }

        public static BlackDuckHubCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider => this._package;

        public static void Initialize(Package package)
        {
            Instance = new BlackDuckHubCommand(package);
        }

        private void ShowToolWindow(object sender, EventArgs e)
        {
            var window = this._package.FindToolWindow(typeof(BlackDuckHubPane), 0, true);

            if (window?.Frame == null)
            {
                throw new NotSupportedException("Cannot create Black Duck Hub window");
            }

            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
