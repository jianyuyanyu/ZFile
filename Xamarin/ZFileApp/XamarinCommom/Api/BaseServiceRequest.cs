using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinCommom.Api
{

    /// <summary>
    /// 请求服务基类
    /// </summary>
    public class BaseServiceRequest
    {
        public async Task<Response> GetRequest<Response>(BaseRequest br, bool IsAuto = true)
            where Response : class
        {
            RestRequest request = new RestRequest();
            request.Method = br.Method;
            request.AddHeader("Content-Type", br.ContentType);
            if (!request.Method.Equals(Method.POST))
            {
                string pms = br.GetPropertiesObject();
                return await RestSharpCertificateMethod.RequestBehavior<Response>(br.apiUrl + pms, request, IsAuto);
            }

            if (br.IsJson)
            {
                foreach (var item in br.parameters)
                    request.AddJsonBody(JsonConvert.SerializeObject(item.Value));

                return await RestSharpCertificateMethod.RequestBehavior<Response>(br.apiUrl, request, IsAuto);
            }
            else
            {
                string url = br.apiUrl + "?";
                int Num, Count;
                Count = br.parameters.Count;
                Num = 0;
                foreach (var item in br.parameters)
                {
                    Num++;
                    if (Num == Count)
                        url += item.Key + "=" + item.Value ;
                    else
                        url += item.Key + "=" + item.Value + "&";
                }
             
                return await RestSharpCertificateMethod.RequestBehavior<Response>(url, request, IsAuto);
            }


        }

    }
}
