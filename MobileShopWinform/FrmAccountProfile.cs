using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmAccountProfile : Form
    {
        public FrmAccountProfile()
        {
            InitializeComponent();
        }

        private void GetData()
        {
            int userID = FrmLogin.curUserID;
            string query = $"select UserFullName, UserName, PassWord from tblUsers where UserID = {userID}";
            SqlDataReader reader = SqlCommon.ExecuteReader(query);
            if (reader.HasRows)
            {
                reader.Read();
                txtUserName.Text = reader["UserName"].ToString().Trim();
                txtFullName.Text = reader["UserFullName"].ToString().Trim();
            }
        }

        private void FrmAccountProfile_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text;
            string newPassWord = txtNewPassWord.Text;

            if (fullName.Length == 0)
            {
                MyMessageBox.Warning("Không được bỏ trống họ và tên");
                txtFullName.Focus();
                return;
            }

            // newPassWord.Length == 0 là trường hợp người dùng không muốn đổi mật khẩu
            if (newPassWord.Length == 0)
            {
                string query = @"
                update tblUsers
                set UserFullName = N'{0}' 
                where UserID = {1};";
                SqlCommon.ExecuteNonQuery(string.Format(query, fullName, FrmLogin.curUserID));
            }
            else
            {
                string query = @"
                update tblUsers
                set UserFullName = N'{0}', PassWord = '{1}'
                where UserID = {2};";
                SqlCommon.ExecuteNonQuery(string.Format(query, fullName, Common.EncodePassWord(newPassWord), FrmLogin.curUserID));
            }

            MyMessageBox.Information("Cập nhật thông tin thành công!");
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
