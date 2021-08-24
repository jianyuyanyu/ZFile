
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XamarinCommom.Commom;

namespace XamarinCommom.Api
{
    /// <summary>
    /// 请求基类
    /// </summary>
    public class BaseRequest
    {
        [JsonIgnore]
        public string apiUrl { get { return Contract.webUrl + route; } }
        /// <summary>
        /// 路由地址
        /// </summary>
        [JsonIgnore]
        public virtual string route { get; set; }

        [JsonIgnore]
        public virtual string ContentType { get; set; } = "application/json";

        [JsonIgnore]
        public virtual Method Method { get; set; } = Method.GET;

        [JsonIgnore]
        public Dictionary<string, object> parameters { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> formFiles { get; set; }

        /// <summary>
        /// 是否以JSON格式处理数据
        /// </summary>
        [JsonIgnore]
        public virtual bool IsJson { get; set; } = true;


        /// <summary>
        /// 获取请求对象得属性转换值
        /// </summary>
        /// <returns> </returns>
        public string GetPropertiesObject()
        {
            StringBuilder builder = new StringBuilder();
            var type = this.GetType();
            var propertyArray = type.GetProperties();
            foreach (PropertyInfo Property in propertyArray)
            {
                var JsonIgnoreAttr = Property.GetCustomAttribute<JsonIgnoreAttribute>();
                if (JsonIgnoreAttr != null) continue;
                var pvalue = Property.GetValue(this);
                if (pvalue != null)
                {
                    StringBuilder pbuilder = new StringBuilder();
                    if (pvalue.GetType().FullName.Contains("System."))
                    {
                        if (builder.ToString() == string.Empty) builder.Append("?");
                        builder.Append($"{Property.Name}={pvalue}&");
                    }
                    else
                    {
                        var QpropertyArray = pvalue.GetType().GetProperties();
                        if (QpropertyArray != null && QpropertyArray.Length > 0)
                        {
                            foreach (PropertyInfo Qproperty in QpropertyArray)
                            {
                                var Qprevent = Qproperty.GetCustomAttribute<JsonIgnoreAttribute>();
                                if (Qprevent != null) continue;
                                var Qpvalue = Qproperty.GetValue(pvalue);
                                if (Qpvalue != null && Qpvalue.ToString() != "")
                                {
                                    if (builder.ToString() == string.Empty) builder.Append("?");
                                    builder.Append($"{Qproperty.Name}={Qpvalue}&");
                                }
                            }
                        }
                    }
                    builder.Append(pbuilder.ToString());

                }
                return builder.ToString().Trim('&');

            }
            return builder.ToString();
        }
    }
}
