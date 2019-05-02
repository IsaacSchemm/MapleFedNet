using System;
using System.Collections.Generic;
using System.Text;

namespace Mastodon.Common {
	public interface IMastodonCredentials {
		string Domain { get; }
		string Token { get; }
	}

	public class MastodonCredentials : IMastodonCredentials {
		public string Domain { get; }
		public string Token { get; }

		public MastodonCredentials(string domain, string token) {
			Domain = domain ?? throw new ArgumentNullException(nameof(domain));
			Token = token ?? throw new ArgumentNullException(nameof(token));
		}
	}
}
