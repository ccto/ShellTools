using ShellToolForSQLServer.Dao;
using ShellToolForSQLServer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShellToolForSQLServer
{
    public partial class FormMain : Form
    {
        public Dictionary<int, ConStrInfo> DicConStr = new Dictionary<int, ConStrInfo>();
        public FormMain()
        {
            InitializeComponent();
        }

        private void 数据库连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 数据库还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDBBackupRestore dbBackupRestore = new FormDBBackupRestore();
            dbBackupRestore.Show();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                tbFolderPath.Text = folder.SelectedPath;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            List<ConStrInfo> lstConStr = new List<ConStrInfo>();
            var dao = new ConStrDao();
            DataTable dt = dao.GetList("").Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                var tempItemp = new ConStrInfo()
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    ConName = dr["ConName"].ToString(),
                    ConStrContent = dr["ConStrContent"].ToString()
                };

                lstConStr.Add(tempItemp);

                cbDatabase.Items.Add(tempItemp);
            }
            cbDatabase.DisplayMember = "ConName";
            cbDatabase.ValueMember = "ID";
            cbDatabase.SelectedIndex = 0;

            DicConStr = lstConStr.ToDictionary(m => m.ID, m => m);
        }

        private void btnCheckSQL_Click(object sender, EventArgs e)
        {
            //遍历文件夹下的所有文件

            int conStrId = ((ConStrInfo)cbDatabase.SelectedItem).ID;
            var strModel = new ConStrDao().GetModel(conStrId);
            string conStr = strModel.ConStrContent;

            var dBFileInfo = new DatabaseDao().GetFileInfo(conStrId);

            string dbCurrentName = dBFileInfo.Name;
            string sqlDBName = tbDBReplaceDBName.Text;

            string folderPath = tbFolderPath.Text;
            DirectoryInfo theFolder = new DirectoryInfo(folderPath);

            foreach (FileInfo file in theFolder.GetFiles().OrderBy(m => m.Name))
            {
                FileStream fileStream = file.OpenRead();
                StreamReader streamReader = new StreamReader(fileStream);

                string strFirstLine = streamReader.ReadLine();
                //string strSQL = streamReader.ReadToEnd().ToString();
                string strOtherLine = streamReader.ReadToEnd().ToString();
                //string strOtherLine = strSQL.Substring(0, strFirstLine.Length);

                if (strFirstLine.IndexOf(sqlDBName) < 0)
                {
                    rtbResultContent.Text = rtbResultContent.Text + file.Name + "[数据库名称不一致]" + Environment.NewLine;
                    break;
                }
                else
                {
                    strFirstLine = strFirstLine.Replace(sqlDBName, dbCurrentName) + Environment.NewLine;

                    string strSQL = strFirstLine + strOtherLine;

                    string resultCheck = SqlHelper.ValidateSQL(conStr, strSQL.Split("GO".ToCharArray()));

                    try
                    {
                        SqlHelper.ExecuteNonQuery(conStr, CommandType.Text, strSQL);
                    }
                    catch (Exception ex)
                    {
                        string aa = ex.Message;

                    }

                    if (resultCheck == "OK")
                    {
                        rtbResultContent.Text = rtbResultContent.Text + file.Name + "[成功]" + Environment.NewLine;
                    }
                    else
                    {
                        rtbResultContent.Text = rtbResultContent.Text + file.Name + "[失败]" + Environment.NewLine + resultCheck;
                        break;
                    }




                }

            }
        }

        private void btnExecuteSQL_Click(object sender, EventArgs e)
        {

        }
    }
}
