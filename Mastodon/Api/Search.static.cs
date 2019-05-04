using System.Threading.Tasks;
using Maplesharp.Common;
using Maplesharp.Model;

namespace Maplesharp.Api
{
    public class Search
    {
        /// <summary>
        ///     Searching for content
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="q">The search query</param>
        /// <param name="resolve">Whether to resolve non-local accounts</param>
        /// <returns>
        ///     Returns <see cref="Results" />. If <see cref="q" /> is a URL, Mastodon will attempt to fetch the provided
        ///     account or status. Otherwise, it will do a local account and hashtag search
        /// </returns>
        public static async Task<Results> Searching(string domain, string q, bool resolve = false)
        {
            return await HttpHelper.Instance.GetAsync<Results>($"{HttpHelper.HTTPS}{domain}{Constants.Search}", string.Empty, (nameof(q), q), (nameof(resolve), resolve.ToString()));
        }
    }
}