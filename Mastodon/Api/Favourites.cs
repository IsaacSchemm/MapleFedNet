﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mastodon.Model;

namespace Mastodon.Api
{
    public partial class Favourites : Base
    {
        /// <summary>
        /// Fetching a user's favourites
        /// </summary>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <returns>Returns an array of <see cref="StatusModel"/> favourited by the authenticated user</returns>
        public async Task<ArrayModel<StatusModel>> Fetching(int max_id = 0, int since_id = 0)
        {
            return await Fetching(Domain, AccessToken, max_id, since_id);
        }

        public Favourites(string domain, string accessToken) : base(domain, accessToken)
        {
        }
    }
}
