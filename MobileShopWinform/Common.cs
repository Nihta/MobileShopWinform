using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
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
}
