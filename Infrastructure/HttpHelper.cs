using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class HttpHelper
    {
        public static TOut HttpPost<TIn, TOut>(string url, TIn data, int secondTimeOut = 30) 
        {
            return HttpPost<TIn, TOut>(url, data, secondTimeOut, null, null);
        }

        /// <summary>
        /// 发起POST同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>        
        /// <returns></returns>
        public static TOut HttpPost<TIn, TOut>(string url, TIn postData, int timeOut = 30, string contentType = null, Dictionary<string, string> headers = null)
        {
            using (var client = new HttpClient())
            {
                string postDataStr = postData.SerializeToString();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = new TimeSpan(0, 0, timeOut);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                using (var httpContent = new StringContent(postDataStr, Encoding.UTF8))
                {
                    if (contentType != null)
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    }

                    HttpResponseMessage response = client.PostAsync(url, httpContent).Result;
                    var json = response.Content.ReadAsStringAsync().Result;
                    return json.JsonToObject<TOut>();
                }
            }
        }

        /// <summary>
        /// 发起POST异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>        
        /// <returns></returns>
        public static async Task<T> HttpPostAsync<T>(string url, string postData = null, string contentType = null, int timeOut = 30, Dictionary<string, string> headers = null)
        {
            postData = postData ?? "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = new TimeSpan(0, 0, timeOut);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
                {
                    if (contentType != null)
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    }

                    HttpResponseMessage response = await client.PostAsync(url, httpContent);
                    var json = await response.Content.ReadAsStringAsync();
                    return json.JsonToObject<T>();
                }
            }
        }


        public static async Task<T> PostAsync<T>(string url, object data) where T : class, new()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string content = JsonConvert.SerializeObject(data);
                    var buffer = Encoding.UTF8.GetBytes(content);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync(url, byteContent).ConfigureAwait(false);
                    string result = await response.Content.ReadAsStringAsync();

                    var json = response.Content.ReadAsStringAsync().Result;
                    return json.JsonToObject<T>();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    throw new Exception($"response :{responseContent}", ex);
                }
                throw;
            }
        }

        public static T HttpGet<T>(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage response = client.GetAsync(url).Result;
                var json =response.Content.ReadAsStringAsync().Result;
                return json.JsonToObject<T>();
            }
        }

        /// <summary>
        /// 发起GET同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static T HttpGet<T>(string url, Dictionary<string, string> headers = null)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    } 
                }
                HttpResponseMessage response = client.GetAsync(url).Result;
                var json = response.Content.ReadAsStringAsync().Result;
                return json.JsonToObject<T>();
            }
        }

        /// <summary>
        /// 发起GET异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async Task<T> HttpGetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                HttpResponseMessage response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                return json.JsonToObject<T>();
            }
        }

    }
}
