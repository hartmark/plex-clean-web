using System;
using System.Diagnostics;
using Microsoft.Win32;
using PlexServiceClient.DataContract;

namespace PlexServiceClient
{
    using RestSharp;
    using RestSharp.Authenticators;

    public class PlexClient
    {
        public IRestResponse<GetAccessToken> GetAccessToken(string username, string password)
        {
            var client = new RestClient("https://plex.tv/users/sign_in.json");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");

            request.AddHeader("X-Plex-Device-Name", "Plex Web (clean)");
            request.AddHeader("X-Plex-Version", "pre alpha: 0.0.1");
            request.AddHeader("X-Plex-Product", "Plex Web");

            request.AddHeader("X-Plex-Device", $"{OsName}");
            request.AddHeader("X-Plex-Client-Identifier", "clean plex client");

            client.Authenticator = new HttpBasicAuthenticator(username, password);

            var response = client.Execute<GetAccessToken>(request);
            return response;
        }

        private static string OsName
        {
            get
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
                Debug.Assert(registryKey != null, nameof(registryKey) + " != null");
                var osName = (string)registryKey.GetValue("productName");
                return osName;
            }
        }
    }
}
