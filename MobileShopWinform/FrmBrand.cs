using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmBrand : Form
    {
        private ControlHelper control = new ControlHelper();

        #region sql
        public static void FillCombobox(ComboBox cb)
        {
            string query = @"select BrandID, BrandName from tblBrands";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            cb.DataSource = dataTable;
            cb.DisplayMember = "BrandName";
            cb.ValueMember = "BrandID";
        }
        #endregion

        public FrmBrand()
        {
            InitializeComponent();

            dgvBrand.AutoGenerateColumns = true;
            dgvBrand.Columns.Add(Common.CreateDgvCol(30, "BrandID", "ID"));
            dgvBrand.Columns.Add(Common.CreateDgvCol(200, "BrandName", "Tên nhãn hàng"));
            dgvBrand.Columns.Add(Common.CreateDgvCol(300, "BrandDesc", "Mô tả"));
        }

        private void GetDgvData()
        {
            string query = @"
                select b.BrandID, b.BrandName, b.BrandDesc
                from tblBrands b
                ";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dgvBrand.DataSource = dataTable;
        }

        private void FrmBrand_Load(object sender, EventArgs e)
        {
            // Init control
            control.AddBtnControls(btnAdd, btnEdit, btnDelete, btnSave, btnCancel);
            control.AddTextBoxs(txtName, txtDesc);
            control.AddDataGridView(dgvBrand);
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
            int idNeedDel = Common.GetCurID(dgvBrand, "BrandID");

            if (MyMessageBox.Question("Bạn có chắc chắn xoá bản ghi này không?"))
            {
                string query = $"DELETE FROM tblBrands WHERE BrandID = {idNeedDel}";
                SqlCommon.ExecuteNonQuery(query);

                GetDgvData();
            }
        }

        private bool IsInvalid()
        {
            if (!MyValidation.IsTextInvalid(txtName.Text, 1, 30, "Tên nhãn hàng"))
            {
                txtName.Focus();
                return false;
            }

            if (!MyValidation.IsTextInvalid(txtDesc.Text, 1, 50, "Mô tả", false))
            {
                txtName.Focus();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsInvalid())
            {
                string name = txtName.Text;
                string desc = txtDesc.Text;

                switch (control.GetMode())
                {
                    case ControlHelper.ControlMode.Add:
                        {
                            string query = string.Format(@"
                            insert into tblBrands (BrandName, BrandDesc)
                            values (N'{0}', N'{1}');
                            ", name, desc);
                            SqlCommon.ExecuteNonQuery(query);
                            control.ClearTextBox();
                        }
                        break;
                    case ControlHelper.ControlMode.Edit:
                        {
                            int idNeedEdit = Common.GetCurID(dgvBrand, "BrandID");

                            string query = string.Format(@"
                            update tblBrands
                            set BrandName  = N'{0}', BrandDesc = N'{1}'
                            where BrandID = {2};
                            ", name, desc, idNeedEdit);

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

        private void dgvBrand_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            txtName.Text = dgvBrand.Rows[idx].Cells["BrandName"].Value.ToString();
            txtDesc.Text = dgvBrand.Rows[idx].Cells["BrandDesc"].Value.ToString();
        }
    }
}
