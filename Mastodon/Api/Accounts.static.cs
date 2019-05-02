using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public class Accounts
    {
		/// <summary>
		///     Fetching an account
		/// </summary>
		/// <param name="credentials">Mastodon credentials</param>
		/// <param name="id">Account Id</param>
		/// <returns>
		///     <see cref="Account" />
		/// </returns>
		public static async Task<Account> Fetching(IMastodonCredentials credentials, long id)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.GetAsync<Account>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsFetching.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     Getting the current user
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <returns></returns>
        public static async Task<Account> VerifyCredentials(IMastodonCredentials credentials)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.GetAsync<Account>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsVerifyCredentials}", token, null);
        }

        /// <summary>
        ///     Updating the current user
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="display_name">The name to display in the user's profile</param>
        /// <param name="note">A new biography for the user</param>
        /// <param name="avatar">Byte array of the image file</param>
        /// <param name="header">Byte array of the image file</param>
        /// <returns></returns>
        public static async Task UpdateCredentials(IMastodonCredentials credentials, string display_name, string note,
            byte[] avatar, byte[] header)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            await HttpHelper.Instance.PatchAsync($"{HttpHelper.HTTPS}{domain}{Constants.AccountsUpdateCredentials}", token,
                new (string, HttpContent)[]
                {
                    (nameof(display_name), new StringContent(display_name)),
                    (nameof(note), new StringContent(note)),
                    (nameof(avatar), new StreamContent(new MemoryStream(avatar))),
                    (nameof(header), new StreamContent(new MemoryStream(header)))
                });
        }

        /// <summary>
        ///     Getting an account's followers
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">Account Id</param>
        /// <param name="max_id">(optional) Get a list of followers with ID less than or equal this value</param>
        /// <param name="since_id">(optional) Get a list of followers with ID greater than this value</param>
        /// <param name="limit">(optional) Maximum number of accounts to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of <see cref="Account" /></returns>
        public static async Task<MastodonList<Account>> Followers(IMastodonCredentials credentials, long id, long max_id = 0,
            long since_id = 0, int limit = 40)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            if (limit > 80 || limit < 0)
                throw new ArgumentOutOfRangeException($"{nameof(limit)}");
            return await HttpHelper.Instance.GetListAsync<Account>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsFollowers.Id(id.ToString())}", token, param: new[]
                {
                    (nameof(max_id), max_id.ToString()),
                    (nameof(since_id), since_id.ToString()),
                    (nameof(limit), limit.ToString())
                });
        }

        /// <summary>
        ///     Getting who account is following
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">Account Id</param>
        /// <param name="max_id">(optional) Get a list of following with ID less than or equal this value</param>
        /// <param name="since_id">(optional) Get a list of following with ID greater than this value</param>
        /// <returns>Returns an array of <see cref="Account" /></returns>
        public static async Task<MastodonList<Account>> Following(IMastodonCredentials credentials, long id, long max_id = 0,
            long since_id = 0)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Account>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsFollowing.Id(id.ToString())}", token, max_id, since_id);
        }

        /// <summary>
        ///     Getting an account's statuses
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">Account Id</param>
        /// <param name="max_id">(optional) Get a list of Statuses with ID less than or equal this value</param>
        /// <param name="since_id">(optional) Get a list of Statuses with ID greater than this value</param>
        /// <param name="only_media">(optional) Only return statuses that have media attachments</param>
        /// <param name="exclude_replies">(optional) Skip statuses that reply to other statuses</param>
        /// <returns>Returns an array of <see cref="Status" /></returns>
        public static async Task<MastodonList<Status>> Statuses(IMastodonCredentials credentials, long id, long max_id = 0,
            long since_id = 0, bool only_media = false, bool exclude_replies = false, bool pinned = false,
            int limit = 20)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Status>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsStatuses.Id(id.ToString())}", token, param: new[]
                {
                    (nameof(max_id), max_id.ToString()),
                    (nameof(since_id), since_id.ToString()),
                    (nameof(only_media), only_media.ToString()),
                    (nameof(exclude_replies), exclude_replies.ToString()),
                    (nameof(pinned), pinned.ToString()),
                    (nameof(limit), limit.ToString())
                });
        }

        /// <summary>
        ///     Following an account
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">Account Id</param>
        /// <returns>Returns the target account's <see cref="Relationship" /></returns>
        public static async Task<Relationship> Follow(IMastodonCredentials credentials, long id)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Relationship, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsFollow.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     Unfollowing an account
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">Account Id</param>
        /// <returns>Returns the target account's <see cref="Relationship" /></returns>
        public static async Task<Relationship> UnFollow(IMastodonCredentials credentials, long id)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Relationship, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsUnFollow.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     Blocking an account
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">Account Id</param>
        /// <returns>Returns the target account's <see cref="Relationship" /></returns>
        public static async Task<Relationship> Block(IMastodonCredentials credentials, long id)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Relationship, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsBlock.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     Unblocking  an account
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">Account Id</param>
        /// <returns>Returns the target account's <see cref="Relationship" /></returns>
        public static async Task<Relationship> UnBlock(IMastodonCredentials credentials, long id)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Relationship, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsUnBlock.Id(id.ToString())}", token, null);
        }


        /// <summary>
        ///     Muting an account
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">Account Id</param>
        /// <returns>Returns the target account's <see cref="Relationship" /></returns>
        public static async Task<Relationship> Mute(IMastodonCredentials credentials, long id, bool notifications = true)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Relationship, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsMute.Id(id.ToString())}", token,
                (nameof(notifications), new StringContent(notifications.ToString())));
        }

        /// <summary>
        ///     Unmuting   an account
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">Account Id</param>
        /// <returns>Returns the target account's <see cref="Relationship" /></returns>
        public static async Task<Relationship> UnMute(IMastodonCredentials credentials, long id)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Relationship, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsUnMute.Id(id.ToString())}", token, null);
        }

        /// <summary>
        ///     Getting an account's relationships
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="id">(can be array) Account IDs</param>
        /// <returns>Returns an array of <see cref="Relationship" /> of the current user to a list of given accounts</returns>
        public static async Task<MastodonList<Relationship>> Relationships(IMastodonCredentials credentials,
            params long[] id)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Relationship>(
                $"{HttpHelper.HTTPS}{domain}{Constants.AccountsRelationships}", token,
                param: HttpHelper.Instance.ArrayEncode(nameof(id), id.Select(v => v.ToString()).ToArray()).ToArray());
        }

        /// <summary>
        ///     Searching for accounts
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="q">What to search for</param>
        /// <param name="limit">Maximum number of matching accounts to return (default: 40)</param>
        /// <returns>
        ///     Returns an array of matching <see cref="Account" />. Will lookup an account remotely if the search term
        ///     is in the username@domain format and not yet in the database
        /// </returns>
        public static async Task<MastodonList<Account>> Search(IMastodonCredentials credentials, string q, int limit = 40,
            bool following = false)
        {
			string domain = credentials.Domain;
			string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<Account>($"{HttpHelper.HTTPS}{domain}{Constants.AccountsSearch}",
                token, param: new[]
                {
                    (nameof(q), q),
                    (nameof(limit), limit.ToString()),
                    (nameof(following), following.ToString())
                });
        }
    }
}