using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZT.Application.AppService;
using ZT.Application.Filters;
using ZT.Domain.Sys;
using ZT.Sugar;

namespace ZT.Application.Sys
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/7 14:01:17 
    /// Description   ：   登录日志/操作日志/任务日志服务接口
    ///********************************************/
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1"), NoAuditLog]
    public class SysLogService : IApplicationService
    {
        private readonly SugarRepository<SysLog> _thisRepository;
        public SysLogService(SugarRepository<SysLog> thisRepository)
        {
            _thisRepository = thisRepository;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(SysLogDto model)
        {
            return await _thisRepository.InsertAsync(model.Adapt<SysLog>());
        }
    }
}
