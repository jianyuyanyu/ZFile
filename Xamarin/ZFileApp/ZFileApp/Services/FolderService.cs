using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinCommom.Api;
using XamarinCommom.Models;

namespace ZFileApp.Services
{

   public interface IFolderService
    {
        Task<BaseResponse<FolderData>> GetSpaceInfo(int Type, int FolderId);
    }
    public class FolderService : IFolderService
    {

        public async Task<BaseResponse<FolderData>> GetSpaceInfo(int Type, int FolderId = 2)
        {
            var model = await new BaseServiceRequest().GetRequest<BaseResponse<FolderData>>(new GetSpaceInfoRequst()
            {
                parameters = new Dictionary<string, object>()
                {
                    {"Type",Type },
                    {"FolderId",FolderId },
                },
                Method = Method.POST,
                IsJson = false
            });
            return model;
        }
    }

    public class GetSpaceInfoRequst : BaseRequest
    {
        public override string route { get => "api/GetSpaceInfo"; }

      
    }
}
