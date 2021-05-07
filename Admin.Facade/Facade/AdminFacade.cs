using Admin.Data.Factory;
using Admin.Entity.WebDB;
using Admin.IData;
using System.Collections.Generic;
using System.Data;
using Tool.SqlCore;
using Tool;

using Tool.Utils;
using System.Net;
using System.Text;

using System.Threading.Tasks;
using System;

namespace Admin.Facade.Facade
{
    public class AdminFacade
    {

        #region Fields

        private readonly IAdminDBInterface AdminData;

        #endregion

        #region 构造函数
        public AdminFacade()
        {
            AdminData = ClassFactory.GetIAdminDBProvider();
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
            return AdminData.GetBaseUserByBaseName(BaseName);
        }

        /// <summary>
        /// 根据玩家UserId获取用户
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public BaseUser GetBaseUserByUserId(int UserId)
        {
            return AdminData.GetBaseUserByUserId(UserId);
        }

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public BaseUser GetBaseUserByID(int ID)
        {
            return AdminData.GetBaseUserByID(ID);
        }

        /// <summary>
        /// 获取管理用户列表
        /// </summary>
        /// <param name="whereSort"></param>
        /// <returns></returns>
        public PagerSet GetBaseUserList(WhereSort whereSort)
        {
            return AdminData.GetPager(new PagerParameters
            {
                IsSql = false,
                Table = "V_BaseUser",
                WhereStr = whereSort.Where(),
                PKey = whereSort.IsSort("ORDER BY ID "),
                PageIndex = whereSort.Page,
                PageSize = whereSort.Limit
            });
        }

        /// <summary>
        /// 新增后台管理用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int InsertBaseUser(BaseUser info)
        {
            return AdminData.InsertBaseUser(info);
        }

        /// <summary>
        /// 修改后台管理用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateBaseUser(BaseUser info)
        {
            return AdminData.UpdateBaseUser(info);
        }

        /// <summary>
        ///  禁用启用登陆
        /// </summary>
        /// <param name="Id">管理员ID</param>
        /// <param name="Nullity">Nullity</param>
        /// <returns></returns>
        public int SetNullity(string Idlist, int Nullity)
        {
            return AdminData.SetNullity(Idlist, Nullity);
        }

        /// <summary>
        /// 谷歌验证
        /// </summary>
        /// <param name="idlist">ID</param>
        /// <param name="IsIdent">类型</param>
        /// <returns></returns>
        public int SetBaseIsIdent(string idlist, int IsIdent) 
        {
            return AdminData.SetBaseIsIdent(idlist, IsIdent);
        }

        /// <summary>
        /// 批量删除管理用户
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public int DeleteBaseUser(string idlist)
        {
            return AdminData.DeleteBaseUser(idlist);
        }

        /// <summary>
        /// 获取用户上次登陆的时间和地址
        /// </summary>
        /// <param name="BaseID"></param>
        /// <returns></returns>
        public DataTable GetBaseUserToLoginMsg(int BaseID)
        {
            return AdminData.GetBaseUserToLoginMsg(BaseID);
        }

        /// <summary>
        /// 修改后台管理用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateBaseUserToLoginMsg(int ID, DateTime LastLoginDate, string LastLoginIP, DateTime FinalLoginDate, string FinalLoginIP)
        {
            return AdminData.UpdateBaseUserToLoginMsg(ID, LastLoginDate, LastLoginIP, FinalLoginDate, FinalLoginIP);
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
            return AdminData.InsertBaseLog(log);
        }

        /// <summary>
        /// 获取操作日志列表
        /// </summary>
        /// <param name="whereSort">分页查询对象</param>
        /// <returns></returns>
        public PagerSet GetBaseLog(WhereSort whereSort)
        {
            return AdminData.GetPager(new PagerParameters
            {
                IsSql = false,
                Table = BaseLog.Tablename,
                WhereStr = whereSort.Where(),
                PKey = whereSort.IsSort("ORDER BY BaseTime DESC "),
                PageIndex = whereSort.Page,
                PageSize = whereSort.Limit
            });
        }

        /// <summary>
        /// 删除七天前的日志
        /// </summary>
        /// <returns></returns>
        public int DeleteBaseLog()
        {
            return AdminData.DeleteBaseLog();
        }

        #endregion

        #region 菜单操作模块

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetMenuByUserId()
        {
            return AdminData.GetMenuByUserId();
        }
        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <returns></returns>
        public DataSet GetPermissionByUserId(int ID)
        {
            return AdminData.GetPermissionByUserId(ID);
        }

        /// <summary>
        /// 获取所有菜单列表
        /// </summary>
        /// <param name="whereQuery"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagerSet GetPermissionDirectoryList(string whereQuery, int pageIndex, int pageSize)
        {
            return AdminData.GetPager(new PagerParameters
            {
                IsSql = false,
                Table = PermissionDirectory.Tablename,
                WhereStr = whereQuery,
                PKey = "ORDER BY ID ",
                PageIndex = pageIndex,
                PageSize = pageSize
            });
        }

        /// <summary>
        /// 根据id获取菜单列表和详细信息
        /// </summary>
        /// <param name="UserID">目标人ID</param>
        /// <returns></returns>
        public Message GetPermission(int ID)
        {
            return AdminData.GetPermission(ID);
        }

        /// <summary>
        /// 获取单独菜单对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public PermissionDirectory GetPermissionDirectory(int ID)
        {
            return AdminData.GetPermissionDirectory(ID);
        }
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public int InsertPermissionDirectory(PermissionDirectory Config)
        {
            return AdminData.InsertPermissionDirectory(Config);
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public int UpdatePermissionDirectory(PermissionDirectory Config)
        {
            return AdminData.UpdatePermissionDirectory(Config);
        }
        /// <summary>
        /// 批量删除菜单
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public int DeletePermissionDirectory(string idlist)
        {
            return AdminData.DeletePermissionDirectory(idlist);
        }
        #endregion

        #region 角色操作

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="whereSort"></param>
        /// <returns></returns>
        public PagerSet GetRoleDirectory(WhereSort whereSort)
        {
            return AdminData.GetPager(new PagerParameters
            {
                IsSql = false,
                Table = RoleDirectory.Tablename,
                WhereStr = whereSort.Where(),
                PKey = whereSort.IsSort("ORDER BY ID "),
                PageIndex = whereSort.Page,
                PageSize = whereSort.Limit
            });
        }

        /// <summary>
        /// 根据角色id获取角色信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public RoleDirectory GetRoleDirectoryByID(int ID)
        {
            return AdminData.GetRoleDirectoryByID(ID);
        }

        /// <summary>
        /// 根据角色名称获取角色信息
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public RoleDirectory GetRoleDirectoryRoleName(string RoleName)
        {
            return AdminData.GetRoleDirectoryRoleName(RoleName);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int InsertRoleDirectory(RoleDirectory info)
        {
            return AdminData.InsertRoleDirectory(info);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateRoleDirectory(RoleDirectory info)
        {
            return AdminData.UpdateRoleDirectory(info);
        }

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public int DeleteRoleDirectory(string idlist)
        {
            return AdminData.DeleteRoleDirectory(idlist);
        }

        /// <summary>
        /// 获取角色信息列表
        /// </summary>
        /// <returns></returns>
        public IList<RoleDirectory> GetRoleDirectoryList()
        {
            return AdminData.GetRoleDirectoryList();
        }

        /// <summary>
        /// 根据角色Id获取角色信息以及角色权限
        /// </summary>
        /// <param name="UserID">目标人ID</param>
        /// <returns></returns>
        public Message GetBaseUserInformation(int UserID)
        {
            return AdminData.GetBaseUserInformation(UserID);
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
            return AdminData.InsertAllocationDirectory(info);
        }
        #endregion

        #endregion

        #region 语句访问公共接口

        /// <summary>
        /// 设置日志通道
        /// </summary>
        /// <param name="logger"></param>
        public void SetLogger(Microsoft.Extensions.Logging.ILogger logger) => AdminData.SetLogger(logger);

        /// <summary>
        /// sql语句访问公共接口
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataSet SqlMessages(string sql)
        {
            return AdminData.SqlMessages(sql);
        }

        /// <summary>
        /// sql语句访问公共接口
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>受影响行数</returns>
        public int SqlMessAgesCounts(string sql)
        {
            return AdminData.SqlMessAgesCounts(sql);
        }

        #endregion

        #endregion
    }
}
