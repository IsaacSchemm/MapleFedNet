using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public class Mutes
    {
        /// <summary>
        ///     Fetching a user's mutes
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="Account" /> muted by the authenticated user</returns>
        public static async Task<MastodonList<Account>> Fetching(IMastodonCredentials credentials, string max_id = "",
            string since_id = "", int limit = 40)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Account>($"{HttpHelper.HTTPS}{domain}{Constants.MutesFetching}",
                token, max_id, since_id, (nameof(limit), limit.ToString()));
        }
    }
}