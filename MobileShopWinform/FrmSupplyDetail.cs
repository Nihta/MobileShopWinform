using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmSupplyDetail : Form
    {

        private ControlHelper control = new ControlHelper();

        public FrmSupplyDetail()
        {
            InitializeComponent();

            dgvSupplyDetail.AutoGenerateColumns = true;
            dgvSupplyDetail.Columns.Add(Common.CreateDgvCol(30, "SupplyDetailID", "ID"));
            dgvSupplyDetail.Columns.Add(Common.CreateDgvCol(200, "ProductName", "Tên mặt hàng"));
            dgvSupplyDetail.Columns.Add(Common.CreateDgvCol(200, "SupplierName", "Nhà cung cấp"));
            dgvSupplyDetail.Columns.Add(Common.CreateDgvCol(100, "SupplyDetailQuantity", "Số lượng"));
            dgvSupplyDetail.Columns.Add(Common.CreateDgvCol(100, "SupplyDetailTotalAmount", "Tổng tiền"));
            dgvSupplyDetail.Columns.Add(Common.CreateDgvCol(100, "SupplyDetailDate", "Ngày nhập"));
            dgvSupplyDetail.Columns.Add(Common.CreateDgvCol(200, "SupplyDetailNote", "Ghi chú"));

            dgvSupplyDetail.Columns.Add(Common.CreateDgvCol(30, "SupplierID", "Ẩn cái này"));
            dgvSupplyDetail.Columns["SupplierID"].Visible = false;
            dgvSupplyDetail.Columns.Add(Common.CreateDgvCol(30, "ProductID", "Ẩn cái này"));
            dgvSupplyDetail.Columns["ProductID"].Visible = false;

            numericUpDownQuantity.Enabled = false;
            dateTimePickerDate.Enabled = false;
        }

        private void GetDgvData()
        {
            string query = @"
                select
                    SupplyDetailID, SupplyDetailQuantity, SupplyDetailNote,
                    SupplyDetailTotalAmount, SupplyDetailDate, tP.ProductID,
                    ProductName, tS.SupplierID, SupplierName
                from tblSupplyDetails
                join tblProducts tP on tP.ProductID = tblSupplyDetails.ProductID
                join tblSuppliers tS on tblSupplyDetails.SupplierID = tS.SupplierID
                order by SupplyDetailID desc;
            ";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dgvSupplyDetail.DataSource = dataTable;
        }

        private void GetcbProductData()
        {
            string query = @"select ProductID, ProductName from tblProducts";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            cbProduct.DataSource = dataTable;
            cbProduct.DisplayMember = "ProductName";
            cbProduct.ValueMember = "ProductID";
        }

        private void GetcbSupData()
        {
            string query = @"select SupplierID, SupplierName from tblSuppliers";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            cbSup.DataSource = dataTable;
            cbSup.DisplayMember = "SupplierName";
            cbSup.ValueMember = "SupplierID";
        }

        private void FrmSupplyDetail_Load(object sender, EventArgs e)
        {
            // Init control
            control.AddBtnControls(btnAdd, btnEdit, btnDelete, btnSave, btnCancel);
            control.AddTextBoxs(txtNote, txtNoteTotalAmount);
            control.AddComboBoxs(cbSup, cbProduct);
            control.AddDataGridView(dgvSupplyDetail);
            control.SwitchMode(ControlHelper.ControlMode.None);

            GetDgvData();
            GetcbProductData();
            GetcbSupData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            control.HandledAddClick();
            cbProduct.Focus();

            numericUpDownQuantity.Value = 0;
            txtNoteTotalAmount.Clear();

            numericUpDownQuantity.Enabled = true;
            dateTimePickerDate.Enabled = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            control.HandledEditClick();
            cbProduct.Focus();

            numericUpDownQuantity.Enabled = true;
            dateTimePickerDate.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int idNeedDel = Common.GetCurID(dgvSupplyDetail, "SupplyDetailID");

            if (MyMessageBox.Question("Xoá bản ghi dẫn đến nhiều rủi ro!\nBạn có chắc chắn xoá bản ghi này không?"))
            {
                string query = $"DELETE FROM tblSupplyDetails WHERE SupplyDetailID = {idNeedDel}";
                SqlCommon.ExecuteNonQuery(query);

                GetDgvData();
            }

        }
        private bool IsInvalid()
        {
            if ((int)numericUpDownQuantity.Value == 0)
            {
                MyMessageBox.Warning("Số lượng phải khác 0!");
                numericUpDownQuantity.Focus();
                return false;
            }

            if (txtNoteTotalAmount.Text.Length == 0)
            {
                MyMessageBox.Warning("Bạn chưa nhập tên tổng tiền!");
                txtNoteTotalAmount.Focus();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsInvalid())
            {
                string note = txtNote.Text;
                int totalAmount = Convert.ToInt32(txtNoteTotalAmount.Text);
                string date = dateTimePickerDate.Text;
                int productID = Convert.ToInt32(cbProduct.SelectedValue.ToString());
                int supplierID = Convert.ToInt32(cbSup.SelectedValue.ToString());
                int quantity = (int)numericUpDownQuantity.Value;


                switch (control.GetMode())
                {
                    case ControlHelper.ControlMode.Add:
                        {
                            string query = string.Format(@"
                            insert into tblSupplyDetails
                                (SupplyDetailQuantity, SupplyDetailNote, SupplyDetailDate,
                                SupplyDetailTotalAmount, ProductID, SupplierID)
                            values ({0}, N'{1}', '{2}', {3}, {4}, {5});
                            ", quantity, note, date, totalAmount, productID, supplierID);
                            SqlCommon.ExecuteNonQuery(query);
                            control.ClearTextBox();
                        }
                        break;
                    case ControlHelper.ControlMode.Edit:
                        {
                            int idNeedEdit = Common.GetCurID(dgvSupplyDetail, "SupplyDetailID");

                            string query = string.Format(@"
                            update tblSupplyDetails
                            set SupplyDetailQuantity = {0}, SupplyDetailNote = N'{1}',
                                SupplyDetailDate = '{2}', SupplyDetailTotalAmount = {3},
                                ProductID = {4}, SupplierID = {5}
                            where SupplyDetailID = {6};
                            ", quantity, note, date, totalAmount, productID, supplierID, idNeedEdit);

                            SqlCommon.ExecuteNonQuery(query);
                        }
                        break;
                }
                // ! Chưa hoạt động với order (cần trừ đi số đã bán)
                // Cập nhật lại số lượng mặt hàng
                string queryGetPrAm = $"select sum(SupplyDetailQuantity) from tblSupplyDetails where ProductID = {productID}";
                int productAmount = Convert.ToInt32(SqlCommon.ExecuteScalar(queryGetPrAm).ToString());
                SqlCommon.ExecuteNonQuery($"update tblProducts set ProductAmount = {productAmount} where ProductID = {productID};");

                GetDgvData();
                control.SwitchMode(ControlHelper.ControlMode.None);
                numericUpDownQuantity.Enabled = false;
                dateTimePickerDate.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            control.HandleCancelClick();
            numericUpDownQuantity.Enabled = false;
            dateTimePickerDate.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvSupplyDetail_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            txtNote.Text = dgvSupplyDetail.Rows[idx].Cells["SupplyDetailNote"].Value.ToString();
            txtNoteTotalAmount.Text = dgvSupplyDetail.Rows[idx].Cells["SupplyDetailTotalAmount"].Value.ToString();
            cbSup.SelectedValue = Convert.ToInt32(dgvSupplyDetail.Rows[idx].Cells["SupplierID"].Value.ToString());
            cbProduct.SelectedValue = Convert.ToInt32(dgvSupplyDetail.Rows[idx].Cells["ProductID"].Value.ToString());
            numericUpDownQuantity.Value = Convert.ToDecimal(dgvSupplyDetail.Rows[idx].Cells["SupplyDetailQuantity"].Value.ToString());
            // Đang lỗi
            //dateTimePickerDate.Text = dgvSupplyDetail.Rows[idx].Cells["SupplyDetailDate"].Value.ToString().Trim();
        }

        private void numericUpDownQuantity_ValueChanged(object sender, EventArgs e)
        {
            if (cbProduct.SelectedValue != null && control.GetMode() != ControlHelper.ControlMode.None)
            {
                int productID = Convert.ToInt32(cbProduct.SelectedValue.ToString());
                string query = $"select ProductPrice from tblProducts where ProductID = {productID}";
                int price = Convert.ToInt32(SqlCommon.ExecuteScalar(query).ToString());
                txtNoteTotalAmount.Text = ((int)numericUpDownQuantity.Value * price).ToString();
            }
        }
    }
}
