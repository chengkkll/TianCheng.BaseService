using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TianCheng.BaseService.PlugIn.LoadApi
{
    /// <summary>
    /// LoadApi
    /// </summary>
    public class LoadApi
    {
        #region Get
        /// <summary>
        /// 以Get形式调用并返回对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T GetObject<T>(string apiUrl, string token = "")
        {
            string json = GetJson(apiUrl, token);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 以Get形式调用并返回Json
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        static public string GetJson(string apiUrl, string token = "")
        {
            var result = Load(ApiMethodType.GET, apiUrl, "", token);
            result.Wait();
            return result.Result;
        }
        #endregion

        #region Post
        /// <summary>
        /// 以Post形式调用并返回对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="postObject"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T PostObject<T>(string apiUrl, object postObject, string token = "")
        {
            string json = PostJson(apiUrl, postObject, token);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 以Post形式调用并返回Json
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="postObject"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        static public string PostJson(string apiUrl, object postObject, string token = "")
        {
            var result = Load(ApiMethodType.POST, apiUrl, postObject, token);
            result.Wait();
            return result.Result;
        }
        #endregion

        #region Core
        /// <summary>
        /// 调用api请求处理
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="apiUrl"></param>
        /// <param name="postObject"></param>
        /// <param name="token"></param>
        /// <param name="tokenScheme"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private async static Task<string> Load(ApiMethodType methodType, string apiUrl, object postObject, string token = "", string tokenScheme = "Bearer", int timeout = 1000)
        {

            HttpClientHandler handler = new HttpClientHandler();
            //handler.Proxy = null;
            //handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (var httpClient = new HttpClient(handler))
            {
                //设置Token信息
                if (!String.IsNullOrWhiteSpace(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                //设置Post数据
                StringContent content = null;
                if (postObject != null)
                {
                    string postJson = Newtonsoft.Json.JsonConvert.SerializeObject(postObject);
                    content = new StringContent(postJson, Encoding.UTF8, "application/json");
                }
                else
                {
                    content = new StringContent("");
                }

                //发送请求
                HttpResponseMessage response = null;
                switch (methodType)
                {
                    case ApiMethodType.GET:
                        {
                            response = await httpClient.GetAsync(apiUrl);
                            break;
                        }
                    case ApiMethodType.POST:
                        {
                            response = await httpClient.PostAsync(apiUrl, content);
                            break;
                        }
                    case ApiMethodType.PUT:
                        {
                            response = await httpClient.PutAsync(apiUrl, content);
                            break;
                        }
                    case ApiMethodType.DELETE:
                        {
                            response = await httpClient.DeleteAsync(apiUrl);
                            break;
                        }
                    case ApiMethodType.PATCH:
                        {
                            HttpRequestMessage requestMessage = new HttpRequestMessage
                            {
                                Method = new HttpMethod("PATCH")
                            };
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            requestMessage.RequestUri = new Uri(apiUrl);
                            response = await httpClient.SendAsync(requestMessage);
                            break;
                        }
                    case ApiMethodType.OPTIONS:
                        {
                            HttpRequestMessage requestMessage = new HttpRequestMessage
                            {
                                Method = HttpMethod.Options,
                                Content = content
                            };
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            requestMessage.RequestUri = new Uri(apiUrl);
                            response = await httpClient.SendAsync(requestMessage);
                            break;
                        }
                    default:
                        {
                            response = await httpClient.PostAsync(apiUrl, content);
                            break;
                        }
                }

                //获取返回值
                if (response != null)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        #endregion
    }
}
