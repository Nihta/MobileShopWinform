using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmFrmStatisticCost : Form
    {
        public FrmFrmStatisticCost()
        {
            InitializeComponent();

            dgvSupplyDetail.AutoGenerateColumns = false;
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
        }

        private void GetData(string whereQuery = "")
        {
            string query = string.Format(@"
                select
                    SupplyDetailID, SupplyDetailQuantity, SupplyDetailNote,
                    SupplyDetailTotalAmount, SupplyDetailDate, tP.ProductID,
                    ProductName, tS.SupplierID, SupplierName
                from tblSupplyDetails
                join tblProducts tP on tP.ProductID = tblSupplyDetails.ProductID
                join tblSuppliers tS on tblSupplyDetails.SupplierID = tS.SupplierID
                {0}
                order by SupplyDetailID desc;
            ", whereQuery);
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dgvSupplyDetail.DataSource = dataTable;

            // Count sup
            string queryCountSup = string.Format(@"
            select count(distinct tS.SupplierID)
            from tblSupplyDetails
            join tblProducts tP on tP.ProductID = tblSupplyDetails.ProductID
            join tblSuppliers tS on tblSupplyDetails.SupplierID = tS.SupplierID
            {0}
            ", whereQuery);
            var resQueryCountSup = SqlCommon.ExecuteScalar(queryCountSup);
            if (resQueryCountSup.ToString() != "")
            {
                labelSup.Text = resQueryCountSup.ToString() + " người";
            }
            else
            {
                labelSup.Text = "0 người";
            }

            // Sum total cost
            string querySumCost = string.Format(@"
            select sum(SupplyDetailTotalAmount)
            from tblSupplyDetails
            join tblProducts tP on tP.ProductID = tblSupplyDetails.ProductID
            join tblSuppliers tS on tblSupplyDetails.SupplierID = tS.SupplierID
            {0}
            ", whereQuery);
            var resQuerySumCost = SqlCommon.ExecuteScalar(querySumCost);
            if (resQuerySumCost.ToString() != "")
            {
                labelCost.Text = resQuerySumCost.ToString() + " VND";
            }
            else
            {
                labelCost.Text = "0 VND";
            }
        }

        private void FrmFrmStatisticCost_Load(object sender, EventArgs e)
        {
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerEnd.Value = DateTime.Now;

            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnDay_Click(object sender, EventArgs e)
        {
            string whereQuery = $"where CONVERT(VARCHAR, '{dateTimePickerStart.Text}', 103) >= CONVERT(VARCHAR, SupplyDetailDate, 103)";
            GetData(whereQuery);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            string whereQuery = $"where CONVERT(VARCHAR, '{dateTimePickerStart.Text}', 103) <= CONVERT(VARCHAR, SupplyDetailDate, 103) and CONVERT(VARCHAR, SupplyDetailDate, 103) <= CONVERT(VARCHAR, '{dateTimePickerEnd.Text}', 103)";
            GetData(whereQuery);
        }
    }
}
