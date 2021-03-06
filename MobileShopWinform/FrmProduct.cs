using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmProduct : Form
    {
        public enum mode
        {
            nomal,
            select,
        };

        public static int productIdSelected = -1;

        private mode formMode;
        private ControlHelper control = new ControlHelper();

        #region Sql
        public static int GetPrice(int productID)
        {
            string query = $"select ProductPrice from tblProducts where ProductID = {productID}";
            int price = Convert.ToInt32(SqlCommon.ExecuteScalar(query).ToString());
            return price;
        }

        public static int GetNumOfItem(int productID)
        {
            string query = $"select ProductAmount from tblProducts where  ProductID = {productID}";
            int numOfItem = Convert.ToInt32(SqlCommon.ExecuteScalar(query).ToString());
            return numOfItem;
        }

        public static int UpdateNumOfItem(int productID, int change)
        {
            int newValue = GetNumOfItem(productID) + change;
            string query = $"update tblProducts set ProductAmount = {newValue} where ProductID = {productID};";
            int numOfItem = Convert.ToInt32(SqlCommon.ExecuteScalar(query).ToString());
            return numOfItem;
        }

        public static void FillCombox(ComboBox cb)
        {
            string query = "select ProductID, ProductName from tblProducts where ProductAmount != 0";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            cb.DataSource = dataTable;
            cb.DisplayMember = "ProductName";
            cb.ValueMember = "ProductID";
        }

        public static void UpdateTotalAmount(int productID)
        {
            string queryProductAmountBuy = $"select sum(SupplyDetailQuantity) from tblSupplyDetails where ProductID = {productID}";
            int productAmountBuy = Convert.ToInt32(SqlCommon.ExecuteScalar(queryProductAmountBuy).ToString());

            string queryProductAmountSell = $"select sum(OrderDetailQuantity) from tblOrderDetails where ProductID = {productID}";
            string res = SqlCommon.ExecuteScalar(queryProductAmountSell).ToString();
            int productAmountSell = 0;
            if (res != "")
            {
                productAmountSell = Convert.ToInt32(res);
            }

            SqlCommon.ExecuteNonQuery($"update tblProducts set ProductAmount = {productAmountBuy - productAmountSell} where ProductID = {productID};");
        }
        #endregion

        public FrmProduct(mode mode = mode.nomal)
        {
            InitializeComponent();

            dgvProduct.AutoGenerateColumns = false;
            dgvProduct.Columns.Add(Common.CreateDgvCol(30, "ProductID", "ID"));
            dgvProduct.Columns.Add(Common.CreateDgvCol(200, "ProductName", "Tên mặt hàng"));
            dgvProduct.Columns.Add(Common.CreateDgvCol(100, "ProductPrice", "Giá"));
            dgvProduct.Columns.Add(Common.CreateDgvCol(100, "ProductAmount", "Số lượng"));
            dgvProduct.Columns.Add(Common.CreateDgvCol(100, "CategoryName", "Danh mục"));
            dgvProduct.Columns.Add(Common.CreateDgvCol(100, "BrandName", "Nhãn hàng"));
            dgvProduct.Columns.Add(Common.CreateDgvCol(300, "ProductDesc", "Mô tả"));

            dgvProduct.Columns.Add(Common.CreateDgvCol(30, "CategoryID", "Nhãn hàng"));
            dgvProduct.Columns["CategoryID"].Visible = false;
            dgvProduct.Columns.Add(Common.CreateDgvCol(30, "BrandID", "Nhãn hàng"));
            dgvProduct.Columns["BrandID"].Visible = false;

            this.formMode = mode;
            if (formMode == mode.select)
            {
                FrmProduct.productIdSelected = -1;
                btnSelect.Show();
            }
        }


        private void GetDgvData(string where = "")
        {
            string query = string.Format(@"
                select  ProductID, ProductName, ProductPrice, ProductDesc, ProductAmount, t.BrandID, BrandName, tC.CategoryID, CategoryName from tblProducts
                join tblBrands t on t.BrandID = tblProducts.BrandID
                join tblCategorys tC on tC.CategoryID = tblProducts.CategoryID
                {0}
            ", where);
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dgvProduct.DataSource = dataTable;
        }

        private void GetcbCategoryData()
        {
            FrmCategory.FillCombobox(cbCategory);
            FrmCategory.FillCombobox(cbSearchCate);
        }

        private void GetcbBrandData()
        {
            FrmBrand.FillCombobox(cbBrand);
            FrmBrand.FillCombobox(cbSearchBrand);
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            // Init control
            control.AddBtnControls(btnAdd, btnEdit, btnDelete, btnSave, btnCancel);
            control.AddTextBoxs(txtName, txtPrice, txtDesc);
            control.AddDataGridView(dgvProduct);
            control.AddComboBoxs(cbBrand, cbCategory);
            control.SwitchMode(ControlHelper.ControlMode.None);

            GetDgvData();
            GetcbCategoryData();
            GetcbBrandData();
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
            int idNeedDel = Common.GetCurID(dgvProduct, "ProductID");

            if (MyMessageBox.Question("Bạn có chắc chắn xoá bản ghi này không?"))
            {
                string query = $"DELETE FROM tblProducts WHERE ProductID = {idNeedDel}";
                SqlCommon.ExecuteNonQuery(query);

                GetDgvData();
            }
        }

        private bool IsInvalid()
        {
            if (!MyValidation.IsTextInvalid(txtName.Text, 1, 30, "Tên mặt hàng"))
            {
                txtName.Focus();
                return false;
            }

            if (!MyValidation.IsNumeric(txtPrice.Text, "Giá mặt hàng"))
            {
                txtPrice.Focus();
                return false;
            }

            if (txtDesc.Text.Length != 0 && !MyValidation.IsTextInvalid(txtDesc.Text, 0, 50, "Mô tả", false))
            {
                txtDesc.Focus();
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
                int price = Convert.ToInt32(txtPrice.Text);
                int brandID = Convert.ToInt32(cbBrand.SelectedValue.ToString());
                int categoryID = Convert.ToInt32(cbCategory.SelectedValue.ToString());

                switch (control.GetMode())
                {
                    case ControlHelper.ControlMode.Add:
                        {
                            string query = string.Format(@"
                            insert into tblProducts (ProductName, ProductDesc, ProductPrice, BrandID, CategoryID)
                            values (N'{0}', N'{1}', {2}, {3}, {4});
                            ", name, desc, price, brandID, categoryID);
                            SqlCommon.ExecuteNonQuery(query);
                            control.ClearTextBox();
                        }
                        break;
                    case ControlHelper.ControlMode.Edit:
                        {
                            int idNeedEdit = Common.GetCurID(dgvProduct, "ProductID");

                            string query = string.Format(@"
                            update tblProducts
                            set ProductName = N'{0}',ProductDesc = N'{1}',ProductPrice = {2},BrandID = {3},CategoryID = {4}
                            where ProductID = {5};
                            ", name, desc, price, brandID, categoryID, idNeedEdit);

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

        private void dgvProduct_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;

            txtName.Text = dgvProduct.Rows[idx].Cells["ProductName"].Value.ToString();
            txtPrice.Text = dgvProduct.Rows[idx].Cells["ProductPrice"].Value.ToString();
            txtDesc.Text = dgvProduct.Rows[idx].Cells["ProductDesc"].Value.ToString();

            cbCategory.SelectedValue = Convert.ToInt32(dgvProduct.Rows[idx].Cells["CategoryID"].Value.ToString());
            cbBrand.SelectedValue = Convert.ToInt32(dgvProduct.Rows[idx].Cells["BrandID"].Value.ToString());
        }


        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvProduct.CurrentRow != null)
            {
                int curRowIdx = dgvProduct.CurrentRow.Index;
                int idSelected = Convert.ToInt32(dgvProduct.Rows[curRowIdx].Cells["ProductID"].Value.ToString());

                FrmProduct.productIdSelected = idSelected;

                this.Close();
            }
            else
            {
                MyMessageBox.Error("Không thể chọn!");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            GetDgvData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int categoryID = Convert.ToInt32(cbSearchCate.SelectedValue.ToString());
            int bardID = Convert.ToInt32(cbSearchBrand.SelectedValue.ToString());
            string productName = txtSearchName.Text;
            string whereQuery = $"where ProductName like N'%{productName}%' and t.BrandID = {bardID} and tC.CategoryID = {categoryID}";
            GetDgvData(whereQuery);
        }
    }
}
