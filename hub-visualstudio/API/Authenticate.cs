using System;
using System.Net;
using BlackDuckHub.VisualStudio.Properties;
using RestSharp;
using System.Linq;
using BlackDuckHub.VisualStudio.Classes;

namespace BlackDuckHub.VisualStudio.API
{
    public static class Authenticate
    {

        internal const string CsrfHeaderKey = "X-CSRF-TOKEN";

        public static RestClient EstablishHubSession(HubSettings hubSettings)
        {
            var client = new RestClient(hubSettings.ServerUrl) { CookieContainer = new CookieContainer() };
            if (!String.IsNullOrWhiteSpace(hubSettings.ProxyHost))
            {
                var proxy = new WebProxy();
                var uriBuilder = new UriBuilder();
                uriBuilder.Host = hubSettings.ProxyHost;
                int port;
                if (int.TryParse(hubSettings.ProxyPort, out port))
                {
                    uriBuilder.Port = port;
                }
                try
                {
                    var uri = uriBuilder.Uri;
                    proxy.Address = uri;
                    proxy.Credentials = new NetworkCredential(hubSettings.ProxyUsername, hubSettings.ProxyPassword);
                    client.Proxy = proxy;
                }
                catch (Exception e)
                {
                    throw new Exception("Hub Proxy Error", e);
                }
            }

/* //Required to debug on the QA server as of 6/12/2017 due to SSL self signed cert. This ignores SSL errors.
            ServicePointManager.ServerCertificateValidationCallback +=
            (sender, certificate, chain, sslPolicyErrors) => true;
*/


            if (!String.IsNullOrEmpty(hubSettings.Timeout))
                client.Timeout = TimeSpan.FromSeconds(Int32.Parse(hubSettings.Timeout)).Milliseconds;

            var authRequest =
                new RestRequest("j_spring_security_check", Method.POST).AddParameter(
                    "application/x-www-form-urlencoded",
                    $"j_username={hubSettings.Username}&j_password={hubSettings.Password}",
                    ParameterType.RequestBody);

            var response = client.Execute(authRequest);

            var csrfParam = response.Headers.Where(param => param.Name == CsrfHeaderKey && !String.IsNullOrEmpty(param?.Value?.ToString())).FirstOrDefault();
            if (csrfParam != null)
            {
                client.DefaultParameters.Add(csrfParam);
            }

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return client;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }
            else if (response.ErrorException != null)
            {
                return null;
            }
            else
            {
                return client;//to be safe, otherwise return the client.
            }

        }
    }
}