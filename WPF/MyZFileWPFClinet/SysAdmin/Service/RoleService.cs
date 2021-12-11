using Component.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin
{
    public class RoleService
    {
        public async Task<BaseResponse<List<SysOrganizeTree>>> Gettree()
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse<List<SysOrganizeTree>>>(new GettreeRequest());
        }

        public async Task<PagesResponse<List<SysRole>>> Getpages(int page, int limit)
        {
            return await new BaseServiceRequest().GetRequest<PagesResponse<List<SysRole>>>(new GetRolepages()
            {
                parameters = new Dictionary<string, object>()
            {
                { "page", page},
                { "limit",limit}
            }
            });
        }

        public async Task<BaseResponse> Add(SysRole parm)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new AddRoleGroup()
            {
                parameters = new Dictionary<string, object>() { { "", parm } }
            });
        }

        public async  Task<BaseResponse>  Del(object parm)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new DelRole()
            {
                parameters = new Dictionary<string, object>() { { "", parm } }
            });
        }
    }

    public class AddRoleGroup : BaseRequest
    {
         public override string route { get => "api/role/add"; }
        public override Method Method { get => Method = Method.POST; }
    }
    public class GetRolepages : BaseRequest
    {
        public override string route { get => "api/role/getpages"; }
        public override Method Method { get => Method = Method.GET; }
        public override bool IsJson { get => false; }
    }

    public class DelRole : BaseRequest
    {
        public override string route { get => "api/role/delete"; }
        public override Method Method { get => Method = Method.POST; }
    }



}
