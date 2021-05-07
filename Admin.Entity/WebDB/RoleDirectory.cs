/*
 * 版本： 4.5
 * 日期：2019/8/20 17:10:13
 * 
 * 描述：实体类
 * 
 */

using System;
using System.Collections.Generic;

namespace Admin.Entity.WebDB
{

        /// <summary>
        /// 实体类 RoleDirectory  (属性说明自动提取数据库字段的描述信息)
        /// </summary>
    [Serializable]
    public partial class RoleDirectory
    {

        #region 常量 

        /// <summary>
        /// 表名
        /// </summary>
        public const string Tablename = "RoleDirectory";

        /// <summary>
        /// Id，描述:角色标识
        /// </summary>
        public const string _Id = "Id";

        /// <summary>
        /// RoleName，描述:用户组名称
        /// </summary>
        public const string _RoleName = "RoleName";

        /// <summary>
        /// Descriptions，描述:角色描述
        /// </summary>
        public const string _Descriptions = "Descriptions";

        /// <summary>
        /// HomeState，描述:是否开启首页权限（0为关闭，1为开启）
        /// </summary>
        public const string _HomeState = "HomeState";

        #endregion

        #region 私有变量

        private int p_id;
        private string p_rolename;
        private string p_descriptions;
        private bool p_homestate;

        #endregion

        #region 构造函数 

        /// <summary>
        /// 初始化RoleDirectory
        /// </summary>
        public RoleDirectory()
        {
            p_id = 0;
            p_rolename = string.Empty;
            p_descriptions = string.Empty;
            p_homestate = false;
        }

        #endregion

        #region 公共属性 

        /// <summary>
        /// Id，描述:角色标识
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
        /// RoleName，描述:用户组名称
        /// </summary>
        public string RoleName
        {
            set
            {
                p_rolename = value;
            }
            get
            {
                return p_rolename;
            }
        }

        /// <summary>
        /// Descriptions，描述:角色描述
        /// </summary>
        public string Descriptions
        {
            set
            {
                p_descriptions = value;
            }
            get
            {
                return p_descriptions;
            }
        }

        /// <summary>
        /// HomeState，描述:是否开启首页权限（0为关闭，1为开启）
        /// </summary>
        public bool HomeState
        {
            set
            {
                p_homestate=value;
            }
            get
            {
                return p_homestate;
            }
        }

        #endregion
    }
}

