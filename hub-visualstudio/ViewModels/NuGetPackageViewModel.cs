using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackDuckHub.VisualStudio.ViewModels
{
    public class NuGetPackageViewModel
    {
        public enum PackageStatus
        {
            Vulnerable,
            Secure,
            NotFound
        }

        public class NuGetPackage
        {
            public PackageStatus PackageStatus { get; set; }
            public string Forge { get; set; }
            public string HubLink { get; set; }
            public string Package { get; set; }
            public string Version { get; set; }
            public string License { get; set; }
            public string NumHighVulns { get; set; }
            public string NumMediumVulns { get; set; }
            public string NumLowVulns { get; set; }
            public string HighVulnsTooltip { get; set; }
            public string MediumVulnsTooltip { get; set; }
            public string LowVulnsTooltip { get; set; }
            public string PackageLink { get; set; }
            public string LinkText { get; set; }
            public string VsProject { get; set; }
            public List<Vulnerability> Vulnerabilities { get; set; }

            public NuGetPackage()
            {
                Vulnerabilities = new List<Vulnerability>();
            }
        }

        public class Vulnerability
        {
            public string VulnLink { get; set; }
        }

        public class PackageViewModel
        {
            public List<NuGetPackage> Packages { get; set; }
        }
    }
}