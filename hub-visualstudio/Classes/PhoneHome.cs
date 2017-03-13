using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackDuckHub.VisualStudio.Classes
{
    public class PhoneHome
    {
        public class InfoMap
        {
            public string blackDuckName { get; set; }
            public string blackDuckVersion { get; set; }
            public string thirdPartyName { get; set; }
            public string thirdPartyVersion { get; set; }
            public string pluginVersion { get; set; }
        }

        public class RootObject
        {
            public string regId { get; set; }
            public string source { get; set; }
            public InfoMap infoMap { get; set; }
        }
    }
}
