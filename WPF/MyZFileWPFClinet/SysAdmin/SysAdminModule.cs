using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SysAdmin.Dialogs;
using SysAdmin.ViewModels;
using SysAdmin.ViewModels.DialogViewModel;
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
            //权限管理    
            containerRegistry.RegisterForNavigation<Authorization>(typeof(Authorization).Name + "View");
            //字典管理        
            containerRegistry.RegisterForNavigation<Key>(typeof(Key).Name + "View");
            //系统设置       
            containerRegistry.RegisterForNavigation<SysSetting>(typeof(SysSetting).Name + "View");



            containerRegistry.RegisterDialog<AddRoleGroupDialog, AddRoleGroupViewModel>("AddRoleGroup");
            containerRegistry.RegisterDialog<AddRole, AddRoleViewModel>();

            containerRegistry.RegisterDialog<AddDepartment,AddDepartmentViewModel>();

          
            

            //注册服务
            containerRegistry.Register<DepartmentService>();
            containerRegistry.Register<RoleService>();


        }
    }
}