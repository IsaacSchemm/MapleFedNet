using Newtonsoft.Json;

namespace Maplesharp.Model
{
    public class List
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}