using System.Threading.Tasks;
using Maplesharp.Common;
using Maplesharp.Model;

namespace Maplesharp.Api
{
    public static class Instances
    {
        public static async Task<Instance> Instance(string domain)
        {
            return await HttpHelper.Instance.GetAsync<Instance>($"{HttpHelper.HTTPS}{domain}{Constants.Instance}", string.Empty,
                null);
        }

        public static async Task<MastodonList<Emoji>> CustomEmojis(string domain)
        {
            return await HttpHelper.Instance.GetListAsync<Emoji>($"{HttpHelper.HTTPS}{domain}{Constants.CustomEmojis}",
                string.Empty);
        }
        
        
    }
}