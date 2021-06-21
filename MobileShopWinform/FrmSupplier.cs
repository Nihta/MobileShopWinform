using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmSupplier : Form
    {
        private ControlHelper control = new ControlHelper();

        public FrmSupplier()
        {
            InitializeComponent();

            dgvSup.AutoGenerateColumns = true;
            dgvSup.Columns.Add(Common.CreateDgvCol(30, "SupplierID", "ID"));
            dgvSup.Columns.Add(Common.CreateDgvCol(200, "SupplierName", "Tên nhà cung cấp"));
            dgvSup.Columns.Add(Common.CreateDgvCol(140, "SupplierAddress", "Địa chỉ"));
            dgvSup.Columns.Add(Common.CreateDgvCol(100, "SupplierPhone", "Số điện thoại"));
            dgvSup.Columns.Add(Common.CreateDgvCol(200, "SupplierEmail", "Email"));
        }

        private void GetDgvData()
        {
            string query = @"select s.SupplierID, s.SupplierName, s.SupplierAddress, s.SupplierPhone, s.SupplierEmail from tblSuppliers s";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dgvSup.DataSource = dataTable;
        }


        private void FrmSupplier_Load(object sender, EventArgs e)
        {
            // Init control
            control.AddBtnControls(btnAdd, btnEdit, btnDelete, btnSave, btnCancel);
            control.AddTextBoxs(txtName, txtAddress, txtEmail, txtPhone);
            control.AddDataGridView(dgvSup);
            control.SwitchMode(ControlHelper.ControlMode.None);

            GetDgvData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            control.HandledAddClick();
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            control.HandledEditClick();
            txtName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int idNeedDel = Common.GetCurID(dgvSup, "SupplierID");

            if (MyMessageBox.Question("Bạn có chắc chắn xoá bản ghi này không?"))
            {
                string query = $"DELETE FROM tblSuppliers WHERE SupplierID = {idNeedDel}";
                SqlCommon.ExecuteNonQuery(query);

                GetDgvData();
            }
        }

        private bool IsInvalid()
        {
            if (txtName.Text.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập tên nhà cung cấp!");
                txtName.Focus();
                return false;
            }

            if (txtAddress.Text.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập địa chỉ!");
                txtAddress.Focus();
                return false;
            }

            if (txtPhone.Text.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập tên số điện thoại!");
                txtPhone.Focus();
                return false;
            }

            if (txtEmail.Text.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập địa chỉ email!");
                txtEmail.Focus();
                return false;
            }

            return true;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsInvalid())
            {
                string name = txtName.Text;
                string address = txtAddress.Text;
                string phone = txtPhone.Text;
                string email = txtEmail.Text;

                switch (control.GetMode())
                {
                    case ControlHelper.ControlMode.Add:
                        {
                            string query = string.Format(@"
                            insert into tblSuppliers (SupplierName, SupplierAddress, SupplierPhone, SupplierEmail)
                            values (N'{0}', N'{1}', '{2}', N'{3}');
                            ", name, address, phone, email);
                            SqlCommon.ExecuteNonQuery(query);
                            control.ClearTextBox();
                        }
                        break;
                    case ControlHelper.ControlMode.Edit:
                        {
                            int idNeedEdit = Common.GetCurID(dgvSup, "SupplierID");

                            string query = string.Format(@"
                            update tblSuppliers
                            set SupplierName = N'{0}', SupplierAddress = N'{1}', SupplierPhone = '{2}', SupplierEmail = N'{3}'
                            where SupplierID = {4};
                            ", name, address, phone, email, idNeedEdit);

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

        private void dgvCustomer_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;

            txtName.Text = dgvSup.Rows[idx].Cells["SupplierName"].Value.ToString();
            txtAddress.Text = dgvSup.Rows[idx].Cells["SupplierAddress"].Value.ToString();
            txtPhone.Text = dgvSup.Rows[idx].Cells["SupplierPhone"].Value.ToString();
            txtEmail.Text = dgvSup.Rows[idx].Cells["SupplierEmail"].Value.ToString();
        }
    }
}
