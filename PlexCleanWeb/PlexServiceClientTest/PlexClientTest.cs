using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Xunit.Abstractions;

namespace PlexServiceClientTest
{
    using PlexServiceClient;

    using Xunit;

    public partial class PlexClientTest
    {
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// run these commands in project folder to define credentials in secret store
        /// dotnet user-secrets set plexuser "username"
        /// dotnet user-secrets set plexpass "password"
        /// dotnet user-secrets set plexurl "url", for example http://192.168.1.158:32400
        /// </summary>
        private readonly string _plexuser;
        private readonly string _plexpass;
        private readonly string _plexurl;


        IConfiguration Configuration { get; }

        public PlexClientTest(ITestOutputHelper output)
        {
            _output = output;

            var builder = new ConfigurationBuilder()
                .AddUserSecrets<PlexClientTest>();

            Configuration = builder.Build();

            _plexuser = Configuration["plexuser"];
            _plexpass = Configuration["plexpass"];
            _plexurl = Configuration["plexurl"];
        }

        [Fact]
        public void GetAccessToken_ShouldHandleIncorrectCredentials()
        {
            var plexClient = new PlexClient();
            var response = plexClient.GetAccessToken("user", "password");

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [SkippableFact]
        public void GetAccessToken_ShouldHandleCorrectCredentials()
        {
            Skip.If(_plexuser == null || _plexpass == null, "Define username and password in secrets!");

            var plexClient = new PlexClient();
            var response = plexClient.GetAccessToken(_plexuser, _plexpass);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            _output.WriteLine($"authentication_token: {response.Data.user.authentication_token}");
            _output.WriteLine(Environment.NewLine);
            _output.WriteLine(JsonConvert.SerializeObject(response.Data, Formatting.Indented));
        }

        private string GetAccessToken()
        {
            Skip.If(_plexuser == null || _plexpass == null, "Define username and password in secrets!");

            var plexClient = new PlexClient();
            var response = plexClient.GetAccessToken(_plexuser, _plexpass);
            return response.Data.user.authentication_token;
        }

        [SkippableFact]
        public void LibrarySections_Works()
        {
            Skip.If(_plexurl == null, "Define plexurl in secrets!");

            var client = new RestClient($"{_plexurl}/library/sections");

            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");

            request.AddHeader("X-Plex-Token", GetAccessToken());

            var response = client.Execute(request);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            _output.WriteLine(response.Content.PrettyPringJsonString());
        }
    }
}
