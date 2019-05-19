using Newtonsoft.Json;

namespace MapleFedNet.Model
{
    public class Error
    {
        /// <summary>
        ///     A textual description of the error
        /// </summary>
        [JsonProperty("error")]
        public string Description { get; set; }
    }
}