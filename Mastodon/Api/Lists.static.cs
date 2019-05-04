using System.Threading.Tasks;
using Maplesharp.Common;
using Maplesharp.Model;

namespace Maplesharp.Api
{
    public static class Lists
    {
        public static async Task<MastodonList<List>> GetLists(IMastodonCredentials credentials)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<List>($"{HttpHelper.HTTPS}{domain}{Constants.List}", token);
        }

        public static async Task<MastodonList<List>> ListsByMembership(IMastodonCredentials credentials)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<List>($"{HttpHelper.HTTPS}{domain}{Constants.ListsByMembership}",
                token);
        }

        public static async Task<MastodonList<List>> AccountsInList(IMastodonCredentials credentials)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetListAsync<List>($"{HttpHelper.HTTPS}{domain}{Constants.AccountsInList}", token);
        }

        public static async Task<List> ListById(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.GetAsync<List>($"{HttpHelper.HTTPS}{domain}{Constants.ListById}", token,
                param: (nameof(id), id));
        }

        public static async Task<List> CreateList(IMastodonCredentials credentials, string title)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PostAsync<List, string>($"{HttpHelper.HTTPS}{domain}{Constants.List}", token,
                (nameof(title), title));
        }

        public static async Task<List> UpdateList(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.PutAsync<List, string>($"{HttpHelper.HTTPS}{domain}{Constants.ListById}", token,
                param: (nameof(id), id));
        }
        
        public static async Task<List> DeleteList(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            return await HttpHelper.Instance.DeleteAsync<List, string>($"{HttpHelper.HTTPS}{domain}{Constants.ListById}", token,
                param: (nameof(id), id));
        }

        public static async Task AddAccount(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            await HttpHelper.Instance.PostAsync($"{HttpHelper.HTTPS}{domain}{Constants.AccountsInList}", token,
                (nameof(id), id));
        }
        
        public static async Task RemoveAccount(IMastodonCredentials credentials, string id)
        {
            string domain = credentials.Domain;
            string token = credentials.Token;
            await HttpHelper.Instance.DeleteAsync($"{HttpHelper.HTTPS}{domain}{Constants.AccountsInList}", token,
                (nameof(id), id));
        }
        
    }
}