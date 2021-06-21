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
    public partial class FrmCategory : Form
    {
        private ControlHelper control = new ControlHelper();

        public FrmCategory()
        {
            InitializeComponent();

            dgvCategory.AutoGenerateColumns = true;
            dgvCategory.Columns.Add(Common.CreateDgvCol(30, "CategoryID", "ID"));
            dgvCategory.Columns.Add(Common.CreateDgvCol(200, "CategoryName", "Tên danh mục"));
            dgvCategory.Columns.Add(Common.CreateDgvCol(300, "CategoryDesc", "Mô tả"));
        }

        private void GetDgvData()
        {
            string query = @"select c.CategoryID, c.CategoryName, c.CategoryDesc from tblCategorys c;";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dgvCategory.DataSource = dataTable;
        }

        private void FrmCategory_Load(object sender, EventArgs e)
        {
            // Init control
            control.AddBtnControls(btnAdd, btnEdit, btnDelete, btnSave, btnCancel);
            control.AddTextBoxs(txtName, txtDesc);
            control.AddDataGridView(dgvCategory);
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
            int idNeedDel = Common.GetCurID(dgvCategory, "CategoryID");

            if (MyMessageBox.Question("Bạn có chắc chắn xoá bản ghi này không?"))
            {
                string query = $"DELETE FROM tblCategorys WHERE CategoryID = {idNeedDel}";
                SqlCommon.ExecuteNonQuery(query);

                GetDgvData();
            }
        }

        private bool IsInvalid()
        {
            if (txtName.Text.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập tên danh mục!");
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
insert into tblCategorys (CategoryName, CategoryDesc)
values (N'{0}', N'{1}');
                            ", name, desc);
                            SqlCommon.ExecuteNonQuery(query);
                            control.ClearTextBox();
                        }
                        break;
                    case ControlHelper.ControlMode.Edit:
                        {
                            int idNeedEdit = Common.GetCurID(dgvCategory, "CategoryID");

                            string query = string.Format(@"
update tblCategorys
set CategoryName  = N'{0}', CategoryDesc = N'{1}'
where CategoryID = {2};
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
            txtName.Text = dgvCategory.Rows[idx].Cells["CategoryName"].Value.ToString();
            txtDesc.Text = dgvCategory.Rows[idx].Cells["CategoryDesc"].Value.ToString();
        }
    }
}
