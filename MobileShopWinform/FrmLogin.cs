using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmLogin : Form
    {
        public static int curUserID;

        #region sql
        public static int Login(string userName, string passWord)
        {
            string passWordEncode = Common.EncodePassWord(passWord);

            string query = string.Format(@"
            select UserID, UserFullName, UserName, PassWord
            from tblUsers
            where UserName = '{0}' and  PassWord = '{1}'
            ", userName, passWordEncode);

            SqlDataReader reader = SqlCommon.ExecuteReader(query);

            if (reader.HasRows)
            {
                reader.Read();
                return Convert.ToInt32(reader["UserID"].ToString());
            }

            return -1;
        }
        #endregion

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            // ! For dev
            txtUserName.Text = "nihta";
            txtPassWord.Text = "123";
        }

        public bool IsInputInvalid()
        {
            string userName = txtUserName.Text;
            string passWord = txtPassWord.Text;

            if (userName.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập tên đăng nhập!");
                txtUserName.Focus();
                return false;
            }

            if (passWord.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập mật khẩu!");
                txtPassWord.Focus();
                return false;
            }

            return true;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (IsInputInvalid())
            {
                string userName = txtUserName.Text;
                string passWord = txtPassWord.Text;

                int userID = Login(userName, passWord);
                if (userID == -1)
                {
                    MyMessageBox.Warning("Tên đăng nhập hoặc mật khẩu không chính xác!");
                }
                else
                {
                    FrmLogin.curUserID = userID;

                    FrmMain frmMain = new FrmMain();
                    this.Hide();

                    // Xoá mật khẩu
                    txtPassWord.Focus();
                    txtPassWord.Clear();

                    frmMain.ShowDialog();
                    this.Show();
                }
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
