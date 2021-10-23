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
            return await new BaseServiceRequest().GetRequest<BaseResponse<List<SysOrganizeTree>>>(new GettreeRequest());
        }

        public async Task<BaseResponse> Add(AddSysOrganize parm)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new AddReqest()
            {
                parameters = new Dictionary<string, object>() { { "", parm } }
            });
        }

        public async Task<PagesResponse<List<SysOrganize>>> Getpages(int page, int limit)
        {
            return await new BaseServiceRequest().GetRequest<PagesResponse<List<SysOrganize>>>(new Getpages()
            {
                parameters = new Dictionary<string, object>()
            {
                { "page", page},
                { "limit",limit}
            }
            });
        }


        public async Task<BaseResponse> Edit(AddSysOrganize parm)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new EditRequset()
            {
                parameters = new Dictionary<string, object>() { { "", parm } }
            });
        }
    }

    public class GettreeRequest : BaseRequest
    {
        public override string route { get => "api/Organize/gettree"; }
        public override Method Method { get => Method = Method.POST; }

    }


    public class AddReqest : BaseRequest
    {
        public override string route { get => "api/Organize/add"; }

        public override Method Method { get => Method = Method.POST; }
    }

    public class Getpages : BaseRequest
    {
        public override string route { get => "api/organize/getpages"; }
        public override Method Method { get => Method = Method.GET; }
        public override bool IsJson { get => false; }
    }

    public class EditRequset : BaseRequest
    {
        public override string route { get => "api/Organize/edit"; }
        public override Method Method { get => Method = Method.POST; }

    }

}
