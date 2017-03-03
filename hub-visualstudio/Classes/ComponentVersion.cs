using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackDuckHub.VisualStudio.Classes
{
    public class ComponentVersion
    {
        public class License2
        {
            public string name { get; set; }
            public string ownership { get; set; }
            public string codeSharing { get; set; }
            public List<object> licenses { get; set; }
            public string license { get; set; }
        }

        public class License
        {
            public string type { get; set; }
            public List<License2> licenses { get; set; }
        }

        public class Link
        {
            public string rel { get; set; }
            public string href { get; set; }
        }

        public class Meta
        {
            public List<string> allow { get; set; }
            public string href { get; set; }
            public List<Link> links { get; set; }
        }

        public class RootObject
        {
            public string versionName { get; set; }
            public string releasedOn { get; set; }
            public string source { get; set; }
            public License license { get; set; }
            public Meta _meta { get; set; }
        }
    }
}
