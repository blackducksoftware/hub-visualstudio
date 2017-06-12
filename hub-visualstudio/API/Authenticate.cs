using System;
using System.Net;
using BlackDuckHub.VisualStudio.Properties;
using RestSharp;
using System.Linq;

namespace BlackDuckHub.VisualStudio.API
{
    public static class Authenticate
    {

        internal const string CsrfHeaderKey = "X-CSRF-TOKEN";

        public static RestClient EstablishHubSession(string[] hubSettings)
        {
            var client = new RestClient(hubSettings[0]) { CookieContainer = new CookieContainer() };

/* //Required to debug on the QA server as of 6/12/2017 due to SSL self signed cert. This ignores SSL errors.
            ServicePointManager.ServerCertificateValidationCallback +=
            (sender, certificate, chain, sslPolicyErrors) => true;
*/

            if (!String.IsNullOrEmpty(hubSettings[3]))
                client.Timeout = TimeSpan.FromSeconds(Int32.Parse(hubSettings[3])).Milliseconds;

            var authRequest =
                new RestRequest("j_spring_security_check", Method.POST).AddParameter(
                    "application/x-www-form-urlencoded",
                    $"j_username={hubSettings[1]}&j_password={hubSettings[2]}",
                    ParameterType.RequestBody);

            var response = client.Execute(authRequest);

            var csrfParam = response.Headers.Where(param => param.Name == CsrfHeaderKey && !String.IsNullOrEmpty(param?.Value?.ToString())).FirstOrDefault();
            if (csrfParam != null)
            {
                client.DefaultParameters.Add(csrfParam);
            }

            return client;
        }
    }
}