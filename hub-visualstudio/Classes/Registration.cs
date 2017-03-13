using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackDuckHub.VisualStudio.Classes
{
    public class Registration
    {
        public class ISCAN
        {
            public string feature { get; set; }
            public long softValue { get; set; }
            public long hardValue { get; set; }
            public bool valid { get; set; }
            public string state { get; set; }
        }

        public class DEEPVULNERABILITYINTELLIGENCE
        {
            public string feature { get; set; }
            public long softValue { get; set; }
            public long hardValue { get; set; }
            public bool valid { get; set; }
            public string state { get; set; }
        }

        public class CONNECTANDSHARE
        {
            public string feature { get; set; }
            public bool valid { get; set; }
            public string state { get; set; }
        }

        public class RESTAPI
        {
            public string feature { get; set; }
            public long softValue { get; set; }
            public long hardValue { get; set; }
            public bool valid { get; set; }
            public string state { get; set; }
        }

        public class POLICYMANAGEMENT
        {
            public string feature { get; set; }
            public long softValue { get; set; }
            public long hardValue { get; set; }
            public bool valid { get; set; }
            public string state { get; set; }
        }

        public class OSSATTRIBUTION
        {
            public string feature { get; set; }
            public long softValue { get; set; }
            public long hardValue { get; set; }
            public bool valid { get; set; }
            public string state { get; set; }
        }

        public class CODESEARCH
        {
            public string feature { get; set; }
            public bool valid { get; set; }
            public string state { get; set; }
        }

        public class RISKMANAGEMENT
        {
            public string feature { get; set; }
            public long softValue { get; set; }
            public long hardValue { get; set; }
            public bool valid { get; set; }
            public string state { get; set; }
        }

        public class PROJECTMANAGEMENT
        {
            public string feature { get; set; }
            public long softValue { get; set; }
            public long hardValue { get; set; }
            public bool valid { get; set; }
            public string state { get; set; }
        }

        public class NOTIFICATIONS
        {
            public string feature { get; set; }
            public long softValue { get; set; }
            public long hardValue { get; set; }
            public bool valid { get; set; }
            public string state { get; set; }
        }

        public class RegistrationFeatures
        {
            public ISCAN ISCAN { get; set; }
            public DEEPVULNERABILITYINTELLIGENCE DEEP_VULNERABILITY_INTELLIGENCE { get; set; }
            public CONNECTANDSHARE CONNECT_AND_SHARE { get; set; }
            public RESTAPI REST_API { get; set; }
            public POLICYMANAGEMENT POLICY_MANAGEMENT { get; set; }
            public OSSATTRIBUTION OSS_ATTRIBUTION { get; set; }
            public CODESEARCH CODE_SEARCH { get; set; }
            public RISKMANAGEMENT RISK_MANAGEMENT { get; set; }
            public PROJECTMANAGEMENT PROJECT_MANAGEMENT { get; set; }
            public NOTIFICATIONS NOTIFICATIONS { get; set; }
        }

        public class USERLIMIT
        {
            public string attribute { get; set; }
            public long actualValue { get; set; }
            public bool unlimited { get; set; }
        }

        public class INDEXEDLINESOFCODE
        {
            public string attribute { get; set; }
            public long actualValue { get; set; }
            public bool unlimited { get; set; }
        }

        public class CODELOCATIONBYTESLIMIT
        {
            public string attribute { get; set; }
            public long actualValue { get; set; }
            public bool unlimited { get; set; }
        }

        public class PROJECTRELEASELIMIT
        {
            public string attribute { get; set; }
            public long softValue { get; set; }
            public long actualValue { get; set; }
            public bool unlimited { get; set; }
        }

        public class CODEBASEMANAGEDLINESOFCODE
        {
            public string attribute { get; set; }
            public long actualValue { get; set; }
            public bool unlimited { get; set; }
        }

        public class CODELOCATIONLIMIT
        {
            public string attribute { get; set; }
            public long actualValue { get; set; }
            public bool unlimited { get; set; }
        }

        public class MANAGEDCODEBASEBYTESNEW
        {
            public string attribute { get; set; }
            public long actualValue { get; set; }
            public bool unlimited { get; set; }
        }

        public class CUSTOMPROJECTLIMIT
        {
            public string attribute { get; set; }
            public long softValue { get; set; }
            public long actualValue { get; set; }
            public bool unlimited { get; set; }
        }

        public class RegistrationAttributes
        {
            public USERLIMIT USER_LIMIT { get; set; }
            public INDEXEDLINESOFCODE INDEXED_LINES_OF_CODE { get; set; }
            public CODELOCATIONBYTESLIMIT CODE_LOCATION_BYTES_LIMIT { get; set; }
            public PROJECTRELEASELIMIT PROJECT_RELEASE_LIMIT { get; set; }
            public CODEBASEMANAGEDLINESOFCODE CODEBASE_MANAGED_LINES_OF_CODE { get; set; }
            public CODELOCATIONLIMIT CODE_LOCATION_LIMIT { get; set; }
            public MANAGEDCODEBASEBYTESNEW MANAGED_CODEBASE_BYTES_NEW { get; set; }
            public CUSTOMPROJECTLIMIT CUSTOM_PROJECT_LIMIT { get; set; }
        }

        public class RootObject
        {
            public string state { get; set; }
            public bool valid { get; set; }
            public string warningExpirationDate { get; set; }
            public string expirationDate { get; set; }
            public RegistrationFeatures registrationFeatures { get; set; }
            public List<object> registrationMessages { get; set; }
            public List<object> warnings { get; set; }
            public string registrationId { get; set; }
            public string licenseUsageWindowDate { get; set; }
            public RegistrationAttributes registrationAttributes { get; set; }
        }
    }
}
