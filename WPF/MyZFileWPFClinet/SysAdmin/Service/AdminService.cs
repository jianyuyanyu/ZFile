using Component.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Service
{
    public class AdminService
    {
        public async Task<PagesResponse<List<SysAdmin>>> Getpages(int page, int limit)
        {
            return await new BaseServiceRequest().GetRequest<PagesResponse<List<SysAdmin>>>(new GetAdminpages()
            {
                parameters = new Dictionary<string, object>()
            {
                { "page", page},
                { "limit",limit}
            }
            });
        }
    }


    public class GetAdminpages : BaseRequest
    {
        public override string route { get => "api/admin/getpages"; }
        public override Method Method { get => Method = Method.GET; }
        public override bool IsJson { get => false; }
    }
}
