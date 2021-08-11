using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFile.Common.ApiClient;
using ZFile.Common.EnumHelper;
using ZFile.Core.Model;
using ZFile.Service.Interfaces;
using ZFile.Service.Repository;

namespace ZFile.Service.Implements
{
    public class SysAppseting : BaseService<FileAppseting>, ISysAppseting
    {
        public async Task<ApiResult<string>> GetValueByKey(string strKey, string strDefault = "")
        {
            var res = new ApiResult<string>
            {
                statusCode = (int)ApiEnum.Error
            };
            string strValue = "";
            var keyres = await Db.Queryable<FileAppseting>().Where(o => o.Key == strKey).ToListAsync();
            if (keyres.Count() > 0)
            {
                strValue = keyres[0].Value;
            }
            else
            {
                strValue = strDefault;
            }
            res.data = strValue;
            res.statusCode =(int) ApiEnum.Status;
            return res;

        }
    }
}
