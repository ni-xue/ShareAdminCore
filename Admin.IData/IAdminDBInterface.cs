
using Admin.Entity.WebDB;

using System;
using System.Collections.Generic;
using System.Data;
using Tool.SqlCore;

namespace Admin.IData
{
    public interface IAdminDBInterface
    {

        #region 底层业务接口

        #region 管理后台逻辑

        #region 角色菜单权限模块

        #region 管理员用户模块
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="BaseName"></param>
        /// <returns></returns>
        BaseUser GetBaseUserByBaseName(string BaseName);


        /// <summary>
        /// 根据玩家UserId获取用户
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        BaseUser GetBaseUserByUserId(int UserId);


        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        BaseUser GetBaseUserByID(int ID);

        /// <summary>
        /// 新增后台管理用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        int InsertBaseUser(BaseUser info);

        /// <summary>
        /// 修改后台管理用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        int UpdateBaseUser(BaseUser info);

        /// <summary>
        ///  禁用启用登陆
        /// </summary>
        /// <param name="Id">管理员ID</param>
        /// <param name="Nullity">Nullity</param>
        /// <returns></returns>
        int SetNullity(string Idlist, int Nullity);

        /// <summary>
        /// 谷歌验证
        /// </summary>
        /// <param name="idlist">ID</param>
        /// <param name="IsIdent">类型</param>
        /// <returns></returns>
        int SetBaseIsIdent(string idlist, int IsIdent);

        /// <summary>
        /// 批量删除管理用户
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        int DeleteBaseUser(string idlist);

        /// <summary>
        /// 获取用户上次登陆的时间和地址
        /// </summary>
        /// <param name="BaseID"></param>
        /// <returns></returns>
        DataTable GetBaseUserToLoginMsg(int BaseID);

        /// <summary>
        /// 修改后台管理用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        int UpdateBaseUserToLoginMsg(int ID, DateTime LastLoginDate, string LastLoginIP, DateTime FinalLoginDate, string FinalLoginIP);

        #endregion

        #region 后台操作日志模块
        /// <summary>
        /// 写入操作日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        int InsertBaseLog(BaseLog log);

        /// <summary>
        /// 删除七天前的日志
        /// </summary>
        /// <returns></returns>
        int DeleteBaseLog();


        #endregion

        #region 菜单操作模块

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        DataSet GetMenuByUserId();

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <returns></returns>
        DataSet GetPermissionByUserId(int ID);

        /// <summary>
        /// 根据id获取菜单列表和详细信息
        /// </summary>
        /// <param name="UserID">目标人ID</param>
        /// <returns></returns>
        Message GetPermission(int ID);

        /// <summary>
        /// 获取单独菜单对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        PermissionDirectory GetPermissionDirectory(int ID);

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        int InsertPermissionDirectory(PermissionDirectory Config);

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        int UpdatePermissionDirectory(PermissionDirectory Config);

        /// <summary>
        /// 批量删除菜单
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        int DeletePermissionDirectory(string idlist);

        #endregion

        #region 角色操作

        /// <summary>
        /// 根据角色id获取角色信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        RoleDirectory GetRoleDirectoryByID(int ID);

        /// <summary>
        /// 根据角色名称获取角色信息
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        RoleDirectory GetRoleDirectoryRoleName(string RoleName);

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        int InsertRoleDirectory(RoleDirectory info);


        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        int UpdateRoleDirectory(RoleDirectory info);


        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        int DeleteRoleDirectory(string idlist);


        /// <summary>
        /// 获取角色信息列表
        /// </summary>
        /// <returns></returns>
        IList<RoleDirectory> GetRoleDirectoryList();


        /// <summary>
        /// 根据角色Id获取角色信息以及角色权限
        /// </summary>
        /// <param name="UserID">目标人ID</param>
        /// <returns></returns>
        Message GetBaseUserInformation(int UserID);


        #endregion

        #region 权限操作
        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        int InsertAllocationDirectory(AllocationDirectory info);

        #endregion

        #endregion

        #endregion

        #region sql语句访问公共接口

        /// <summary>
        /// 设置日志通道
        /// </summary>
        /// <param name="logger"></param>
        void SetLogger(Microsoft.Extensions.Logging.ILogger logger);

        /// <summary>
        /// sql语句访问公共接口
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        DataSet SqlMessages(string sql);


        /// <summary>
        /// sql语句访问公共接口
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>受影响行数</returns>
        int SqlMessAgesCounts(string sql);

        /// <summary>
        /// 分页公共接口
        /// </summary>
        /// <param name="parameters">分页数据对象</param>
        /// <returns></returns>
        PagerSet GetPager(PagerParameters parameters);

        /// <summary>
        /// 删除公共接口
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Where">删除条件</param>
        /// <returns>返回受影响行数</returns>
        int Delete(string Table, string Where);
        #endregion

        #endregion

    }
}
