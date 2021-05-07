/*
 * 版本： 4.5
 * 日期：2018/11/21 15:13:41
 * 
 * 描述：实体类
 * 
 */

using System;

namespace Admin.Entity.WebDB
{

    /// <summary>
    /// 实体类 PermissionDirectory  (属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class PermissionDirectory
    {

        #region 常量 

        /// <summary>
        /// 表名
        /// </summary>
        public const string Tablename = "PermissionDirectory";

        /// <summary>
        /// Id，描述:
        /// </summary>
        public const string _Id = "Id";

        /// <summary>
        /// ParentID，描述:父级一级Id,若无父级 则为0
        /// </summary>
        public const string _ParentID = "ParentID";

        /// <summary>
        /// Controller，描述:导航关键字（关联操作权限）
        /// </summary>
        public const string _Controller = "Controller";

        /// <summary>
        /// Action，描述:操作权限目录
        /// </summary>
        public const string _Action = "Action";

        /// <summary>
        /// Title，描述:标题说明
        /// </summary>
        public const string _Title = "Title";

        /// <summary>
        /// ImgStyle，描述:图片链接
        /// </summary>
        public const string _ImgStyle = "ImgStyle";

        /// <summary>
        /// Keywords，描述:节点链接（非空）
        /// </summary>
        public const string _Keywords = "Keywords";

        /// <summary>
        /// Sort_Id，描述:排序Id
        /// </summary>
        public const string _Sort_Id = "Sort_Id";

        /// <summary>
        /// HideStatus，描述:隐藏状态 0显示，1隐藏
        /// </summary>
        public const string _HideStatus = "HideStatus";

        /// <summary>
        /// Grades，描述:菜单等级
        /// </summary>
        public const string _Grades = "Grades";

        /// <summary>
        /// State，描述:状态（是菜单还是页面 0：菜单，1：页面）
        /// </summary>
        public const string _State = "State";

        /// <summary>
        /// Authority，描述:操作权限目录
        /// </summary>
        public const string _Authority = "Authority";

        #endregion

        #region 私有变量

        private int p_id;
        private int p_parentid;
        private string p_controller;
        private string p_action;
        private string p_title;
        private string p_imgstyle;
        private string p_keywords;
        private int p_sort_id;
        private int p_hidestatus;
        private int p_grades;
        private int p_state;
        private string p_authority;

        #endregion

        #region 构造函数 

        /// <summary>
        /// 初始化PermissionDirectory
        /// </summary>
        public PermissionDirectory()
        {
            p_id = 0;
            p_parentid = 0;
            p_controller = string.Empty;
            p_action = string.Empty;
            p_title = string.Empty;
            p_imgstyle = string.Empty;
            p_keywords = string.Empty;
            p_sort_id = 0;
            p_hidestatus = 0;
            p_grades = 0;
            p_state = 0;
            p_authority = string.Empty;
        }

        #endregion

        #region 公共属性 

        /// <summary>
        /// Id，描述:
        /// </summary>
        public int Id
        {
            set
            {
                p_id = value;
            }
            get
            {
                return p_id;
            }
        }

        /// <summary>
        /// ParentID，描述:父级一级Id,若无父级 则为0
        /// </summary>
        public int ParentID
        {
            set
            {
                p_parentid = value;
            }
            get
            {
                return p_parentid;
            }
        }

        /// <summary>
        /// Controller，描述:导航关键字（关联操作权限）
        /// </summary>
        public string Controller
        {
            set
            {
                p_controller = value;
            }
            get
            {
                return p_controller;
            }
        }

        /// <summary>
        /// Action，描述:操作权限目录
        /// </summary>
        public string Action
        {
            set
            {
                p_action = value;
            }
            get
            {
                return p_action;
            }
        }

        /// <summary>
        /// Title，描述:标题说明
        /// </summary>
        public string Title
        {
            set
            {
                p_title = value;
            }
            get
            {
                return p_title;
            }
        }

        /// <summary>
        /// ImgStyle，描述:图片链接
        /// </summary>
        public string ImgStyle
        {
            set
            {
                p_imgstyle = value;
            }
            get
            {
                return p_imgstyle;
            }
        }

        /// <summary>
        /// Keywords，描述:节点链接（非空）
        /// </summary>
        public string Keywords
        {
            set
            {
                p_keywords = value;
            }
            get
            {
                return p_keywords;
            }
        }

        /// <summary>
        /// Sort_Id，描述:排序Id
        /// </summary>
        public int Sort_Id
        {
            set
            {
                p_sort_id = value;
            }
            get
            {
                return p_sort_id;
            }
        }

        /// <summary>
        /// HideStatus，描述:隐藏状态 0显示，1隐藏
        /// </summary>
        public int HideStatus
        {
            set
            {
                p_hidestatus = value;
            }
            get
            {
                return p_hidestatus;
            }
        }

        /// <summary>
        /// Grades，描述:菜单等级
        /// </summary>
        public int Grades
        {
            set
            {
                p_grades = value;
            }
            get
            {
                return p_grades;
            }
        }

        /// <summary>
        /// State，描述:状态（是菜单还是页面 0：菜单，1：页面）
        /// </summary>
        public int State
        {
            set
            {
                p_state = value;
            }
            get
            {
                return p_state;
            }
        }

        /// <summary>
        /// Authority，描述:操作权限目录
        /// </summary>
        public string Authority
        {
            set
            {
                p_authority = value;
            }
            get
            {
                return p_authority;
            }
        }

        #endregion
    }
}

