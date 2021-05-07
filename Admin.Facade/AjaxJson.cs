using System.Collections.Generic;
using Tool;
using Tool.Utils.Data;
// ReSharper disable InconsistentNaming

namespace Admin.Facade
{
    /// <summary>
    /// Ajax异步请求返回数据类
    /// </summary>
    public class AjaxJson
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 接口版本
        /// </summary>
        public double apiVersion { get; set; }

        /// <summary>
        /// 数据项列表
        /// </summary>
        public Dictionary<string, object> data { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AjaxJson()
        {
            code = 0;
            msg = "";
            apiVersion = 3.1;
            //SetDataItem("apiVersion", AppConfig.CodeMode.Demo);
            data = new Dictionary<string, object>();
        }

        /// <summary>
        /// 为数据项赋值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetPage(Tool.SqlCore.PagerSet pagerSet)
        {
            SetDataItem("PageCount", pagerSet.PageCount);

            SetDataItem("RecordCount", pagerSet.RecordCount);

            SetDataItem("PageIndex", pagerSet.PageIndex);

            SetDataItem("PageSize", pagerSet.PageSize);
        }

        /// <summary>
        /// 为数据项赋值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetDataItem(string key, object value)
        {
            if (value is not null && value is System.Data.DataTable)
            {
                value = value.ToVar<System.Data.DataTable>().ToDictionary();
            }
            else if (value is not null && value is System.Data.DataSet)
            {
                throw new System.Exception("无法添加DataSet数据源");
            }
            if (data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }
        }

        /// <summary>
        /// 为数据项赋值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="IsJson">是否是Json字符串</param>
        public void SetDataItem(string key, object value, bool IsJson)
        {
            if (IsJson)
            {
                value = value.ToString().JsonDynamic();
            }

            if (data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }
        }

        /// <summary>
        /// 为数据键值对赋值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetIDictionary(IDictionary<string, object> keyValues)
        {
            foreach (var keyValue in keyValues)
            {
                if (data.ContainsKey(keyValue.Key))
                {
                    data[keyValue.Key] = keyValue.Value;
                }
                else
                {
                    data.Add(keyValue.Key, keyValue.Value);
                }
            }
        }

        /// <summary>
        /// 为数据键值对赋值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetDataSet(System.Data.DataSet dataSet)
        {
            var keys = dataSet.ToDictionary();
            for (int i = 0; i < keys.Count; i++)
            {
                SetDataItem("Table" + i, keys[i]);
            }
        }

        ///// <summary>
        ///// 获取数据项值
        ///// </summary>
        ///// <param name="key">键</param>
        ///// <param name="value">值</param>
        //public object GetDataItemValue(string key, object value)
        //{
        //    return data[key];
        //}

        /// <summary>
        /// 序列化AjaxJson
        /// </summary>
        /// <returns>Json字符串</returns>
        public string SerializeToJson()
        {
            return this.ToJson();
        }
    }
}
