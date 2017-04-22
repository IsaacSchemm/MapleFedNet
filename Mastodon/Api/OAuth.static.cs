﻿using Mastodon.Common;
using Mastodon.Model.OAuth;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mastodon.Api
{
    public partial class OAuth
    {
        public static async Task<TokenModel> GetAccessTokenByCode(string domain, string client_id, string client_secret, string redirect_uri, string code)
        {
            return await HttpHelper.PostAsync<TokenModel, string>($"{HttpHelper.HTTPS}{domain}{Constants.OAuthToken}", null, new []
            {
                ( nameof(client_id), client_id ),
                ( nameof(client_secret), client_secret ),
                ( nameof(redirect_uri), redirect_uri ),
                ( "grant_type", "authorization_code" ),
                ( nameof(code), code)
            });
        }
        public static async Task<TokenModel> GetAccessTokenByPassword(string domain, string client_id, string client_secret, string redirect_uri, string username, string password)
        {
            return await HttpHelper.PostAsync<TokenModel, string>($"{HttpHelper.HTTPS}{domain}{Constants.OAuthToken}", null, new[]
            {
                ( nameof(client_id), client_id ),
                ( nameof(client_secret), client_secret ),
                ( nameof(redirect_uri), redirect_uri ),
                ( "grant_type", "password" ),
                ( nameof(username), username ),
                ( nameof(password), password )
            });
        }
    }
}
