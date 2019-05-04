using System.Net.Http;
using System.Threading.Tasks;
using Maplesharp.Common;
using Maplesharp.Model;

namespace Maplesharp.Api
{
    public class Blocks
    {
        /// <summary>
        ///     Fetching a user's blocks
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="Account" /> blocked by the authenticated user</returns>
        public static async Task<MastodonList<Account>> Fetching(IMastodonCredentials credentials, string max_id = "",
            string since_id = "", int limit = 40)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Account>($"{HttpHelper.HTTPS}{domain}{Constants.BlocksFetching}",
                token, max_id, since_id, (nameof(limit), limit.ToString()));
        }

        public static async Task<MastodonList<string>> Domain(IMastodonCredentials credentials, string max_id = "",
			string since_id = "", int limit = 40)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<string>($"{HttpHelper.HTTPS}{domain}{Constants.BlocksDomain}",
                token, max_id, since_id, (nameof(limit), limit.ToString()));
        }

        public static async Task BlockDomain(IMastodonCredentials credentials, string domainToBlock)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            await HttpHelper.Instance.PostAsync($"{HttpHelper.HTTPS}{domain}{Constants.BlocksDomain}", token,
                ("domain", domainToBlock));
        }

        public static async Task UnblockDomain(IMastodonCredentials credentials, string domainToUnblock)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            await HttpHelper.Instance.DeleteAsync($"{HttpHelper.HTTPS}{domain}{Constants.BlocksDomain}", token,
                ("domain", new StringContent(domainToUnblock)));
        }
    }
}