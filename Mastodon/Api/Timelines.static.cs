using System.Threading.Tasks;
using Maplesharp.Common;
using Maplesharp.Model;

namespace Maplesharp.Api
{
    /// <summary>
    ///     Retrieving a timeline
    /// </summary>
    public static class Timelines
    {
        public static async Task<MastodonList<Status>> Home(IMastodonCredentials credentials, string max_id = "",
            string since_id = "", bool only_media = false, int limit = 20)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Status>($"{HttpHelper.HTTPS}{domain}{Constants.TimelineHome}", token,
                max_id, since_id,
                (nameof(only_media), only_media.ToString()),
                (nameof(limit), limit.ToString()));
        }

        public static async Task<MastodonList<Status>> Public(string domain, string max_id = "", string since_id = "",
            bool local = false, bool only_media = false, int limit = 20)
        {
            return await HttpHelper.Instance.GetListAsync<Status>($"{HttpHelper.HTTPS}{domain}{Constants.TimelinePublic}",
                string.Empty, max_id, since_id, (nameof(local), local.ToString()),
                (nameof(only_media), only_media.ToString()),
                (nameof(limit), limit.ToString()));
        }

        public static async Task<MastodonList<Status>> HashTag(string domain, string hashtag, string max_id = "",
            string since_id = "", bool local = false, bool only_media = false, int limit = 20)
        {
            return await HttpHelper.Instance.GetListAsync<Status>(
                $"{HttpHelper.HTTPS}{domain}{Constants.TimelineTag.Id(hashtag)}", string.Empty, max_id, since_id,
                (nameof(local), local.ToString()),
                (nameof(only_media), only_media.ToString()),
                (nameof(limit), limit.ToString()));
        }
        
        public static async Task<MastodonList<Status>> List(IMastodonCredentials credentials, string id, string max_id = "",
            string since_id = "", bool only_media = false, int limit = 20)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Status>($"{HttpHelper.HTTPS}{domain}{Constants.TimelineList.Id(id)}", token,
                max_id, since_id,
                (nameof(only_media), only_media.ToString()),
                (nameof(limit), limit.ToString()));
        }
    }
}