# MapleFedNet
MapleFedNet is a .NET Standard wrapper for the [Mastodon](https://github.com/tootsuite/mastodon) API.

MapleFedNet is based on [Mastodon.Net](https://github.com/Tlaster/Mastodon.Net), but with a few major changes:

* Functions that take parameters for both domain and access token now combine
  them into one parameter of type `IMastodonCredentials`
    * A `MastodonCredentials` immutable class is available if you don't want to create your own
* Post IDs are now treated as alphanumeric strings (for Pleroma compatibility)
* `params T[]` parameters and optional parameters are no longer used in the same methods

# Sample

```
var domain = "mstdn.jp";
var clientName = "MapleFedNet";
var userName = "";
var password = "";

var oauth = await Apps.Register(domain, clientName, scopes: new[] {Scope.Follow, Scope.Read, Scope.Write});
var token = await OAuth.GetAccessTokenByPassword(domain, oauth.ClientId, oauth.ClientSecret, userName, password, Scope.Follow, Scope.Read, Scope.Write);

var credentials = new MastodonCredentials(domain, token.AccessToken);
var timeline = await Timelines.Home(credentials);
var notify = await Notifications.Fetching(credentials);
var toot = await Statuses.Posting(credentials, "Toot!");
```

# License
```
MIT License

Copyright (c) 2017 Tlaster
Copyright (c) 2019-2020 Isaac Schemm

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
