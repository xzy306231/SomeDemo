using LearnApp.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Security;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LearnApp.Shared.BaseBusinessDto;

namespace LearnApp.Shared.Utils
{
    public static class HttpExtend
    {
        //private static IsoDateTimeConverter timejson = new IsoDateTimeConverter
        //{
        //    DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
        //};
        private static bool Callback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public static string Get(this string apiAddress)
        {
            string ret = string.Empty;
            try
            {
                var isSSL = apiAddress.ToLower().StartsWith("https://");
                ServicePointManager.ServerCertificateValidationCallback = Callback;

                using (var httpHandler = new HttpClientHandler())
                {
                    using (var httpClient = new HttpClient(!isSSL ? httpHandler : new HttpClientHandler
                    {
                        ClientCertificateOptions = ClientCertificateOption.Automatic,
                        SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
                        ServerCertificateCustomValidationCallback = (x, y, z, m) => true
                    }))
                    {
                        var result = httpClient.GetAsync(apiAddress).Result;
                        ret = result.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }
        public static T Get<T>(this string aptAddress)
        {
            try
            {
                var result = aptAddress.Get();
                return result.ToJson<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static T GetFdResult<T>(this string url, bool isFdJsonResult = true)
        {
            if (isFdJsonResult)
            {
                return url.Get<FdJsonResult<T>>().Data;
            }
            else
            {
                return url.Get<T>();
            }
        }
        public async static Task<T> PostAsync<T>(this string requestUri, object content, MediaTypeHeader mediaTypeHeader = MediaTypeHeader.Json, bool isFdJsonResult = true)
        {
            var requestResult = await requestUri.PostAsync(content, mediaTypeHeader);

            if (string.IsNullOrEmpty(requestResult)) return default;
            if (isFdJsonResult)
            {
                return requestResult.ToJson<FdJsonResult<T>>().Data;
            }
            else
            {
                return requestResult.ToJson<T>();
            }
        }
        public async static Task<string> PostAsync(this string requestUri, object content, MediaTypeHeader mediaTypeHeader = MediaTypeHeader.Json, Dictionary<string, string> headers = null, string referrer = "")
        {
            var isSSL = requestUri.ToLower().StartsWith("https://");
            HttpResponseMessage requestResult;
            var ret = "";

            using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                using (var httpHandler = new HttpClientHandler())
                {
                    using (var httpClient = new HttpClient(!isSSL ? httpHandler : new HttpClientHandler
                    {
                        ClientCertificateOptions = ClientCertificateOption.Automatic,
                        SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
                        ServerCertificateCustomValidationCallback = (x, y, z, m) => true
                    }))
                    {

                        if (string.IsNullOrEmpty(referrer))
                        {
                            httpRequestMessage.Headers.Referrer = new Uri(requestUri);
                        }
                        else
                        {
                            httpRequestMessage.Headers.Referrer = new Uri(referrer);
                        }

                        if (headers != null)
                        {
                            foreach (var header in headers)
                            {
                                httpRequestMessage.Headers.Add(header.Key, header.Value);
                            }
                        }

                        requestResult = httpClient.PostAsync(requestUri, GetHttpContent(content, mediaTypeHeader)).Result;
                        ret = await requestResult.Content.ReadAsStringAsync();
                    }
                }
            }

            if (!requestResult.IsSuccessStatusCode)
            {
                throw new NotSupportedException($"远程地址[{requestUri}]调用出错：{ret}");
            }

            return ret;
        }
        public static string Post(this string apiAddress, object inputDto, bool isHump = false)
        {
            var isSSL = apiAddress.ToLower().StartsWith("https://");
            string ret = string.Empty;
            // var requestJson = Newtonsoft.Json.JsonConvert.SerializeObject(inputDto, timejson);
            var requestJson = System.Text.Json.JsonSerializer.Serialize(inputDto);
            if (isHump)
            {
                //requestJson = Newtonsoft.Json.JsonConvert.SerializeObject(inputDto, Formatting.None, new JsonSerializerSettings
                //{
                //    ContractResolver = new CamelCasePropertyNamesContractResolver()
                //});
                requestJson = System.Text.Json.JsonSerializer.Serialize(inputDto, new System.Text.Json.JsonSerializerOptions { });
            }
            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, apiAddress))
            {
                using (var httpHandler = new HttpClientHandler())
                {
                    using (var httpClient = new HttpClient(!isSSL ? httpHandler : new HttpClientHandler
                    {
                        ClientCertificateOptions = ClientCertificateOption.Automatic,
                        SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
                        ServerCertificateCustomValidationCallback = (x, y, z, m) => true
                    }))
                    {
                        var result = httpClient.PostAsync(apiAddress, httpContent).Result;
                        ret = result.Content.ReadAsStringAsync().Result;
                        if (!result.IsSuccessStatusCode)
                        {
                            throw new NotSupportedException($"远程地址[{apiAddress}]调用出错：{ret}");
                        }
                        return ret;
                    }
                }
            }
        }
        public static T Post<T>(this string aptAddress, object inputDto, bool isFd = true, bool isHump = false)
        {
            var result = aptAddress.Post(inputDto, isHump);
            if (isFd)
                return result.ToJson<FdJsonResult<T>>().Data;
            return result.ToJson<T>();

        }
        private static HttpContent GetHttpContent(object content, MediaTypeHeader mediaTypeHeader)
        {
            HttpContent httpContent;

            switch (mediaTypeHeader)
            {
                case MediaTypeHeader.FormData: httpContent = new ByteArrayContent((byte[])content); break;
                case MediaTypeHeader.Json: httpContent = new StringContent(JsonExtend.J2String(content)); break;
                case MediaTypeHeader.Xml: httpContent = new StringContent(content.ToString()); break;
                default: httpContent = new StringContent(content.ToString()); break;
            }

            httpContent.Headers.ContentType = new MediaTypeHeaderValue(mediaTypeHeader.GetDescription());
            return httpContent;
        }

        private static string GetDescription<T>(this T t)
        {
            int hashCode = t.GetHashCode();
            Dictionary<int, string> enumValueDescription = GetEnumValueDescription<T>();
            if (enumValueDescription != null && enumValueDescription.ContainsKey(hashCode))
            {
                return enumValueDescription[hashCode];
            }

            return string.Empty;
        }
        private static Dictionary<int, string> GetEnumValueDescription<T>()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            FieldInfo[] fields = typeof(T).GetFields();
            FieldInfo[] array = fields;
            foreach (FieldInfo fieldInfo in array)
            {
                if (fieldInfo.FieldType.IsEnum)
                {
                    object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
                    string value = ((customAttributes.Length == 0) ? fieldInfo.Name : ((DescriptionAttribute)customAttributes[0]).Description);
                    dictionary.Add(Convert.ToInt32(fieldInfo.GetValue(null)), value);
                }
            }
            return dictionary;
        }
    }
}
