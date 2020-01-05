using System;
using System.Collections.Generic;

namespace MapleFedNet.Common {
	public interface IMastodonCredentials {
		string Domain { get; }
		string Token { get; }
	}

	public class MastodonCredentials : IMastodonCredentials, IEquatable<MastodonCredentials> {
		public string Domain { get; }
		public string Token { get; }

		public MastodonCredentials(string domain, string token) {
			Domain = domain ?? throw new ArgumentNullException(nameof(domain));
			Token = token ?? throw new ArgumentNullException(nameof(token));
		}

		public override bool Equals(object obj) {
			return Equals(obj as MastodonCredentials);
		}

		public bool Equals(MastodonCredentials other) {
			return other != null &&
				   Domain == other.Domain &&
				   Token == other.Token;
		}

		public override int GetHashCode() {
			var hashCode = -1645284213;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Domain);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Token);
			return hashCode;
		}

		public static bool operator ==(MastodonCredentials left, MastodonCredentials right) {
			return EqualityComparer<MastodonCredentials>.Default.Equals(left, right);
		}

		public static bool operator !=(MastodonCredentials left, MastodonCredentials right) {
			return !(left == right);
		}
	}
}
