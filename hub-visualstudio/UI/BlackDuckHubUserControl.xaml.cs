using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using BlackDuckHub.VisualStudio.API;
using BlackDuckHub.VisualStudio.Helpers;
using BlackDuckHub.VisualStudio.ViewModels;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;
using Process = System.Diagnostics.Process;
using Task = System.Threading.Tasks.Task;
using System;
using BlackDuckHub.VisualStudio.Classes;

namespace BlackDuckHub.VisualStudio.UI
{
    public partial class BlackDuckHubUserControl : IVsSolutionEvents
    {
        private BlackDuckHubPackage _package;
        private List<NuGetPackageViewModel.NuGetPackage> _packagesList = new List<NuGetPackageViewModel.NuGetPackage>();
        private List<string> _validProjectsList = new List<string>();
        private string _riskAnalysisStatus;
        private readonly DTE _dte = (DTE)Package.GetGlobalService(typeof(DTE));


        private IComponentModel _componentModel;
        private IVsPackageInstallerServices _installerServices;

        public BlackDuckHubUserControl()
        {
            InitializeComponent();
        }

        public void Initialize(BlackDuckHubPackage package)
        {
            if (package != null)
                _package = package;

            uint cookie = 0;

            BlackDuckHubPackage._solutionService.AdviseSolutionEvents(this, out cookie);

            _componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
            _installerServices = _componentModel.GetService<IVsPackageInstallerServices>();

            if (_dte.Solution.IsOpen) return;

            btnRunHubScan.IsEnabled = false;
            cmbProjects.IsEnabled = false;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            btnRunHubScan.IsEnabled = true;

            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        private async void btnRunHubScan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ExecuteHubCommunication();
            }
            catch (Exception ex)
            {
                tbStatus.Foreground = Brushes.Red;
                pbStatus.Visibility = Visibility.Hidden;
                sepStatus.Visibility = Visibility.Hidden;
                tbStatus.Text = Properties.Resources.MessageError;
                TaskManager.AddError(Properties.Resources.PaneTitle + ": " + ex.ToString());
            }
            
        }

        private async Task ExecuteHubCommunication()
        {
            _installerServices = _componentModel.GetService<IVsPackageInstallerServices>();

            var solution = (IVsSolution)Package.GetGlobalService(typeof(IVsSolution));
            var projects = SolutionExplorer.GetProjects(solution);

            if (projects.Count() > 0)
            {
                tbStatus.SetResourceReference(ForegroundProperty, VsBrushes.PanelTextKey);
                cmbProjects.IsEnabled = false;
                tbStatus.Visibility = Visibility.Visible;
                pbStatus.Visibility = Visibility.Visible;
                sepStatus.Visibility = Visibility.Visible;
                tbStatus.Text = Properties.Resources.StatusRunning;

                if (await GetPackagesAndRiskInformation(projects))
                {
                    dgPackages.ItemsSource = _packagesList;
                    dgPackages.Items.Refresh();
                    tbStatus.Text = Properties.Resources.StatusComplete;
                    cmbProjects.ItemsSource = null;
                    cmbProjects.ItemsSource = _validProjectsList;
                    cmbProjects.IsEnabled = true;
                    cmbProjects.SelectedItem = Properties.Resources.ItemAll;
                    pbStatus.Visibility = Visibility.Hidden;
                    sepStatus.Visibility = Visibility.Hidden;
                }
                else
                {
                    tbStatus.Foreground = Brushes.Red;
                    tbStatus.Text = _riskAnalysisStatus;
                    pbStatus.Visibility = Visibility.Hidden;
                    sepStatus.Visibility = Visibility.Hidden;
                }
            }
        }

        private async Task<bool> GetPackagesAndRiskInformation(IEnumerable<Project> projects)
        {
            var status = false;
            await Task.Run(() =>
            {
                var hubSettings = new HubSettings()
                {
                    ServerUrl = _package.HubServerUrl,
                    Username = _package.HubUsername,
                    Password =_package.HubPassword,
                    Timeout = _package.HubTimeout,
                    ProxyHost = _package.ProxyHost,
                    ProxyPort = _package.ProxyPort,
                    ProxyUsername = _package.ProxyUsername,
                    ProxyPassword = _package.ProxyPassword
                };

                if (!_installerServices.GetInstalledPackages().Any()) return;
                if (!HubSettingsAssistant.HasHubSettings(hubSettings))
                {
                    _riskAnalysisStatus = Properties.Resources.MesageNoHubSettings;
                    status = false;
                }
                else
                {
                    var client = Authenticate.EstablishHubSession(hubSettings);

                    if (client == null)
                    {
                        _riskAnalysisStatus = Properties.Resources.MessageConnectionUnsuccessful;
                        status = false;
                    }
                    else
                    {
                        String version = HubVersion.GetHubVersionNumberString(client);
                        _packagesList.Clear();
                        _validProjectsList.Clear();

                        _validProjectsList.Add(Properties.Resources.ItemAll);

                        foreach (var project in projects)
                        {
                            List<IVsPackageMetadata> packages = null;

                            try
                            {
                                packages = _installerServices.GetInstalledPackages(project).ToList();
                                _validProjectsList.Add(project.Name);
                            }
                            catch { continue; }

                            var projectPath = project.Properties;

                            foreach (var package in packages)
                            {
                                var index =
                                    _packagesList.FindIndex(
                                        item => (item.Package == package.Id) && (item.Version == package.VersionString));

                                if (index < 0)
                                    _packagesList.Add(new NuGetPackageViewModel.NuGetPackage
                                    {
                                        Forge = Properties.Resources.Forge,
                                        Package = package.Id,
                                        Version = package.VersionString,
                                        VsProject = project.Name
                                    });
                                else
                                    _packagesList[index].VsProject = _packagesList[index].VsProject + "|" + project.Name;
                            }
                        }

                        foreach (var item in _packagesList)
                        {
                            var externalId = $"{item.Forge}|{item.Package}|{item.Version}";

                            //Get Component
                            var getComponentResponse = API.Component.GetComponent(externalId, client);

                            //Get Component Version
                            if ((getComponentResponse.Data.items?.Count == 1) &&
                                (getComponentResponse.Data.items[0].version != null))
                            {
                                var versionId = getComponentResponse.Data.items[0].version.Substring(getComponentResponse.Data.items[0].version.LastIndexOf("/") + 1);

                                if (int.Parse(version.Split('.')[0]) < 4)
                                {
                                    item.HubLink = _package.HubServerUrl + "/#versions/id:" + versionId + "/view:overview";
                                }
                                else
                                {
                                    item.HubLink = _package.HubServerUrl + "/ui/versions/id:" + versionId + "/view:overview";
                                }


                                var getComponentVersionResponse =
                                    API.ComponentVersion.GetComponentVersion(getComponentResponse, _package.HubServerUrl,
                                        client);

                                var vulnHref = "";

                                //Obtain license(s)
                                var licenseList = new List<string>();
                                if (getComponentVersionResponse.Data.license.licenses.Count == 0)
                                {
                                    licenseList.Add(getComponentVersionResponse.Data.license.licenseDisplay);
                                }

                                foreach (var license in getComponentVersionResponse.Data.license.licenses)
                                {
                                    licenseList.Add(license.name);
                                }

                                var licenses = string.Join(",", licenseList);

                                item.License = licenses;

                                //Get Security Risk
                                foreach (var link in getComponentVersionResponse.Data._meta.links)
                                {
                                    if (link.rel == "vulnerabilities")
                                    {
                                        vulnHref = link.href;
                                    }
                                }

                                if (vulnHref == null) continue;

                                var getVulnerabilitiesResponse =
                                    API.ComponentVulnerability.GetVulnerabilities(getComponentVersionResponse,
                                        _package.HubServerUrl, client, vulnHref);

                                if (getVulnerabilitiesResponse.Data.totalCount != 0)
                                {
                                    item.PackageStatus = NuGetPackageViewModel.PackageStatus.Vulnerable;

                                    var highVulns = 0;
                                    var mediumVulns = 0;
                                    var lowVulns = 0;

                                    foreach (var vuln in getVulnerabilitiesResponse.Data.items)
                                    {
                                        var vulnLink = vuln._meta.href;

                                        switch (vuln.severity)
                                        {
                                            case "HIGH":
                                                highVulns++;
                                                break;
                                            case "MEDIUM":
                                                mediumVulns++;
                                                break;
                                            default:
                                                lowVulns++;
                                                break;
                                        }
                                    }

                                    if (highVulns > 0) {
                                        item.NumHighVulns = highVulns.ToString();
                                        item.HighVulnsTooltip = (highVulns == 1) ? highVulns.ToString() + " High " + Properties.Resources.SeverityTooltipSingle : highVulns.ToString() + " High " + Properties.Resources.SeverityTooltip;
                                    }
                                    else {
                                        item.NumHighVulns = null;
                                    }

                                    if (mediumVulns > 0)
                                    {
                                        item.NumMediumVulns = mediumVulns.ToString();
                                        item.MediumVulnsTooltip = (mediumVulns == 1) ? mediumVulns.ToString() + " Medium " + Properties.Resources.SeverityTooltipSingle : mediumVulns.ToString() + " Medium " + Properties.Resources.SeverityTooltip;
                                    }
                                    else
                                    {
                                        item.NumMediumVulns = null;
                                    }

                                    if (lowVulns > 0)
                                    {
                                        item.NumLowVulns = lowVulns.ToString();
                                        item.LowVulnsTooltip = (lowVulns == 1) ? lowVulns.ToString() + " Low " + Properties.Resources.SeverityTooltipSingle : lowVulns.ToString() + " Low " + Properties.Resources.SeverityTooltip;
                                    }
                                    else
                                    {
                                        item.NumLowVulns = null;
                                    }

                                }
                                else
                                {
                                    item.PackageStatus = NuGetPackageViewModel.PackageStatus.Secure;
                                }
                            }
                            else
                            {
                                item.PackageStatus = NuGetPackageViewModel.PackageStatus.NotFound;
                            }
                        }

                        if (_packagesList.Count > 0)
                            _packagesList = _packagesList.OrderBy(x => x.PackageStatus).ThenBy(y => y.Package).ToList();

                        _validProjectsList.Sort();

                        status = true;
                        Task.Run(() => CollectData.CallHome(client, hubSettings.ServerUrl, _dte.Version));
                    }
                }
            });
            return status;
        }

        private void dgPackagesRow_DoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //View component version in Hub
                var package = dgPackages.SelectedItem as NuGetPackageViewModel.NuGetPackage;
                if (package.HubLink != null)
                {
                    Process.Start(package.HubLink);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Resources.MessageError);
                TaskManager.AddError(Properties.Resources.PaneTitle + ": " + ex.ToString());
            }
        }

        private void cmbProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((cmbProjects.SelectedItem != null) && (cmbProjects.SelectedItem.ToString() != Properties.Resources.ItemAll))
                {
                    var filteredList =
                        _packagesList.Where(x => x.VsProject.Contains(cmbProjects.SelectedItem.ToString())).ToList();
                    dgPackages.ItemsSource = filteredList;
                }
                else
                {
                    dgPackages.ItemsSource = _packagesList;
                }
                dgPackages.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Resources.MessageError);
                TaskManager.AddError(Properties.Resources.PaneTitle + ": " + ex.ToString());
            }
        }

        private void dgPackages_GotFocus(object sender, RoutedEventArgs e)
        {
            dgPackages.Tag = true;
        }

        private void dgPackages_LostFocus(object sender, RoutedEventArgs e)
        {
            dgPackages.Tag = false;
        }

    }
}