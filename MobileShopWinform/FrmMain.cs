using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShopWinform
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
        }

        private void nhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSupplier f = new FrmSupplier();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
