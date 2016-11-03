using System;
using System.Collections.Generic;
using System.Text;

namespace ShellToolForSQLServer.Model
{
    /// <summary>
    /// ConStr:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ConStrInfo
    {
        public ConStrInfo()
        { }
        #region Model
        private int _id;
        private string _conname;
        private string _constrcontent;
        /// <summary>
        /// 主键
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 连接保存的名称
        /// </summary>
        public string ConName
        {
            set { _conname = value; }
            get { return _conname; }
        }
        /// <summary>
        /// 连接字符串内容
        /// </summary>
        public string ConStrContent
        {
            set { _constrcontent = value; }
            get { return _constrcontent; }
        }
        #endregion Model

    }
}
