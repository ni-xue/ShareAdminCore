using System;
using System.Collections.Generic;
using System.Data;
using Tool;
using Tool.Utils;

namespace Admin.Facade.PermissionsMenu
{
    public class Menu
    {
        /// <summary>
        /// 菜单对象
        /// </summary>
        public static List<MenuHead> Menus { get; private set; } = new();

        /// <summary>
        /// 当前所有需要权限的页面
        /// </summary>
        public static List<Rout> Routs { get; private set; } = new();

        /// <summary>
        /// 网站进程创建之初，实例化后台菜单
        /// </summary>
        /// <param name="dataSet"></param>
        public static void Initialize(DataSet dataSet)
        {
            Menus = GetMenus(dataSet, out List<Rout> routs, true, true);
            Routs = routs;
        }

        /// <summary>
        /// 网站菜单发生变化时，刷新后台菜单
        /// </summary>
        /// <param name="dataSet"></param>
        public static void Reload()
        {
            var MenuByData = FacadeManage.AideAdminFacade.GetMenuByUserId();
            Initialize(MenuByData);
        }

        /// <summary>
        /// 公共菜单调用方法
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static List<MenuHead> GetMenus(DataSet dataSet)
        {
            return GetMenus(dataSet, out _, false, true);
        }

        /// <summary>
        /// 公共菜单调用方法
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="isRouts"></param>
        /// <returns></returns>
        public static List<MenuHead> GetMenus(DataSet dataSet, out List<Rout> routs, bool isRouts, bool isInitialize = false)
        {
            List<Rout> m_routs = new();
            if (isInitialize)
            {
                m_routs.Add(new Rout("Admin", "Index", ActionEnum.View, ActionEnum.All));
            }
            else if (isRouts)
            {
                try
                {
                    bool HomeState = dataSet.Tables[3].Rows[0]["HomeState"].ToVar<bool>();
                    if (HomeState)
                    {
                        m_routs.Add(new Rout("Admin", "Index", ActionEnum.View, ActionEnum.All));
                    }
                    else
                    {
                        m_routs.Add(new Rout("Admin", "Index", ActionEnum.View, ActionEnum.NOT));
                    }
                }
                catch (Exception)
                {
                    m_routs.Add(new Rout("Admin", "Index", ActionEnum.View, ActionEnum.All));
                }
            }
            //获取菜单的对象
            List<MenuHead> Menus = new();

            if (!Validate.CheckedDataSet(dataSet))
            {
                throw new Exception("菜单对象为空，请检查原因。");
            }

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                int Id = dataRow["Id"].ToVar<int>();

                List<MenuLeft> menuLefts = new();

                foreach (DataRow dataRow_1 in dataSet.Tables[1].Rows)
                {
                    int Id_1 = dataRow_1["Id"].ToVar<int>();

                    int ParentID_1 = dataRow_1["ParentID"].ToVar<int>();

                    if (Id == ParentID_1)
                    {
                        List<MenuLeft> menuLefts_1 = new();

                        int State_1 = dataRow_1["State"].ToVar<int>();

                        if (State_1 == 0)
                        {
                            foreach (DataRow dataRow_2 in dataSet.Tables[2].Rows)
                            {
                                int Id_2 = dataRow_2["Id"].ToVar<int>();
                                int ParentID_2 = dataRow_2["ParentID"].ToVar<int>();

                                if (Id_1 == ParentID_2)
                                {
                                    bool isPermissionType = dataRow_2.Table.Columns["PermissionType"] != null;

                                    int HideStatus = dataRow_2["HideStatus"].ToVar<int>();

                                    if (isRouts && HideStatus == 0)
                                    {
                                        if (isPermissionType)
                                        {
                                            m_routs.Add(new Rout(dataRow_2["Controller"].ToString(), dataRow_2["Action"].ToString(), dataRow_2["Authority"].ToString(), dataRow_2["PermissionType"].ToString()));
                                        }
                                        else
                                        {
                                            m_routs.Add(new Rout(dataRow_2["Controller"].ToString(), dataRow_2["Action"].ToString(), dataRow_2["Authority"].ToString()));
                                        }
                                    }

                                    string _permissionType = isPermissionType ? string.IsNullOrWhiteSpace(dataRow_2["PermissionType"].ToString()) ? "" : dataRow_2["PermissionType"].ToString() : "";

                                    if (!isPermissionType || isInitialize)
                                    {
                                        menuLefts_1.Add(new MenuLeft
                                        (
                                        Id_2,
                                        dataRow_2["Controller"].ToString(),
                                        dataRow_2["Action"].ToString(),
                                        dataRow_2["Keywords"].ToString(),
                                        dataRow_2["Title"].ToString(),
                                        dataRow_2["ImgStyle"].ToString(),
                                        dataRow_2["Sort_Id"].ToVar<int>(),
                                        HideStatus,
                                        dataRow_2["Grades"].ToVar<int>(),
                                        dataRow_2["State"].ToVar<int>(),
                                        dataRow_2["Authority"].ToString(),
                                        _permissionType, //isPermissionType ? dataRow_2["PermissionType"].ToString() : "",
                                        dataRow_2.Table.Columns["ActionId"] == null ? 0 : dataRow_2["ActionId"].ToVar<int>(),
                                        null
                                        ));
                                    }
                                    else if (!isInitialize && isPermissionType && _permissionType.Length > 0)
                                    {
                                        menuLefts_1.Add(new MenuLeft
                                        (
                                        Id_2,
                                        dataRow_2["Controller"].ToString(),
                                        dataRow_2["Action"].ToString(),
                                        dataRow_2["Keywords"].ToString(),
                                        dataRow_2["Title"].ToString(),
                                        dataRow_2["ImgStyle"].ToString(),
                                        dataRow_2["Sort_Id"].ToVar<int>(),
                                        HideStatus,
                                        dataRow_2["Grades"].ToVar<int>(),
                                        dataRow_2["State"].ToVar<int>(),
                                        dataRow_2["Authority"].ToString(),
                                        _permissionType, // isPermissionType ? dataRow_2["PermissionType"].ToString() : "",
                                        dataRow_2.Table.Columns["ActionId"] == null ? 0 : dataRow_2["ActionId"].ToVar<int>(),
                                        null
                                        ));
                                    }
                                }
                            }
                        }
                        else if (isRouts && dataRow_1["HideStatus"].ToVar<int>() == 0)
                        {
                            bool isPermissionType = dataRow_1.Table.Columns["PermissionType"] != null;

                            if (isPermissionType)
                            {
                                m_routs.Add(new Rout(dataRow_1["Controller"].ToString(), dataRow_1["Action"].ToString(), dataRow_1["Authority"].ToString(), dataRow_1["PermissionType"].ToString()));
                            }
                            else
                            {
                                m_routs.Add(new Rout(dataRow_1["Controller"].ToString(), dataRow_1["Action"].ToString(), dataRow_1["Authority"].ToString()));
                            }
                        }

                        menuLefts.Add(new MenuLeft
                        (
                         Id_1,
                         dataRow_1["Controller"].ToString(),
                         dataRow_1["Action"].ToString(),
                         dataRow_1["Keywords"].ToString(),
                         dataRow_1["Title"].ToString(),
                         dataRow_1["ImgStyle"].ToString(),
                         dataRow_1["Sort_Id"].ToVar<int>(),
                         dataRow_1["HideStatus"].ToVar<int>(),
                         dataRow_1["Grades"].ToVar<int>(),
                         State_1,
                         dataRow_1["Authority"].ToString(),
                         dataRow_1.Table.Columns["PermissionType"] == null ? "" : dataRow_1["PermissionType"].ToString(),
                         dataRow_1.Table.Columns["ActionId"] == null ? 0 : dataRow_1["ActionId"].ToVar<int>(),
                         menuLefts_1.Count == 0 ? null : menuLefts_1
                        ));
                    }
                }

                MenuHead menuHead = new
                (
                 Id, dataRow["Title"].ToString(),
                 dataRow["ImgStyle"].ToString(),
                 dataRow["Sort_Id"].ToVar<int>(),
                 dataRow["HideStatus"].ToVar<int>(),
                 dataRow["Authority"].ToString(),
                 dataRow.Table.Columns["PermissionType"] == null ? "" : dataRow["PermissionType"].ToString(),
                 dataRow.Table.Columns["ActionId"] == null ? 0 : dataRow["ActionId"].ToVar<int>(),
                 menuLefts.Count == 0 ? null : menuLefts
                );
                Menus.Add(menuHead);
            }
            routs = m_routs;
            return Menus;
        }

        /// <summary>
        /// 公共菜单调用方法 (获取菜单文件夹的排序方式)
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static List<MenuHead> GetMenusClip(DataSet dataSet)
        {
            //获取菜单的对象
            List<MenuHead> Menus = new();

            if (!Validate.CheckedDataSet(dataSet))
            {
                throw new Exception("菜单对象为空，请检查原因。");
            }

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                int Id = dataRow["Id"].ToVar<int>();

                List<MenuLeft> menuLefts = new();

                foreach (DataRow dataRow_1 in dataSet.Tables[1].Rows)
                {
                    int Id_1 = dataRow_1["Id"].ToVar<int>();

                    int ParentID_1 = dataRow_1["ParentID"].ToVar<int>();

                    if (Id == ParentID_1)
                    {
                        menuLefts.Add(new MenuLeft
                        (
                        Id_1,
                        null,
                        null,
                        null,
                        dataRow_1["Title"].ToString(),
                        dataRow_1["ImgStyle"].ToString(),
                        dataRow_1["Sort_Id"].ToVar<int>(),
                        dataRow_1["HideStatus"].ToVar<int>(),
                        dataRow_1["Grades"].ToVar<int>(),
                        0,
                        dataRow_1["Authority"].ToString(),
                        null,
                        0,
                        null
                        ));
                    }
                }

                MenuHead menuHead = new MenuHead
                (
                Id, dataRow["Title"].ToString(),
                dataRow["ImgStyle"].ToString(),
                dataRow["Sort_Id"].ToVar<int>(),
                dataRow["HideStatus"].ToVar<int>(),
                dataRow["Authority"].ToString(),
                null,
                0,
                menuLefts.Count == 0 ? null : menuLefts
                );
                Menus.Add(menuHead);
            }

            return Menus;
        }


        public static MenusInfoResult GetMenusInfoResult(List<MenuHead> menus, UserInfo userInfo)
        {
            MenusInfoResult menusInfoResult = new();
            menusInfoResult.UserInfo = userInfo;

            foreach (var item in menus)
            {
                if (item.HideStatus == 0)
                {
                    SystemMenu treeNode = new()
                    {
                        Id = item.Id,
                        Icon = item.ImgStyle,
                        Title = item.Title,
                        Sort = item.Sort_Id
                    };

                    GetTreeNodeListByNoLockedDTOArray(treeNode, item.MenuLefts);

                    menusInfoResult.MenuInfo.Add(treeNode);
                }
            }

            return menusInfoResult;

            static void GetTreeNodeListByNoLockedDTOArray(SystemMenu treeNode, List<MenuLeft> menuLefts)
            {
                if (menuLefts is not null)
                {
                    treeNode.Child ??= new List<SystemMenu>();
                    foreach (var item1 in menuLefts)
                    {
                        if (item1.HideStatus == 0)
                        {
                            SystemMenu treeNode1 = new()
                            {
                                Id = item1.Id,
                                Icon = item1.ImgStyle,
                                Href = string.IsNullOrEmpty(item1.Controller) ? "" : $"{item1.Controller}/{item1.Action}{item1.Keywords}",
                                Title = item1.Title,
                                Sort = item1.Sort_Id
                            };

                            treeNode.Child.Add(treeNode1);

                            GetTreeNodeListByNoLockedDTOArray(treeNode1, item1.MenuDetails);
                        }
                    }

                }
                //return treeNode;
            }
        }
    }
}
