using System.Collections.Generic;

namespace Mastodon.Model
{
    public class MastodonList<T> : List<T>
    {
        public MastodonList()
        {
        }

        public MastodonList(IEnumerable<T> collection) : base(collection)
        {
        }

        public MastodonList(int capacity) : base(capacity)
        {
        }

        public string MaxId { get; internal set; }
        public string SinceId { get; internal set; }
    }
}