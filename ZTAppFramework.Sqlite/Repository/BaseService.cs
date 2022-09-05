using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.SqliteCore.Models;

namespace ZTAppFramework.SqliteCore.Repository
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  Zt
    /// 创建时间    ：  2022/9/5 9:28:50 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class BaseService<T>: DbContext where T : class, new()
    {


        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        public async Task<SqlResult<long>> AddAsync(T parm, bool Async = true)
        {
            SqlResult<long>? res = new SqlResult<long>() {success=false };
            try
            {
                var result = Async ? await freeSql.Insert<T>(parm).AppendData(parm).ExecuteIdentityAsync() : freeSql.Insert<T>(parm).AppendData(parm).ExecuteIdentity();
                res.data = result;

            }
            catch (Exception ex)
            {

            }
            return res;
        }

    }
}
