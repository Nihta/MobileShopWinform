﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmCustomer : Form
    {
        private ControlHelper control = new ControlHelper();

        public FrmCustomer()
        {
            InitializeComponent();

            dgvCustomer.AutoGenerateColumns = true;
            dgvCustomer.Columns.Add(Common.CreateDgvCol(30, "CustomerID", "ID"));
            dgvCustomer.Columns.Add(Common.CreateDgvCol(200, "CustomerFirstName", "Họ"));
            dgvCustomer.Columns.Add(Common.CreateDgvCol(140, "CustomerLastName", "Tên"));
            dgvCustomer.Columns.Add(Common.CreateDgvCol(140, "CustomerAddress", "Địa chỉ"));
            dgvCustomer.Columns.Add(Common.CreateDgvCol(100, "CustomerPhone", "Số điện thoại"));
            dgvCustomer.Columns.Add(Common.CreateDgvCol(200, "CustomerEmail", "Email"));
        }

        private void GetDgvData()
        {
            string query = @"select CustomerID, CustomerFirstName, CustomerLastName, CustomerAddress, CustomerPhone, CustomerEmail from tblCustomers c";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dgvCustomer.DataSource = dataTable;
        }

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            // Init control
            control.AddBtnControls(btnAdd, btnEdit, btnDelete, btnSave, btnCancel);
            control.AddTextBoxs(txtFName, txtLName, txtAddress, txtEmail, txtPhone);
            control.AddDataGridView(dgvCustomer);
            control.SwitchMode(ControlHelper.ControlMode.None);

            GetDgvData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            control.HandledAddClick();
            txtFName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            control.HandledEditClick();
            txtFName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int idNeedDel = Common.GetCurID(dgvCustomer, "CustomerID");

            if (MyMessageBox.Question("Bạn có chắc chắn xoá bản ghi này không?"))
            {
                string query = $"DELETE FROM tblCustomers WHERE CustomerID = {idNeedDel}";
                SqlCommon.ExecuteNonQuery(query);

                GetDgvData();
            }
        }

        private bool IsInvalid()
        {
            if (txtFName.Text.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập họ!");
                txtFName.Focus();
                return false;
            }

            if (txtLName.Text.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập tên!");
                txtLName.Focus();
                return false;
            }

            if (txtPhone.Text.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập tên số điện thoại!");
                txtPhone.Focus();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsInvalid())
            {
                string fName = txtFName.Text;
                string lName = txtLName.Text;
                string address = txtAddress.Text;
                string phone = txtPhone.Text;
                string email = txtEmail.Text;

                switch (control.GetMode())
                {
                    case ControlHelper.ControlMode.Add:
                        {
                            string query = string.Format(@"
insert into tblCustomers (CustomerFirstName, CustomerLastName, CustomerAddress, CustomerPhone, CustomerEmail)
values (N'{0}', N'{1}', N'{2}', '{3}', N'{4}');
                            ", fName, lName, address, phone, email);
                            SqlCommon.ExecuteNonQuery(query);
                            control.ClearTextBox();
                        }
                        break;
                    case ControlHelper.ControlMode.Edit:
                        {
                            int idNeedEdit = Common.GetCurID(dgvCustomer, "CustomerID");

                            string query = string.Format(@"
update tblCustomers
set CustomerFirstName = N'{0}', CustomerLastName = N'{1}', CustomerAddress = N'{2}', CustomerPhone = '{3}', CustomerEmail = N'{4}'
where CustomerID = {5};
                                ", fName, lName, address, phone, email, idNeedEdit);

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

            txtFName.Text = dgvCustomer.Rows[idx].Cells["CustomerFirstName"].Value.ToString();
            txtLName.Text = dgvCustomer.Rows[idx].Cells["CustomerLastName"].Value.ToString();
            txtAddress.Text = dgvCustomer.Rows[idx].Cells["CustomerAddress"].Value.ToString();
            txtPhone.Text = dgvCustomer.Rows[idx].Cells["CustomerPhone"].Value.ToString();
            txtEmail.Text = dgvCustomer.Rows[idx].Cells["CustomerEmail"].Value.ToString();
        }
    }
}
