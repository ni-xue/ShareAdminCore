using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Entity.Config;
using Admin.Facade;
using Admin.Facade.Helper;
using Microsoft.AspNetCore.Http;
using Tool.Web.Api;

namespace AdminCore.Api
{
    public class Upload : MinApi
    {
        #region 上传文件处理===================================
        public async Task<IApiOut> UpLoadFile([ApiVal(Val.File)] IFormFile file, [ApiVal(Val.Service)] UpLoad upFiles, string DelFilePath)//string DelFilePath, string Filedata, string IsWater, string IsThumbnail
        {
            AjaxJson _ajv = new();
            string _delfile = DelFilePath?.TrimStart('/');
            bool _iswater = false; //默认不打水印
            bool _isthumbnail = false; //默认不生成缩略图

            //if (IsWater == "1")
            //    _iswater = true;
            //if (IsThumbnail == "1")
            //    _isthumbnail = true;

            if (file == null)
            {
                //Json("{\"status\": 0, \"msg\": \"请选择要上传文件！\"}");
                _ajv.code = 1;
                _ajv.msg = "请选择要上传文件！";
                return await ApiOut.JsonAsync(_ajv);
            }
            //UpLoad upFiles = new();
            //删除已存在的旧文件，旧文件不为空且应是上传文件，防止跨目录删除
            if (upFiles.FileSaveAs(file, _isthumbnail, _iswater, ref _ajv) && !string.IsNullOrEmpty(_delfile) && _delfile.StartsWith(upFiles.Config.Webpath + upFiles.Config.Filepath, StringComparison.OrdinalIgnoreCase))
            {
                upFiles.DeleteUpFile(_delfile);
            }
            //返回成功信息
            return await ApiOut.JsonAsync(_ajv);
        }
        #endregion
    }
}
