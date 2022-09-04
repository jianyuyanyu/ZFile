using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramewrok.Application.Stared.HttpManager;
using ZTAppFreamework.Stared.Attributes;

namespace ZTAppFramework.Application.Service
{

    public class AppServiceBase
    {
        protected ApiClinetRepository _apiClinet;
        public virtual string ApiServiceUrl { get => ""; }

        public AppServiceBase(ApiClinetRepository apiClinet)
        {
            _apiClinet = apiClinet;
        }

        protected string GetEndpoint()
        {
            string EventStr = "";
            foreach (var Properties in GetType().GetMembers())
            {
                var info = Properties.GetCustomAttribute<ApiUrlAttribute>();
                if (info != null)
                {
                    EventStr = info.Value;
                    break;
                }
            }
            if (string.IsNullOrEmpty(EventStr)) throw new Exception(GetType().Name+ "Not Full ApiUrlAttribute");
            return ApiServiceUrl + "/" + EventStr;
        }
    }
}
