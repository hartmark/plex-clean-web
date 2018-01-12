using Newtonsoft.Json;

namespace PlexServiceClientTest
{
    public static class StringExtensions
    {
        public static string PrettyPringJsonString(this string uglyJsonString)
        {
            var jsonObject = JsonConvert.DeserializeObject(uglyJsonString);
            return JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        }
    }
}
