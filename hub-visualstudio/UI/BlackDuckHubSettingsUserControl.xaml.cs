using System.Windows;
using BlackDuckHub.VisualStudio.API;
using BlackDuckHub.VisualStudio.Helpers;

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
            var hubSettings = new string[] { txtHubServerUrl.Text, txtHubUsername.Text, txtHubPassword.Password, txtHubTimeout.Text };
            if (HubSettingsAssistant.HasHubSettings(hubSettings))
            {
                var client = Authenticate.EstablishHubSession(hubSettings);

                MessageBox.Show(client.CookieContainer.Count > 0
                    ? Properties.Resources.MessageConnectionSuccessful
                    : Properties.Resources.MessageConnectionUnsuccessful);
            }
            else
                MessageBox.Show(Properties.Resources.MesageNoHubSettings);
        }
    }
}