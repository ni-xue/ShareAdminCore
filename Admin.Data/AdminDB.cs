using Admin.Data.Factory;

using Admin.Entity.WebDB;

using Admin.IData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Tool;
using Tool.SqlCore;
using Tool.Utils;
using Tool.Utils.Data;

namespace Admin.Data
{
    public sealed class AdminDB : BaseDataProvider, IAdminDBInterface
    {
        #region Fields

        #region 用户权限菜单模块
        /// <summary>
        /// 用户表访问对象
        /// </summary>
        private readonly ITableProvider aideIBaseUser;

        /// <summary>
        /// 操作日志访问对象
        /// </summary>
        private readonly ITableProvider aideIBaseLog;


        /// <summary>
        /// 菜单访问对象
        /// </summary>
        private readonly ITableProvider aideIPermissionDirectory;


        /// <summary>
        /// 角色访问对象
        /// </summary>
        private readonly ITableProvider aideIRoleDirectory;


        /// <summary>
        /// 用户权限访问对象
        /// </summary>
        private readonly ITableProvider aideIAllocationDirectory;


        #endregion

        #endregion

        #region 构造方法

        public AdminDB(string connString)
     : base(connString, DbProviderType.SqlServer1, new SqlServerProvider())
        {
            #region 用户权限菜单模块
            aideIBaseUser = GetTableProvider(BaseUser.Tablename);

            aideIBaseLog = GetTableProvider(BaseLog.Tablename);

            aideIPermissionDirectory = GetTableProvider(PermissionDirectory.Tablename);

            aideIRoleDirectory = GetTableProvider(RoleDirectory.Tablename);

            aideIAllocationDirectory = GetTableProvider(AllocationDirectory.Tablename);
            #endregion

            //base.IsSqlLog = false;
        }

        #endregion

        #region 语句访问公共接口

        /// <summary>
        /// 设置日志通道
        /// </summary>
        /// <param name="logger"></param>
        public void SetLogger(Microsoft.Extensions.Logging.ILogger logger) => Database.SetLogger(logger);

        /// <summary>
        /// sql语句访问公共接口
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataSet SqlMessages(string sql)
        {
            string SqlWhere = $"{sql}";
            return Database.ExecuteDataset(SqlWhere);
        }

        /// <summary>
        /// sql语句访问公共接口
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>受影响行数</returns>
        public int SqlMessAgesCounts(string sql)
        {
            string SqlWhere = $"{sql}";
            return Database.ExecuteNonQuery(SqlWhere);
        }

        /// <summary>
        /// 分页公共接口
        /// </summary>
        /// <param name="parameters">分页数据对象</param>
        /// <returns></returns>
        public PagerSet GetPager(PagerParameters parameters)
        {
            return GetPagerSet(parameters);
        }

        /// <summary>
        /// 删除公共接口
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Where">删除条件</param>
        /// <returns>返回受影响行数</returns>
        public int Delete(string Table, string Where)
        {
            return Database.ExecuteNonQuery($"DELETE FROM {Table} WHERE {Where}");
        }

        #endregion

        #region 管理后台逻辑

        #region 角色菜单权限模块

        #region 管理员用户模块
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="BaseName"></param>
        /// <returns></returns>
        public BaseUser GetBaseUserByBaseName(string BaseName)
        {
            BaseUser info = aideIBaseUser.GetObject<BaseUser>($" (NOLOCK)WHERE BaseName='{BaseName}'");
            return info;
        }

        /// <summary>
        /// 根据玩家UserId获取用户
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public BaseUser GetBaseUserByUserId(int UserId)
        {
            BaseUser info = aideIBaseUser.GetObject<BaseUser>($" (NOLOCK)WHERE UserId={UserId}");
            return info;
        }

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public BaseUser GetBaseUserByID(int ID)
        {
            BaseUser info = aideIBaseUser.GetObject<BaseUser>($" (NOLOCK)WHERE [ID]='{ID}'");
            return info;
        }

        /// <summary>
        /// 新增后台管理用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int InsertBaseUser(BaseUser info)
        {
            Dictionary<string, object> keyValuePairs = info.ToDictionary();
            keyValuePairs.Remove("ID");
            return aideIBaseUser.Insert(keyValuePairs);
        }

        /// <summary>
        /// 修改后台管理用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateBaseUser(BaseUser info)
        {
            Dictionary<string, object> keyValuePairs = info.ToDictionary();
            int ID = info.ID;
            keyValuePairs.Remove(BaseUser._ID);
            if (string.IsNullOrWhiteSpace(info.BasePwd))
            {
                keyValuePairs.Remove(BaseUser._BasePwd);
            }
            return aideIBaseUser.Update(keyValuePairs, $"id={ID}"); ;
        }

        /// <summary>
        ///  禁用启用登陆
        /// </summary>
        /// <param name="Id">管理员ID</param>
        /// <param name="Nullity">Nullity</param>
        /// <returns></returns>
        public int SetNullity(string Idlist, int Nullity)
        {
            if (string.IsNullOrWhiteSpace(Idlist))
            {
                throw new ArithmeticException("带入参数不能为空，参数错误");
            }
            Dictionary<string, object> keyValues = new()
            {
                { BaseUser._Nullity, Nullity }
            };
            return aideIBaseUser.Update(keyValues, $"[ID] IN ({Idlist})");
        }

        /// <summary>
        /// 解锁两步认证
        /// </summary>
        /// <returns></returns>
        public int SetBaseIsIdent(string idlist, int IsIdent)
        {
            Dictionary<string, object> keyValues = new()
            {
                { BaseUser._IsIdent, IsIdent }
            };
            return aideIBaseUser.Update(keyValues, $"[Id] in ({idlist})");
        }

        /// <summary>
        /// 批量删除管理用户
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public int DeleteBaseUser(string idlist)
        {
            return aideIBaseUser.Delete($"ID in({idlist})");
        }

        /// <summary>
        /// 获取用户上次登陆的时间和地址
        /// </summary>
        /// <param name="BaseID"></param>
        /// <returns></returns>
        public DataTable GetBaseUserToLoginMsg(int BaseID)
        {
            string sql = $"select top 1 BaseIp,BaseTime from [dbo].[BaseLog] where BaseID={BaseID} and ModuleType='Login' order by ID desc ";
            return Database.Select(sql);//aideIBaseLog
        }

        /// <summary>
        /// 修改后台管理用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateBaseUserToLoginMsg(int ID, DateTime LastLoginDate, string LastLoginIP, DateTime FinalLoginDate, string FinalLoginIP)
        {
            Dictionary<string, object> keyValues = new();
            keyValues.Add(BaseUser._LastLoginDate, LastLoginDate);
            keyValues.Add(BaseUser._LastLoginIP, LastLoginIP);
            keyValues.Add(BaseUser._FinalLoginDate, FinalLoginDate);
            keyValues.Add(BaseUser._FinalLoginIP, FinalLoginIP);
            return aideIBaseUser.Update(keyValues, $"ID={ID}"); ;
        }

        #endregion

        #region 后台操作日志模块

        /// <summary>
        /// 写入操作日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int InsertBaseLog(BaseLog log)
        {
            Dictionary<string, object> keyValuePairs = log.ToDictionary();
            keyValuePairs.Remove("ID");
            return aideIBaseLog.Insert(keyValuePairs);
        }

        /// <summary>
        /// 删除七天前的日志
        /// </summary>
        /// <returns></returns>
        public int DeleteBaseLog()
        {
            return aideIBaseLog.Delete($"BaseTime < dateadd(day,-7,getdate()) ");
        }

        #endregion

        #region 菜单操作模块

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetMenuByUserId()
        {
            return Database.ExecuteDataset(CommandType.StoredProcedure, "NET_PM_GetMenuByUserID");
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <returns></returns>
        public DataSet GetPermissionByUserId(int ID)
        {
            var prams = new List<DbParameter> { Database.GetInParam("dwUserID", ID) };

            return Database.ExecuteDataset(CommandType.StoredProcedure, "NET_PM_GetPermissionByUserID", prams.ToArray());
        }

        /// <summary>
        /// 根据id获取菜单列表和详细信息
        /// </summary>
        /// <param name="UserID">目标人ID</param>
        /// <returns></returns>
        public Message GetPermission(int ID)
        {
            var prams = new List<DbParameter>
            {
                Database.GetInParam("strID", ID),
                Database.GetOutParam("strErrorDescribe", typeof(string), 127)
            };
            return MessageHelper.GetMessageForDataSet(Database, "NET_PM_GetPermission", prams);
        }

        /// <summary>
        /// 获取单独菜单对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public PermissionDirectory GetPermissionDirectory(int ID)
        {
            PermissionDirectory Config = aideIPermissionDirectory.GetObject<PermissionDirectory>($" (NOLOCK)WHERE [ID]={ID}");
            return Config;
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public int InsertPermissionDirectory(PermissionDirectory Config)
        {
            Dictionary<string, object> keyValuePairs = Config.ToDictionary();
            keyValuePairs.Remove("Id");
            return aideIPermissionDirectory.Insert(keyValuePairs);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public int UpdatePermissionDirectory(PermissionDirectory Config)
        {
            Dictionary<string, object> keyValuePairs = Config.ToDictionary();
            keyValuePairs.Remove("Id");
            return aideIPermissionDirectory.Update(keyValuePairs, $"id={Config.Id}");
        }
        /// <summary>
        /// 批量删除菜单
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public int DeletePermissionDirectory(string idlist)
        {
            return aideIPermissionDirectory.Delete($"ID in({idlist})");
        }

        #endregion

        #region 角色操作

        /// <summary>
        /// 根据角色id获取角色信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public RoleDirectory GetRoleDirectoryByID(int ID)
        {
            RoleDirectory info = aideIRoleDirectory.GetObject<RoleDirectory>($" (NOLOCK)WHERE [ID]={ID}");
            return info;
        }

        /// <summary>
        /// 根据角色名称获取角色信息
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public RoleDirectory GetRoleDirectoryRoleName(string RoleName)
        {
            RoleDirectory info = aideIRoleDirectory.GetObject<RoleDirectory>($" (NOLOCK)WHERE [RoleName]=N'{RoleName}'");
            return info;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int InsertRoleDirectory(RoleDirectory info)
        {
            Dictionary<string, object> keyValuePairs = info.ToDictionary();
            keyValuePairs.Remove("Id");
            return aideIRoleDirectory.Insert(keyValuePairs);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateRoleDirectory(RoleDirectory info)
        {
            Dictionary<string, object> keyValuePairs = info.ToDictionary();
            keyValuePairs.Remove("Id");
            return aideIRoleDirectory.Update(keyValuePairs, $"id={info.Id}");
        }

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public int DeleteRoleDirectory(string idlist)
        {
            return aideIRoleDirectory.Delete($"ID in({idlist})");
        }

        /// <summary>
        /// 获取角色信息列表
        /// </summary>
        /// <returns></returns>
        public IList<RoleDirectory> GetRoleDirectoryList()
        {
            return aideIRoleDirectory.GetObjectList<RoleDirectory>(string.Format(" (NOLOCK)WHERE 1=1"));
        }

        /// <summary>
        /// 根据角色Id获取角色信息以及角色权限
        /// </summary>
        /// <param name="UserID">目标人ID</param>
        /// <returns></returns>
        public Message GetBaseUserInformation(int UserID)
        {
            var prams = new List<DbParameter>
            {
                Database.GetInParam("strUserID", UserID),
                Database.GetOutParam("strErrorDescribe", typeof(string), 127)
            };
            return MessageHelper.GetMessageForDataSet(Database, "NET_PM_GetRoleInformation", prams);
        }

        #endregion

        #region 权限操作

        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int InsertAllocationDirectory(AllocationDirectory info)
        {
            Dictionary<string, object> keyValuePairs = info.ToDictionary();
            keyValuePairs.Remove("Id");
            return aideIAllocationDirectory.Insert(keyValuePairs);
        }

        #endregion

        #endregion

        #endregion
    }
}
