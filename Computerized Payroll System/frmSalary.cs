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
    public partial class frmSalary : Form
    {
        public frmSalary()
        {
            InitializeComponent();
            GetTransactionData();
        }

        SqlConnection openCon = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=dbPayroll; " +
                                       "connection timeout=30");

        public DataTable GetTransactionData()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT t.* " +
                                        "FROM tblEmpInfo as e inner join transactions as t on e.empID = t.empID " + 
                                        "WHERE e.empID = @empID";
                command.Parameters.AddWithValue("@empID", frmLogin.empID);

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView1.Columns[0].HeaderText = "TRANSACTION NO.";
                    dataGridView1.Columns[1].HeaderText = "EPLOYEE ID";
                    dataGridView1.Columns[2].HeaderText = "FROM DATE";
                    dataGridView1.Columns[3].HeaderText = "TO DATE";
                    dataGridView1.Columns[4].HeaderText = "TOTAL REGULAR HOURS";
                    dataGridView1.Columns[5].HeaderText = "TOTAL OVERTIME HOURS";
                    dataGridView1.Columns[6].HeaderText = "TOTAL NIGHT DIFF HOURS";
                    dataGridView1.Columns[7].HeaderText = "TOTAL REGULAR PAY";
                    dataGridView1.Columns[8].HeaderText = "TOTAL OVERTIME PAY";
                    dataGridView1.Columns[9].HeaderText = "TOTAL NIGHT DIFF PAY";
                    dataGridView1.Columns[10].HeaderText = "GROSS PAY";
                    dataGridView1.Columns[11].HeaderText = "SSS";
                    dataGridView1.Columns[12].HeaderText = "PHILHEALTH";
                    dataGridView1.Columns[13].HeaderText = "PAG-IBIG";
                    dataGridView1.Columns[14].HeaderText = "CASH ADVANCE";
                    dataGridView1.Columns[15].HeaderText = "NET PAY";
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
                return dt;
            }
        }

        public DataTable GetTransactionDataWDate()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT t.* " + 
                                        " FROM tblEmpInfo as e inner join transactions as t on e.empID = t.empID" + 
                                        " WHERE e.empID = @empID and t.FromDate = @FromDate and t.ToDate = @ToDate";
                command.Parameters.AddWithValue("@empID", frmLogin.empID);
                command.Parameters.AddWithValue("@FromDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@ToDate", dateTimePicker2.Value.ToString("yyyy-MM-dd"));

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].HeaderText = "EMPLOYEE ID";
                    dataGridView1.Columns[1].HeaderText = "LAST NAME";
                    dataGridView1.Columns[2].HeaderText = "FIRST NAME";
                    dataGridView1.Columns[3].HeaderText = "MIDDLE NAME";
                    dataGridView1.Columns[4].HeaderText = "POSITION";
                    dataGridView1.Columns[5].HeaderText = "FROM";
                    dataGridView1.Columns[6].HeaderText = "TO";
                    dataGridView1.Columns[7].HeaderText = "REGULAR HOURS";
                    dataGridView1.Columns[8].HeaderText = "OVERTIME HOURS";
                    dataGridView1.Columns[9].HeaderText = "NIGHT DIFFERENTIAL HOURS";
                    dataGridView1.Columns[10].HeaderText = "REGULAR PAY";
                    dataGridView1.Columns[11].HeaderText = "OVERTIME PAY";
                    dataGridView1.Columns[12].HeaderText = "NIGHT DIFFERENTIAL PAY";
                    dataGridView1.Columns[13].HeaderText = "GROSS PAY";
                    dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
                return dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetTransactionData();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            GetTransactionDataWDate();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            GetTransactionDataWDate();
        }

    }
}
