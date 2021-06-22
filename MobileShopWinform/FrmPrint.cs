using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmPrint : Form
    {
        private int orderID;

        public FrmPrint(int orderID)
        {
            InitializeComponent();
            this.orderID = orderID;

            dgvOrderDetail.AutoGenerateColumns = true;
            dgvOrderDetail.Columns.Add(Common.CreateDgvCol(30, "OrderDetailID", "ID"));
            dgvOrderDetail.Columns.Add(Common.CreateDgvCol(260, "ProductName", "Tên mặt hàng"));
            dgvOrderDetail.Columns.Add(Common.CreateDgvCol(120, "ProductPrice", "Đơn giá (VND)"));
            dgvOrderDetail.Columns.Add(Common.CreateDgvCol(80, "OrderDetailQuantity", "Số lượng"));
            dgvOrderDetail.Columns.Add(Common.CreateDgvCol(110, "OrderDetailTotalAmount", "Tổng tiền (VND)"));
            dgvOrderDetail.Columns.Add(Common.CreateDgvCol(150, "OrderDetailNote", "Ghi chú"));
        }

        private void GetData()
        {
            labelOrderID.Text = orderID.ToString();

            string query = string.Format($"select OrderDate, CustemerID, OrderTotalAmount from tblOrders where OrderID = {orderID}");
            SqlDataReader res = SqlCommon.ExecuteReader(query);

            if (res.HasRows)
            {
                res.Read();

                labelDate.Text = res["OrderDate"].ToString();
                labelTotalAmount.Text = res["OrderTotalAmount"].ToString() + " VND";

                string customerID = res["CustemerID"].ToString();
                // Get Customer Info
                string queryCustomer = string.Format($"select c.CustomerFirstName + ' ' + c.CustomerLastName as FullName, CustomerAddress, CustomerPhone, CustomerEmail from tblCustomers c where CustomerID = {customerID}");
                SqlDataReader resCustomer = SqlCommon.ExecuteReader(queryCustomer);
                if (resCustomer.HasRows)
                {
                    resCustomer.Read();
                    labelFullName.Text = resCustomer["FullName"].ToString();
                    labelPhone.Text = resCustomer["CustomerPhone"].ToString();
                }

                // Get OrderDetail
                string queryGetOrderDetail = string.Format(@"
                    select OrderDetailID, ProductName, OrderDetailQuantity,
                    OrderDetailNote, ProductPrice, ProductPrice * OrderDetailQuantity as OrderDetailTotalAmount
                    from tblOrderDetails
                    join tblProducts tP on tP.ProductID = tblOrderDetails.ProductID
                    where OrderID = {0}
                    ", orderID);
                SqlDataReader dataReader = SqlCommon.ExecuteReader(queryGetOrderDetail);
                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);
                dgvOrderDetail.DataSource = dataTable;
            }
        }
        private void FrmPrint_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
