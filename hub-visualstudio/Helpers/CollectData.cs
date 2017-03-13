using BlackDuckHub.VisualStudio.API;
using RestSharp;
using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace BlackDuckHub.VisualStudio.Helpers
{
    public static class CollectData
    {
        public static async void CallHome(RestClient client, string hubServerUrl, string hubVersion)
        {
            try
            {
                var getRegistrationResponse = API.Registration.GetRegistration(client);

                var regId = getRegistrationResponse.Data.registrationId;

                if (regId == null)
                {
                    regId = MD5Hash(hubServerUrl);
                }

                switch (hubVersion)
                {
                    case "14.0":
                        hubVersion = "2015";
                        break;
                    case "15.0":
                        hubVersion = "2017";
                        break;
                    default:
                        hubVersion = "Unknown";
                        break;
                }


                Classes.PhoneHome.RootObject phoneHome = new Classes.PhoneHome.RootObject();
                phoneHome.regId = regId;
                phoneHome.source = "Integrations";

                Classes.PhoneHome.InfoMap infoMap = new Classes.PhoneHome.InfoMap();

                infoMap.blackDuckName = "Hub";
                infoMap.blackDuckVersion = HubVersion.GetHubVersion(client).Replace('"', ' ').Trim(); ;
                infoMap.thirdPartyName = "Visual-Studio";
                infoMap.thirdPartyVersion = hubVersion;
                infoMap.pluginVersion = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                                                   Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "." +
                                                    Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();

                phoneHome.infoMap = infoMap;

                API.PhoneHome.SendPhoneHome(phoneHome);

            }
            catch (Exception ex)
            {
                //Do nothing
            }
        }

        private static string MD5Hash(string input)
        {
            //https://coderwall.com/p/4puszg/c-convert-string-to-md5-hash
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
