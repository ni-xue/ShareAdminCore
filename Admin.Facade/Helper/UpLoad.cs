using Admin.Entity.Config;
using Agent.Facade.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tool.Utils;
using Tool.Web;

namespace Admin.Facade.Helper
{
    public class UpLoad
    {
        public readonly SiteConfig Config;

        public string BasePath { get; private set; }

        public UpLoad()
        {
            Config = HelpClass.GetSiteConfig();
        }

        public void SetBasePath(string basePath) 
        {
            this.BasePath = basePath;
        }

        /// <summary>
        /// 裁剪图片并保存
        /// </summary>
        public bool CropSaveAs(string fileName, string newFileName, int maxWidth, int maxHeight, int cropWidth, int cropHeight, int X, int Y)
        {
            string fileExt = fileName.Split('.')[1]; //文件扩展名，不含“.”
            if (!IsImage(fileExt))
            {
                return false;
            }
            string newFileDir = GetFullPath(newFileName.Substring(0, newFileName.LastIndexOf(@"/") + 1));
            //检查是否有该路径，没有则创建
            if (!Directory.Exists(newFileDir))
            {
                Directory.CreateDirectory(newFileDir);
            }
            try
            {
                string fileFullPath = GetFullPath(fileName);
                string toFileFullPath = GetFullPath(newFileName);
                return Thumbnail.MakeThumbnailImage(fileFullPath, toFileFullPath, 180, 180, cropWidth, cropHeight, X, Y);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 文件上传方法
        /// </summary>
        /// <param name="postedFile">文件流</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isWater">是否打水印</param>
        /// <returns>上传后文件信息</returns>
        public bool FileSaveAs(Microsoft.AspNetCore.Http.IFormFile postedFile, bool isThumbnail, bool isWater, ref AjaxJson _ajv)
        {
            try
            {
                string fileExt = Path.GetExtension(postedFile.FileName)?[1..];// postedFile.FileName.Split('.')[1]; //文件扩展名，不含“.”
                long fileSize = postedFile.Length; //获得文件大小，以字节为单位
                string fileName = postedFile.FileName[(postedFile.FileName.LastIndexOf(@"\") + 1)..]; //取得原文件名
                string newFileName = HelpClass.GetOrderIDByPrefix("Up", 12) + "." + fileExt; //随机生成新的文件名
                string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
                string upLoadPath = GetUpLoadPath(); //上传目录相对路径
                string fullUpLoadPath = GetFullPath(upLoadPath); //上传目录的物理路径
                string newFilePath = upLoadPath + newFileName; //上传后的路径
                string newThumbnailPath = upLoadPath + newThumbnailFileName; //上传后的缩略图路径

                //检查文件扩展名是否合法
                if (!CheckFileExt(fileExt))
                {
                    _ajv.code = 5;
                    _ajv.msg = $"不允许上传{fileExt}类型的文件！";
                    return false;
                }
                //检查文件大小是否合法
                if (!CheckFileSize(fileExt, fileSize))
                {
                    _ajv.code = 10;
                    _ajv.msg = "文件超过限制的大小！";
                    return false;
                }
                //检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(fullUpLoadPath))
                {
                    Directory.CreateDirectory(fullUpLoadPath);
                }

                //保存文件
                postedFile.Save(fullUpLoadPath + newFileName).Wait();
                //如果是图片，检查图片是否超出最大尺寸，是则裁剪
                if (IsImage(fileExt) && (this.Config.Imgmaxheight > 0 || this.Config.Imgmaxwidth > 0))
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newFileName,
                        this.Config.Imgmaxwidth, this.Config.Imgmaxheight);
                }
                //如果是图片，检查是否需要生成缩略图，是则生成
                if (IsImage(fileExt) && isThumbnail && this.Config.Thumbnailwidth > 0 && this.Config.Thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newThumbnailFileName,
                        this.Config.Thumbnailwidth, this.Config.Thumbnailheight, "Cut");
                }
                else
                {
                    newThumbnailPath = newFilePath; //不生成缩略图则返回原图
                }
                //如果是图片，检查是否需要打水印
                if (IsWaterMark(fileExt) && isWater)
                {
                    switch (this.Config.Watermarktype)
                    {
                        case 1:
                            WaterMark.AddImageSignText(GetFullPath(newFilePath), GetFullPath(newFilePath),
                                this.Config.Watermarktext, this.Config.Watermarkposition,
                                this.Config.Watermarkimgquality, this.Config.Watermarkfont, this.Config.Watermarkfontsize);
                            break;
                        case 2:
                            WaterMark.AddImageSignPic(GetFullPath(newFilePath), GetFullPath(newFilePath),
                                GetFullPath(this.Config.Watermarkpic), this.Config.Watermarkposition,
                                this.Config.Watermarkimgquality, this.Config.Watermarktransparency);
                            break;
                    }
                }
                //处理完毕，返回JOSN格式的文件信息
                //return $"{{\"status\": 1, \"msg\": \"上传文件成功！\", \"name\": \"{fileName}\", \"path\": \"{newFilePath}\", \"thumb\": \"{newThumbnailPath}\", \"size\": {fileSize}, \"ext\": \"{fileExt}\"}}";
                _ajv.code = 0;
                _ajv.msg = "上传成功！";
                _ajv.SetDataItem("url", $"/{newFilePath}");
                return true;
            }
            catch
            {
                _ajv.code = 500;
                _ajv.msg = "上传过程中发生意外错误！";
                return false; //"{\"status\": 0, \"msg\": \"上传过程中发生意外错误！\"}";
            }
        }

        /// <summary>
        /// 保存远程文件到本地
        /// </summary>
        /// <param name="fileUri">URI地址</param>
        /// <returns>上传后的路径</returns>
        public string RemoteSaveAs(string fileUri)
        {
            WebClient client = new();
            string fileExt = string.Empty; //文件扩展名，不含“.”
            if (fileUri.LastIndexOf(".") == -1)
            {
                fileExt = "gif";
            }
            else
            {
                fileExt = fileUri.Split('.')[1];
            }
            string newFileName = HelpClass.GetOrderIDByPrefix("Up", 12) + "." + fileExt; //随机生成新的文件名
            string upLoadPath = GetUpLoadPath(); //上传目录相对路径
            string fullUpLoadPath = GetFullPath(upLoadPath); //上传目录的物理路径
            string newFilePath = upLoadPath + newFileName; //上传后的路径
            //检查上传的物理路径是否存在，不存在则创建
            if (!Directory.Exists(fullUpLoadPath))
            {
                Directory.CreateDirectory(fullUpLoadPath);
            }

            try
            {
                client.DownloadFile(fileUri, fullUpLoadPath + newFileName);
                //如果是图片，检查是否需要打水印
                if (IsWaterMark(fileExt))
                {
                    switch (this.Config.Watermarktype)
                    {
                        case 1:
                            WaterMark.AddImageSignText(GetFullPath(newFilePath), GetFullPath(newFilePath),
                                this.Config.Watermarktext, this.Config.Watermarkposition,
                                this.Config.Watermarkimgquality, this.Config.Watermarkfont, this.Config.Watermarkfontsize);
                            break;
                        case 2:
                            WaterMark.AddImageSignPic(GetFullPath(newFilePath), GetFullPath(newFilePath),
                                GetFullPath(this.Config.Watermarkpic), this.Config.Watermarkposition,
                                this.Config.Watermarkimgquality, this.Config.Watermarktransparency);
                            break;
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
            client.Dispose();
            return newFilePath;
        }

        #region 私有方法
        /// <summary>
        /// 返回上传目录相对路径
        /// </summary>
        /// <param name="fileName">上传文件名</param>
        private string GetUpLoadPath()
        {
            string path = this.Config.Webpath + this.Config.Filepath + "/"; //站点目录+上传目录
            switch (this.Config.Filesave)
            {
                case 1: //按年月日每天一个文件夹
                    path += DateTime.Now.ToString("yyyyMMdd");
                    break;
                default: //按年月/日存入不同的文件夹
                    path += DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
                    break;
            }
            return path + "/";
        }

        /// <summary>
        /// 是否需要打水印
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsWaterMark(string _fileExt)
        {
            //判断是否开启水印
            if (this.Config.Watermarktype > 0)
            {
                //判断是否可以打水印的图片类型
                ArrayList al = new()
                {
                    "bmp",
                    "jpeg",
                    "jpg",
                    "png"
                };
                if (al.Contains(_fileExt.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private static bool IsImage(string _fileExt)
        {
            ArrayList al = new()
            {
                "bmp",
                "jpeg",
                "jpg",
                "gif",
                "png"
            };
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        private bool CheckFileExt(string _fileExt)
        {
            //检查危险文件
            string[] excExt = { "asp", "aspx", "ashx", "asa", "asmx", "asax", "php", "jsp", "htm", "html" };
            for (int i = 0; i < excExt.Length; i++)
            {
                if (excExt[i].ToLower() == _fileExt.ToLower())
                {
                    return false;
                }
            }
            //检查合法文件
            string[] allowExt = (this.Config.Fileextension + "," + this.Config.Videoextension).Split(',');
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i].ToLower() == _fileExt.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查文件大小是否合法
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <param name="_fileSize">文件大小(B)</param>
        private bool CheckFileSize(string _fileExt, long _fileSize)
        {
            //将视频扩展名转换成ArrayList
            ArrayList lsVideoExt = new(this.Config.Videoextension.ToLower().Split(','));
            //判断是否为图片文件
            if (IsImage(_fileExt))
            {
                if (this.Config.Imgsize > 0 && _fileSize > this.Config.Imgsize * 1024)
                {
                    return false;
                }
            }
            else if (lsVideoExt.Contains(_fileExt.ToLower()))
            {
                if (_fileSize > this.Config.Videosize * 1024)
                {
                    return false;
                }
            }
            else
            {
                if (this.Config.Attachsize > 0 && _fileSize > this.Config.Attachsize * 1024)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 删除上传的文件(及缩略图)
        /// </summary>
        /// <param name="_filepath"></param>
        public void DeleteUpFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return;
            }
            string fullpath = GetFullPath(_filepath); //原图
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            if (_filepath.LastIndexOf("/") >= 0)
            {
                string thumbnailpath = $"{_filepath.Substring(0, _filepath.LastIndexOf("/"))}mall_{_filepath[(_filepath.LastIndexOf("/") + 1)..]}";
                string fullTPATH = GetFullPath(thumbnailpath); //宿略图
                if (File.Exists(fullTPATH))
                {
                    File.Delete(fullTPATH);
                }
            }
        }

        public string GetFullPath(string path) 
        {
           return Path.Combine(BasePath, path);
        }
    }
}
