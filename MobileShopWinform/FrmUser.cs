using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmUser : Form
    {
        private ControlHelper control = new ControlHelper();

        public FrmUser()
        {
            InitializeComponent();

            dgvUser.AutoGenerateColumns = true;
            dgvUser.Columns.Add(Common.CreateDgvCol(30, "UserID", "ID"));
            dgvUser.Columns.Add(Common.CreateDgvCol(300, "UserFullName", "Họ và tên"));
            dgvUser.Columns.Add(Common.CreateDgvCol(300, "UserName", "Tên đăng nhập"));
        }


        private void GetDgvData()
        {
            string query = @"
            select UserID, UserFullName, UserName, PassWord
            from tblUsers";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dgvUser.DataSource = dataTable;
        }

        private void FrmUser_Load(object sender, EventArgs e)
        {
            // Init control
            control.AddBtnControls(btnAdd, btnEdit, btnDelete, btnSave, btnCancel);
            control.AddTextBoxs(txtFullName, txtPassWord, txtUserName);
            control.AddDataGridView(dgvUser);
            control.SwitchMode(ControlHelper.ControlMode.None);

            GetDgvData();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            control.HandledAddClick();
            txtFullName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            control.HandledEditClick();
            txtFullName.Focus();
            txtUserName.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int idNeedDel = Common.GetCurID(dgvUser, "UserID");

            if (MyMessageBox.Question("Bạn có chắc chắn xoá bản ghi này không?"))
            {
                string query = $"DELETE FROM tblUsers WHERE UserID = {idNeedDel}";
                SqlCommon.ExecuteNonQuery(query);

                GetDgvData();
            }
        }

        private bool IsInvalid()
        {
            if (!MyValidation.IsTextInvalid(txtFullName.Text, 1, 30, "Họ và tên"))
            {
                txtFullName.Focus();
                return false;
            }

            if (!MyValidation.IsUserNameInvalid(txtUserName.Text))
            {
                txtUserName.Focus();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsInvalid())
            {
                string fullNname = txtFullName.Text;
                string userName = txtUserName.Text;
                string pas = txtPassWord.Text;
                string paswordEncode = Common.EncodePassWord(pas);

                switch (control.GetMode())
                {
                    case ControlHelper.ControlMode.Add:
                        {
                            if (pas.Length == 0)
                            {
                                MyMessageBox.Warning("Bạn chưa nhập mật khẩu");
                                return;
                            }

                            string query = string.Format(@"
                            insert into tblUsers
                            (UserFullName, UserName, PassWord)
                            values (N'{0}', N'{1}', '{2}');
                            ", fullNname, userName, paswordEncode);
                            SqlCommon.ExecuteNonQuery(query);
                            control.ClearTextBox();
                        }
                        break;
                    case ControlHelper.ControlMode.Edit:
                        {
                            int idNeedEdit = Common.GetCurID(dgvUser, "UserID");

                            string query;

                            if (pas.Length != 0)
                            {
                                query = string.Format(@"
                                update tblUsers
                                set UserFullName  = N'{0}', PassWord = '{1}'
                                where UserID = {2};
                                ", fullNname, paswordEncode, idNeedEdit);
                            } else
                            {
                                query = string.Format(@"
                                update tblUsers
                                set UserFullName = N'{0}'
                                where UserID = {1};
                                ", fullNname, idNeedEdit);
                            }

                            SqlCommon.ExecuteNonQuery(query);
                        }
                        break;
                }

                GetDgvData();
                control.SwitchMode(ControlHelper.ControlMode.None);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            control.HandleCancelClick();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUser_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            txtFullName.Text = dgvUser.Rows[idx].Cells["UserFullName"].Value.ToString();
            txtUserName.Text = dgvUser.Rows[idx].Cells["UserName"].Value.ToString();
        }
    }
}
