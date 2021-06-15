using Admin.Entity.Config;
using QRCoder;
using System;
using System.Linq;
using System.Text;
using Tool;
using Tool.Utils;

namespace Admin.Facade
{
    public class HelpClass
    {
        #region 获取上传配置

        public static SiteConfig GetSiteConfig()
        {
            SiteConfig config = new()
            {
                Webpath = AppSettings.Get("SiteConfig:webpath"),
                Filepath = AppSettings.Get("SiteConfig:filepath"),
                Filesave = AppSettings.Get("SiteConfig:filesave").ToInt(),
                Fileextension = AppSettings.Get("SiteConfig:fileextension"),
                Videoextension = AppSettings.Get("SiteConfig:videoextension"),
                Attachsize = AppSettings.Get("SiteConfig:attachsize").ToInt(),
                Videosize = AppSettings.Get("SiteConfig:videosize").ToInt(),
                Imgsize = AppSettings.Get("SiteConfig:imgsize").ToInt(),
                Imgmaxheight = AppSettings.Get("SiteConfig:imgmaxheight").ToInt(),
                Imgmaxwidth = AppSettings.Get("SiteConfig:imgmaxwidth").ToInt(),
                Thumbnailheight = AppSettings.Get("SiteConfig:thumbnailheight").ToInt(),
                Thumbnailwidth = AppSettings.Get("SiteConfig:thumbnailwidth").ToInt(),
                Watermarktype = AppSettings.Get("SiteConfig:watermarktype").ToInt(),
                Watermarkposition = AppSettings.Get("SiteConfig:watermarkposition").ToInt(),
                Watermarkimgquality = AppSettings.Get("SiteConfig:watermarkimgquality").ToInt(),
                Watermarkpic = AppSettings.Get("SiteConfig:watermarkpic"),
                Watermarktransparency = AppSettings.Get("SiteConfig:watermarktransparency").ToInt(),
                Watermarktext = AppSettings.Get("SiteConfig:watermarktext"),
                Watermarkfont = AppSettings.Get("SiteConfig:watermarkfont"),
                Watermarkfontsize = AppSettings.Get("SiteConfig:watermarkfontsize").ToInt()
            };
            return config;
        }

        #endregion

        /// <summary>
        /// 获取交易流水号
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetOrderIDByPrefix(string prefix)
        {
            return GetOrderIDByPrefix(prefix, 20);
        }

        /// <summary>
        /// 获取交易流水号
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetOrderIDByPrefix(string prefix, int orderIDLength)
        {
            //构造订单号 (形如:20101201102322159111111)
            int randomLength = 6;
            StringBuilder tradeNoBuffer = new(prefix);

            tradeNoBuffer.Append(TextUtility.GetDateTimeLongString());

            if ((tradeNoBuffer.Length + randomLength) > orderIDLength)
                randomLength = orderIDLength - tradeNoBuffer.Length;

            tradeNoBuffer.Append(TextUtility.CreateRandom(randomLength, true, false, false, false, ""));

            return tradeNoBuffer.ToString();
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="issuer">标题</param>
        /// <param name="accountTitleNoSpaces">用户名</param>
        /// <param name="accountSecretKey">秘钥</param>
        /// <returns></returns>
        public static string GetQrBase64Imageg(string issuer, string accountTitleNoSpaces, string accountSecretKey)
        {
            accountTitleNoSpaces = RemoveWhitespace(accountTitleNoSpaces);
            string provisionUrl = string.Format("otpauth://totp/{2}:{0}?secret={1}&issuer={2}", accountTitleNoSpaces, accountSecretKey, UrlEncode(issuer));
            QRCodeGenerator qrGenerator = new();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(provisionUrl, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(2);

            return $"data:image/png;base64,{qrCodeImageAsBase64}";
        }

        private static string UrlEncode(string value)
        {
            StringBuilder result = new();
            string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            foreach (char symbol in value)
            {
                if (validChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + string.Format("{0:X2}", (int)symbol));
                }
            }
            return result.ToString().Replace(" ", "%20");
        }

        private static string RemoveWhitespace(string str)
        {
            return new string(str.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
    }
}
