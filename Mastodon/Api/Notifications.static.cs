using System.Net.Http;
using System.Threading.Tasks;
using Maplesharp.Common;
using Maplesharp.Model;

namespace Maplesharp.Api
{
    public class Notifications
    {
        /// <summary>
        ///     Fetching a user's notifications
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns a list of <see cref="Notification" /> for the authenticated user</returns>
        public static async Task<MastodonList<Notification>> Fetching(IMastodonCredentials credentials, string max_id = "",
            string since_id = "", int limit = 15, NotificationType? exclude_types = null)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Notification>(
                $"{HttpHelper.HTTPS}{domain}{Constants.NotificationsFetching}", token, max_id, since_id,
                (nameof(limit), limit.ToString()),
                (nameof(exclude_types), exclude_types?.ToString("F")?.ToLower()));
        }

        /// <summary>
        ///     Getting a single notification
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the <see cref="Notification" />.</returns>
        public static async Task<Notification> GetSingle(IMastodonCredentials credentials, long id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetAsync<Notification>(
                $"{HttpHelper.HTTPS}{domain}{Constants.NotificationsSingle.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     Clearing notifications
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <returns>Deletes all notifications from the Mastodon server for the authenticated user. Returns an empty object.</returns>
        public static async Task Clear(IMastodonCredentials credentials)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            await HttpHelper.Instance.PostAsync<HttpContent>($"{HttpHelper.HTTPS}{domain}{Constants.NotificationsClear}", token,
                null);
        }

        public static async Task Dismiss(IMastodonCredentials credentials, long id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            await HttpHelper.Instance.PostAsync($"{HttpHelper.HTTPS}{domain}{Constants.NotificationsDismiss}", token,
                (nameof(id), id));
        }
    }
}