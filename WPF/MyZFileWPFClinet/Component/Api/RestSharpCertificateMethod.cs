using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Api
{
    /// <summary>
    /// RestSharp Client
    /// </summary>
    public static class RestSharpCertificateMethod
    {

        public static string Token = "";
        /// <summary>
        /// 自定义请求
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task<Response> RequestBehavior<Response>(string url, RestRequest request,bool IsAuto=true)
           where Response : class
        {
            RestClient client = new RestClient(url);
            if(IsAuto)
                client.AddDefaultHeader("Authorization", "Bearer "+Token);
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Response>(response.Content);
            else
                return new BaseResponse()
                {
                    statusCode = (int)response.StatusCode,
                    message = response.StatusDescription ?? response.ErrorMessage,
                } as Response;
        }
    }
}
