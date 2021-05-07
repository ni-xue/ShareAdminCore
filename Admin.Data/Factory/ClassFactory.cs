using Admin.IData;
using Tool.SqlCore;
using Tool.Utils;

namespace Admin.Data.Factory
{
    public class ClassFactory
    {
        /// <summary>
        /// 创建用户库对象实例
        /// </summary>
        /// <returns></returns>
        public static IAdminDBInterface GetIAdminDBProvider()
        {
            return ProxyFactory.CreateInstance<AdminDB>(AppSettings.Get("ConnectionStrings:AdminDB"));
        }
    }
}
