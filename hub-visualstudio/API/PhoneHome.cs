using RestSharp;

namespace BlackDuckHub.VisualStudio.API
{
    public static class PhoneHome
    {
        public static void SendPhoneHome(Classes.PhoneHome.RootObject phoneHome)
        {
            var collectClient = new RestClient("https://collect.blackducksoftware.com");

            var phoneHomeRequest = new RestRequest(Method.POST);
            phoneHomeRequest.RequestFormat = DataFormat.Json;
            phoneHomeRequest.AddBody(phoneHome);

            collectClient.Execute(phoneHomeRequest);
        }
    }
}
