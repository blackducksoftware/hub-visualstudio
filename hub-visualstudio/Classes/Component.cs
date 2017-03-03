using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackDuckHub.VisualStudio.Classes
{
    public class Component
    {
        public class Item
        {
            public string componentName { get; set; }
            public string versionName { get; set; }
            public string originId { get; set; }
            public string component { get; set; }
            public string version { get; set; }
        }

        public class Meta
        {
            public List<string> allow { get; set; }
            public string href { get; set; }
            public List<object> links { get; set; }
        }

        public class RootObject
        {
            public int totalCount { get; set; }
            public List<Item> items { get; set; }
            public Meta _meta { get; set; }
            public List<object> appliedFilters { get; set; }
        }
    }
}
