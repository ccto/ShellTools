using ShellToolForSQLServer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace ShellToolForSQLServer.Dao
{
    public class ConStrDao
    {

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(ConStrInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ConStr(");
            strSql.Append("ConName,ConStrContent)");
            strSql.Append(" values (");
            strSql.Append("@ConName,@ConStrContent)");
            OleDbParameter[] parameters = {
                    new OleDbParameter("@ConName", OleDbType.VarChar,255),
                    new OleDbParameter("@ConStrContent", OleDbType.VarChar,0)};
            parameters[0].Value = model.ConName;
            parameters[1].Value = model.ConStrContent;

            int rows = AccessHelper.ExecuteNonQuery(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(ConStrInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ConStr set ");
            strSql.Append("ConName=@ConName,");
            strSql.Append("ConStrContent=@ConStrContent");
            strSql.Append(" where ID=@ID");
            OleDbParameter[] parameters = {
                    new OleDbParameter("@ConName", OleDbType.VarChar,255),
                    new OleDbParameter("@ConStrContent", OleDbType.VarChar,0),
                    new OleDbParameter("@ID", OleDbType.Integer,4)};
            parameters[0].Value = model.ConName;
            parameters[1].Value = model.ConStrContent;
            parameters[2].Value = model.ID;

            int rows = AccessHelper.ExecuteNonQuery(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ConStr ");
            strSql.Append(" where ID=@ID");
            OleDbParameter[] parameters = {
                    new OleDbParameter("@ID", OleDbType.Integer,4)
            };
            parameters[0].Value = ID;

            int rows = AccessHelper.ExecuteNonQuery(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ConStr ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
            int rows = AccessHelper.ExecuteNonQuery(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ConStrInfo GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,ConName,ConStrContent from ConStr ");
            strSql.Append(" where ID=@ID");
            OleDbParameter[] parameters = {
                    new OleDbParameter("@ID", OleDbType.Integer,4)
            };
            parameters[0].Value = ID;

            ConStrInfo model = new ConStrInfo();
            DataSet ds = AccessHelper.ExecuteDataSet(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ConStrInfo DataRowToModel(DataRow row)
        {
            ConStrInfo model = new ConStrInfo();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["ConName"] != null)
                {
                    model.ConName = row["ConName"].ToString();
                }
                if (row["ConStrContent"] != null)
                {
                    model.ConStrContent = row["ConStrContent"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,ConName,ConStrContent ");
            strSql.Append(" FROM ConStr ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return AccessHelper.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM ConStr ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = AccessHelper.ExecuteScalar(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ID desc");
            }
            strSql.Append(")AS Row, T.*  from ConStr T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return AccessHelper.ExecuteDataSet(strSql.ToString());
        }
    }
}
