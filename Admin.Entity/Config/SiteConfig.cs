using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Entity.Config
{
    /// <summary>
    /// 站点配置实体类
    /// </summary>
    [Serializable]
    public class SiteConfig
    {
        //public SiteConfig(){ }

        private string _webname = "";
        private string _weburl = "";
        private string _webcompany = "";
        private string _webaddress = "";
        private string _webtel = "";
        private string _webfax = "";
        private string _webmail = "";
        private string _webcrod = "";

        private string _webpath = "";
        private string _webmanagepath = "";
        private int _staticstatus = 0;
        private string _staticextension = "";
        private int _memberstatus = 1;
        private int _commentstatus = 0;
        private int _logstatus = 0;
        private int _webstatus = 1;
        private string _webclosereason = "";
        private string _webcountcode = "";


        private string _filepath = "";
        private int _filesave = 1;
        private string _fileextension = "";
        private string _videoextension = "";
        private long _attachsize = 0;
        private long _videosize = 0;
        private long _imgsize = 0;
        private int _imgmaxheight = 0;
        private int _imgmaxwidth = 0;
        private int _thumbnailheight = 0;
        private int _thumbnailwidth = 0;
        private int _watermarktype = 0;
        private int _watermarkposition = 9;
        private int _watermarkimgquality = 80;
        private string _watermarkpic = "";
        private int _watermarktransparency = 10;
        private string _watermarktext = "";
        private string _watermarkfont = "";
        private int _watermarkfontsize = 12;

        private string _sysdatabaseprefix = "ucenter_";
        private string _sysencryptstring = "DTcms";

        #region 主站基本信息==================================
        /// <summary>
        /// 网站名称
        /// </summary>
        public string Webname
        {
            get { return _webname; }
            set { _webname = value; }
        }
        /// <summary>
        /// 网站域名
        /// </summary>
        public string Weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Webcompany
        {
            get { return _webcompany; }
            set { _webcompany = value; }
        }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string Webaddress
        {
            get { return _webaddress; }
            set { _webaddress = value; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Webtel
        {
            get { return _webtel; }
            set { _webtel = value; }
        }
        /// <summary>
        /// 传真号码
        /// </summary>
        public string Webfax
        {
            get { return _webfax; }
            set { _webfax = value; }
        }
        /// <summary>
        /// 管理员邮箱
        /// </summary>
        public string Webmail
        {
            get { return _webmail; }
            set { _webmail = value; }
        }
        /// <summary>
        /// 网站备案号
        /// </summary>
        public string Webcrod
        {
            get { return _webcrod; }
            set { _webcrod = value; }
        }
        #endregion

        #region 功能权限设置==================================
        /// <summary>
        /// 网站安装目录
        /// </summary>
        public string Webpath
        {
            get { return _webpath; }
            set { _webpath = value; }
        }
        /// <summary>
        /// 网站管理目录
        /// </summary>
        public string Webmanagepath
        {
            get { return _webmanagepath; }
            set { _webmanagepath = value; }
        }
        /// <summary>
        /// 是否开启生成静态
        /// </summary>
        public int Staticstatus
        {
            get { return _staticstatus; }
            set { _staticstatus = value; }
        }
        /// <summary>
        /// 生成静态扩展名
        /// </summary>
        public string Staticextension
        {
            get { return _staticextension; }
            set { _staticextension = value; }
        }
        /// <summary>
        /// 开启会员功能
        /// </summary>
        public int Memberstatus
        {
            get { return _memberstatus; }
            set { _memberstatus = value; }
        }
        /// <summary>
        /// 开启评论审核
        /// </summary>
        public int Commentstatus
        {
            get { return _commentstatus; }
            set { _commentstatus = value; }
        }
        /// <summary>
        /// 后台管理日志
        /// </summary>
        public int Logstatus
        {
            get { return _logstatus; }
            set { _logstatus = value; }
        }
        /// <summary>
        /// 是否关闭网站
        /// </summary>
        public int Webstatus
        {
            get { return _webstatus; }
            set { _webstatus = value; }
        }
        /// <summary>
        /// 关闭原因描述
        /// </summary>
        public string Webclosereason
        {
            get { return _webclosereason; }
            set { _webclosereason = value; }
        }
        /// <summary>
        /// 网站统计代码
        /// </summary>
        public string Webcountcode
        {
            get { return _webcountcode; }
            set { _webcountcode = value; }
        }
        #endregion

        #region 文件上传设置==================================
        /// <summary>
        /// 附件上传目录
        /// </summary>
        public string Filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        /// <summary>
        /// 附件保存方式
        /// </summary>
        public int Filesave
        {
            get { return _filesave; }
            set { _filesave = value; }
        }
        /// <summary>
        /// 附件上传类型
        /// </summary>
        public string Fileextension
        {
            get { return _fileextension; }
            set { _fileextension = value; }
        }
        /// <summary>
        /// 视频上传类型
        /// </summary>
        public string Videoextension
        {
            get { return _videoextension; }
            set { _videoextension = value; }
        }
        /// <summary>
        /// 文件上传大小
        /// </summary>
        public long Attachsize
        {
            get { return _attachsize; }
            set { _attachsize = value; }
        }
        /// <summary>
        /// 视频上传大小
        /// </summary>
        public long Videosize
        {
            get { return _videosize; }
            set { _videosize = value; }
        }
        /// <summary>
        /// 图片上传大小
        /// </summary>
        public long Imgsize
        {
            get { return _imgsize; }
            set { _imgsize = value; }
        }
        /// <summary>
        /// 图片最大高度(像素)
        /// </summary>
        public int Imgmaxheight
        {
            get { return _imgmaxheight; }
            set { _imgmaxheight = value; }
        }
        /// <summary>
        /// 图片最大宽度(像素)
        /// </summary>
        public int Imgmaxwidth
        {
            get { return _imgmaxwidth; }
            set { _imgmaxwidth = value; }
        }
        /// <summary>
        /// 生成缩略图高度(像素)
        /// </summary>
        public int Thumbnailheight
        {
            get { return _thumbnailheight; }
            set { _thumbnailheight = value; }
        }
        /// <summary>
        /// 生成缩略图宽度(像素)
        /// </summary>
        public int Thumbnailwidth
        {
            get { return _thumbnailwidth; }
            set { _thumbnailwidth = value; }
        }
        /// <summary>
        /// 图片水印类型
        /// </summary>
        public int Watermarktype
        {
            get { return _watermarktype; }
            set { _watermarktype = value; }
        }
        /// <summary>
        /// 图片水印位置
        /// </summary>
        public int Watermarkposition
        {
            get { return _watermarkposition; }
            set { _watermarkposition = value; }
        }
        /// <summary>
        /// 图片生成质量
        /// </summary>
        public int Watermarkimgquality
        {
            get { return _watermarkimgquality; }
            set { _watermarkimgquality = value; }
        }
        /// <summary>
        /// 图片水印文件
        /// </summary>
        public string Watermarkpic
        {
            get { return _watermarkpic; }
            set { _watermarkpic = value; }
        }
        /// <summary>
        /// 水印透明度
        /// </summary>
        public int Watermarktransparency
        {
            get { return _watermarktransparency; }
            set { _watermarktransparency = value; }
        }
        /// <summary>
        /// 水印文字
        /// </summary>
        public string Watermarktext
        {
            get { return _watermarktext; }
            set { _watermarktext = value; }
        }
        /// <summary>
        /// 文字字体
        /// </summary>
        public string Watermarkfont
        {
            get { return _watermarkfont; }
            set { _watermarkfont = value; }
        }
        /// <summary>
        /// 文字大小(像素)
        /// </summary>
        public int Watermarkfontsize
        {
            get { return _watermarkfontsize; }
            set { _watermarkfontsize = value; }
        }
        #endregion

        #region 安装初始化设置================================
        /// <summary>
        /// 数据库表前缀
        /// </summary>
        public string Sysdatabaseprefix
        {
            get { return _sysdatabaseprefix; }
            set { _sysdatabaseprefix = value; }
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        public string Sysencryptstring
        {
            get { return _sysencryptstring; }
            set { _sysencryptstring = value; }
        }
        #endregion
    }

}
