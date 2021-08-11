using Component;
using Component.Api;
using Component.Dto;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkModel
{
    public class WorkService
    {
        public async Task<BaseResponse<FolderData>> GetFolderInfo(int Type, int FolderId = 2)
        {
            var model = await new BaseServiceRequest().GetRequest<BaseResponse<FolderData>>(new GetFolderRequst()
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

        public async Task<BaseResponse> CheckAuth()
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(
                new CheckAuth()
                {
                    parameters = new Dictionary<string, object>()
                    {
                        { "code","qycode"},
                        { "secret","qycode"}
                    },
                    Method = Method.POST,
                }
                );
        }

        public async Task<BaseResponse<DownFileInfo>> GetDownFileInfo(int FileID)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse<DownFileInfo>>(
              new GetDownFileInfo()
              {
                  parameters = new Dictionary<string, object>()
                  {
                      {"FileId",FileID}
                  },
                  Method = Method.POST,
                  IsJson = false
              }
              );
        }


        public async Task<BaseResponse> DelFileRequst(List<DelFile> dto)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(new DelFileRequst()
            {

                parameters = new Dictionary<string, object>()
             {
                 {"",dto }
             },
                Method = Method.POST,

            });
        }

    }



    public class GetFolderRequst : BaseRequest
    {
        public override string route { get => "api/GetFolderInfo"; }

        public override string ContentType { get => "*/*"; }
    }


    public class CheckAuth : BaseRequest
    {
        public override string route { get => "api/Checkauth"; }

        public override bool IsJson { get => false; }
    }

    public class GetDownFileInfo : BaseRequest
    {
        public override string route { get => "api/GetDownLoadFileInfo"; }
    }

    public class DelFileRequst : BaseRequest
    {
        public override string route { get => "api/DelFile"; }
    }

}
