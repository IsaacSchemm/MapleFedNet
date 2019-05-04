using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Maplesharp.Common;
using Maplesharp.Model;

namespace Maplesharp.Api
{
    public class Media
    {
        /// <summary>
        ///     Uploading a media attachment
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="file">Media to be uploaded</param>
        /// <returns>Returns an <see cref="Attachment" /> that can be used when creating a status</returns>
        public static async Task<Attachment> Uploading(IMastodonCredentials credentials, byte[] file,
            string description = null)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Attachment, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.MediaUploading}", token,
                (nameof(file), new StreamContent(new MemoryStream(file))),
                (nameof(description), new StringContent(description)));
        }
    }
}