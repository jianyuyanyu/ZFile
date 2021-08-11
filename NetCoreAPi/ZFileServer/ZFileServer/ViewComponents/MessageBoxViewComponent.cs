using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZFileServer.Domain;

namespace ZFileServer.ViewComponents
{
    public class MessageBoxViewComponent : ViewComponent
    {
        //private readonly LogHandler _logHandler;

        //public MessageBoxViewComponent(LogHandler logHandler)//依赖注入日志操作实例
        //{
        //    this._logHandler = logHandler;
        //}

        public async Task<IViewComponentResult> InvokeAsync(MessageBoxVO options)//传入参数为MessageBoxVO类型的options（弹窗信息选项）
        {
            if (options == null || (string.IsNullOrEmpty(options.Msg) && string.IsNullOrEmpty(options.RedrectUrl)))
                return Content("");//如果弹窗信息选项为空或内容和跳转URL均为空，证明不跳转，该组件直接输出空内容即可
            //如果需要记录日志，可以在此进行操作如：await  _logHandler.CustomMessage(model);
            return View(options);//否则返回视图（一段弹窗控制的JS）
        }
    }
}
