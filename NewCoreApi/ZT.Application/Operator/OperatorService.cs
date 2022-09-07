using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZT.Application.AppService;
using ZT.Application.Sys;
using ZT.Common.Utils.Config;
using ZT.Domain.Core.Jwt;

namespace ZT.Application.Operator
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/7 14:12:09 
    /// Description   ：  操作人服务
    ///********************************************/
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class OperatorService : IApplicationService
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        private readonly SysLogService _logService;


        public OperatorService(IHttpContextAccessor httpContextAccessor, SysLogService logService)
        {
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }


        public OperatorUser User => GetTokenUser();

        private OperatorUser GetTokenUser()
        {
            var paramToken = _httpContextAccessor.HttpContext?.Request.Headers["accessToken"].ToString();
            if (string.IsNullOrEmpty(paramToken))
            {
                return new OperatorUser();
            }
            var token = JwtAuthService.SerializeJwt(paramToken);
            return new OperatorUser()
            {
                Id = token.Id,
                RoleArray = token.RoleArray.StrToListLong(),
                TenantId = token.TenantId,
                Username = token.FullName,
            };
        }

    }
}
