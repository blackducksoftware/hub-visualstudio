using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackDuckHub.VisualStudio.Classes
{
    public class HubSettings
    {
        public string ServerUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Timeout { get; set; }

        public string ProxyHost { get; set; }
        public string ProxyPort { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }

        public bool HasSettings()
        {
            return !string.IsNullOrEmpty(ServerUrl) && !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Timeout);
        }
    }
}
