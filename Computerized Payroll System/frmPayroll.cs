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
    public partial class frmPayroll : Form
    {
        public frmPayroll()
        {
            InitializeComponent();
        }

        private void frmPayroll_Load(object sender, EventArgs e)
        {
            GetData();
            GetTransactionData();
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
                command.CommandText = "SELECT  empID, lastName, firstName, middleName, position from tblEmpInfo ";

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].HeaderText = "EMPLOYEE ID";
                    dataGridView1.Columns[1].HeaderText = "LAST NAME";
                    dataGridView1.Columns[2].HeaderText = "FIRST NAME";
                    dataGridView1.Columns[3].HeaderText = "MIDDLE NAME";
                    dataGridView1.Columns[4].HeaderText = "POSITION";
                    dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
                return dt;
            }
        }

        public DataTable GetTransactionData()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT  t.* " +
                                        "FROM transactions as t WHERE t.empID = '" + txtEmpID.Text + "';";

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                    dataGridView2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView2.Columns[0].HeaderText = "TRANSACTION NO.";
                    dataGridView2.Columns[1].HeaderText = "EPLOYEE ID";
                    dataGridView2.Columns[2].HeaderText = "FROM DATE";
                    dataGridView2.Columns[3].HeaderText = "TO DATE";
                    dataGridView2.Columns[4].HeaderText = "TOTAL REGULAR HOURS";
                    dataGridView2.Columns[5].HeaderText = "TOTAL OVERTIME HOURS";
                    dataGridView2.Columns[6].HeaderText = "TOTAL NIGHT DIFF HOURS";
                    dataGridView2.Columns[7].HeaderText = "TOTAL REGULAR PAY";
                    dataGridView2.Columns[8].HeaderText = "TOTAL OVERTIME PAY";
                    dataGridView2.Columns[9].HeaderText = "TOTAL NIGHT DIFF PAY";
                    dataGridView2.Columns[10].HeaderText = "GROSS PAY";
                    dataGridView2.Columns[11].HeaderText = "SSS";
                    dataGridView2.Columns[12].HeaderText = "PHILHEALTH";
                    dataGridView2.Columns[13].HeaderText = "PAG-IBIG";
                    dataGridView2.Columns[14].HeaderText = "CASH ADVANCE";
                    dataGridView2.Columns[15].HeaderText = "NET PAY";
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
                return dt;
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 1 && dataGridView1.SelectedRows[0].Index != dataGridView1.Rows.Count - 1)
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = openCon;
                    com.CommandType = CommandType.Text;
                    com.CommandText = "select empID, lastName, firstName, middleName, position from tblEmpInfo where empID = @empID";
                    com.Parameters.AddWithValue("@empID", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    SqlDataAdapter sda = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    txtEmpID.Text = dt.Rows[0][0].ToString();
                    txtLastName.Text = dt.Rows[0][1].ToString();
                    txtFirstName.Text = dt.Rows[0][2].ToString();
                    txtMiddleName.Text = dt.Rows[0][3].ToString();
                    txtPosition.Text = dt.Rows[0][4].ToString();
                }
                GetTransactionData();
            }
            else
            {
                MessageBox.Show("Please select a row.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dtpFrom.ResetText();
            dtpTo.ResetText();
            tbTHours.Text = "";
            tbNHours.Text = "";
            tbOHours.Text = "";
            tbND.Text = "";
            tbLHA.Text = "";
            tbSHA.Text = "";
            tbSU.Text = "";
            tbNDP.Text = "";
            tbGrossPay.Text = "";
            tbNP.Text = "";
            tbOP.Text = "";
        }

        private void btnCancel1_Click(object sender, EventArgs e)
        {
            dtpFrom.ResetText();
            dtpTo.ResetText();
            tbTHours.Text = "";
            tbNHours.Text = "";
            tbOHours.Text = "";
            tbND.Text = "";
            tbLHA.Text = "";
            tbSHA.Text = "";
            tbSU.Text = "";
            tbNetPay.Text = "";
            tbNDP.Text = "";
            tbGrossPay.Text = "";
            tbNP.Text = "";
            tbOP.Text = "";
            tbSSSD.Text = "0.00";
            tbPHD.Text = "0.00";
            tbPID.Text = "0.00";
            tbCAD.Text = "0.00";
        }

        private void btnGrossPay_Click(object sender, EventArgs e)
        {
            if (txtEmpID.Text != "")
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = openCon;
                    command.CommandText = "select l.empID, sum(d.totalHours)/60 + sum(d.totalHours)%60/100.0 as totalHours, " +
                                            "sum(d.regularHours)/60 + sum(d.regularHours)%60/100.0 as regularHours, " +
                                            "sum(d.overtimeHours)/60 + sum(d.overtimeHours)%60/100.0 as OT, " +
                                            "sum(d.nightDiffHours)/60 + sum(d.nightDiffHours)%60/100.0 as ND, " +
                                            "sum(l.legalHoliday) as lh, SUM(l.specialHoliday) as sh, SUM(l.sunday) as sun " +
                                            "from DailyHours_Pay as d inner join logRecords as l on d.LogID = l.logID " +
                                            "where l.empID = @empID and l.logdate between @fromDate and @toDate " +
                                            "GROUP BY l.empID";
                    command.Parameters.AddWithValue("@empID", txtEmpID.Text);
                    command.Parameters.AddWithValue("@fromDate", dtpFrom.Value.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@toDate", dtpTo.Value.ToString("yyyy-MM-dd"));

                    try
                    {
                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        try
                        {
                            tbTHours.Text = Math.Round(decimal.Parse(dt.Rows[0][1].ToString()), 2).ToString();
                            tbNHours.Text = Math.Round(decimal.Parse(dt.Rows[0][2].ToString()), 2).ToString();
                            tbOHours.Text = Math.Round(decimal.Parse(dt.Rows[0][3].ToString()), 2).ToString();
                            tbND.Text = Math.Round(decimal.Parse(dt.Rows[0][4].ToString()), 2).ToString();
                            tbLHA.Text = dt.Rows[0][5].ToString();
                            tbSHA.Text = dt.Rows[0][6].ToString();
                            tbSU.Text = dt.Rows[0][7].ToString();
                        }
                        catch (Exception ex)
                        { MessageBox.Show(ex.Message); }
                    }
                    catch (SqlException ex)
                    { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("Choose an employee first.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "select l.empID, sum(l.regularPay) as regularPay, sum(l.overtimePay) as overtimePay, sum(l.nightDiffPay) as NDP " +
                                        "from DailyHours_Pay as l full join transactions as t on l.empID = t.empID " +
                                        "where l.empID = @empID and l.logid in (select logID from logrecords where logDate between @fromDate and @toDate )" +
                                        "GROUP BY l.empID";
                command.Parameters.AddWithValue("@empID", txtEmpID.Text);
                command.Parameters.AddWithValue("@fromDate", dtpFrom.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@toDate", dtpTo.Value.ToString("yyyy-MM-dd"));

                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                try
                {
                    tbNP.Text = dt.Rows[0][1].ToString();
                    tbOP.Text = dt.Rows[0][2].ToString();
                    tbNDP.Text = dt.Rows[0][3].ToString();
                    double np, op, nd;
                    double.TryParse(tbNP.Text, out np);
                    double.TryParse(tbOP.Text, out op);
                    double.TryParse(tbNDP.Text, out nd);
                    double grossPay = np + op + nd;
                    tbGrossPay.Text = grossPay.ToString();
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }

            GetData();
        }

            decimal totalHours;
            decimal regularHours;
            decimal overtimeHours;
            decimal nightDiffHours;
            decimal regularPay;
            decimal otPay;
            decimal ndPay;
            decimal grossPaytb;
            decimal sss;
            decimal ph;
            decimal ca;
            decimal pi;

            decimal netPay;

        private void btnNetPay_Click(object sender, EventArgs e)
        {
            try
            {
                totalHours = decimal.Parse(tbTHours.Text);
                regularHours = decimal.Parse(tbNHours.Text);
                overtimeHours = decimal.Parse(tbOHours.Text);
                nightDiffHours = decimal.Parse(tbND.Text);
                regularPay = decimal.Parse(tbNP.Text);
                otPay = decimal.Parse(tbOP.Text);
                decimal.TryParse(tbNDP.Text, out ndPay);
                grossPaytb = decimal.Parse(tbGrossPay.Text);
                sss = decimal.Parse(tbSSSD.Text);
                ph = decimal.Parse(tbPHD.Text);
                ca = decimal.Parse(tbCAD.Text);
                pi = decimal.Parse(tbPID.Text);

                netPay = grossPaytb - sss - ph - pi - ca;

                tbNetPay.Text = "" + netPay;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "insert into transactions (empID, fromDate, toDate, TotalRegularHours, TotalOvertimeHours, TotalNightDiffHours, " +
                                        " TotalRegularPay, TotalOvertimePay, TotalNightDiffPay, GrossPay, SSS, PhilHealth, PagIBIG, CashAdvance, NetPay)" +
                                        " values(@empID, @fromDate, @toDate, @regularHours, @overtimeHours, @nightDiffHours, @regularPay, " +
                                        "@overtimePay, @nightDiffPay, @grossPay, @sss, @ph, @pi, @ca, @netPay)";
                command.Parameters.AddWithValue("@empID", txtEmpID.Text);
                command.Parameters.AddWithValue("@fromDate", dtpFrom.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@toDate", dtpTo.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@regularHours", regularHours);
                command.Parameters.AddWithValue("@overtimeHours", overtimeHours);
                command.Parameters.AddWithValue("@nightDiffHours", nightDiffHours);
                command.Parameters.AddWithValue("@regularPay", regularPay);
                command.Parameters.AddWithValue("@overtimePay", otPay);
                command.Parameters.AddWithValue("@nightDiffPay", ndPay);
                command.Parameters.AddWithValue("@grossPay", grossPaytb);
                command.Parameters.AddWithValue("@sss", sss);
                command.Parameters.AddWithValue("@ph", ph);
                command.Parameters.AddWithValue("@pi", pi);
                command.Parameters.AddWithValue("@ca", ca);
                command.Parameters.AddWithValue("@netPay", netPay);

                try
                {
                    openCon.Open();
                    int recordsAffected = command.ExecuteNonQuery();
                    MessageBox.Show("Salary Computed!");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    openCon.Close();
                }

                dtpFrom.ResetText();
                dtpTo.ResetText();
                tbTHours.Text = "";
                tbNHours.Text = "";
                tbOHours.Text = "";
                tbND.Text = "";
                tbLHA.Text = "";
                tbSHA.Text = "";
                tbSU.Text = "";
                tbNetPay.Text = "";
                tbNDP.Text = "";
                tbGrossPay.Text = "";
                tbNP.Text = "";
                tbOP.Text = "";
                tbSSSD.Text = "0.00";
                tbPHD.Text = "0.00";
                tbPID.Text = "0.00";
                tbCAD.Text = "0.00";
            }

            GetTransactionData();
        }

        private void dateFrom_ValueChanged(object sender, EventArgs e)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT  t.* " +
                                        "FROM transactions as t WHERE t.empID = '" + txtEmpID.Text + "' and FromDate = @FD and ToDate = @TD;";
                command.Parameters.AddWithValue("@FD", dtpFrom.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@TD", dtpTo.Value.ToString("yyyy-MM-dd"));

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                    dataGridView2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView2.Columns[0].HeaderText = "TRANSACTION NO.";
                    dataGridView2.Columns[1].HeaderText = "EPLOYEE ID";
                    dataGridView2.Columns[2].HeaderText = "FROM DATE";
                    dataGridView2.Columns[3].HeaderText = "TO DATE";
                    dataGridView2.Columns[4].HeaderText = "TOTAL REGULAR HOURS";
                    dataGridView2.Columns[5].HeaderText = "TOTAL OVERTIME HOURS";
                    dataGridView2.Columns[6].HeaderText = "TOTAL NIGHT DIFF HOURS";
                    dataGridView2.Columns[7].HeaderText = "TOTAL REGULAR PAY";
                    dataGridView2.Columns[8].HeaderText = "TOTAL OVERTIME PAY";
                    dataGridView2.Columns[9].HeaderText = "TOTAL NIGHT DIFF PAY";
                    dataGridView2.Columns[10].HeaderText = "GROSS PAY";
                    dataGridView2.Columns[11].HeaderText = "SSS";
                    dataGridView2.Columns[12].HeaderText = "PHILHEALTH";
                    dataGridView2.Columns[13].HeaderText = "PAG-IBIG";
                    dataGridView2.Columns[14].HeaderText = "CASH ADVANCE";
                    dataGridView2.Columns[15].HeaderText = "NET PAY";
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
            }
        }

        private void txtEmpID_TextChanged(object sender, EventArgs e)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "Select LastName, FirstName, MiddleName, Position from tblempInfo where empID like '" + txtEmpID.Text + "%'";

                try
                {
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    txtLastName.Text = dt.Rows[0][0].ToString();
                    txtFirstName.Text = dt.Rows[0][1].ToString();
                    txtMiddleName.Text = dt.Rows[0][2].ToString();
                    txtPosition.Text = dt.Rows[0][3].ToString();
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        private void dateTo_ValueChanged(object sender, EventArgs e)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT  t.* " +
                                        "FROM transactions as t WHERE t.empID = '" + txtEmpID.Text + "' and FromDate = @FD and  ToDate = @TD;";
                command.Parameters.AddWithValue("@FD", dtpFrom.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@TD", dtpTo.Value.ToString("yyyy-MM-dd"));

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                    dataGridView2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView2.Columns[0].HeaderText = "TRANSACTION NO.";
                    dataGridView2.Columns[1].HeaderText = "EPLOYEE ID";
                    dataGridView2.Columns[2].HeaderText = "FROM DATE";
                    dataGridView2.Columns[3].HeaderText = "TO DATE";
                    dataGridView2.Columns[4].HeaderText = "TOTAL REGULAR HOURS";
                    dataGridView2.Columns[5].HeaderText = "TOTAL OVERTIME HOURS";
                    dataGridView2.Columns[6].HeaderText = "TOTAL NIGHT DIFF HOURS";
                    dataGridView2.Columns[7].HeaderText = "TOTAL REGULAR PAY";
                    dataGridView2.Columns[8].HeaderText = "TOTAL OVERTIME PAY";
                    dataGridView2.Columns[9].HeaderText = "TOTAL NIGHT DIFF PAY";
                    dataGridView2.Columns[10].HeaderText = "GROSS PAY";
                    dataGridView2.Columns[11].HeaderText = "SSS";
                    dataGridView2.Columns[12].HeaderText = "PHILHEALTH";
                    dataGridView2.Columns[13].HeaderText = "PAG-IBIG";
                    dataGridView2.Columns[14].HeaderText = "CASH ADVANCE";
                    dataGridView2.Columns[15].HeaderText = "NET PAY";
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
            }
        }

        private void tbSSSD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            { e.Handled = true; }
            TextBox txtDecimal = sender as TextBox;
            if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void tbPHD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            { e.Handled = true; }
            TextBox txtDecimal = sender as TextBox;
            if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void tbPID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            { e.Handled = true; }
            TextBox txtDecimal = sender as TextBox;
            if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void tbCAD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            { e.Handled = true; }
            TextBox txtDecimal = sender as TextBox;
            if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT  t.* " +
                                        "FROM transactions as t WHERE t.empID = '" + txtEmpID.Text + "';";

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                    dataGridView2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView2.Columns[0].HeaderText = "TRANSACTION NO.";
                    dataGridView2.Columns[1].HeaderText = "EPLOYEE ID";
                    dataGridView2.Columns[2].HeaderText = "FROM DATE";
                    dataGridView2.Columns[3].HeaderText = "TO DATE";
                    dataGridView2.Columns[4].HeaderText = "TOTAL REGULAR HOURS";
                    dataGridView2.Columns[5].HeaderText = "TOTAL OVERTIME HOURS";
                    dataGridView2.Columns[6].HeaderText = "TOTAL NIGHT DIFF HOURS";
                    dataGridView2.Columns[7].HeaderText = "TOTAL REGULAR PAY";
                    dataGridView2.Columns[8].HeaderText = "TOTAL OVERTIME PAY";
                    dataGridView2.Columns[9].HeaderText = "TOTAL NIGHT DIFF PAY";
                    dataGridView2.Columns[10].HeaderText = "GROSS PAY";
                    dataGridView2.Columns[11].HeaderText = "SSS";
                    dataGridView2.Columns[12].HeaderText = "PHILHEALTH";
                    dataGridView2.Columns[13].HeaderText = "PAG-IBIG";
                    dataGridView2.Columns[14].HeaderText = "CASH ADVANCE";
                    dataGridView2.Columns[15].HeaderText = "NET PAY";
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
            }
        }
    }
}
