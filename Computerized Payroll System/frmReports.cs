using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using DGVPrinterHelper;

namespace Computerized_Payroll_System
{
    public partial class frmReports : Form
    {
        public frmReports()
        {
            InitializeComponent();
        }

        SqlConnection openCon = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=dbPayroll; " +
                                       "connection timeout=30");

        private void frmReports_Load(object sender, EventArgs e)
        {
            GetTransactionData();
        }

        public DataTable GetTransactionData()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT CONCAT(e.LastName,', ',e.FirstName) as Employee, e.position, e.rate, t.totalregularHours, t.totalovertimeHours, t.totalnightDiffHours, t.totalregularPay, t.totalovertimePay, t.totalnightDiffPay, t.GrossPay, t.sss, t.PhilHealth, t.PagIBIG, t.CashAdvance, t.NetPay "+
                                        "FROM tblEmpInfo as e inner join transactions as t on e.empID = t.empID " + 
                                        "WHERE t.FromDate = @FromDate and t.ToDate = @ToDate";
                command.Parameters.AddWithValue("@FromDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@ToDate", dateTimePicker2.Value.ToString("yyyy-MM-dd"));

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView2.DataSource = dt;
                    dataGridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns[0].HeaderText = "EMP NAME";
                    dataGridView2.Columns[1].HeaderText = "POSITION";
                    dataGridView2.Columns[2].HeaderText = "RATE";
                    dataGridView2.Columns[3].HeaderText = "REG HRS";
                    dataGridView2.Columns[4].HeaderText = "OT HRS";
                    dataGridView2.Columns[5].HeaderText = "ND. HRS";
                    dataGridView2.Columns[6].HeaderText = "REG PAY";
                    dataGridView2.Columns[7].HeaderText = "OT PAY";
                    dataGridView2.Columns[8].HeaderText = "ND PAY";
                    dataGridView2.Columns[9].HeaderText = "GROSS PAY";
                    dataGridView2.Columns[10].HeaderText = "SSS";
                    dataGridView2.Columns[11].HeaderText = "PHILHEALTH";
                    dataGridView2.Columns[12].HeaderText = "PAGIBIG";
                    dataGridView2.Columns[13].HeaderText = "CA";
                    dataGridView2.Columns[14].HeaderText = "NET PAY";
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
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "TOLITS NANCY SPORTWEAR";//Header
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date.ToString("MM/dd/yyyy"));
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.FooterSpacing = 15;
            //Print landscape mode
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintDataGridView(dataGridView2);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            GetTransactionData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            GetTransactionData();
        }
    }
}
