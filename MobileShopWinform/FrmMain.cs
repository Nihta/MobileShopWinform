using System;
using System.Data;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmMain : Form
    {
        private int totalAmount = 0;
        DataTable dtOrderDetail = new DataTable();

        public FrmMain()
        {
            InitializeComponent();

            // ConfigDataGridViewOrderDetail
            dgvOrderDetail.AutoGenerateColumns = false;
            dgvOrderDetail.Columns.Add(Common.CreateDgvCol(200, "ProductName", "Tên sản phẩm"));
            dgvOrderDetail.Columns.Add(Common.CreateDgvCol(80, "Quantity", "Số lượng"));
            dgvOrderDetail.Columns.Add(Common.CreateDgvCol(100, "Price", "Tổng tiền"));
            dgvOrderDetail.Columns.Add(Common.CreateDgvCol(300, "Note", "Ghi chú"));

            // ! Chặn sort
            foreach (DataGridViewColumn column in dgvOrderDetail.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // ConfigDataTableOrderDetail
            DataTableCommon.AddCol(dtOrderDetail, "ProductName", "System.String");
            DataTableCommon.AddCol(dtOrderDetail, "Quantity", "System.Int32");
            DataTableCommon.AddCol(dtOrderDetail, "Note", "System.String");
            DataTableCommon.AddCol(dtOrderDetail, "Price", "System.Int32");
            DataTableCommon.AddCol(dtOrderDetail, "ProductID", "System.Int32");
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            dgvOrderDetail.DataSource = dtOrderDetail;

            FrmProduct.FillCombox(cbProduct);
            FrmCustomer.FillCombox(cbCustomer);
            dateTimePickerOrder.Value = DateTime.Now;
        }

        private void btnAddOrderItem_Click(object sender, EventArgs e)
        {
            string productName = cbProduct.Text;
            int productId = Convert.ToInt32(cbProduct.SelectedValue.ToString());
            int quantity = Convert.ToInt32(numericUpDownQuantity.Value);
            string note = txtNote.Text;

            // Validate
            if (quantity == 0)
            {
                MyMessageBox.Warning("Số lượng mặt hàng phải khác không!");
                return;
            }

            if (!MyValidation.IsTextInvalid(note, 0, 50, "Ghi chú", false))
            {
                txtNote.Focus();
                return;
            }

            int price = FrmProduct.GetPrice(productId) * quantity;

            DataRow newRow = dtOrderDetail.NewRow();

            newRow["ProductName"] = productName;
            newRow["Quantity"] = quantity;
            newRow["Note"] = note;
            newRow["Price"] = price;
            newRow["ProductID"] = productId;

            dtOrderDetail.Rows.Add(newRow);

            totalAmount += price;
            txtTotalOrder.Text = totalAmount.ToString();

            // Clear input data
            numericUpDownQuantity.Value = 0;
            txtNote.Clear();
        }

        private void btnDelOrderItem_Click(object sender, EventArgs e)
        {
            if (dgvOrderDetail.CurrentRow != null)
            {
                int curRowIdx = dgvOrderDetail.CurrentRow.Index;

                int price = Convert.ToInt32(dgvOrderDetail.Rows[curRowIdx].Cells["Price"].Value.ToString());
                this.totalAmount -= price;
                txtTotalOrder.Text = totalAmount.ToString();

                dtOrderDetail.Rows[curRowIdx].Delete();
            }
            else
            {
                MyMessageBox.Warning("Không có gì để xoá!");
            }
        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            if (MyMessageBox.Question("Bạn có chắc chắn xoá toàn bộ bản ghi?"))
            {
                dtOrderDetail.Clear();
                this.totalAmount = 0;
                txtTotalOrder.Text = totalAmount.ToString();
            }
        }

        private void btnAddOrderAndPrint_Click(object sender, EventArgs e)
        {
            int custumerIDSelected = Convert.ToInt32(cbCustomer.SelectedValue.ToString());

            if (dtOrderDetail.Rows.Count == 0)
            {
                MyMessageBox.Warning("Giỏ hàng trống!");
                cbProduct.Focus();
                return;
            }

            if (custumerIDSelected < 1)
            {
                MyMessageBox.Warning("Chưa có thông tin người mua!");
                btnSearchCustomer.Focus();
                return;
            }

            if (!MyMessageBox.Question("Bạn có chắn chắn?"))
            {
                return;
            }

            // Add order
            string date = dateTimePickerOrder.Text;
            string queryAddOrder = string.Format(@"
                insert into tblOrders
                (OrderTotalAmount, OrderDate, CustemerID)
                values ({0}, '{1}', {2});
                ", totalAmount, date, custumerIDSelected);
            SqlCommon.ExecuteNonQuery(queryAddOrder);

            // Get last orderID
            string queryGetLastOrderID = @"select top 1 OrderID from tblOrders order by OrderID desc";
            int lastOrderID = Convert.ToInt32(SqlCommon.ExecuteScalar(queryGetLastOrderID).ToString());

            foreach (DataRow dataRow in dtOrderDetail.Rows)
            {
                // Add order detail
                string queryAddOrderDetail = string.Format(@"
                    insert into tblOrderDetails 
                        (OrderDetailQuantity, OrderDetailNote, ProductID, OrderID)
                    values ({0}, N'{1}', {2}, {3});
                ",
                Convert.ToInt32(dataRow["Quantity"].ToString()),
                dataRow["Note"].ToString(),
                Convert.ToInt32(dataRow["ProductID"].ToString()),
                lastOrderID
                );

                SqlCommon.ExecuteNonQuery(queryAddOrderDetail);
                FrmProduct.UpdateTotalAmount(Convert.ToInt32(dataRow["ProductID"].ToString()));

            }

            MyMessageBox.Information("Thành công!");
            dtOrderDetail.Clear();

            FrmPrint f = new FrmPrint(lastOrderID);
            this.Hide();
            f.ShowDialog();
            this.Show();

            dtOrderDetail.Clear();
            this.totalAmount = 0;
            txtTotalOrder.Text = totalAmount.ToString();
            dateTimePickerOrder.Value = DateTime.Now;
        }

        private void btnLastOrder_Click(object sender, EventArgs e)
        {
            string queryGetLastOrderID = @"select top 1 OrderID from tblOrders order by OrderID desc";
            int lastOrderID = Convert.ToInt32(SqlCommon.ExecuteScalar(queryGetLastOrderID).ToString());
            FrmPrint f = new FrmPrint(lastOrderID);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FrmCustomer f = new FrmCustomer(FrmCustomer.mode.select))
            {
                f.ShowDialog();
            }
            this.Show();
            FrmCustomer.FillCombox(cbCustomer);
            cbCustomer.SelectedValue = FrmCustomer.customerIdSelected;
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FrmProduct f = new FrmProduct(FrmProduct.mode.select))
            {
                f.ShowDialog();
            }
            this.Show();
            FrmProduct.FillCombox(cbProduct);
            cbProduct.SelectedValue = FrmProduct.productIdSelected;
        }

        #region Menu strip
        private void nhãnHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBrand f = new FrmBrand();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void danhMụcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCategory f = new FrmCategory();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCustomer f = new FrmCustomer();
            this.Hide();
            f.ShowDialog();
            this.Show();
            FrmCustomer.FillCombox(cbCustomer);
        }

        private void nhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSupplier f = new FrmSupplier();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void sảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmProduct f = new FrmProduct();
            this.Hide();
            f.ShowDialog();
            this.Show();
            FrmProduct.FillCombox(cbProduct);
        }

        private void đăngXuấtToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSupplyDetail f = new FrmSupplyDetail();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAccountProfile f = new FrmAccountProfile();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void thốngKêToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmStatisticRevenue f = new FrmStatisticRevenue();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void lịchSửNhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmFrmStatisticCost f = new FrmFrmStatisticCost();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void ngườiDùngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmUser f = new FrmUser();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
        #endregion

        // Tạm thời không dùng do tổng tiền bị disable
        private void txtTotalOrder_TextChanged(object sender, EventArgs e)
        {
            if (MyValidation.IsNumeric(txtTotalOrder.Text, "Tổng tiền"))
            {
                this.totalAmount = Convert.ToInt32(txtTotalOrder.Text.ToString());
            }
        }

        private void cbProduct_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbProduct.SelectedValue != null && cbProduct.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                string tmp = cbProduct.SelectedValue.ToString();
                int productID = Convert.ToInt32(tmp);
                int numOfItem = FrmProduct.GetNumOfItem(productID);
                numericUpDownQuantity.Maximum = numOfItem;
            }
        }
    }
}
