using RestSharp;

namespace BlackDuckHub.VisualStudio.API
{
    public static class Component
    {
        public static IRestResponse<Classes.Component.RootObject> GetComponent(string externalId, RestClient client)
        {
            var getComponent = new RestRequest("/api/components?q=id:{externalId}");
            getComponent.AddUrlSegment("externalId", externalId);
            return client.Execute<Classes.Component.RootObject>(getComponent);
        }
    }
}