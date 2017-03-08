using System;
using System.Net;
using BlackDuckHub.VisualStudio.Properties;
using RestSharp;

namespace BlackDuckHub.VisualStudio.API
{
    public static class Authenticate
    {
        public static RestClient EstablishHubSession(string[] hubSettings)
        {
            var client = new RestClient(hubSettings[0]) {CookieContainer = new CookieContainer()};

            if (!String.IsNullOrEmpty(hubSettings[3]))
                client.Timeout = TimeSpan.FromSeconds(Int32.Parse(hubSettings[3])).Milliseconds;

            var authRequest =
                new RestRequest(Resources.LoginSecurity, Method.POST).AddParameter(
                    "application/x-www-form-urlencoded",
                    $"j_username={hubSettings[1]}&j_password={hubSettings[2]}",
                    ParameterType.RequestBody);
            client.Execute(authRequest);

            return client;
        }
    }
}