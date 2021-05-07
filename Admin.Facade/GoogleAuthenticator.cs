using System;
using System.Security.Cryptography;
using System.Text;
namespace Admin.Facade
{
    public class GoogleAuthenticator
    {

        /// <summary>
        /// 初始化验证码生成规则
        /// </summary>
        /// <param name="key">秘钥(手机使用Base32码)</param>
        /// <param name="duration">验证码间隔多久刷新一次（默认30秒和google同步）</param>
        public GoogleAuthenticator(long duration = 30, string key = "xeon997@foxmail.com")
        {
            this.SERECT_KEY = key;
            this.SERECT_KEY_MOBILE = Base32.ToString(Encoding.UTF8.GetBytes(key));
            this.DURATION_TIME = duration;
        }

        /// <summary>
        /// 间隔时间
        /// </summary>
        private long DURATION_TIME { get; set; }

        /// <summary>
        /// 迭代次数
        /// </summary>
        private long COUNTER
        {
            get
            {
                return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds / DURATION_TIME;
            }
        }

        /// <summary>
        /// 秘钥
        /// </summary>
        private string SERECT_KEY { get; set; }

        /// <summary>
        /// 手机端输入的秘钥
        /// </summary>
        private string SERECT_KEY_MOBILE { get; set; }

        /// <summary>
        /// 到期秒数
        /// </summary>
        public long EXPIRE_SECONDS
        {
            get
            {
                return DURATION_TIME - ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds % DURATION_TIME);
            }
        }

        /// <summary>
        /// 获取手机端秘钥
        /// </summary>
        /// <returns></returns>
        public string GetMobilePhoneKey()
        {
            if (SERECT_KEY_MOBILE == null)
                throw new ArgumentNullException("SERECT_KEY_MOBILE");
            return SERECT_KEY_MOBILE;
        }

        /// <summary>
        /// 生成认证码
        /// </summary>
        /// <returns>返回验证码</returns>
        public string GenerateCode()
        {
            return GenerateHashedCode(SERECT_KEY, COUNTER);
        }

        /// <summary>
        /// 按照次数生成哈希编码
        /// </summary>
        /// <param name="secret">秘钥</param>
        /// <param name="iterationNumber">迭代次数</param>
        /// <param name="digits">生成位数</param>
        /// <returns>返回验证码</returns>
        private static string GenerateHashedCode(string secret, long iterationNumber, int digits = 6)
        {
            byte[] counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(counter);

            byte[] key = Encoding.ASCII.GetBytes(secret);

            HMACSHA1 hmac = new(key, true);

            byte[] hash = hmac.ComputeHash(counter);

            int offset = hash[^1] & 0xf;

            int binary =
                ((hash[offset] & 0x7f) << 24)
                | ((hash[offset + 1] & 0xff) << 16)
                | ((hash[offset + 2] & 0xff) << 8)
                | (hash[offset + 3] & 0xff);

            int password = binary % (int)Math.Pow(10, digits); // 6 digits

            return password.ToString(new string('0', digits));
        }
    }
}
