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
                webpath = AppSettings.Get("SiteConfig:webpath"),
                filepath = AppSettings.Get("SiteConfig:filepath"),
                filesave = AppSettings.Get("SiteConfig:filesave").ToInt(),
                fileextension = AppSettings.Get("SiteConfig:fileextension"),
                videoextension = AppSettings.Get("SiteConfig:videoextension"),
                attachsize = AppSettings.Get("SiteConfig:attachsize").ToInt(),
                videosize = AppSettings.Get("SiteConfig:videosize").ToInt(),
                imgsize = AppSettings.Get("SiteConfig:imgsize").ToInt(),
                imgmaxheight = AppSettings.Get("SiteConfig:imgmaxheight").ToInt(),
                imgmaxwidth = AppSettings.Get("SiteConfig:imgmaxwidth").ToInt(),
                thumbnailheight = AppSettings.Get("SiteConfig:thumbnailheight").ToInt(),
                thumbnailwidth = AppSettings.Get("SiteConfig:thumbnailwidth").ToInt(),
                watermarktype = AppSettings.Get("SiteConfig:watermarktype").ToInt(),
                watermarkposition = AppSettings.Get("SiteConfig:watermarkposition").ToInt(),
                watermarkimgquality = AppSettings.Get("SiteConfig:watermarkimgquality").ToInt(),
                watermarkpic = AppSettings.Get("SiteConfig:watermarkpic"),
                watermarktransparency = AppSettings.Get("SiteConfig:watermarktransparency").ToInt(),
                watermarktext = AppSettings.Get("SiteConfig:watermarktext"),
                watermarkfont = AppSettings.Get("SiteConfig:watermarkfont"),
                watermarkfontsize = AppSettings.Get("SiteConfig:watermarkfontsize").ToInt()
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
