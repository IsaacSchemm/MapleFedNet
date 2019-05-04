using System.Collections.Generic;
using Newtonsoft.Json;

namespace Maplesharp.Model
{
    public class Context
    {
        /// <summary>
        ///     The ancestors of the status in the conversation
        /// </summary>
        [JsonProperty("ancestors")]
        public IEnumerable<Status> Ancestors { get; set; }

        /// <summary>
        ///     The descendants of the status in the conversation
        /// </summary>
        [JsonProperty("descendants")]
        public IEnumerable<Status> Descendants { get; set; }
    }
}