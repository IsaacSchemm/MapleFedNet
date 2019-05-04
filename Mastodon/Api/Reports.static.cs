using System.Linq;
using System.Threading.Tasks;
using Maplesharp.Common;
using Maplesharp.Model;

namespace Maplesharp.Api
{
    public class Reports
    {
        /// <summary>
        ///     Fetching a user's reports
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns a list of <see cref="Report" /> made by the authenticated user</returns>
        public static async Task<MastodonList<Report>> Fetching(IMastodonCredentials credentials, string max_id = "",
            string since_id = "")
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Report>($"{HttpHelper.HTTPS}{domain}{Constants.ReportsFetching}",
                token, max_id, since_id);
        }

        /// <summary>
        ///     Reporting a user
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="account_id">The ID of the account to report</param>
        /// <param name="comment">A comment to associate with the report</param>
        /// <param name="status_ids">The IDs of statuses to report (can be an array)</param>
        /// <returns>Returns the finished <see cref="Report" />.</returns>
        public static async Task<Report> Reporting(IMastodonCredentials credentials, long account_id, string comment,
            params string[] status_ids)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            var param = HttpHelper.Instance.ArrayEncode(nameof(status_ids), status_ids.ToArray())
                .ToList();
            param.Add((nameof(account_id), account_id.ToString()));
            param.Add((nameof(comment), comment));
            return await HttpHelper.Instance.PostAsync<Report, string>($"{HttpHelper.HTTPS}{domain}{Constants.ReportsReporting}",
                token, param.ToArray());
        }
    }
}