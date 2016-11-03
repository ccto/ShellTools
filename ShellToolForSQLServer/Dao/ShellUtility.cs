using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShellToolForSQLServer.Dao
{
    public class ShellUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">数据库用户名</param>
        /// <param name="pwd">数据库密码</param>
        /// <param name="databasename">数据库名称</param>
        /// <param name="targetdir">sql文件路劲</param>
        public static void excutesqlfile(string userName, string pwd, string databasename, string targetdir, string fileName)
        {
            System.Diagnostics.Process sqlProcess = new System.Diagnostics.Process();
            sqlProcess.StartInfo.FileName = "osql.exe ";
            sqlProcess.StartInfo.Arguments = " -U " + userName + " -P " + pwd + " -d " + databasename + " -i " + targetdir + fileName;
            sqlProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            sqlProcess.Start();
            sqlProcess.WaitForExit();//程序安装过程中执行
            sqlProcess.Close();
        }

        public static void ExcuteSQLFile(string userName, string pwd, string databasename, string filePath)
        {
            System.Diagnostics.Process sqlProcess = new System.Diagnostics.Process();
            sqlProcess.StartInfo.FileName = "osql.exe ";
            sqlProcess.StartInfo.Arguments = " -U " + userName + " -P " + pwd + " -d " + databasename + " -i " + filePath;
            sqlProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            sqlProcess.Start();
            sqlProcess.WaitForExit();//程序安装过程中执行
            sqlProcess.Close();
        }

        /// <summary>
        /// 用Sqlcmd 来执行SQL文件
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="databasename"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ExcuteSQLFile4Sqlcmd(string userName, string pwd, string databasename, string filePath)
        {
            string result = "OK";

            System.Diagnostics.Process sqlProcess = new System.Diagnostics.Process();
            sqlProcess.StartInfo.FileName = "sqlcmd.exe ";
            sqlProcess.StartInfo.Arguments = " -U " + userName + " -P " + pwd + " -d " + databasename + " -i " + filePath;
            sqlProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            try
            {

                sqlProcess.Start();
                sqlProcess.WaitForExit();//程序安装过程中执行
                sqlProcess.Close();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                sqlProcess.Close();
            }
            return result;
        }

    }
}
