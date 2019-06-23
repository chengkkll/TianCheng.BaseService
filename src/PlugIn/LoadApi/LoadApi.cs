using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TianCheng.BaseService
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
        public static T Get<T>(string apiUrl, string token = "")
        {
            string json = Get(apiUrl, token);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 以Get形式调用并返回Json
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        static public string Get(string apiUrl, string token = "")
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
        public static T Post<T>(string apiUrl, object postObject = null, string token = "")
        {
            string json = Post(apiUrl, postObject, token);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 以Post形式调用并返回Json
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="postObject"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        static public string Post(string apiUrl, object postObject = null, string token = "")
        {
            var result = Load(ApiMethodType.POST, apiUrl, postObject, token);
            result.Wait();
            return result.Result;
        }
        #endregion

        #region Put
        /// <summary>
        /// 以Put形式调用并返回对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="putObject"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T Put<T>(string apiUrl, object putObject = null, string token = "")
        {
            string json = Put(apiUrl, putObject, token);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 以Put形式调用并返回Json
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="putObject"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        static public string Put(string apiUrl, object putObject = null, string token = "")
        {
            var result = Load(ApiMethodType.PUT, apiUrl, putObject, token);
            result.Wait();
            return result.Result;
        }
        #endregion

        #region Patch
        /// <summary>
        /// 以Put形式调用并返回对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="patchObject"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T Patch<T>(string apiUrl, object patchObject = null, string token = "")
        {
            string json = Patch(apiUrl, patchObject, token);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 以Put形式调用并返回Json
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="patchObject"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        static public string Patch(string apiUrl, object patchObject = null, string token = "")
        {
            var result = Load(ApiMethodType.PATCH, apiUrl, patchObject, token);
            result.Wait();
            return result.Result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// 以Put形式调用并返回对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T Delete<T>(string apiUrl, string token = "")
        {
            string json = Patch(apiUrl, token);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 以Put形式调用并返回Json
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        static public string Delete(string apiUrl, string token = "")
        {
            var result = Load(ApiMethodType.DELETE, apiUrl, null, token);
            result.Wait();
            return result.Result;
        }
        #endregion

        #region Options
        /// <summary>
        /// 以Put形式调用并返回对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T Options<T>(string apiUrl, string token = "")
        {
            string json = Patch(apiUrl, token);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 以Put形式调用并返回Json
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        static public string Options(string apiUrl, string token = "")
        {
            var result = Load(ApiMethodType.OPTIONS, apiUrl, null, token);
            result.Wait();
            return result.Result;
        }
        #endregion

        /// <summary>
        /// 登录后的Token信息
        /// </summary>
        public static string Token { get; set; }

        #region Core
        /// <summary>
        /// 调用api请求处理
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="apiUrl"></param>
        /// <param name="postObject"></param>
        /// <param name="token"></param>
        /// <param name="tokenScheme"></param>
        /// <returns></returns>
        private static async Task<string> Load(ApiMethodType methodType, string apiUrl, object postObject, string token = "", string tokenScheme = "Bearer")
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                token = Token;
            }

            HttpClientHandler handler = new HttpClientHandler();
            //handler.Proxy = null;
            //handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (var httpClient = new HttpClient(handler))
            {
                // 设置Token信息
                if (!string.IsNullOrWhiteSpace(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenScheme, token);
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
                            response = await httpClient.PostAsync(apiUrl, GetJsonContent(postObject));
                            break;
                        }
                    case ApiMethodType.PUT:
                        {
                            response = await httpClient.PutAsync(apiUrl, GetJsonContent(postObject));
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
                            //requestMessage.Headers.Authorization = new AuthenticationHeaderValue(tokenScheme, token);
                            //requestMessage.RequestUri = new Uri(apiUrl);
                            response = await httpClient.SendAsync(requestMessage);
                            break;
                        }
                    case ApiMethodType.OPTIONS:
                        {
                            HttpRequestMessage requestMessage = new HttpRequestMessage
                            {
                                Method = HttpMethod.Options,
                                Content = GetJsonContent(postObject)
                            };
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(tokenScheme, token);
                            requestMessage.RequestUri = new Uri(apiUrl);
                            response = await httpClient.SendAsync(requestMessage);
                            break;
                        }
                    default:
                        {
                            response = await httpClient.PostAsync(apiUrl, GetJsonContent(postObject));
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
        /// <summary>
        /// 获取请求时的体信息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        static private StringContent GetJsonContent(object content)
        {
            if (content == null)
            {
                return new StringContent("");
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(content);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        #endregion
    }
}
