using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Admin.Model.Users;
using ZTAppFramewrok.Application.Stared.DTO;

namespace ZTAppFramework.Admin
{
    public class AdminMapper : Profile
    {

        public AdminMapper()
        {
            CreateMap<UserLoginModel, UserInfoDto>()
                .ForMember(X => X.User, opt => opt.MapFrom(str => str.UserName))
               
                .ReverseMap();
        }
    }
}
