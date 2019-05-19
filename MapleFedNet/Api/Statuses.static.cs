using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MapleFedNet.Common;
using MapleFedNet.Model;

namespace MapleFedNet.Api
{
    public class Statuses
    {
        /// <summary>
        ///     Fetching a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="id"></param>
        /// <returns>Returns a <see cref="Status" /></returns>
        public static async Task<Status> Fetching(string domain, string id)
        {
            return await HttpHelper.Instance.GetAsync<Status>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesFetching.Id(id.ToString())}", string.Empty, null);
        }

        /// <summary>
        ///     Getting status context
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="id"></param>
        /// <returns>Returns a <see cref="Context" /></returns>
        public static async Task<Context> Context(string domain, string id)
        {
            return await HttpHelper.Instance.GetAsync<Context>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesContext.Id(id.ToString())}", string.Empty, null);
        }

        /// <summary>
        ///     Getting a card associated with a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="id"></param>
        /// <returns>Returns a <see cref="Card" /></returns>
        public static async Task<Card> Card(string domain, string id)
        {
            return await HttpHelper.Instance.GetAsync<Card>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesCard.Id(id.ToString())}", string.Empty, null);
        }

        /// <summary>
        ///     Getting who reblogged a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="id"></param>
        /// <returns>Returns an array of <see cref="Account" /></returns>
        public static async Task<MastodonList<Account>> RebloggedBy(string domain, string id, string max_id = "",
            string since_id = "", int limit = 40)
        {
            return await HttpHelper.Instance.GetListAsync<Account>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesRebloggedBy.Id(id.ToString())}", string.Empty, max_id, since_id,
                (nameof(limit), limit.ToString()));
        }

        /// <summary>
        ///     Getting who favourited a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="id"></param>
        /// <returns>Returns an array of <see cref="Account" /></returns>
        public static async Task<MastodonList<Account>> FavouritedBy(string domain, string id, string max_id = "",
            string since_id = "", int limit = 40)
        {
            return await HttpHelper.Instance.GetListAsync<Account>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesFavouritedBy.Id(id.ToString())}", string.Empty, max_id, since_id,
                (nameof(limit), limit.ToString()));
        }

        /// <summary>
        ///     Posting a new status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="status">The text of the status</param>
        /// <param name="in_reply_to_id">(optional) local ID of the status you want to reply to</param>
        /// <param name="sensitive"> (optional) set this to mark the media of the status as NSFW</param>
        /// <param name="spoiler_text">(optional) text to be shown as a warning before the actual content</param>
        /// <param name="visibility">
        ///     (optional) either  <see cref="Visibility.Public" />,
        ///     <see cref="Visibility.Unlisted" />, <see cref="Visibility.Private" />,
        ///     <see cref="Visibility.Direct" />
        /// </param>
        /// <param name="media_ids">(optional) array of media IDs to attach to the status (maximum 4)</param>
        /// <returns></returns>
        public static async Task<Status> Posting(IMastodonCredentials credentials, string status, int in_reply_to_id = 0,
            bool sensitive = false, string spoiler_text = "", Visibility visibility = Visibility.Public,
            IEnumerable<int> media_ids = null)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            if (media_ids != null && media_ids.Count() > 4) throw new ArgumentOutOfRangeException(nameof(media_ids));
            ICollection<(string, string)> param;
            if (media_ids != null && media_ids.Any())
                param = HttpHelper.Instance.ArrayEncode(nameof(media_ids), media_ids.Select(v => v.ToString()).ToArray())
                    .ToList();
            else
                param = new List<(string, string)>();
            param.Add((nameof(status), status));
            param.Add((nameof(in_reply_to_id), in_reply_to_id.ToString()));
            param.Add((nameof(sensitive), sensitive.ToString()));
            param.Add((nameof(spoiler_text), spoiler_text));
            param.Add((nameof(visibility), visibility.ToString("F").ToLowerInvariant()));
            return await HttpHelper.Instance.PostAsync<Status, string>($"{HttpHelper.HTTPS}{domain}{Constants.StatusesPost}",
                token, param.ToArray());
        }

        /// <summary>
        ///     Deleting a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task Delete(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            await HttpHelper.Instance.DeleteAsync<HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesDelete.Id(id.ToString())}",
                token);
        }

        /// <summary>
        ///     Reblogging a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="Status" /></returns>
        public static async Task<Status> Reblog(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Status, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesReblog.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     UnReblogging a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="Status" /></returns>
        public static async Task<Status> UnReblog(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Status, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesUnReblog.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     Favouriting a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="Status" /></returns>
        public static async Task<Status> Favourite(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Status, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesFavourite.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     UnFavouriting a status
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>Returns the target <see cref="Status" /></returns>
        public static async Task<Status> UnFavourite(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Status, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesUnFavourite.Id(id.ToString())}", token, null);
        }
        
        
        public static async Task<Status> Pin(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Status, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesPin.Id(id.ToString())}", token, null);
        }

        public static async Task<Status> UnPin(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Status, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesUnpin.Id(id.ToString())}", token, null);
        }
        
        public static async Task<Status> Mute(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Status, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesMute.Id(id.ToString())}", token, null);
        }

        public static async Task<Status> UnMute(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Status, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.StatusesUnmute.Id(id.ToString())}", token, null);
        }
    }
}