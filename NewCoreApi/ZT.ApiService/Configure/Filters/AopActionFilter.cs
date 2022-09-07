using Microsoft.AspNetCore.Mvc.Filters;

namespace ZT.ApiService.Configure.Filters
{

    /// <summary>
    /// 方法过滤器
    /// </summary>
    public class AopActionFilter : IAsyncActionFilter
    {
        private static readonly List<string> IgnoreApi = new()
    {
        "api/sysfile/",
        "api/captcha",
        "/chathub"
    };

        private static readonly List<string> IgnorePowerApi = new()
    {
        "api/sysfile/",
        "api/captcha",
        "/chathub",
        "login"
    };

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return Task.CompletedTask;
        }
    }
}
