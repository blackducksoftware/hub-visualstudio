using System;
using System.Runtime.InteropServices;
using BlackDuckHub.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;

namespace BlackDuckHub.VisualStudio.UI
{
    [Guid(GuidStrings.GuidToolWindow)]
    public sealed class BlackDuckHubPane : ToolWindowPane
    {
        private readonly BlackDuckHubUserControl _hubScanResultsUserControl;

        public BlackDuckHubPane() : base()
        {
            this.Caption = "Black Duck Hub";

            _hubScanResultsUserControl = new BlackDuckHubUserControl();
            base.Content = _hubScanResultsUserControl;
        }

        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();

            var hubScanResultsCommandPackagePackage = (BlackDuckHubPackage)base.Package;
            _hubScanResultsUserControl.Initialize(hubScanResultsCommandPackagePackage);
        }

        internal object GetVsService(Type service)
        {
            return GetService(service);
        }
    }
}
