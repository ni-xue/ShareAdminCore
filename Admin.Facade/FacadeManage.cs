using Admin.Facade.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Facade
{
    /// <summary>
    /// 逻辑层管理类
    /// </summary>
    public class FacadeManage
    {
        internal static readonly object lockObj = new();

        ///// <summary>
        ///// 帐号库逻辑
        ///// </summary>
        private static volatile AdminFacade _aideAdminFacade;

        /// <summary>
        /// 后台账号库
        /// </summary>
        public static AdminFacade AideAdminFacade
        {
            get
            {
                if (_aideAdminFacade == null)
                {
                    lock (lockObj)
                    {
                        if (_aideAdminFacade == null)
                            _aideAdminFacade = new AdminFacade();
                    }
                }
                return _aideAdminFacade;
            }
        }

        /// <summary>
        /// 日志操作系统
        /// </summary>
        /// <param name="loggerFactory"></param>
        public static void UseSqlLog(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory) 
        {
            AideAdminFacade.SetLogger(loggerFactory.CreateLogger("Admin.Sql"));
        }
    }
}
