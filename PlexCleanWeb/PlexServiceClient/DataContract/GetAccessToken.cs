using System;
using System.Collections.Generic;

namespace PlexServiceClient.DataContract
{
    public class Subscription
    {
        public bool active { get; set; }
        public string status { get; set; }
        public object plan { get; set; }
        public List<string> features { get; set; }
    }

    public class Roles
    {
        public List<object> roles { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public string email { get; set; }
        public DateTime joined_at { get; set; }
        public string username { get; set; }
        public string title { get; set; }
        public string thumb { get; set; }
        public bool hasPassword { get; set; }
        public string authToken { get; set; }
        public string authentication_token { get; set; }
        public Subscription subscription { get; set; }
        public Roles roles { get; set; }
        public List<object> entitlements { get; set; }
        public DateTime confirmedAt { get; set; }
        public int forumId { get; set; }
        public bool rememberMe { get; set; }
    }

    public class GetAccessToken
    {
        public User user { get; set; }
    }
}