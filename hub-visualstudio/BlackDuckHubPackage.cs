using System;
using System.Runtime.InteropServices;
using System.Windows;
using BlackDuckHub.VisualStudio.Commands;
using BlackDuckHub.VisualStudio.Settings;
using BlackDuckHub.VisualStudio.UI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using BlackDuckHub.VisualStudio.Helpers;

namespace BlackDuckHub.VisualStudio
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", AssemblyVersionInformation.Version, IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidStrings.GuidPackage)]
    [ProvideOptionPage(typeof(HubSettings), "Black Duck Hub", "Settings", 0, 0, true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(BlackDuckHubPane))]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    public sealed class BlackDuckHubPackage : Package
    {
        public static IVsSolution _solutionService { get; set; }

        public BlackDuckHubPackage()
        {

        }
        protected override void Initialize()
        {
            base.Initialize();

            _solutionService = GetService(typeof(SVsSolution)) as IVsSolution;

            TaskManager.Initialize(this);
            BlackDuckHubCommand.Initialize(this);
        }

        [Guid(GuidStrings.GuidCustomPage)]
        public class HubSettings : UIElementDialogPage
        {
            public string HubServerUrlString { get; set; }
            public string HubUsernameString { get; set; }
            public string HubPasswordString { get; set; }
            public string HubTimeoutString { get; set; }

            protected override UIElement Child => new HubSettingsUserControl(this);
        }

        public string HubServerUrl
        {
            get
            {
                var page = (HubSettings)GetDialogPage(typeof(HubSettings));
                return page.HubServerUrlString;
            }
        }

        public string HubUsername
        {
            get
            {
                var page = (HubSettings)GetDialogPage(typeof(HubSettings));
                return page.HubUsernameString;
            }
        }

        public string HubPassword
        {
            get
            {
                var page = (HubSettings)GetDialogPage(typeof(HubSettings));
                return page.HubPasswordString;
            }
        }

        public string HubTimeout
        {
            get
            {
                var page = (HubSettings)GetDialogPage(typeof(HubSettings));
                return page.HubTimeoutString;
            }
        }

    }
}