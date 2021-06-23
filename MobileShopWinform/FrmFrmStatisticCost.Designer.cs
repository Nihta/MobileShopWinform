
namespace MobileShopWinform
{
    partial class FrmFrmStatisticCost
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dgvSupplyDetail = new System.Windows.Forms.DataGridView();
            this.btnFilter = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSup = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCost = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnDay = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnViewMore = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplyDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.CustomFormat = "dd/MM/yyyy";
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStart.Location = new System.Drawing.Point(20, 30);
            this.dateTimePickerStart.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerStart.MinDate = new System.DateTime(2021, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(178, 20);
            this.dateTimePickerStart.TabIndex = 18;
            this.dateTimePickerStart.Value = new System.DateTime(2021, 5, 19, 0, 0, 0, 0);
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "dd/MM/yyyy";
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(230, 30);
            this.dateTimePickerEnd.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerEnd.MinDate = new System.DateTime(2021, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(178, 20);
            this.dateTimePickerEnd.TabIndex = 20;
            this.dateTimePickerEnd.Value = new System.DateTime(2021, 5, 19, 0, 0, 0, 0);
            // 
            // dgvOrder
            // 
            this.dgvSupplyDetail.AllowUserToAddRows = false;
            this.dgvSupplyDetail.AllowUserToDeleteRows = false;
            this.dgvSupplyDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSupplyDetail.Location = new System.Drawing.Point(6, 19);
            this.dgvSupplyDetail.MultiSelect = false;
            this.dgvSupplyDetail.Name = "dgvOrder";
            this.dgvSupplyDetail.ReadOnly = true;
            this.dgvSupplyDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSupplyDetail.Size = new System.Drawing.Size(653, 467);
            this.dgvSupplyDetail.TabIndex = 21;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(440, 29);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 23);
            this.btnFilter.TabIndex = 22;
            this.btnFilter.Text = "Lọc phạm vi";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelSup);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelCost);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnAll);
            this.groupBox1.Controls.Add(this.btnFilter);
            this.groupBox1.Controls.Add(this.btnDay);
            this.groupBox1.Controls.Add(this.dateTimePickerStart);
            this.groupBox1.Controls.Add(this.dateTimePickerEnd);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 422);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Phạm vi thống kê";
            // 
            // labelSup
            // 
            this.labelSup.AutoSize = true;
            this.labelSup.Location = new System.Drawing.Point(156, 180);
            this.labelSup.Name = "labelSup";
            this.labelSup.Size = new System.Drawing.Size(13, 13);
            this.labelSup.TabIndex = 32;
            this.labelSup.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Số lượng nhà cung cấp:";
            // 
            // labelCost
            // 
            this.labelCost.AutoSize = true;
            this.labelCost.Location = new System.Drawing.Point(156, 144);
            this.labelCost.Name = "labelCost";
            this.labelCost.Size = new System.Drawing.Size(13, 13);
            this.labelCost.TabIndex = 30;
            this.labelCost.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Chi phí:";
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(440, 68);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(75, 23);
            this.btnAll.TabIndex = 28;
            this.btnAll.Text = "Tất cả";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnDay
            // 
            this.btnDay.Location = new System.Drawing.Point(20, 68);
            this.btnDay.Name = "btnDay";
            this.btnDay.Size = new System.Drawing.Size(75, 23);
            this.btnDay.TabIndex = 25;
            this.btnDay.Text = "Ngày";
            this.btnDay.UseVisualStyleBackColor = true;
            this.btnDay.Click += new System.EventHandler(this.btnDay_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(20, 21);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 33;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnViewMore
            // 
            this.btnViewMore.Location = new System.Drawing.Point(440, 21);
            this.btnViewMore.Name = "btnViewMore";
            this.btnViewMore.Size = new System.Drawing.Size(75, 23);
            this.btnViewMore.TabIndex = 34;
            this.btnViewMore.Text = "Xem chi tiết";
            this.btnViewMore.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnViewMore);
            this.panel1.Location = new System.Drawing.Point(12, 440);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(536, 64);
            this.panel1.TabIndex = 33;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvSupplyDetail);
            this.groupBox2.Location = new System.Drawing.Point(554, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(665, 492);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chi phí";
            // 
            // FrmFrmStatisticCost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 508);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Name = "FrmFrmStatisticCost";
            this.Text = "Thống kê chi phí";
            this.Load += new System.EventHandler(this.FrmFrmStatisticCost_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplyDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.DataGridView dgvSupplyDetail;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelSup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnDay;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnViewMore;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}