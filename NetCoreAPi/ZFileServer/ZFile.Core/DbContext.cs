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
            Db.CodeFirst.BackupTable().InitTables<FileAppseting>();
            Db.CodeFirst.BackupTable().InitTables<FileDocument>();
            Db.CodeFirst.BackupTable().InitTables<FT_File>();
            Db.CodeFirst.BackupTable().InitTables<FT_File_UserAuth>();
            Db.CodeFirst.BackupTable().InitTables<FT_File_Vesion>();
            Db.CodeFirst.BackupTable().InitTables<FT_Folder>();
            Db.CodeFirst.BackupTable().InitTables<Qycode>();
            Db.CodeFirst.BackupTable().InitTables<UserInfo>();
            Db.CodeFirst.BackupTable().InitTables<UserLog>();
        }

        public SimpleClient<FileAppseting> FileAppseting => new SimpleClient<FileAppseting>(Db);
        public SimpleClient<FileDocument> Documents => new SimpleClient<FileDocument>(Db);
        public SimpleClient<FT_File> FT_File => new SimpleClient<FT_File>(Db);
        public SimpleClient<FT_File_UserAuth> FT_File_UserAuth => new SimpleClient<FT_File_UserAuth>(Db);
        public SimpleClient<FT_File_Vesion> FT_File_Vesion => new SimpleClient<FT_File_Vesion>(Db);
        public SimpleClient<FT_Folder> FT_Folder => new SimpleClient<FT_Folder>(Db);
        public SimpleClient<Qycode> Qycode => new SimpleClient<Qycode>(Db);
        public SimpleClient<UserInfo> UserInfo => new SimpleClient<UserInfo>(Db);
        public SimpleClient<UserLog> UserLogin => new SimpleClient<UserLog>(Db);
    }
}
