using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MobileShopWinform
{
    class Common
    {
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));
            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public static string EncodePassWord(string passWord)
        {
            return MD5Hash(MD5Hash(MD5Hash(passWord)));
        }

        public static int GetCurID(DataGridView dgv, string fieldNameID)
        {
            int curDgvRow = dgv.CurrentRow.Index;
            return Convert.ToInt32(dgv.Rows[curDgvRow].Cells[fieldNameID].Value.ToString());
        }

        public static DataGridViewColumn CreateDgvCol(int width, string name, string headerText = "", string dataPropertyName = "")
        {
            if (dataPropertyName == "")
            {
                dataPropertyName = name;
            }

            if (headerText == "")
            {
                headerText = name;
            }

            DataGridViewColumn col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = dataPropertyName;
            col.Name = name;
            col.HeaderText = headerText;
            col.Width = width;

            return col;
        }

        public static void ClearTextBox(params TextBox[] textBoxes)
        {
            foreach (TextBox tb in textBoxes)
            {
                tb.Clear();
            }
        }
    }

    class SqlCommon
    {
        public static string connectString = @"Data Source=.\sqlexpress;Initial Catalog=dbWfMoblieShop;Integrated Security=True";

        private SqlDataAdapter dataAdapter;
        private SqlCommandBuilder commandBuilder;

        public SqlCommandBuilder CommandBuilder { get => commandBuilder; set => commandBuilder = value; }

        public static int ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    int res = cmd.ExecuteNonQuery();
                    conn.Close();

                    return res;
                }
            }
        }

        public static object ExecuteScalar(string commandText, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    object res = cmd.ExecuteScalar();
                    conn.Close();

                    return res;
                }
            }
        }

        public static SqlDataReader ExecuteReader(string commandText, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connectString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
        }

        public DataTable ExecuteQuery(string commandText, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connectString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                DataTable dataTable = new DataTable();

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = cmd;

                CommandBuilder = new SqlCommandBuilder(dataAdapter);

                dataAdapter.Fill(dataTable);

                return dataTable;
            }
        }

        public void Update(DataTable dataTable)
        {
            CommandBuilder.GetUpdateCommand();
            dataAdapter.Update(dataTable);
        }
    }

    class DataTableCommon
    {
        public static void AddCol(DataTable dataTable, string colName, string type = "System.String", bool isAutoInc = false, bool isReadOnly = false, bool isUnique = false)
        {
            DataColumn column = new DataColumn();

            column.DataType = Type.GetType(type);
            column.ColumnName = colName;
            column.AutoIncrement = isAutoInc;
            column.ReadOnly = isReadOnly;
            column.Unique = isUnique;

            dataTable.Columns.Add(column);
        }

        public static void RemoveRow(DataTable dataTable, string rowIDName, int rowIDValue)
        {
            for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dataTable.Rows[i];
                if (Convert.ToInt32(row[rowIDName].ToString()) == rowIDValue)
                {
                    row.Delete();
                    return;
                }
            }
        }
    }

    class MyMessageBox
    {

        public static void Information(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warning(string message)
        {
            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool Question(string message)
        {
            DialogResult dialogResult = MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return dialogResult == DialogResult.Yes;
        }
    }

    class ControlHelper
    {
        private ControlMode mode = ControlMode.None;

        public enum ControlMode
        {
            /// <summary>
            /// Chế độ mặc định
            /// </summary>
            None,
            /// <summary>
            /// Chế độ thêm bản ghi
            /// </summary>
            Add,
            /// <summary>
            /// Chế độ chỉnh sửa
            /// </summary>
            Edit,
        }

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnSave;
        private Button btnCancel;

        private TextBox[] textBoxes;
        private ComboBox[] comboBoxes = null;

        private DataGridView dataGridView;

        public void AddBtnControls(Button btnAdd, Button btnEdit, Button btnDelete, Button btnSave, Button btnCancel)
        {
            this.btnAdd = btnAdd;
            this.btnEdit = btnEdit;
            this.btnDelete = btnDelete;
            this.btnSave = btnSave;
            this.btnCancel = btnCancel;
        }

        public void AddTextBoxs(params TextBox[] textBoxes)
        {
            this.textBoxes = textBoxes;
        }

        public void AddComboBoxs(params ComboBox[] comboBoxes)
        {
            this.comboBoxes = comboBoxes;
        }

        public void AddDataGridView(DataGridView dataGridView)
        {
            this.dataGridView = dataGridView;
        }

        public void EnableControl(bool isEnable)
        {
            btnAdd.Enabled = isEnable;
            btnEdit.Enabled = isEnable;
            btnDelete.Enabled = isEnable;
            btnSave.Enabled = !isEnable;
            btnCancel.Enabled = !isEnable;
        }

        public void EnableTextBox(bool isEnable)
        {
            foreach (TextBox tb in textBoxes)
            {
                tb.Enabled = isEnable;
            }
        }

        public void ClearTextBox()
        {
            foreach (TextBox tb in textBoxes)
            {
                tb.Clear();
            }
        }

        private void EnableComboBox(bool isEnable)
        {
            if (comboBoxes != null && comboBoxes.Length != 0)
            {
                foreach (ComboBox cb in comboBoxes)
                {
                    cb.Enabled = isEnable;
                }
            }
        }

        private void EnableDataGridView(bool isEnable)
        {
            if (dataGridView != null)
            {
                dataGridView.Enabled = isEnable;
            }
        }

        public ControlMode GetMode()
        {
            return mode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode">
        /// Các mode được hỗ trợ: "add", "edit", null
        /// </param>
        public void SwitchMode(ControlMode mode)
        {
            switch (mode)
            {
                case ControlMode.None:
                    EnableTextBox(false);
                    EnableComboBox(false);
                    EnableControl(true);
                    EnableDataGridView(true);
                    break;
                case ControlMode.Add:
                    EnableTextBox(true);
                    EnableComboBox(true);
                    EnableControl(false);
                    EnableDataGridView(false);
                    break;
                case ControlMode.Edit:
                    EnableTextBox(true);
                    EnableComboBox(true);
                    EnableControl(false);
                    EnableDataGridView(false);
                    break;
            }
            this.mode = mode;
        }

        #region HandleEvents
        public void HandledAddClick()
        {
            SwitchMode(ControlMode.Add);
            ClearTextBox();
        }

        public void HandledEditClick()
        {
            SwitchMode(ControlMode.Edit);
        }

        public void HandleCancelClick()
        {
            SwitchMode(ControlMode.None);
        }
        #endregion
    }

    class MyValidation
    {
        public static bool IsEmpty(string text, string name = "")
        {
            if (name != "" && text.Length == 0)
            {
                MyMessageBox.Warning($"{name} không được để trống!");
            }

            return text.Length == 0;
        }

        public static bool IsNumeric(string text, string name)
        {
            if (!Regex.IsMatch(text, @"^\d+$"))
            {
                MyMessageBox.Warning($"{name} không phải là số hợp lệ!");
                return false;
            }

            return true;
        }

        public static bool IsInRange(string text, int min, int max, string name)
        {
            if (text.Length < min)
            {
                MyMessageBox.Warning($"{name} quá ngắn!");
                return false;
            }
            else if (text.Length > max)
            {
                MyMessageBox.Warning($"{name} quá dài!");
                return false;
            }

            return true;
        }

        public static bool CommonValidation(string text, int min, int max, string name)
        {
            if (IsEmpty(text, name))
            {
                return false;
            }
            else if (!IsInRange(text, min, max, name))
            {
                return false;
            }

            return true;
        }

        public static bool IsPhoneInvalid(string text)
        {
            string name = "Số điện thoại";

            if (!Regex.IsMatch(text, @"^[0-9]*$"))
            {
                MyMessageBox.Warning($"Số điện thoại không hợp lệ!\n{name} chỉ bao gồm các số 0->9");
                return false;
            }

            return true;
        }

        public static bool IsEmailInvalid(string text)
        {
            if (!Regex.IsMatch(text, @"^\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b$"))
            {
                MyMessageBox.Warning($"Email không hợp lệ!");
                return false;
            }

            return true;
        }
    }

}
