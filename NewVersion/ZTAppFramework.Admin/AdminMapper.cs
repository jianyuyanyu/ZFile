using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Admin.Model.Device;
using ZTAppFramework.Admin.Model.Menus;
using ZTAppFramework.Admin.Model.Users;
using ZTAppFramewrok.Application.Stared;

namespace ZTAppFramework.Admin
{
    public class AdminMapper : Profile
    {

        public AdminMapper()
        {
            CreateMap<UserLoginModel, LoginParam>()
                .ForMember(X => X.Account, opt => opt.MapFrom(str => str.UserName))
                .ForMember(X => X.Password, opt => opt.MapFrom(str => str.Password))
                .ReverseMap();

            CreateMap<MenuModel, SysMenuDto>().ReverseMap();
            CreateMap<DeviceUseModel, DeviceUseDto>().ReverseMap();
            CreateMap<MachineInfoModel, MachineInfoDto>().ReverseMap();
            
        }
    }
}
