using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmStatisticRevenue : Form
    {
        public FrmStatisticRevenue()
        {
            InitializeComponent();

            dgvOrder.AutoGenerateColumns = false;
            dgvOrder.Columns.Add(Common.CreateDgvCol(50, "OrderID", "ID"));
            dgvOrder.Columns.Add(Common.CreateDgvCol(200, "FullName", "Khách hàng"));
            dgvOrder.Columns.Add(Common.CreateDgvCol(130, "CustomerPhone", "Số điện thoại"));
            dgvOrder.Columns.Add(Common.CreateDgvCol(100, "OrderDate", "Ngày mua"));
            dgvOrder.Columns.Add(Common.CreateDgvCol(130, "OrderTotalAmount", "Tổng tiền"));
        }

        private void GetData(string whereQuery = "")
        {
            string query = string.Format(@"
                select OrderID, OrderDate, CustemerID, CustomerFirstName + ' ' + CustomerLastName as FullName,
                       CustomerPhone, OrderTotalAmount
                from tblOrders
                join tblCustomers tC on tC.CustomerID = tblOrders.CustemerID
                {0}
                order by OrderDate desc, FullName
            ", whereQuery);
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dgvOrder.DataSource = dataTable;

            // Count customer
            string queryCountCus = string.Format(@"
            select count(distinct CustomerID)
            from tblOrders
            join tblCustomers tC on tC.CustomerID = tblOrders.CustemerID
            {0}
            ", whereQuery);
            var resQueryCountCus = SqlCommon.ExecuteScalar(queryCountCus);
            if (resQueryCountCus.ToString() != "")
            {
                labelCustomer.Text = resQueryCountCus.ToString() + " người";
            }
            else
            {
                labelCustomer.Text = "0 người";
            }           

            // Sum total amount
            string querySumTotalAmount = string.Format(@"
            select sum(OrderTotalAmount)
            from tblOrders
            join tblCustomers tC on tC.CustomerID = tblOrders.CustemerID
            {0}
            ", whereQuery);
            var resQuerySumTotalAmount = SqlCommon.ExecuteScalar(querySumTotalAmount);
            if (resQuerySumTotalAmount.ToString() != "")
            {
                labelTotalAmount.Text = resQuerySumTotalAmount.ToString() + " VND";
            }
            else
            {
                labelTotalAmount.Text = "0 VND";
            }
        }

        private void FrmStatistic_Load(object sender, EventArgs e)
        {
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerEnd.Value = DateTime.Now;

            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnViewMore_Click(object sender, EventArgs e)
        {
            int orderID = Common.GetCurID(dgvOrder, "OrderID");
            FrmPrint f = new FrmPrint(orderID);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnDay_Click(object sender, EventArgs e)
        {
            string whereQuery = $"where CONVERT(VARCHAR, '{dateTimePickerStart.Text}', 103) >= CONVERT(VARCHAR, OrderDate, 103)";
            GetData(whereQuery);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            string whereQuery = $"where CONVERT(VARCHAR, '{dateTimePickerStart.Text}', 103) <= CONVERT(VARCHAR, OrderDate, 103) and CONVERT(VARCHAR, OrderDate, 103) <= CONVERT(VARCHAR, '{dateTimePickerEnd.Text}', 103)";
            GetData(whereQuery);
        }
    }
}
