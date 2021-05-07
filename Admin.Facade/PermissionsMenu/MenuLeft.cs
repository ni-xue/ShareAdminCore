using System.Collections.Generic;

namespace Admin.Facade.PermissionsMenu
{
    public class MenuLeft
    {
        /// <summary>
        /// 初始化菜单对象
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Controller"></param>
        /// <param name="Action"></param>
        /// <param name="Keywords"></param>
        /// <param name="Title"></param>
        /// <param name="ImgStyle"></param>
        /// <param name="Sort_Id"></param>
        /// <param name="HideStatus"></param>
        /// <param name="Grades"></param>
        /// <param name="State"></param>
        /// <param name="Authority"></param>
        /// <param name="PermissionType"></param>
        /// <param name="MenuDetails"></param>
        public MenuLeft(int Id, string Controller, string Action, string Keywords, string Title, string ImgStyle, int Sort_Id, int HideStatus, int Grades, int State, string Authority, string PermissionType, int ActionId, List<MenuLeft> MenuDetails)
        {
            this.Id = Id;
            this.Controller = Controller;
            this.Action = Action;
            this.Keywords = Keywords;
            this.Title = Title;
            this.ImgStyle = ImgStyle;
            this.Sort_Id = Sort_Id;
            this.HideStatus = HideStatus;
            this.Grades = Grades;
            this.State = State;
            this.Authority = Authority;
            this.PermissionType = PermissionType;
            this.ActionId = ActionId;
            this.MenuDetails = MenuDetails;
        }

        /// <summary>
        /// 菜单id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Controller { get; }

        /// <summary>
        /// 视图名称
        /// </summary>
        public string Action { get; }

        /// <summary>
        /// 可能存在的GTE参数
        /// </summary>
        public string Keywords { get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 样式
        /// </summary>
        public string ImgStyle { get; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public int Sort_Id { get; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public int HideStatus { get; }

        /// <summary>
        /// 当前菜单的等级
        /// </summary>
        public int Grades { get; }

        /// <summary>
        /// 当前是菜单还是页面
        /// </summary>
        public int State { get; }

        /// <summary>
        /// 当前权限
        /// </summary>
        public string Authority { get; }

        /// <summary>
        /// 角色权限
        /// </summary>
        public string PermissionType { get; }

        /// <summary>
        /// 角色权限Id
        /// </summary>
        public int ActionId { get; }

        /// <summary>
        /// 左菜单详情对象
        /// </summary>
        public List<MenuLeft> MenuDetails { get; }
    }
}
