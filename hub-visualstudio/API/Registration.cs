using RestSharp;

namespace BlackDuckHub.VisualStudio.API
{
    public static class Registration
    {
        public static IRestResponse<Classes.Registration.RootObject> GetRegistration(RestClient client)
        {
            var getRegistration = new RestRequest("/api/v1/registrations");
            return client.Execute<Classes.Registration.RootObject>(getRegistration);
        }
    }
}
