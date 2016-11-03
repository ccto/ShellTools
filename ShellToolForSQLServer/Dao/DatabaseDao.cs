using ShellToolForSQLServer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ShellToolForSQLServer.Dao
{
    public class DatabaseDao
    {
        public DatabaseFileInfo GetFileInfo(int conStrId)
        {
            DatabaseFileInfo model = new DatabaseFileInfo();

            var strModel = new ConStrDao().GetModel(conStrId);
            SqlConnection conn = new SqlConnection(strModel.ConStrContent);

            //数据库名称
            string databaseName = conn.Database;
            model.Name = databaseName;

            string strRestoreSql = "select database_id,name,physical_name AS CurrentLocation,state_desc,size,type_desc,[type] from sys.master_files where database_id = db_id(N'" + databaseName + "'); ";

            DataTable dt = SqlHelper.GetDataTable(strModel.ConStrContent, CommandType.Text, strRestoreSql, null);

            foreach (DataRow r in dt.Rows)
            {
                if (r["type"].ToString() == "0")
                {
                    model.MDFFilePath = r["CurrentLocation"].ToString();
                    model.MDFName = r["name"].ToString();
                }

                if (r["type"].ToString() == "1")
                {
                    model.LogFilePath = r["CurrentLocation"].ToString();
                    model.LogName = r["name"].ToString();
                }
            }

            return model;
        }

        /// <summary>
        /// 获取备份文件的文件信息
        /// </summary>
        /// <param name="conStrId"></param>
        /// <returns></returns>
        public DatabaseFileInfo GetBackupFileInfo(int conStrId, string backupFilePath)
        {
            DatabaseFileInfo model = new DatabaseFileInfo();

            var strModel = new ConStrDao().GetModel(conStrId);
            SqlConnection conn = new SqlConnection(strModel.ConStrContent);

            //数据库名称
            string databaseName = conn.Database;
            model.Name = databaseName;

            string strRestoreSql = "restore filelistonly from disk=N'" + backupFilePath + "' ";

            DataTable dt = SqlHelper.GetDataTable(strModel.ConStrContent, CommandType.Text, strRestoreSql, null);

            foreach (DataRow r in dt.Rows)
            {
                if (r["Type"].ToString() == "D")
                {
                    model.MDFFilePath = r["PhysicalName"].ToString();
                    model.MDFName = r["LogicalName"].ToString();
                }

                if (r["Type"].ToString() == "L")
                {
                    model.LogFilePath = r["PhysicalName"].ToString();
                    model.LogName = r["LogicalName"].ToString();
                }
            }

            return model;
        }

        public List<string> GetAllTable(int conStrId)
        {
            List<string> lstAllTables = new List<string>();
            //连接字符串获取
            var strModel = new ConStrDao().GetModel(conStrId);
            string conStr = strModel.ConStrContent;

            string sqlStr = "SELECT Name FROM SysObjects Where XType='U' ORDER BY Name";

            DataTable dt = SqlHelper.GetDataTable(conStr, CommandType.Text, sqlStr, null);
            foreach (DataRow dr in dt.Rows)
            {
                lstAllTables.Add(dr["Name"].ToString());
            }
            return lstAllTables;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conStrId"></param>
        /// <param name="tableName"></param>
        /// <param name="filterStr"></param>
        /// <returns></returns>
        public string GetDataInsertSql(int conStrId, string tableName, string filterStr)
        {
            StringBuilder sb = new StringBuilder();

            //连接字符串获取
            var strModel = new ConStrDao().GetModel(conStrId);
            string conStr = strModel.ConStrContent;

            string strFieldSql =
                "SELECT COLUMN_NAME,DATA_TYPE FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME='" + tableName + "'";

            strFieldSql += " and COLUMN_NAME not in ('ReportObjectIdHis--------------','ReportObjectFieldIdHis---------------------','ProductIdHis-----------')";
            DataTable dtFieldInfo = SqlHelper.GetDataTable(conStr, CommandType.Text, strFieldSql, null);

            string sqlQuerySql = "SELECT  * FROM    dbo." + tableName + " " + filterStr;

            DataTable dtData = SqlHelper.GetDataTable(conStr, CommandType.Text, sqlQuerySql, null);

            foreach (DataRow dr in dtData.Rows)
            {
                string insertSql = "INSERT INTO dbo." + tableName + "";
                List<string> lstField = new List<string>();
                List<string> lstData = new List<string>();

                foreach (DataRow column in dtFieldInfo.Rows)
                {
                    lstField.Add(column["COLUMN_NAME"].ToString());
                    string dataValue = "";
                    if (dr[column["COLUMN_NAME"].ToString()] != "NULL")
                    {
                        string dataStrValue = dr[column["COLUMN_NAME"].ToString()] + "";
                        switch (column["DATA_TYPE"].ToString())
                        {
                            case "datetime":
                                {
                                    dataValue = "'" + dataStrValue + "'";
                                }
                                break;
                            case "int":
                                {
                                    if (!string.IsNullOrEmpty(dataStrValue))
                                    {
                                        dataValue = dataStrValue + "";
                                    }
                                    else
                                    {
                                        dataValue = "NULL";
                                    }
                                }
                                break;
                            case "nvarchar":
                                {
                                    dataValue = "'" + dataStrValue.Replace("'", "''") + "'";
                                }
                                break;
                            case "smallint":
                                {
                                    if (!string.IsNullOrEmpty(dataStrValue))
                                    {
                                        dataValue = dataStrValue + "";
                                    }
                                    else
                                    {
                                        dataValue = "NULL";
                                    }
                                }
                                break;
                            case "uniqueidentifier":
                                {
                                    if (!string.IsNullOrEmpty(dataStrValue))
                                    {
                                        dataValue = "'" + dataStrValue.Replace("'", "''") + "'";
                                    }
                                    else
                                    {
                                        dataValue = "NULL";
                                    }
                                }
                                break;
                            default: dataValue = "'" + dataStrValue.Replace("'", "''") + "'"; break;
                        }
                    }
                    else
                    {
                        dataValue = "NULL";
                    }
                    lstData.Add(dataValue);
                }

                insertSql = insertSql + "(" + string.Join(",", lstField) + ")VALUES  (" + string.Join(",", lstData) + ");" + System.Environment.NewLine;

                sb.Append(insertSql);
            }
            return sb.ToString();
        }

        public string GetDataUpdateSql(int conStrId, string tableName, string filterStr, string idColumnName)
        {
            StringBuilder sb = new StringBuilder();

            //连接字符串获取
            var strModel = new ConStrDao().GetModel(conStrId);
            string conStr = strModel.ConStrContent;

            string strFieldSql =
                "SELECT COLUMN_NAME,DATA_TYPE FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME='" + tableName + "'";
            DataTable dtFieldInfo = SqlHelper.GetDataTable(conStr, CommandType.Text, strFieldSql, null);

            string sqlQuerySql = "SELECT  * FROM    dbo." + tableName + " " + filterStr;

            DataTable dtData = SqlHelper.GetDataTable(conStr, CommandType.Text, sqlQuerySql, null);

            foreach (DataRow dr in dtData.Rows)
            {
                List<string> lstColumn = new List<string> { "FilterSqlMdx" };
                string insertSql = "UPDATE dbo." + tableName + " set ";

                foreach (DataRow column in dtFieldInfo.Rows)
                {
                    var tempStr = "";
                    tempStr += " " + column["COLUMN_NAME"].ToString() + "=";
                    string dataValue = "";
                    if (dr[column["COLUMN_NAME"].ToString()] != "NULL")
                    {
                        string dataStrValue = dr[column["COLUMN_NAME"].ToString()] + "";
                        switch (column["DATA_TYPE"].ToString())
                        {
                            case "datetime":
                                {
                                    dataValue = "'" + dataStrValue + "'";
                                }
                                break;
                            case "int":
                                {
                                    if (!string.IsNullOrEmpty(dataStrValue))
                                    {
                                        dataValue = dataStrValue + "";
                                    }
                                    else
                                    {
                                        dataValue = "NULL";
                                    }
                                }
                                break;
                            case "nvarchar":
                                {
                                    dataValue = "'" + dataStrValue.Replace("'", "''") + "'";
                                }
                                break;
                            case "smallint":
                                {
                                    if (!string.IsNullOrEmpty(dataStrValue))
                                    {
                                        dataValue = dataStrValue + "";
                                    }
                                    else
                                    {
                                        dataValue = "NULL";
                                    }
                                }
                                break;
                            default: dataValue = "'" + dataStrValue.Replace("'", "''") + "'"; break;
                        }
                    }
                    else
                    {
                        dataValue = "NULL";
                    }
                    tempStr += dataValue + ",";
                    if (lstColumn.Contains(column["COLUMN_NAME"].ToString()))
                    {
                        insertSql += tempStr;
                    }
                }

                sb.Append(insertSql.Trim(',') + " where " + idColumnName + "=" + dr[idColumnName].ToString() + System.Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
