using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using ZFileServer.Domain;

namespace ZFileServer.Code
{
    public class WebBaseController : Controller //Web视图控制器基类继承Microsoft.AspNetCore.Mvc.Controller
    {
        /// <summary>
        /// 返回对应视图并进行弹窗
        /// </summary>
        /// <param name="msg">提示信息，为null或空字符串则不弹窗</param>
        /// <param name="redrectUrl">跳转地址，为null或空字符串则不跳转</param>
        /// <returns></returns>
        protected IActionResult MessageBoxView(string msg, string redrectUrl = null)
        {
            ViewBag.SystemMessageBoxInfo = new MessageBoxVO()//通过ViewBag将弹窗配置信息传值给视图，视图再传值给MessageBox组件
            {
                Msg = msg,
                RedrectUrl = redrectUrl
            };
            return base.View();//返回对应视图
        }

        /// <summary>
        /// 返回对应视图并进行弹窗
        /// </summary>
        /// <typeparam name="T">视图绑定Model类型</typeparam>
        /// <param name="msg">提示信息，为null或空字符串则不弹窗</param>
        /// <param name="redrectUrl">跳转地址，为null或空字符串则不跳转</param>
        /// <param name="model">传入视图Model，为null则自动通过new创建一个Model实例传入给视图</param>
        /// <returns></returns>
        protected IActionResult MessageBoxView<T>(string msg, string redrectUrl = null, T model = default) where T : new()
        {
            if (model == null)//如果传入视图的Model为Null则通过new创建一个Model实例传入给视图
                model = new T();
            ViewBag.SystemMessageBoxInfo = new MessageBoxVO()//通过ViewBag将弹窗配置信息传值给视图，视图再传值给MessageBox组件
            {
                Msg = msg,
                RedrectUrl = redrectUrl
            };
            return base.View(model);//返回对应视图
        }

    }
}
