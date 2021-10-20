using Component.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeModel
{
   public class MenuService
    {

        public async Task<BaseResponse<List<SysMenuDto>>> GetMenuInfo()
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse<List<SysMenuDto>>>(new UserMenuInfoRequest() { IsJson = false });
        }
    }

    public class UserMenuInfoRequest : BaseRequest
    {
        public override string route { get => "api/Menu/authmenu"; }
    }

}
