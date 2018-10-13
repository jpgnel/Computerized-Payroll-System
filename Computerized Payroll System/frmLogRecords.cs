using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Computerized_Payroll_System
{
    public partial class frmLogRecords : Form
    {
        public frmLogRecords()
        {
            InitializeComponent();
            dataGridInfo();
        }

        public void dataGridInfo()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = GetData();
            dataGridView1.Columns[0].HeaderText = "DATE";
            dataGridView1.Columns[1].HeaderText = "TIME-IN";
            dataGridView1.Columns[2].HeaderText = "TIME-OUT";
            dataGridView1.Columns[3].HeaderText = "TOTAL # OF HOURS";
            dataGridView1.Columns[4].HeaderText = "OVERTIME HOURS";
            dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public DataTable GetData()
        {
            using (SqlConnection openCon = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=sampleDB; " +
                                       "connection timeout=30"))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = openCon;            // <== lacking
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT date, timeIn, timeOut, totalHours, overtimeHours FROM logRecords WHERE empID=@empID";
                    command.Parameters.AddWithValue("@empID", frmLogIn2.empID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    openCon.Open();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }   
    }
}
