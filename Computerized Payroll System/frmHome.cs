using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Computerized_Payroll_System
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
            GetData();
        }

        SqlConnection openCon = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=dbPayroll; " +
                                       "connection timeout=30");
        public DataTable GetData()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT holidayName, date, Holidaytype from Holidays";

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].HeaderText = "NAME";
                    dataGridView1.Columns[1].HeaderText = "DATE";
                    dataGridView1.Columns[2].HeaderText = "HOLIDAY TYPE";
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
                return dt;
            }
        }
    }
}
