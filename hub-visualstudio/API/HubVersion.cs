using RestSharp;

namespace BlackDuckHub.VisualStudio.API
{
    public static class HubVersion
    {
        public static string GetHubVersion(RestClient client)
        {
            var getHubVersion = new RestRequest("/api/v1/current-version");
            client.Timeout = 5000;
            return client.Execute(getHubVersion).Content;
        }

        public static string GetHubVersionNumberString(RestClient client)
        {
            return GetHubVersion(client).Replace("\"", "").Trim();
        }
    }
}