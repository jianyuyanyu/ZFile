using Component.Api;
using Component.Common.Helpers;
using Component.Dto;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Common.Service
{
  public  class DownService
    {

        public async Task<BaseResponse<DownSplitDto>> DownloadFile(string Code, int Index)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse<DownSplitDto>>(
              new DownloadFile()
              {
                  parameters = new Dictionary<string, object>()
                  {
                      {"Code",Code},
                      {"Index",Index }
                  },
                  Method = Method.POST,
                  IsJson = false
              }
              );
        }
        public async Task<BaseResponse<CheckWholeDto>> CheckwholeFile(string md5Client, string spacecode = "qycode")
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse<CheckWholeDto>>(
              new CheckwholeFile()
              {
                  parameters = new Dictionary<string, object>()
                  {
                        { "md5Client",md5Client},
                        { "spacecode",spacecode}
                  },
                  Method = Method.POST,
              }
              );
        }
        public async Task<BaseResponse> UploadServer(fileuploadDto dto)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(
              new UploadFile()
              {
                  parameters = new Dictionary<string, object>()
                  {
                      {"",dto}
                  },
                  Method = Method.POST,
              }
              );
        }

        public async Task<BaseResponse> UploadfileMergeServer(fileuploadDto dto)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(
              new UploadfileMerge()
              {
                  parameters = new Dictionary<string, object>()
                  {
                      {"",dto}
                  },
                  Method = Method.POST,
              }
              );
        }

        public async Task<BaseResponse> UploadfileSuccessServer(List<UploadSuccessDto> dto)
        {
            return await new BaseServiceRequest().GetRequest<BaseResponse>(
              new UploadFileSuccess()
              {
                  parameters = new Dictionary<string, object>()
                  {
                      {"",dto}
                  },
                  Method = Method.POST,

              }
              );
        }

      


     


    }


    public class DownloadFile : BaseRequest
    {
        public override string route { get => "api/DownloadFile"; }
    }

    public class UploadfileMerge : BaseRequest
    {
        public override string route { get => "api/AddFileMerge"; }
    }

    public class UploadFileSuccess : BaseRequest
    {
        public override string route { get => "api/AddFilesuccess"; }
    }

   
    public class CheckwholeFile : BaseRequest
    {
        public override string route { get => "api/CheckwholeFile"; }
        public override bool IsJson { get => false; }
    }

    public class UploadFile : BaseRequest
    {
        public override string route { get => "api/Addupload"; }

    }
}
