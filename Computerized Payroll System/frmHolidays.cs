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
    public partial class frmHolidays : Form
    {
        public frmHolidays()
        {
            InitializeComponent();
        }

        SqlConnection openCon = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=dbPayroll; " +
                                       "connection timeout=30");

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            dtpDate.Value = monthCalendar1.SelectionRange.Start;
        }

        private int type;

        private void insertData()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;            // <== lacking
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into Holidays ( Date, holidayName, holidayType)" +
                                      "VALUES ( @hDate, @hName, @hType)";
                command.Parameters.AddWithValue("@hName", tbName.Text);
                command.Parameters.AddWithValue("@hDate", dtpDate.Value.ToString("MM-dd-yyyy"));
                command.Parameters.AddWithValue("@hType", type);

                try
                {
                    openCon.Open();
                    int recordsAffected = command.ExecuteNonQuery();
                    MessageBox.Show("New holiday added!");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    openCon.Close();
                }
            }
        }

        public DataTable GetData()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Holidays order by date asc";

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].HeaderText = "HOLIDAY ID";
                    dataGridView1.Columns[2].HeaderText = "HOLIDAY NAME";
                    dataGridView1.Columns[1].HeaderText = "HOLIDAY DATE";
                    dataGridView1.Columns[3].HeaderText = "HOLIDAY TYPE";
                    dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
                return dt;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cbType.SelectedIndex == 0)
                type = 0;
            else
                type = 1;

            insertData();
            tbName.Text = "";
            dtpDate.ResetText();
            cbType.ResetText();
            GetData();
        }

        private void frmHolidays_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            tbName.Text = "";
            dtpDate.ResetText();
            cbType.ResetText();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnUpdate.Enabled = true;
            btnSave.Enabled = false;
            btnDelete.Enabled = true;

            if (dataGridView1.Rows.Count > 1 && dataGridView1.SelectedRows[0].Index != dataGridView1.Rows.Count - 1)
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = openCon;
                    com.CommandType = CommandType.Text;
                    com.CommandText = "select holidayName, HolidayType, Date from Holidays where HID = @HID";
                    com.Parameters.AddWithValue("@HID", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    SqlDataAdapter sda = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    tbName.Text = dt.Rows[0][0].ToString();
                    cbType.SelectedIndex = Int32.Parse(dt.Rows[0][1].ToString());
                    dtpDate.Text = dt.Rows[0][2].ToString();
                }
            }
            else
            {
                MessageBox.Show("Please select a row.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 1 && dataGridView1.SelectedRows[0].Index != dataGridView1.Rows.Count - 1)
            {
                if (MessageBox.Show("Are you sure to delete this record?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = openCon;
                        com.CommandType = CommandType.Text;
                        com.CommandText = "Delete from Holidays where HID = @HID";
                        com.Parameters.AddWithValue("@HID", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                        try
                        {
                            openCon.Open();
                            com.ExecuteNonQuery();
                            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                            MessageBox.Show("Holiday Deleted");
                            cbType.Text = "";
                            tbName.Text = "";
                            dtpDate.ResetText();
                            btnSave.Enabled = true;
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
                            GetData();
                        }
                        catch (Exception ex)
                        { MessageBox.Show(ex.Message); }
                        openCon.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cbType.SelectedIndex == 0)
                type = 0;
            else
                type = 1;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "Update Holidays set date = @date, HolidayName = @HName, HolidayType = @HType where HID = @HID";
                command.Parameters.AddWithValue("@date", dtpDate.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@HName", tbName.Text);
                command.Parameters.AddWithValue("@HType", type);
                command.Parameters.AddWithValue("@HID", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

                try
                {
                    openCon.Open();
                    command.ExecuteNonQuery();
                    btnUpdate.Enabled = false;
                    tbName.Text = "";
                    cbType.ResetText();
                    dtpDate.ResetText();
                    btnSave.Enabled = true;
                    btnDelete.Enabled = false;
                    MessageBox.Show("Holiday updated!");
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                openCon.Close();
                GetData();
            }
        }
    }
}
