using SqlSugar;
using System;
using ZFile.Common.ConfigHelper;
using ZFile.Core.Model;
using ZFile.Core.Model.File;
using ZFile.Core.Model.User;

namespace ZFile.Core
{
    public class DbContext
    {

        public SqlSugarClient Db;
        public DbContext()
        {

            Db = new SqlSugarClient(new ConnectionConfig()
            {

                ConnectionString = ConfigExtensions.Configuration["DBConnection:SqllistConnectionString"],
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true
            });


            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                string s = sql;
                //System.Console.WriteLine(sql);
                //    Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                //Console.WriteLine();
            };
           
        }
        public void InitDb()
        {
            Db.CodeFirst.InitTables<FileAppseting>();
            Db.CodeFirst.InitTables<FileDocument>();
            Db.CodeFirst.InitTables<FT_File>();
            Db.CodeFirst.InitTables<FT_File_UserAuth>();
            Db.CodeFirst.InitTables<FT_File_Vesion>();
            Db.CodeFirst.InitTables<FT_Folder>();
            Db.CodeFirst.InitTables<Qycode>();
            Db.CodeFirst.InitTables<SysLog>();
            //权限管理
            Db.CodeFirst.InitTables<SysAdmin>();
            Db.CodeFirst.InitTables<SysCode>();
            Db.CodeFirst.InitTables<SysCodeType>();
            Db.CodeFirst.InitTables<SysOrganize>();
            Db.CodeFirst.InitTables<SysMenu>();
            Db.CodeFirst.InitTables<SysPermissions>();
            Db.CodeFirst.InitTables<SysRole>();
            Db.CodeFirst.InitTables<SysBtnFun>();
            SeedData();
        }

        void SeedData()
        {
            Db.Insertable<SysRole>(new SysRole()
            {
                Guid = "19deef37-c67b-4314-a21c-a51f66e3b97a",
                DepartmentGuid = "0bb7aaaf-8b91-4ce5-b261-72d7bc6157fc",
                DepartmentName = "项目负责人",
                DepartmentGroup = "883deb1c-ddd7-484e-92c1-b3ad3b32e655,0f04b2e0-1ab8-4cc9-b6bf-a3e7dd53f77b,0bb7aaaf-8b91-4ce5-b261-72d7bc6157fc,",
                Name = "商务部经理",
                Codes = "2001",
                IsSystem = false,
                Summary = "",
                ParentGuid = ""
            }
            ).ExecuteCommand();

            Db.Insertable<SysRole>(new SysRole()
            {
                Guid = "4d98e862-af34-4e11-a585-658014333bbc",
                DepartmentGuid = "0bb7aaaf-8b91-4ce5-b261-72d7bc6157fc",
                DepartmentName = "项目负责人",
                DepartmentGroup = ",883deb1c-ddd7-484e-92c1-b3ad3b32e655,0f04b2e0-1ab8-4cc9-b6bf-a3e7dd53f77b,0bb7aaaf-8b91-4ce5-b261-72d7bc6157fc,",
                Name = "项目管理员",
                Codes = "1004",
                IsSystem = false,
                Summary = "只能查看自己的项目",
                ParentGuid = ""
            }).ExecuteCommand();

            Db.Insertable<SysRole>(new SysRole()
            {
                Guid = "d1bbd2f4-ea8f-4c53-9f67-3b6acd9c29fb",
                DepartmentGuid = "dcf99638-5db6-4dd7-a485-31df1160d45a",
                DepartmentName = "xx技术部",
                DepartmentGroup = ",883deb1c-ddd7-484e-92c1-b3ad3b32e655,5533b6c5-ba2e-4659-be29-c860bb41e04d,dcf99638-5db6-4dd7-a485-31df1160d45a,",
                Name = "超级管理员",
                Codes = "1003",
                IsSystem = false,
                Summary = "可查看所有功能",
                ParentGuid = ""
            }).ExecuteCommand();


            //主权限类型
            Db.Insertable<SysCodeType>(new SysCodeType()
            {
                Guid = "ef8c0895-2637-4892-84ec-6bfe0766629c",
                ParentGuid = "",
                Layer = 0,
                Name = "系统权限（千万不要删除）",
                Sort = 19,
                AddTime = DateTime.Now,
                EditTime = DateTime.Now,
                SiteGuid = ""
            }).ExecuteCommand();

            Db.Insertable<SysCodeType>(new SysCodeType()
            {
                Guid = "a88fa4d3-3658-4449-8f4a-7f438964d716",
                ParentGuid = "ef8c0895-2637-4892-84ec-6bfe0766629c",
                Layer = 1,
                Name = "功能权限",
                Sort = 20,
                AddTime = DateTime.Now,
                EditTime = DateTime.Now,
                SiteGuid = ""
            }).ExecuteCommand();

            #region //权限标识字典


            Db.Insertable<SysCode>(new SysCode()
            {
                Guid = "338ec105-546b-497d-9c73-ae3b79b24756",
                ParentGuid = "a88fa4d3-3658-4449-8f4a-7f438964d716",
                CodeType = "Export",
                Name = "导出",
                EnName = "",
                Sort = 355,
                Status = false,
                Summary = "",
                AddTime = DateTime.Now,
                EditTime = DateTime.Now
            }).ExecuteCommand();

            Db.Insertable<SysCode>(new SysCode()
            {
                Guid = "59852730-5d5e-48bc-8726-f317c26a0636",
                ParentGuid = "a88fa4d3-3658-4449-8f4a-7f438964d716",
                CodeType = "Authorize",
                Name = "授权",
                EnName = "",
                Sort = 357,
                Status = false,
                Summary = "",
                AddTime = DateTime.Now,
                EditTime = DateTime.Now
            }).ExecuteCommand();
            Db.Insertable<SysCode>(new SysCode()
            {
                Guid = "75093b9f-c72a-4267-9497-0592aab3b0d6",
                ParentGuid = "a88fa4d3-3658-4449-8f4a-7f438964d716",
                CodeType = "Delete",
                Name = "删除",
                EnName = "",
                Sort = 353,
                Status = false,
                Summary = "",
                AddTime = DateTime.Now,
                EditTime = DateTime.Now
            }).ExecuteCommand();

            Db.Insertable<SysCode>(new SysCode()
            {
                Guid = "a42d0b4f-bec2-4af9-ab46-ff414dbbc118",
                ParentGuid = "a88fa4d3-3658-4449-8f4a-7f438964d716",
                CodeType = "Update",
                Name = "修改",
                EnName = "",
                Sort = 352,
                Status = false,
                Summary = "",
                AddTime = DateTime.Now,
                EditTime = DateTime.Now
            }).ExecuteCommand();

            Db.Insertable<SysCode>(new SysCode()
            {
                Guid = "bddf9b84-4033-463a-a0b5-43b6504112e2",
                ParentGuid = "a88fa4d3-3658-4449-8f4a-7f438964d716",
                CodeType = "Add",
                Name = "添加",
                EnName = "",
                Sort = 351,
                Status = false,
                Summary = "",
                AddTime = DateTime.Now,
                EditTime = DateTime.Now
            }).ExecuteCommand();

            Db.Insertable<SysCode>(new SysCode()
            {
                Guid = "c330b61e-227e-44f7-af02-15940f9d1280",
                ParentGuid = "a88fa4d3-3658-4449-8f4a-7f438964d716",
                CodeType = "Import",
                Name = "导入",
                EnName = "",
                Sort = 356,
                Status = false,
                Summary = "",
                AddTime = DateTime.Now,
                EditTime = DateTime.Now
            }).ExecuteCommand();

            Db.Insertable<SysCode>(new SysCode()
            {
                Guid = "f61b1937-18fb-43e8-b86e-78547d793d37",
                ParentGuid = "a88fa4d3-3658-4449-8f4a-7f438964d716",
                CodeType = "Audit",
                Name = "审核",
                EnName = "",
                Sort = 354,
                Status = false,
                Summary = "",
                AddTime = DateTime.Now,
                EditTime = DateTime.Now
            }).ExecuteCommand();

            #endregion


            #region 菜单管理
            Db.Insertable<SysMenu>(new SysMenu()
            {
                Guid = "a4b3b26f-076a-4267-b03d-613081b13a12",
                SiteGuid="",
                ParentGuid="",
                ParentName="根目录",
                Name= "CMS内容管理",
                NameCode="Risk",
                ParentGuidList= ",a4b3b26f-076a-4267-b03d-613081b13a12,",
                Layer=1,
                Urls="",
                Icon="",
                Sort=50,
                Status=true,
                BtnFunJson="[]",
                EditTime=DateTime.Now,
                AddTIme= DateTime.Now,
            }).ExecuteCommand();

            //系统菜单
            Db.Insertable<SysMenu>(new SysMenu()
            {
                Guid = "5ed17c74-0fff-4f88-8581-3b4f26d005a8",
                SiteGuid = "",
                ParentGuid = "a4b3b26f-076a-4267-b03d-613081b13a12",
                ParentName = "风险预警平台",
                Name = "系统管理",
                NameCode = "Sys",
                ParentGuidList = ",a4b3b26f-076a-4267-b03d-613081b13a12,5ed17c74-0fff-4f88-8581-3b4f26d005a8,",
                Layer = 2,
                Urls = "",
                Icon = "",
                Sort = 2,
                Status = true,
                BtnFunJson = "[]",
                EditTime = DateTime.Now,
                AddTIme = DateTime.Now,
            }).ExecuteCommand();

            Db.Insertable<SysMenu>(new SysMenu()
            {
                Guid = "6d4cfcf7-ff1c-4537-aa3f-75cc9df27583",
                SiteGuid = "",
                ParentGuid = "5ed17c74-0fff-4f88-8581-3b4f26d005a8",
                ParentName = "系统管理",
                Name = "组织机构",
                NameCode = "Department",
                ParentGuidList = ",a4b3b26f-076a-4267-b03d-613081b13a12,5ed17c74-0fff-4f88-8581-3b4f26d005a8,6d4cfcf7-ff1c-4537-aa3f-75cc9df27583,",
                Layer = 3,
                Urls = "",
                Icon = "",
                Sort = 96,
                Status = true,
                BtnFunJson = "[]",
                EditTime = DateTime.Now,
                AddTIme = DateTime.Now,
            }).ExecuteCommand();

            Db.Insertable<SysMenu>(new SysMenu()
            {
                Guid = "404d4b8b-8e3c-42ee-aee5-f29df31308fa",
                SiteGuid = "",
                ParentGuid = "5ed17c74-0fff-4f88-8581-3b4f26d005a8",
                ParentName = "系统管理",
                Name = "菜单管理",
                NameCode = "Menu",
                ParentGuidList = ",a4b3b26f-076a-4267-b03d-613081b13a12,5ed17c74-0fff-4f88-8581-3b4f26d005a8,404d4b8b-8e3c-42ee-aee5-f29df31308fa,",
                Layer = 3,
                Urls = "",
                Icon = "",
                Sort = 7,
                Status = true,
                BtnFunJson = "[]",
                EditTime = DateTime.Now,
                AddTIme = DateTime.Now,
            }).ExecuteCommand();

            Db.Insertable<SysMenu>(new SysMenu()
            {
                Guid = "5ce13ead-971b-4ed4-ad5f-acacccd82381",
                SiteGuid = "",
                ParentGuid = "5ed17c74-0fff-4f88-8581-3b4f26d005a8",
                ParentName = "系统管理",
                Name = "角色管理    ",
                NameCode = "Role",
                ParentGuidList = ",a4b3b26f-076a-4267-b03d-613081b13a12,5ed17c74-0fff-4f88-8581-3b4f26d005a8,5ce13ead-971b-4ed4-ad5f-acacccd82381,",
                Layer = 3,
                Urls = "",
                Icon = "",
                Sort = 5,
                Status = true,
                BtnFunJson = "[]",
                EditTime = DateTime.Now,
                AddTIme = DateTime.Now,
            }).ExecuteCommand();

            Db.Insertable<SysMenu>(new SysMenu()
            {
                Guid = "1fc3d8e8-e3f2-49cf-a652-2341082643df",
                SiteGuid = "",
                ParentGuid = "5ed17c74-0fff-4f88-8581-3b4f26d005a8",
                ParentName = "系统管理",
                Name = "用户管理    ",
                NameCode = "Admin",
                ParentGuidList = ",a4b3b26f-076a-4267-b03d-613081b13a12,5ed17c74-0fff-4f88-8581-3b4f26d005a8,1fc3d8e8-e3f2-49cf-a652-2341082643df,",
                Layer = 3,
                Urls = "",
                Icon = "",
                Sort = 6,
                Status = true,
                BtnFunJson = "[]",
                EditTime = DateTime.Now,
                AddTIme = DateTime.Now,
            }).ExecuteCommand();

            Db.Insertable<SysMenu>(new SysMenu()
            {
                Guid = "946bc7c1-6c84-4934-9bda-5f766a61801b",
                SiteGuid = "",
                ParentGuid = "5ed17c74-0fff-4f88-8581-3b4f26d005a8",
                ParentName = "系统管理",
                Name = "权限管理    ",
                NameCode = "Authorization",
                ParentGuidList = ",a4b3b26f-076a-4267-b03d-613081b13a12,5ed17c74-0fff-4f88-8581-3b4f26d005a8,946bc7c1-6c84-4934-9bda-5f766a61801b,",
                Layer = 3,
                Urls = "",
                Icon = "",
                Sort = 9,
                Status = true,
                BtnFunJson = "[]",
                EditTime = DateTime.Now,
                AddTIme = DateTime.Now,
            }).ExecuteCommand();


            Db.Insertable<SysMenu>(new SysMenu()
            {
                Guid = "a280f6e2-3100-445f-871d-77ea443356a9",
                SiteGuid = "",
                ParentGuid = "5ed17c74-0fff-4f88-8581-3b4f26d005a8",
                ParentName = "系统管理",
                Name = "字典管理    ",
                NameCode = "Key",
                ParentGuidList = ",a4b3b26f-076a-4267-b03d-613081b13a12,5ed17c74-0fff-4f88-8581-3b4f26d005a8,a280f6e2-3100-445f-871d-77ea443356a9,",
                Layer = 3,
                Urls = "",
                Icon = "",
                Sort = 87,
                Status = true,
                BtnFunJson = "[]",
                EditTime = DateTime.Now,
                AddTIme = DateTime.Now,
            }).ExecuteCommand();

            Db.Insertable<SysMenu>(new SysMenu()
            {
                Guid = "97f0f915-43fc-4d19-9ee2-0d1a5c21877b",
                SiteGuid = "",
                ParentGuid = "5ed17c74-0fff-4f88-8581-3b4f26d005a8",
                ParentName = "系统管理",
                Name = "系统设置    ",
                NameCode = "SysSetting",
                ParentGuidList = ",a4b3b26f-076a-4267-b03d-613081b13a12,5ed17c74-0fff-4f88-8581-3b4f26d005a8,97f0f915-43fc-4d19-9ee2-0d1a5c21877b,",
                Layer = 3,
                Urls = "",
                Icon = "",
                Sort = 88,
                Status = true,
                BtnFunJson = "[]",
                EditTime = DateTime.Now,
                AddTIme = DateTime.Now,
            }).ExecuteCommand();


            Db.Insertable<SysMenu>(new SysMenu()
            {
                Guid = "b354ea64-88b6-4032-a26a-fee198e24d9d",
                SiteGuid = "",
                ParentGuid = "5ed17c74-0fff-4f88-8581-3b4f26d005a8",
                ParentName = "系统管理",
                Name = "系统日志    ",
                NameCode = "Log",
                ParentGuidList = ",a4b3b26f-076a-4267-b03d-613081b13a12,5ed17c74-0fff-4f88-8581-3b4f26d005a8,b354ea64-88b6-4032-a26a-fee198e24d9d,",
                Layer = 3,
                Urls = "",
                Icon = "",
                Sort = 145,
                Status = true,
                BtnFunJson = "[]",
                EditTime = DateTime.Now,
                AddTIme = DateTime.Now,
            }).ExecuteCommand();

            #endregion


        }

        public SimpleClient<FileAppseting> FileAppseting => new SimpleClient<FileAppseting>(Db);
        public SimpleClient<FileDocument> Documents => new SimpleClient<FileDocument>(Db);
        public SimpleClient<FT_File> FT_File => new SimpleClient<FT_File>(Db);
        public SimpleClient<FT_File_UserAuth> FT_File_UserAuth => new SimpleClient<FT_File_UserAuth>(Db);
        public SimpleClient<FT_File_Vesion> FT_File_Vesion => new SimpleClient<FT_File_Vesion>(Db);
        public SimpleClient<FT_Folder> FT_Folder => new SimpleClient<FT_Folder>(Db);
        public SimpleClient<Qycode> Qycode => new SimpleClient<Qycode>(Db);
        public SimpleClient<SysAdmin> UserInfo => new SimpleClient<SysAdmin>(Db);

        public SimpleClient<SysCode> SysCodeDb => new SimpleClient<SysCode>(Db);
        public SimpleClient<SysCodeType> SysCodeTypeDb => new SimpleClient<SysCodeType>(Db);
        public SimpleClient<SysOrganize> SysOrganizeDb => new SimpleClient<SysOrganize>(Db);
        public SimpleClient<SysLog> SysLogDb => new SimpleClient<SysLog>(Db);
        public SimpleClient<SysMenu> SysMenuDb => new SimpleClient<SysMenu>(Db);
        public SimpleClient<SysPermissions> SysPermissionsDb => new SimpleClient<SysPermissions>(Db);
        public SimpleClient<SysRole> SysRoleDb => new SimpleClient<SysRole>(Db);

        public SimpleClient<SysBtnFun> SysBtnFunDb => new SimpleClient<SysBtnFun>(Db);
    }
}
