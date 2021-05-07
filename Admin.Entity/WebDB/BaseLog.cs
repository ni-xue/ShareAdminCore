/*
 * 版本： 4.5
 * 日期：2018/12/22 9:31:56
 * 
 * 描述：实体类
 * 
 */

using System;

namespace Admin.Entity.WebDB
{

    /// <summary>
    /// 实体类 BaseLog  (属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BaseLog
    {

        #region 常量 

        /// <summary>
        /// 表名
        /// </summary>
        public const string Tablename = "BaseLog";

        /// <summary>
        /// ID，描述:
        /// </summary>
        public const string _ID = "ID";

        /// <summary>
        /// BaseID，描述:管理员ID
        /// </summary>
        public const string _BaseID = "BaseID";

        /// <summary>
        /// BaseName，描述:管理员名称
        /// </summary>
        public const string _BaseName = "BaseName";

        /// <summary>
        /// ModuleType，描述:模块类型（如：update）
        /// </summary>
        public const string _ModuleType = "ModuleType";

        /// <summary>
        /// ModuleText，描述:模块名称（如：修改了什么东西？）
        /// </summary>
        public const string _ModuleText = "ModuleText";

        /// <summary>
        /// BaseIp，描述:操作Ip
        /// </summary>
        public const string _BaseIp = "BaseIp";

        /// <summary>
        /// BaseTime，描述:操作时间
        /// </summary>
        public const string _BaseTime = "BaseTime";

        #endregion

        #region 私有变量

        private int p_id;
        private int p_baseid;
        private string p_basename;
        private string p_moduletype;
        private string p_moduletext;
        private string p_baseip;
        private DateTime p_basetime;

        #endregion

        #region 构造函数 

        /// <summary>
        /// 初始化BaseLog
        /// </summary>
        public BaseLog()
        {
            p_id = 0;
            p_baseid = 0;
            p_basename = string.Empty;
            p_moduletype = string.Empty;
            p_moduletext = string.Empty;
            p_baseip = string.Empty;
            p_basetime = DateTime.Now;
        }

        #endregion

        #region 公共属性 

        /// <summary>
        /// ID，描述:
        /// </summary>
        public int ID
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
        /// BaseID，描述:管理员ID
        /// </summary>
        public int BaseID
        {
            set
            {
                p_baseid = value;
            }
            get
            {
                return p_baseid;
            }
        }

        /// <summary>
        /// BaseName，描述:管理员名称
        /// </summary>
        public string BaseName
        {
            set
            {
                p_basename = value;
            }
            get
            {
                return p_basename;
            }
        }

        /// <summary>
        /// ModuleType，描述:模块类型（如：update）
        /// </summary>
        public string ModuleType
        {
            set
            {
                p_moduletype = value;
            }
            get
            {
                return p_moduletype;
            }
        }

        /// <summary>
        /// ModuleText，描述:模块名称（如：修改了什么东西？）
        /// </summary>
        public string ModuleText
        {
            set
            {
                p_moduletext = value;
            }
            get
            {
                return p_moduletext;
            }
        }

        /// <summary>
        /// BaseIp，描述:操作Ip
        /// </summary>
        public string BaseIp
        {
            set
            {
                p_baseip = value;
            }
            get
            {
                return p_baseip;
            }
        }

        /// <summary>
        /// BaseTime，描述:操作时间
        /// </summary>
        public DateTime BaseTime
        {
            set
            {
                p_basetime = value;
            }
            get
            {
                return p_basetime;
            }
        }

        #endregion
    }
}

