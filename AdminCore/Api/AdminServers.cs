using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Admin.Entity.WebDB;
using Admin.Facade;
using Admin.Facade.PermissionsMenu;
using Tool;
using Tool.SqlCore;
using Tool.Utils;
using Tool.Utils.Data;
using Tool.Web;
using Tool.Web.Api;

namespace AdminCore.Api
{
    public class AdminServers : ApiAshx
    {
        public const string LogFilePath = "Log/API/Admin/";

        /// <summary>
        /// 当前角色对这个页面的权限信息
        /// </summary>
        private Rout R_Rout;
        /// <summary>
        /// 当前角色的信息
        /// </summary>
        private BaseUser anmininfo;

        /// <summary>
        /// 获取当前接口的权限
        /// </summary>
        private MvcView mvc;

        protected override bool Initialize(Ashx ashx)
        {
            if (ashx.TryGetValue(out mvc))
            {
                if (Session.TryGetValue("Admin", out anmininfo))
                {
                    Rout rout = new("Admin", mvc);
                    var routs = Session.Get<List<Rout>>("Routs");
                    if (Rout.Contains(routs, rout, out R_Rout))
                    {
                        if (mvc.IsAction && !Rout.IsActionEnum(rout.Authority, R_Rout.PermissionType))
                        {
                            Json(new { code = 2010, msg = "您不具备访问该API的权限！" });
                            return false;
                        }
                        else
                        {
                            if (mvc.Action == ActionEnum.Delete)
                            {
                                //加入操作日志
                                AddBaseLog(mvc);
                            }
                        }
                    }
                }
                else
                {
                    Json(new { code = 2001, msg = "用户未登录！" });
                    return false;
                }
            }
            return true;
        }

        #region  权限模块

        #region 菜单管理

        [MvcView("Index", ActionEnum.View)]
        [Ashx(ID = "GetMenusInfos", State = AshxState.Post)]
        public async Task GetMenusInfo()
        {
            var menus = Session.Get<List<MenuHead>>("Menu");
            var userInfo = Session.Get<UserInfo>("UserInfo");
            var menusInfo = Menu.GetMenusInfoResult(menus, userInfo);
            var options = RouteData.GetNewJsonOptions();
            options.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            await JsonAsync(new { code = 0, msg = "获取成功！", data = menusInfo }, options);
        }

        /// <summary>
        /// 更新登陆时间和登陆地址
        /// </summary>
        /// <param name="ID"></param>
        private void EditLoginMsg(int ID)
        {
            DataTable dt = FacadeManage.AideAdminFacade.GetBaseUserToLoginMsg(ID);
            DateTime date = DateTime.Now;
            string ip = Context.GetIP();
            if (!dt.IsEmpty())
            {
                FacadeManage.AideAdminFacade.UpdateBaseUserToLoginMsg(ID, dt.Rows[0]["BaseTime"].ToVar<DateTime>(), dt.Rows[0]["BaseIp"].ToString(), date, ip);
            }
            else
            {
                FacadeManage.AideAdminFacade.UpdateBaseUserToLoginMsg(ID, date, ip, date, ip);
            }
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        [MvcView("MenuList", ActionEnum.View)]
        [Ashx(ID = "GetPermissionDirectoryLists", State = AshxState.Post)]
        public async Task GetPermissionDirectoryList()
        {
            AjaxJson _ajv = new();

            var PermissionDirectoryList = Menu.GetMenus(FacadeManage.AideAdminFacade.GetMenuByUserId());
            if (PermissionDirectoryList != null)
            {
                _ajv.code = 0;
                _ajv.msg = "请求成功！";
                _ajv.SetDataItem("list", PermissionDirectoryList);
            }
            else
            {
                _ajv.code = 1;
                _ajv.msg = "获取失败！";
            }
            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idlist"></param>
        [Ashx(ID = "DeletePermissionDirectorys", State = AshxState.Post)]
        [MvcView("MenuList", ActionEnum.Delete, "菜单删除")]
        public async Task DeletePermissionDirectory(string idlist)
        {
            AjaxJson _ajv = new();
            if (idlist != "")
            {
                if (FacadeManage.AideAdminFacade.DeletePermissionDirectory(idlist) > 0)
                {
                    _ajv.code = 0;
                    _ajv.msg = "删除成功！";
                }
                else
                {
                    _ajv.code = 1;
                    _ajv.msg = "删除失败！";
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "删除ID为空！";
            }

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 根据id获取菜单列表和详细信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        [Ashx(State = AshxState.Post)]
        public async Task GetPermDireID(int ID)
        {
            AjaxJson _ajv = new();

            if (ID != 0)
            {
                var RoleDirID = FacadeManage.AideAdminFacade.GetPermission(ID);

                if (RoleDirID.Success)
                {
                    var data = RoleDirID.EntityList[0].ToVar<DataSet>();
                    var menu = Menu.GetMenusClip(data);
                    _ajv.code = RoleDirID.MessageID;
                    _ajv.msg = RoleDirID.Content;
                    _ajv.SetDataItem("list", menu);
                    if (ID > 0 && data.Tables.Count == 3)
                    {
                        _ajv.SetDataItem("Roles", data.Tables[2].ToEntity<PermissionDirectory>());
                    }
                }
                else
                {
                    _ajv.code = RoleDirID.MessageID;
                    _ajv.msg = RoleDirID.Content;
                }
            }
            else
            {
                _ajv.code = 10;
                _ajv.msg = "角色ID不能为空！";
            }

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="json"></param>
        [Ashx(ID = "UpdatePermissionDirectorys", State = AshxState.Post)]
        [MvcView("MenuList", ActionEnum.Edit, "菜单修改")]
        public async Task UpdatePermissionDirectory([ApiVal(Val.FormMode)] PermissionDirectory Config)
        {
            AjaxJson _ajv = new();

            if (Config != null)
            {
                if (FacadeManage.AideAdminFacade.UpdatePermissionDirectory(Config) > 0)
                {
                    AddBaseLog(mvc, Config.ToJson());
                    _ajv.code = 0;
                    _ajv.msg = "修改成功！";
                }
                else
                {
                    _ajv.code = 1;
                    _ajv.msg = "修改失败！";
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "对象为空！";
            }

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="json"></param>
        [Ashx(ID = "InsertPermissionDirectorys", State = AshxState.Post)]
        [MvcView("MenuList", ActionEnum.Add, "菜单新增")]
        public async Task InsertPermissionDirectory([ApiVal(Val.FormMode)] PermissionDirectory Config)
        {
            AjaxJson _ajv = new();
            if (Config != null)
            {
                if (FacadeManage.AideAdminFacade.InsertPermissionDirectory(Config) > 0)
                {
                    _ajv.code = 0;
                    _ajv.msg = "添加成功！";
                    AddBaseLog(mvc, Config.ToJson());
                }
                else
                {
                    _ajv.code = 1;
                    _ajv.msg = "添加失败！";
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "对象为空！";
            }

            await JsonAsync(_ajv);
        }

        #endregion

        #region 管理员管理

        /// <summary>
        /// 获取管理用户列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        [Ashx(ID = "GetBaseUserLists", State = AshxState.Post)]
        [MvcView("ManageList", ActionEnum.View, "获取管理用户列表")]
        public async Task GetBaseUserList(string BaseName, [ApiVal(Val.FormMode)] WhereSort whereSort)
        {
            AjaxJson _ajv = new();

            if (!string.IsNullOrEmpty(BaseName))
            {
                whereSort.Append($"and BaseName='{BaseName}'");
            }
            var BaseUserList = FacadeManage.AideAdminFacade.GetBaseUserList(whereSort);

            _ajv.code = 0;
            _ajv.msg = "请求成功！";

            _ajv.SetDataItem("PageCount", BaseUserList.PageCount);

            _ajv.SetDataItem("RecordCount", BaseUserList.RecordCount);

            _ajv.SetDataItem("PageIndex", BaseUserList.PageIndex);

            _ajv.SetDataItem("PageSize", BaseUserList.PageSize);

            _ajv.SetDataItem("list", BaseUserList.PageTable.ToDictionary());

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 强制下线
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        [Ashx(ID = "ManageLogouts", State = AshxState.Post)]
        [MvcView("ManageList", ActionEnum.Edit, "强制下线")]
        public async Task ManageLogout(string UserName)
        {
            AjaxJson _ajv = new();
            _ajv.code = RoleAction.Logout(UserName) ? 0 : 1;
            _ajv.msg = "成功";//_ajv.code == 1 ? "失败" : "成功";
            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 获取管理用户详情
        /// </summary>
        /// <param name="ID"></param>
        [Ashx(ID = "GetBaseUsers", State = AshxState.Post)]
        [MvcView("ManageList", ActionEnum.View, "获取管理用户详情")]
        public async Task GetBaseUser(int ID)
        {
            AjaxJson _ajv = new();

            var RoleDirectoryList = FacadeManage.AideAdminFacade.GetRoleDirectoryList();

            _ajv.SetDataItem("RoleDirectoryList", RoleDirectoryList);

            _ajv.code = 0;
            _ajv.msg = "请求成功！";

            if (ID != 0)
            {
                var BaseUserList = FacadeManage.AideAdminFacade.GetBaseUserByID(ID);

                if (BaseUserList != null)
                {
                    _ajv.SetDataItem("list", BaseUserList);
                }
                else
                {
                    _ajv.code = 1;

                    _ajv.msg = "用户不存在！";
                }
            }
            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 新增管理用户
        /// </summary>
        /// <param name="json"></param>
        [Ashx(ID = "InsertBaseUsers", State = AshxState.Post)]
        [MvcView("ManageList", ActionEnum.Add, "管理员新增")]
        public async Task InsertBaseUser([ApiVal(Val.FormMode)] BaseUser info)
        {
            AjaxJson _ajv = new();
            if (info != null)
            {
                if (!string.IsNullOrWhiteSpace(info.BasePwd))
                {
                    if (info.BaseName.Contains("null"))
                    {
                        info.BasePwd = null;
                    }
                    else
                    {
                        info.BasePwd = info.BasePwd.MD5Upper();
                    }
                }
                if (FacadeManage.AideAdminFacade.InsertBaseUser(info) > 0)
                {
                    //加入操作日志
                    AddBaseLog(mvc, info.ToJson());

                    _ajv.code = 0;
                    _ajv.msg = "添加成功！";
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "对象为空！";
            }

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 修改管理用户
        /// </summary>
        /// <param name="json"></param>
        [Ashx(ID = "UpdateBaseUsers", State = AshxState.Post)]
        [MvcView("ManageList", ActionEnum.Edit, "管理员修改")]
        public async Task UpdateBaseUser([ApiVal(Val.FormMode)] BaseUser info)
        {
            AjaxJson _ajv = new();
            if (info != null)
            {
                if (!string.IsNullOrWhiteSpace(info.BasePwd))
                {
                    if (info.BasePwd.Contains("null"))
                    {
                        info.BasePwd = null;
                    }
                    else
                    {
                        info.BasePwd = info.BasePwd.MD5Upper();
                    }
                }
                if (FacadeManage.AideAdminFacade.UpdateBaseUser(info) > 0)
                {
                    //加入操作日志
                    AddBaseLog(mvc, info.ToJson());
                    _ajv.code = 0;
                    _ajv.msg = "修改成功！";
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "对象为空！";
            }

            await JsonAsync(_ajv);
        }

        ///// <summary>
        ///// 修改密码
        ///// </summary>
        ///// <param name="OldPassword"></param>
        ///// <param name="Password"></param>
        ///// <param name="Passwordt"></param>
        //[MvcView("", ActionEnum.Edit, "修改管理员密码")]
        //[Ashx(ID = "UpdatePasswords", State = AshxState.Post)]
        //public async Task UpdatePassword(string OldPassword, string Password, string Passwordt)
        //{
        //    await Task.Run(() =>
        //    {
        //        AjaxJson _ajv = new();
        //        if (string.IsNullOrEmpty(OldPassword))
        //        {
        //            _ajv.code = 200;
        //            _ajv.msg = "请输入旧密码！";
        //            await JsonAsync(_ajv);
        //            return;
        //        }
        //        if (string.IsNullOrEmpty(Password))
        //        {
        //            _ajv.code = 200;
        //            _ajv.msg = "请输入新密码！";
        //            await JsonAsync(_ajv);
        //            return;
        //        }
        //        if (string.IsNullOrEmpty(Passwordt))
        //        {
        //            _ajv.code = 200;
        //            _ajv.msg = "请输入确认密码！";
        //            await JsonAsync(_ajv);
        //            return;
        //        }
        //        BaseUser info = FacadeManage.AideAdminFacade.GetBaseUserByBaseName(anmininfo.BaseName);
        //        if (info != null)
        //        {
        //            if (OldPassword.MD5Upper() != info.BasePwd)
        //            {
        //                _ajv.code = 200;
        //                _ajv.msg = "旧密码输入错误！";
        //                await JsonAsync(_ajv);
        //                return;
        //            }
        //            if (Password != Passwordt)
        //            {
        //                _ajv.code = 200;
        //                _ajv.msg = "两次密码不一致！";
        //                await JsonAsync(_ajv);
        //                return;
        //            }
        //            info.BasePwd = Password.MD5Upper();
        //            if (FacadeManage.AideAdminFacade.UpdateBaseUser(info) > 0)
        //            {
        //                //加入操作日志
        //                FacadeManage.AideAdminFacade.InsertBaseLog(new BaseLog
        //                {
        //                    BaseID = anmininfo.ID,
        //                    BaseName = anmininfo.BaseName,
        //                    ModuleType = mvc.Action.ToString(),
        //                    ModuleText = mvc.Remark + info.ToJson(),
        //                    BaseIp = Context.GetIP(),
        //                    KindID = 0,
        //                    ServerID = 0
        //                });

        //                _ajv.code = 1;
        //                _ajv.msg = "修改成功！";
        //                await JsonAsync(_ajv);
        //                return;
        //            }
        //        }
        //    });
        //}

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idlist"></param>
        [MvcView("ManageList", ActionEnum.Delete, "管理员删除")]
        [Ashx(ID = "DeleteBaseUsers", State = AshxState.Post)]
        public async Task DeleteBaseUser(string idlist)
        {
            AjaxJson _ajv = new();
            if (idlist != "")
            {
                if (idlist.Contains(anmininfo.ID.ToString()))
                {
                    _ajv.code = 101;
                    _ajv.msg = "不能删除含有自己的账号！";
                }
                else
                {
                    if (FacadeManage.AideAdminFacade.DeleteBaseUser(idlist) > 0)
                    {
                        _ajv.code = 0;
                        _ajv.msg = "删除成功！";
                    }
                    else
                    {
                        _ajv.code = 1;
                        _ajv.msg = "删除失败！";
                    }
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "删除ID为空！";
            }
            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 禁用或启用管理员
        /// </summary>
        /// <param name="idlist"></param>
        [MvcView("ManageList", ActionEnum.Edit, "禁用/启用管理员")]
        [Ashx(ID = "SetNullitys", State = AshxState.Post)]
        public async Task SetNullity(string idlist, int Nullity)
        {
            AjaxJson _ajv = new();
            if (idlist != "")
            {
                if (FacadeManage.AideAdminFacade.SetNullity(idlist, Nullity) > 0)
                {
                    //加入操作日志
                    AddBaseLog(mvc, "idlist:" + idlist + "; Nullity:" + Nullity);

                    _ajv.code = 0;
                    _ajv.msg = "操作成功！";
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "删除ID为空！";
            }
            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 管理员解绑两步认证
        /// </summary>
        /// <param name="idlist"></param>
        [MvcView("ManageList", ActionEnum.Edit, "管理员解绑两步认证")]
        [Ashx(ID = "SetBaseIsIdents", State = AshxState.Post)]
        public async Task SetBaseIsIdent(string idlist, int isIdent)
        {
            AjaxJson _ajv = new();
            if (idlist != "")
            {
                if (FacadeManage.AideAdminFacade.SetBaseIsIdent(idlist, isIdent) > 0)
                {
                    //加入操作日志
                    AddBaseLog(mvc, "idlist:" + idlist + "; isIdent:" + isIdent);

                    _ajv.code = 0;
                    _ajv.msg = "解绑成功！";
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "删除ID为空！";
            }
            await JsonAsync(_ajv);
        }

        #endregion

        #region 日志管理

        /// <summary>
        /// 获取操作日志列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        [Ashx(ID = "GetBaseLogs", State = AshxState.Post)]
        [MvcView("ManageLogList", ActionEnum.View, "获取操作日志列表")]
        public async Task GetBaseLog(string BaseName, string ModuleType, string StartTime, string EndTime, [ApiVal(Val.FormMode)] WhereSort whereSort)
        {
            AjaxJson _ajv = new();

            if (!string.IsNullOrEmpty(BaseName))
            {
                whereSort.Append($"and BaseName='{BaseName}'");
            }
            if (ModuleType != "all")
            {
                whereSort.Append($"and ModuleType='{ModuleType}'");
            }
            if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
            {
                whereSort.Append($"and BaseTime>='{StartTime} 00:00:00' and BaseTime<='{EndTime} 23:59:59'");
            }

            var BaseLogList = FacadeManage.AideAdminFacade.GetBaseLog(whereSort);

            _ajv.code = 0;
            _ajv.msg = "请求成功！";

            _ajv.SetPage(BaseLogList);

            _ajv.SetDataItem("list", BaseLogList.PageTable.ToDictionary());

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [Ashx(ID = "DeleteBaseLogs", State = AshxState.Post)]
        [MvcView("ManageLogList", ActionEnum.Delete, "删除日志")]
        public async Task DeleteBaseLog()
        {
            AjaxJson _ajv = new();

            if (FacadeManage.AideAdminFacade.DeleteBaseLog() > 0)
            {
                _ajv.code = 0;
                _ajv.msg = "删除成功！";
            }
            else
            {
                _ajv.code = 1;
                _ajv.msg = "已删除七天内数据！";
            }
            await JsonAsync(_ajv);
        }

        #endregion

        #region 角色管理

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        [Ashx(ID = "GetRoleDirectorys", State = AshxState.Post)]
        [MvcView("RoleManageList", ActionEnum.View, "获取角色列表")]
        public async Task GetRoleDirectory(string RoleName, [ApiVal(Val.FormMode)] WhereSort whereSort)
        {
            AjaxJson _ajv = new();

            if (!string.IsNullOrWhiteSpace(RoleName))
            {
                whereSort.Append($"AND RoleName LIKE N'%{RoleName}%'");
            }

            var RoleDirectoryList = FacadeManage.AideAdminFacade.GetRoleDirectory(whereSort);

            _ajv.code = 0;
            _ajv.msg = "请求成功！";

            _ajv.SetPage(RoleDirectoryList);

            _ajv.SetDataItem("list", RoleDirectoryList.PageTable.ToDictionary());

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 根据id获取角色信息
        /// </summary>
        /// <param name="ID">角色id</param>
        [Ashx(State = AshxState.Post)]
        [MvcView("RoleManageList", ActionEnum.View, "根据id获取角色信息")]
        public async Task GetRoleDirectoryID(int ID)
        {
            AjaxJson _ajv = new();

            if (ID != 0)
            {
                var RoleDirID = FacadeManage.AideAdminFacade.GetBaseUserInformation(ID);

                if (RoleDirID.Success)
                {
                    var data = RoleDirID.EntityList[0].ToVar<DataSet>();
                    var menu = Menu.GetMenus(data);
                    _ajv.code = RoleDirID.MessageID;
                    _ajv.msg = RoleDirID.Content;
                    _ajv.SetDataItem("list", menu);
                    //_ajv.SetDataSet(data);
                    if (ID > 0 && data.Tables.Count == 4)
                    {
                        _ajv.SetDataItem("Roles", new { Id = data.Tables[3].Rows[0][0], RoleName = data.Tables[3].Rows[0][1], Descriptions = data.Tables[3].Rows[0][2], HomeState = data.Tables[3].Rows[0][3] });
                    }
                }
                else
                {
                    _ajv.code = RoleDirID.MessageID;
                    _ajv.msg = RoleDirID.Content;
                }
            }
            else
            {
                _ajv.code = 10;
                _ajv.msg = "角色ID不能为空！";
            }
            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="json"></param>
        [Ashx(ID = "InsertRoleDirectorys", State = AshxState.Post)]
        [MvcView("RoleManageList", ActionEnum.Add, "角色新增")]
        public async Task InsertRoleDirectory([ApiVal(Val.FormMode)] RoleDirectory Config, string json)
        {
            AjaxJson _ajv = new();

            if (Config != null)
            {
                if (FacadeManage.AideAdminFacade.GetRoleDirectoryRoleName(Config.RoleName) != null)
                {
                    _ajv.code = 2;
                    _ajv.msg = "角色名已存在！";
                }
                else
                {
                    if (FacadeManage.AideAdminFacade.InsertRoleDirectory(Config) > 0)
                    {
                        Config.Id = FacadeManage.AideAdminFacade.GetRoleDirectoryRoleName(Config.RoleName).Id;
                        UserPermissionOperation userPermission = new(json, Config.Id);

                        var con = userPermission.Roles.Count > 0 ? FacadeManage.AideAdminFacade.SqlMessAgesCounts(userPermission.ToString()) : 0;

                        //加入操作日志
                        AddBaseLog(mvc, Config.ToJson());

                        if (userPermission.Roles.Count == con)
                        {
                            _ajv.msg = "添加成功（权限也成功）！";
                        }
                        else
                        {
                            _ajv.msg = "添加成功（不完全）！";
                        }
                        _ajv.code = 0;
                    }
                    else
                    {
                        _ajv.code = 1;
                        _ajv.msg = "修改失败！";
                    }
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "对象为空！";
            }

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="json"></param>
        [Ashx(ID = "UpdateRoleDirectorys", State = AshxState.Post)]
        [MvcView("RoleManageList", ActionEnum.Edit, "角色修改")]
        public async Task UpdateRoleDirectory([ApiVal(Val.FormMode)] RoleDirectory Config, string json)
        {
            AjaxJson _ajv = new();
            if (Config != null)
            {
                if (Config.Id == 0)
                {
                    _ajv.code = 100;
                    _ajv.msg = "角色ID不存在！";
                }
                else
                {
                    if (FacadeManage.AideAdminFacade.UpdateRoleDirectory(Config) > 0)
                    {
                        UserPermissionOperation userPermission = new(json, Config.Id);

                        var con = userPermission.Roles.Count > 0 ? FacadeManage.AideAdminFacade.SqlMessAgesCounts(userPermission.ToString()) : 0;

                        _ajv.code = 0;

                        //加入操作日志
                        AddBaseLog(mvc, Config.ToJson());

                        if (userPermission.Roles.Count == con)
                        {
                            _ajv.msg = "修改成功（权限也成功）！";
                        }
                        else
                        {
                            _ajv.msg = "修改成功（不完全）！";
                        }
                    }
                    else
                    {
                        _ajv.code = 1;
                        _ajv.msg = "修改失败！";
                    }
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "对象为空！";
            }

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idlist"></param>
        [Ashx(ID = "DeleteRoleDirectorys", State = AshxState.Post)]
        [MvcView("RoleManageList", ActionEnum.Delete, "角色删除")]
        public async Task DeleteRoleDirectory(string idlist)
        {
            AjaxJson _ajv = new();
            if (!string.IsNullOrWhiteSpace(idlist))
            {
                try
                {
                    if (FacadeManage.AideAdminFacade.DeleteRoleDirectory(idlist) > 0)
                    {
                        _ajv.code = 0;
                        _ajv.msg = "删除成功！";
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"批量删除角色时出现异常：（异常数据：{idlist}）", e);
                    _ajv.code = 1;
                    _ajv.msg = "非法操作！";
                }
            }
            else
            {
                _ajv.code = 100;
                _ajv.msg = "删除ID为空！";
            }
            await JsonAsync(_ajv);

        }
        #endregion

        #region 管理员登陆

        /// <summary>
        /// 管理后台账号登录接口
        /// </summary>
        /// <param name="BaseName"></param>
        /// <param name="BasePwd"></param>
        /// <param name="Code"></param>
        [Ashx(State = AshxState.Post)]
        public async Task Login(string BaseName, string BasePwd, string Code)
        {
            AjaxJson _ajv = new();

            var code = Session.Get("GetCode");

            if (string.IsNullOrEmpty(BaseName) || string.IsNullOrEmpty(BasePwd))
            {
                _ajv.code = 10;
                _ajv.msg = "用户ID和密码不能为空！";
            }
            else if (code == null)
            {
                _ajv.code = 10;
                _ajv.msg = "验证码已过期！";
            }
            else if (!Code.Equals(code))
            {
                _ajv.code = 10;
                _ajv.msg = "验证码错误！";
            }
            else
            {
                anmininfo = FacadeManage.AideAdminFacade.GetBaseUserByBaseName(BaseName);
                if (anmininfo != null)
                {
                    Session.Remove("GetCode");
                    if (BasePwd.MD5Upper() != anmininfo.BasePwd)
                    {
                        _ajv.code = 4;
                        _ajv.msg = "密码错误！";
                    }
                    else if (anmininfo.Nullity == 1)
                    {
                        _ajv.code = 101;
                        _ajv.msg = "您的账号已被冻结，请联系超管！";
                    }
                    else
                    {
                        //登录成功保存用户信息--谷歌验证部分
                        Session.Set("IdentAdmin", anmininfo);
                        //写入登录日志
                        AddBaseLog("Login", "登录");

                        //成功后，进入第二部验证
                        _ajv.code = 0;
                        _ajv.msg = "登录成功！";
                        _ajv.SetDataItem("IsIdent", anmininfo.IsIdent);
                    }
                }
                else
                {
                    _ajv.code = 2;
                    _ajv.msg = "用户名不存在！";
                }
            }
            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 登陆验证
        /// </summary>
        [Ashx(ID = "IGetCode", State = AshxState.Post)]
        public async Task GetCode()
        {
            AjaxJson _ajv = new();
            string code = VerificCodeHelper.GetRandomCodeV2(out int val);
            var codeImage = VerificCodeHelper.GetVCode(code);
            Session.Set("GetCode", val);
            _ajv.code = 0;
            _ajv.msg = "登陆验证";
            _ajv.SetDataItem("Code", "data:image/jpeg;base64," + codeImage.ToBase64());

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 获取谷歌验证码
        /// </summary>
        [Ashx(ID = "IGetggCode", State = AshxState.Post)]
        public async Task GetggCode()
        {
            AjaxJson _ajv = new();

            if (Session.TryGetValue("IdentAdmin", out anmininfo))
            {
                if (anmininfo.IsIdent == 0)
                {
                    GoogleAuthenticator authenticator = new(30, key: $"{anmininfo.BaseName}{anmininfo.ID}{8}");
                    var mobileKey = authenticator.GetMobilePhoneKey();
                    string code = HelpClass.GetQrBase64Imageg("FKHT", anmininfo.BaseName, mobileKey);
                    _ajv.code = 0;
                    _ajv.SetDataItem("ggCode", code);
                }
                else
                {
                    _ajv.code = 5;
                    _ajv.msg = "无法获取二维码，因为已经绑定过了！";
                }
            }
            else
            {
                _ajv.code = 1;
                _ajv.msg = "请刷新界面重新登录！";

            }

            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 验证谷歌验证码
        /// </summary>
        [Ashx(ID = "IIsggCodes", State = AshxState.Post)]
        public async Task IsggCodes(string iscode, string url)
        {
            AjaxJson _ajv = new();

            if (Session.TryGetValue("IdentAdmin", out anmininfo))
            {
                GoogleAuthenticator authenticator = new(30, key: $"{anmininfo.BaseName}{anmininfo.ID}{8}");
                var mobileKey = authenticator.GenerateCode();
#if DEBUG
                mobileKey = iscode;
#endif
                if (!RoleAction.IsRepeatLogin(anmininfo.BaseName))
                {
                    _ajv.code = 105;
                    _ajv.msg = "您的账号已登录，请联系超管！";
                }
                else if (iscode == mobileKey)
                {
                    if (anmininfo.IsIdent == 0)
                    {
                        FacadeManage.AideAdminFacade.SetBaseIsIdent(anmininfo.ID.ToString(), 1);//首次绑定成功后 更新绑定状态 
                        //加入操作日志
                        AddBaseLog("Edit", "管理员绑定两步认证");
                    }

                    _ajv.code = 0;
                    _ajv.msg = "验证成功！";
                    AddBaseLog("Login", "认证成功");//加入认证日志

                    //登录成功保存用户信息
                    RoleAction.Login(Session, anmininfo);
                    EditLoginMsg(anmininfo.ID);
                    Session.Remove("IdentAdmin");

                    url = string.IsNullOrWhiteSpace(url) ? "#/Welcome" : url.StringDecode();
                    _ajv.SetDataItem("url", url);
                }
                else
                {
                    _ajv.code = 1;
                    _ajv.msg = "验证失败，请稍候再试！";
                }
            }
            else
            {
                _ajv.code = 1;
                _ajv.msg = "请刷新界面重新登录！";
            }
            await JsonAsync(_ajv);
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [MvcView("Index", ActionEnum.View)]
        [Ashx(State = AshxState.Post)]
        public async Task Logout([ApiVal(Val.Form)] bool IsMenu = false)
        {
            if (IsMenu) Menu.Reload();

            RoleAction.Logout(Session, anmininfo);

            AjaxJson _ajv = new()
            {
                code = 0,
                msg = "退出登录成功！"
            };

            _ajv.SetDataItem("Url", "/Admin/Login");

            await JsonAsync(_ajv);
        }

        #endregion

        #endregion

        #region 管理后台

        #endregion

        #region Sql 用户操作日志打印

        private static readonly Tool.Utils.ThreadQueue.TaskOueue<BaseLog, bool> taskOueue = new(WriteBaselog);

        public static void StartBaseLog()
        {
            //if (!taskOueue.IsContinueWith)
            //{
            //    taskOueue.ContinueWith += TaskOueue_ContinueWith;
            //}
            taskOueue.ContinueWith += TaskOueue_ContinueWith;
        }

        static bool WriteBaselog(BaseLog log)
        {
            FacadeManage.AideAdminFacade.InsertBaseLog(log);//加入操作日志
            return true;
        }

        static void TaskOueue_ContinueWith(BaseLog arg1, bool arg2, Exception arg3)
        {
            if (arg3 != null)
            {
                Log.Error("队列异常：", arg3, LogFilePath);
            }
        }

        private void AddBaseLog(string ModuleType, string ModuleText)
        {
            taskOueue.Add(new BaseLog
            {
                BaseID = anmininfo.ID,
                BaseName = anmininfo.BaseName,
                ModuleType = ModuleType,
                ModuleText = ModuleText,
                BaseIp = Context.GetIP()
            });
        }

        private void AddBaseLog(MvcView mvc, string Remark = null)
        {
            AddBaseLog(mvc.Action.ToString(), $"{mvc.Remark}---{Remark}");
        }

        ///// <summary>
        ///// 写入操作信息
        ///// </summary>
        ///// <param name="ashxRoute"></param>
        //protected override void OnResult(Ashx ashx)
        //{
        //    while (baseLogs.TryDequeue(out BaseLog log))
        //    {
        //        //加入操作日志
        //        FacadeManage.AideAdminFacade.InsertBaseLog(log);
        //    }
        //}

        #endregion

        /// <summary>
        /// 当API发生异常时触发
        /// </summary>
        /// <param name="ex"></param>
        protected override void AshxException(AshxException ex)
        {
            Log.Error("API接口异常", ex, LogFilePath);
            Json(new
            {
                code = 500,
                msg = "接口发生异常！"
            });
        }

    }
}
