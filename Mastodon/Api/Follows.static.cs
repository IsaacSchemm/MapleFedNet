using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public class Follows
    {
        /// <summary>
        ///     Following a remote user
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="uri">username@domain of the person you want to follow</param>
        /// <returns>Returns the local representation of the followed account, as an <see cref="Account" /></returns>
        public static async Task<Account> Following(IMastodonCredentials credentials, string uri)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Account, string>(
                $"{HttpHelper.HTTPS}{domain}{Constants.FollowsFollowing}", token, (nameof(uri), uri));
        }
    }
}