using System.Windows;
using BlackDuckHub.VisualStudio.API;
using BlackDuckHub.VisualStudio.Helpers;
using System;
using BlackDuckHub.VisualStudio.Classes;

namespace BlackDuckHub.VisualStudio.UI
{
    public partial class HubSettingsUserControl
    {
        private BlackDuckHubPackage.HubSettings _hubSettings = null;

        public HubSettingsUserControl(BlackDuckHubPackage.HubSettings hubSettingsPage)
        {
            InitializeComponent();
            _hubSettings = hubSettingsPage;
            this.DataContext = _hubSettings;
        }

        private void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var hubSettings = new HubSettings()
                {
                    ServerUrl = txtHubServerUrl.Text,
                    Username = txtHubUsername.Text,
                    Password = txtHubPassword.Password,
                    Timeout = txtHubTimeout.Text,

                    ProxyHost = txtProxyHost.Text,
                    ProxyPort = txtProxyPort.Text,
                    ProxyUsername = txtProxyUsername.Text,
                    ProxyPassword = txtProxyPassword.Password
                };

                if (HubSettingsAssistant.HasHubSettings(hubSettings))
                {
                    var client = Authenticate.EstablishHubSession(hubSettings);
                    
                    MessageBox.Show(client != null 
                        ? Properties.Resources.MessageConnectionSuccessful
                        : Properties.Resources.MessageConnectionUnsuccessful);
                }
                else
                    MessageBox.Show(Properties.Resources.MesageNoHubSettings);
            }
             catch (Exception ex)
            {
                MessageBox.Show(Properties.Resources.MessageError);
                TaskManager.AddError(Properties.Resources.PaneTitle + ": " + ex.ToString());
            }

        }
    }
}