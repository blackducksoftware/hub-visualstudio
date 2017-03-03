using RestSharp;

namespace BlackDuckHub.VisualStudio.API
{
    public static class ComponentVersion
    {
        public static IRestResponse<Classes.ComponentVersion.RootObject> GetComponentVersion(IRestResponse<Classes.Component.RootObject> getComponentResponse, string hubServerUrl, RestClient client)
        {
            var getComponentVersion =
                new RestRequest(
                    getComponentResponse.Data.items[0].version.Replace(hubServerUrl, string.Empty));
            return client.Execute<Classes.ComponentVersion.RootObject>(getComponentVersion);
        }
    }
}