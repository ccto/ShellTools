using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShellToolForSQLServer.Model
{
    public class DatabaseFileInfo
    {
        /// <summary>
        /// 数据文件路径
        /// </summary>
        public string MDFFilePath { get; set; }

        /// <summary>
        /// 日志文件路径
        /// </summary>
        public string LogFilePath { get; set; }

        /// <summary>
        /// 逻辑数据文件名
        /// </summary>
        public string MDFName { get; set; }

        /// <summary>
        /// 逻辑日志文件名
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        public string Name { get; set; }
    }
}
