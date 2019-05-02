using System.Net.Http;
using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public class FollowRequests
    {
        /// <summary>
        ///     Fetching a list of follow requests
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="Account" /> which have requested to follow the authenticated user</returns>
        public static async Task<MastodonList<Account>> Fetching(IMastodonCredentials credentials, string max_id = "",
            string since_id = "", int limit = 40)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Account>(
                $"{HttpHelper.HTTPS}{domain}{Constants.FollowRequestsFetching}", token, max_id, since_id,
                (nameof(limit), limit.ToString()));
        }

        /// <summary>
        ///     Authorizing follow requests
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">The id of the account to authorize or reject</param>
        /// <returns></returns>
        public static async Task Authorize(IMastodonCredentials credentials, long id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            await HttpHelper.Instance.PostAsync<HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.FollowRequestsAuthorize.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     Rejecting follow requests
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">The id of the account to authorize or reject</param>
        /// <returns></returns>
        public static async Task Reject(IMastodonCredentials credentials, long id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            await HttpHelper.Instance.PostAsync<HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.FollowRequestsReject.Id(id.ToString())}", token, null);
        }
    }
}