using Component.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin
{
    public class DepartmentService
    {
        public async Task<BaseResponse<List<SysOrganizeTree>>> Gettree()
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse<List<SysOrganizeTree>>>(new GettreeRequest() );
        }
    }

    public class GettreeRequest: BaseRequest
    {
        public override string route { get => "api/Organize/gettree"; }

        public override Method Method { get => Method = Method.POST; }


    }

}
