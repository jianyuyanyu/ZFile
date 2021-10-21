using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SysAdmin.Views;

namespace SysAdmin
{
    public class SysAdminModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //用户管理    
            containerRegistry.RegisterForNavigation<Admin>(typeof(Admin).Name + "View");
            //角色管理    
            containerRegistry.RegisterForNavigation<Role>(typeof(Role).Name + "View");
            //菜单管理
            containerRegistry.RegisterForNavigation<Menu>(typeof(Menu).Name + "View");
            //组织机构
            containerRegistry.RegisterForNavigation<Department>(typeof(Department).Name + "View");
            
        }
    }
}