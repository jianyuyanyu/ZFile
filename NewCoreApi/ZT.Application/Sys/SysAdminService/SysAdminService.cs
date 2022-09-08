using FytSoa.DynamicApi.Attributes;
using Mapster;
using Masuit.Tools.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZT.Application.AppService;

using ZT.Common.Utils;
using ZT.Domain.Core.Cache;
using ZT.Domain.Sys;
using ZT.Sugar;

namespace ZT.Application.Sys
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/8 10:16:19 
    /// Description   ：  管理员表服务接口
    ///********************************************/
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class SysAdminService : IApplicationService
    {

        private readonly SugarRepository<SysAdmin> _thisRepository;
        public SysAdminService(SugarRepository<SysAdmin> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        [NonDynamicMethod]
        public async Task<SysAdminDto> LoginAsync(LoginParam loginParam)
        {
            var code = MemoryService.Default.GetCache<string>(KeyUtils.CAPTCHACODE + loginParam.CodeKey);
            if (!string.Equals(code, loginParam.Code, StringComparison.CurrentCultureIgnoreCase)) throw new ArgumentException("验证码输入错误！~");

            var model = await _thisRepository.GetSingleAsync(m => !m.IsDel && m.LoginAccount == loginParam.Account);
            if (model == null) throw new ArgumentException("账号输入错误！~");

            if (model.LoginPassWord != loginParam.Password.AESEncrypt()) throw new ArgumentException("密码输入错误！~");

            if (!model.Status) throw new ArgumentException("账号被冻结，请联系管理员！~");
            model.LoginTime = DateTime.Now;
            model.LoginCount += 1;
            model.UpLoginTime = model.LoginTime;
            await _thisRepository.UpdateAsync(model);
            return model.Adapt<SysAdminDto>();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SysAdminDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            model.LoginPassWord = model.LoginPassWord.AESDecrypt();
            return model.Adapt<SysAdminDto>();
        }
    }
}
