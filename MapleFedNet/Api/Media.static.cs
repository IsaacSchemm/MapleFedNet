using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MapleFedNet.Common;
using MapleFedNet.Model;

namespace MapleFedNet.Api
{
    public class Media
    {
        /// <summary>
        ///     Uploading a media attachment
        /// </summary>
        /// <param name="credentials">Mastodon credentials</param>
        /// <param name="file">Media to be uploaded</param>
        /// <param name="description">A plain-text description of the media for accessibility (max 420 chars)</param>
        /// <param name="focus">Two floating points used for intelligent cropping</param>
        /// <returns>Returns an <see cref="Attachment" /> that can be used when creating a status</returns>
        public static async Task<Attachment> Uploading(
            IMastodonCredentials credentials,
            byte[] file,
            string description = null,
            (double, double)? focus = null)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<Attachment, HttpContent>(
                $"{HttpHelper.HTTPS}{domain}{Constants.MediaUploading}", token,
                (nameof(file), new StreamContent(new MemoryStream(file))),
                (nameof(description), new StringContent(description)),
                (nameof(focus), focus == null ? null : new StringContent($"{focus.Value.Item1},{focus.Value.Item2}")));
        }
    }
}