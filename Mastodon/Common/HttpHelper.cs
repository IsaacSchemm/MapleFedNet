﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Maplesharp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Maplesharp
{
    public static class MastodonApi
    {
        public static MastodonApiConfigure ApiConfigure { get; } = new MastodonApiConfigure();

        public static void Configure(Action<MastodonApiConfigure> config)
        {
            config?.Invoke(ApiConfigure);
        }
    }

    public class MastodonApiConfigure
    {
        public HttpMessageHandler HttpMessageHandler { get; set; } = new HttpClientHandler
        {
            MaxConnectionsPerServer = 20
        };
    }
}

namespace Maplesharp.Common
{
    internal class HttpHelper
    {
        public const string HTTPS = "https://";
        public const string HTTP = "http://";

        private HttpHelper()
        {
        }

        public static HttpHelper Instance { get; } = new HttpHelper();

        private string UrlEncode(string url, IEnumerable<(string Key, string Value)> param)
        {
            return param != null
                ? $"{url}?{string.Join("&", param.Where(CheckForValue).Select(kvp => $"{kvp.Key}={kvp.Value}"))}"
                : url;
        }

        private bool CheckForValue<T>((string Key, T Value) kvp)
        {
            return !string.IsNullOrEmpty(kvp.Value?.ToString()) &&
                   (!int.TryParse(kvp.Value?.ToString(), out var intValue) || intValue > 0) &&
                   (!bool.TryParse(kvp.Value?.ToString(), out var boolValue) || boolValue);
        }

        public IEnumerable<(string, T)> ArrayEncode<T>(string paramName, params T[] values)
        {
            paramName = $"{paramName}[]";
            return values.Select(value => (paramName, value));
        }

        private HttpClient GetHttpClient(string token, string tokenType = "Bearer")
        {
            return string.IsNullOrEmpty(token)
                ? new HttpClient()
                : new HttpClient
                {
                    DefaultRequestHeaders = {Authorization = new AuthenticationHeaderValue(tokenType, token)}
                };
        }

        public async Task<string> GetAsync(string url, string token,
            params (string Key, string Value)[] param)
        {
            using (var client = GetHttpClient(token))
            {
                return CheckForError(await client.GetStringAsync(UrlEncode(url, param)));
            }
        }

        public async Task<T> GetAsync<T>(string url, string token, params (string Key, string Value)[] param)
        {
            return JsonConvert.DeserializeObject<T>(await GetAsync(url, token, param));
        }

//        public static async Task<MastodonList<T>> GetListAsync<T>(string url, string token, string max_id = "",
//            string since_id = "", params (string Key, string Value)[] param)
//        {
//            var p = new List<(string Key, string Value)>
//            {
//                (nameof(max_id), max_id.ToString()),
//                (nameof(since_id), since_id.ToString())
//            };
//            if (param != null)
//                p.AddRange(param);
//            return await GetListAsync<T>(url, token, p);
//        }

        public async Task<MastodonList<T>> GetListAsync<T>(string url, string token,
            string max_id = "",
			string since_id = "",
            params (string Key, string Value)[] param)
        {
            var p = new List<(string Key, string Value)>
            {
                (nameof(max_id), max_id.ToString()),
                (nameof(since_id), since_id.ToString())
            };
            param = param != null ? param.Concat(p).ToArray() : p.ToArray();
            using (var client = GetHttpClient(token))
            using (var res = await client.GetAsync(UrlEncode(url, param)))
            {
                if (!res.Headers.TryGetValues("Link", out var values))
                    return new MastodonList<T>(
                        JsonConvert.DeserializeObject<List<T>>(await res.Content.ReadAsStringAsync()))
                    {
                        MaxId = "",
                        SinceId = ""
					};
                var links = values.FirstOrDefault().Split(',').Select(s =>
                    Regex.Match(s, "<.*(max_id|since_id)=([0-9A-Za-z]*)(.*)>; rel=\"(.*)\"").Groups).ToList();
                return new MastodonList<T>(
                    JsonConvert.DeserializeObject<List<T>>(await res.Content.ReadAsStringAsync()))
                {
                    MaxId = links.FirstOrDefault(m => m[1].Value == "max_id")?[2]?.Value,
                    SinceId = links.FirstOrDefault(m => m[1].Value == "since_id")?[2]?.Value
				};
            }
        }

        public async Task<T> PostAsync<T, TValue>(string url, string token,
            params (string Key, TValue Value)[] param)
        {
            return JsonConvert.DeserializeObject<T>(await PostAsync(url, token, param));
        }

        public async Task<string> PostAsync<TValue>(string url, string token,
            params (string Key, TValue Value)[] param)
        {
            return await HttpMethodAsync(url, token, HttpMethod.Post, param);
        }

        public async Task<T> PutAsync<T, TValue>(string url, string token,
            params (string Key, TValue Value)[] param)
        {
            return JsonConvert.DeserializeObject<T>(await PutAsync(url, token, param));
        }

        public async Task<string> PutAsync<TValue>(string url, string token,
            params (string Key, TValue Value)[] param)
        {
            return await HttpMethodAsync(url, token, HttpMethod.Put, param);
        }

        public async Task<T> DeleteAsync<T, TValue>(string url, string token,
            params (string Key, TValue Value)[] param)
        {
            return JsonConvert.DeserializeObject<T>(await DeleteAsync(url, token, param));
        }

        public async Task<string> DeleteAsync<TValue>(string url, string token,
            params (string Key, TValue Value)[] param)
        {
            return await HttpMethodAsync(url, token, HttpMethod.Delete, param);
        }

        public async Task<string> PatchAsync<TValue>(string url, string token,
            params (string Key, TValue Value)[] param)
        {
            return await HttpMethodAsync(url, token, new HttpMethod("PATCH"), param);
        }

        private async Task<string> HttpMethodAsync<TValue>(string url, string token, HttpMethod method,
            params (string Key, TValue Value)[] param)
        {
            using (var client = GetHttpClient(token))
            {
                if (param == null)
                    param = new List<(string Key, TValue Value)>().ToArray();
                if (param.Select(p => p.Value).Any(item => item is HttpContent))
                    using (var formData = new MultipartFormDataContent())
                    {
                        foreach (var item in param)
                            if (item.Value is StreamContent)
                                formData.Add(item.Value as StreamContent, item.Key, "file");
                            else if (CheckForValue(item))
                                formData.Add(item.Value as HttpContent, item.Key);
                        using (var res =
                            await client.SendAsync(new HttpRequestMessage(method, url) {Content = formData}))
                        {
                            return CheckForError(await res.Content.ReadAsStringAsync());
                        }
                    }

                client.Timeout = TimeSpan.FromSeconds(30);
                var items = param.Where(CheckForValue)
                    .Select(item => new KeyValuePair<string, string>(item.Key, item.Value.ToString()));
                using (var formData = new FormUrlEncodedContent(items))
                using (var res = await client.SendAsync(new HttpRequestMessage(method, url) {Content = formData}))
                {
                    return CheckForError(await res.Content.ReadAsStringAsync());
                }
            }
        }

        private string CheckForError(string json)
        {
            if (string.IsNullOrEmpty(json))
                return json;
            if (json.StartsWith("<html>")) throw new MastodonException("Return json is not valid");
            try
            {
                var jobj = JsonConvert.DeserializeObject<JObject>(json);
                if (jobj.TryGetValue("error", out var token)) throw new MastodonException(token.Value<string>());
            }
            catch (InvalidCastException e)
            {
            }

            return json;
        }
    }
}