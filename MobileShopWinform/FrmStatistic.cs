using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmStatistic : Form
    {
        public FrmStatistic()
        {
            InitializeComponent();

            dgvOrder.AutoGenerateColumns = false;
            dgvOrder.Columns.Add(Common.CreateDgvCol(50, "OrderID", "ID"));
            dgvOrder.Columns.Add(Common.CreateDgvCol(200, "FullName", "Khách hàng"));
            dgvOrder.Columns.Add(Common.CreateDgvCol(130, "CustomerPhone", "Số điện thoại"));
            dgvOrder.Columns.Add(Common.CreateDgvCol(100, "OrderDate", "Ngày mua"));
            dgvOrder.Columns.Add(Common.CreateDgvCol(130, "OrderTotalAmount", "Tổng tiền"));
        }

        private void GetData()
        {
            string query = @"
            select OrderID, OrderDate, CustemerID, CustomerFirstName + ' ' + CustomerLastName as FullName, CustomerPhone, OrderTotalAmount
            from tblOrders
            join tblCustomers tC on tC.CustomerID = tblOrders.CustemerID
            order by OrderDate desc, FullName
            ";
            SqlDataReader dataReader = SqlCommon.ExecuteReader(query);

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dgvOrder.DataSource = dataTable;
        }

        private void FrmStatistic_Load(object sender, EventArgs e)
        {
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerStart.Value = DateTime.Now;

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
    }
}
