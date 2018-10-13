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
    public partial class frmEmpRecords : Form
    {
        SqlConnection openCon = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=dbPayroll; " +
                                       "connection timeout=30; " +
                                       "MultipleActiveResultSets=true");
        public frmEmpRecords()
        {
            InitializeComponent();
        }

        String gender, bDay, stat, position;
        int age;
        byte[] data;

        //Employee Information Table
        public DataTable GetData()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT empID, lastName, firstName, middleName, position, address, ContactNo FROM tblEmpInfo";

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].HeaderText = "EPLOYEE ID";
                    dataGridView1.Columns[1].HeaderText = "LAST NAME";
                    dataGridView1.Columns[2].HeaderText = "FIRST NAME";
                    dataGridView1.Columns[3].HeaderText = "MIDDLE NAME";
                    dataGridView1.Columns[4].HeaderText = "POSITION";
                    dataGridView1.Columns[5].HeaderText = "ADDRESS";
                    dataGridView1.Columns[6].HeaderText = "CONTACT #";
                    dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
                return dt;
            }
        }

        //Add new employee
        private void insertData()
        {
            using (SqlCommand com = new SqlCommand())
            {
                int eAccess;
                if (cmbPosition.SelectedIndex == 0)
                    eAccess = 1;
                else eAccess = 2;
                com.Connection = openCon;
                com.CommandType = CommandType.Text;

                com.CommandText = "INSERT into empPW (empID, password, LevelOfAccess)" +
                                      "VALUES (@empID, @password, @eAccess)";
                com.Parameters.AddWithValue("@empID", tbEmpID.Text);
                com.Parameters.AddWithValue("@password", "password");
                com.Parameters.AddWithValue("@eAccess", eAccess);

                try
                {
                    openCon.Open();
                    int recordsAffected = com.ExecuteNonQuery();
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

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon; 
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into tblEmpInfo (empID,lastName,firstName,middleName,position, rate, sex,birthday,address,contactNo, picture, status)" +
                                      "VALUES (@employeeID,@lastName,@firstName,@middleName,@position, @rate, @gender,@bDay,@address,@cNum,@picture, @stat)";
                command.Parameters.AddWithValue("@employeeID", tbEmpID.Text);
                command.Parameters.AddWithValue("@lastName", tbLName.Text);
                command.Parameters.AddWithValue("@firstName", tbFName.Text);
                command.Parameters.AddWithValue("@middleName", tbMName.Text);
                command.Parameters.AddWithValue("@position", position);
                command.Parameters.AddWithValue("@gender", gender);
                command.Parameters.AddWithValue("@bDay", bDay);
                command.Parameters.AddWithValue("@address", tbAddress.Text);
                command.Parameters.AddWithValue("@cNum", tbCNum.Text);
                command.Parameters.AddWithValue("@picture", data);
                command.Parameters.AddWithValue("@stat", stat);
                command.Parameters.AddWithValue("@rate", 46.625);
                try
                {
                    openCon.Open();
                    int recordsAffected = command.ExecuteNonQuery();
                    MessageBox.Show("Input Successful!");
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

        //update employee information
        private void updateData()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                pbImage.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                data = stream.ToArray();
            }

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "update tblEmpInfo set lastName = @lastName,firstName = @firstName," +
                                      "middleName = @middleName,position = @position,sex = @gender," +
                                      "Birthday = @bDay,address=@address, contactNo=@cNum," +
                                      "picture = @picture, status = @stat where empID = @employeeID";
                command.Parameters.AddWithValue("@employeeID", tbEmpID.Text);
                command.Parameters.AddWithValue("@lastName", tbLName.Text);
                command.Parameters.AddWithValue("@firstName", tbFName.Text);
                command.Parameters.AddWithValue("@middleName", tbMName.Text);
                command.Parameters.AddWithValue("@position", position);
                command.Parameters.AddWithValue("@gender", gender);
                command.Parameters.AddWithValue("@bDay", bDay);
                command.Parameters.AddWithValue("@address", tbAddress.Text);
                command.Parameters.AddWithValue("@cNum", tbCNum.Text);
                command.Parameters.AddWithValue("@picture", data);
                command.Parameters.AddWithValue("@stat", stat);

                try
                {
                    openCon.Open();
                    int recordsAffected = command.ExecuteNonQuery();
                    MessageBox.Show("Record updated successfuly.");
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

        //validation of inputted values
        private void btnSave_Click(object sender, EventArgs e)
        {
                try
                {
                    if (tbEmpID.MaskCompleted == false)
                    { MessageBox.Show("Invalid Employee ID!"); tbEmpID.Focus(); }
                    else if (tbLName.Text == "")
                    { MessageBox.Show("Last name field can't be empty!"); tbLName.Focus(); }
                    else if (tbFName.Text == "")
                    { MessageBox.Show("First name field can't be empty!"); tbFName.Focus(); }
                    if (rbMale.Checked == true)
                    { gender = "Male"; }
                    else if (rbFemale.Checked == true)
                    { gender = "Female"; }
                    else
                    { MessageBox.Show("Please choose a gender."); }
                    int a = cmbStatus.SelectedIndex;
                    switch (a)
                    {
                        case 0:
                            stat = "Single";
                            break;
                        case 1:
                            stat = "Married";
                            break;
                        case 2:
                            stat = "Widowed";
                            break;
                        case 3:
                            stat = "Separated";
                            break;
                        default:
                            MessageBox.Show("Please select a status.");
                            cmbStatus.Focus();
                            return;
                    }
                    int b = cmbPosition.SelectedIndex;
                    switch (b)
                    {
                        case 0:
                            position = "Admin";
                            break;
                        case 1:
                            position = "Office Personnel";
                            break;
                        case 2:
                            position = "Cutter";
                            break;
                        case 3:
                            position = "Sawer";
                            break;
                        case 4:
                            position = "Zigzagger";
                            break;
                        case 5:
                            position = "Kneading Operator";
                            break;
                        case 6:
                            position = "Quality Control";
                            break;
                        case 7:
                            position = "Laser Operator";
                            break;
                        case 8:
                            position = "Artist";
                            break;
                        case 9:
                            position = "Embroidery Operator";
                            break;
                        case 10:
                            position = "Maintenance";
                            break;
                        default:
                            MessageBox.Show("Please select a position.");
                            cmbPosition.Focus();
                            return;
                    }
                    if (tbCNum.MaskCompleted == false)
                    { MessageBox.Show("Invalid Contact Number!"); tbCNum.Focus(); }
                    if (tbAddress.Text == "")
                    { MessageBox.Show("Address field can't be empty!"); tbAddress.Focus(); }

                    if (age > 18)
                    {
                        bDay = dtpDOB.Value.ToString("MM-dd-yyyy");
                    }
                    else { MessageBox.Show("Invalid Birthday!"); }

                    if (chosePhoto == false)
                    {
                        ImageConverter imgConverter = new ImageConverter();
                        data = (System.Byte[])imgConverter.ConvertTo(pbImage.Image, Type.GetType("System.Byte[]"));
                    }

                    insertData();
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                GetData();
        }

        //clearing textboxes
        private void btnClear_Click(object sender, EventArgs e)
        {
            tbEmpID.Text = "";
            tbLName.Text = "";
            tbFName.Text = "";
            tbMName.Text = "";
            tbAddress.Text = "";
            tbCNum.Text = "";
            dtpDOB.ResetText();
            lblAge.Text = "";
            cmbStatus.ResetText();
            cmbPosition.ResetText();
            pbImage.Image = pbImage.InitialImage;
            btnSave.Enabled = true;
            btnUpdate1.Enabled = false;
        }

        //cancel transaction
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tbEmpID.Text = "";
                tbLName.Text = "";
                tbFName.Text = "";
                tbMName.Text = "";
                tbAddress.Text = "";
                tbCNum.Text = "";
                dtpDOB.ResetText();
                lblAge.Text = "";
                cmbStatus.ResetText();
                cmbPosition.ResetText();
                pbImage.Image = pbImage.InitialImage;
                btnUpdate1.Enabled = false;
                btnSave.Enabled = true;
            }
        }

        Boolean chosePhoto = false;
        //choosing photo
        private void btnChoosePhoto_Click(object sender, EventArgs e)
        {
            chosePhoto = true;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pbImage.Image = new Bitmap(open.FileName);
                Image image = Image.FromFile(open.FileName);

                using (MemoryStream stream = new MemoryStream())
                {
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    data = stream.ToArray();
                }
            }
        }

        //editing employee iformation
        private void btnEdit_Click(object sender, EventArgs e)
        {
            
        }

        //computing age
        private void dtpDOB_ValueChanged(object sender, EventArgs e)
        {
            age = DateTime.Now.Year - dtpDOB.Value.Year - (DateTime.Now.DayOfYear < dtpDOB.Value.DayOfYear ? 1 : 0);
            if (dtpDOB.Value.ToShortDateString() != DateTime.Today.ToShortDateString())
            {
                if (age < 18)
                {
                    MessageBox.Show("Employee can't be underaged.");
                    dtpDOB.Focus();
                }
                else
                {
                    lblAge.Text = age.ToString();
                }
            }
        }

        //searching employees from table
        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (cmbSearch.Text == "Employee ID")
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = openCon;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT empID, lastName, firstName, middleName, position, address, contactNo FROM tblEmpInfo where empID like '" + tbSearch.Text + "%'";
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    openCon.Open();
                    openCon.Close();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            else if (cmbSearch.Text == "Last Name")
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = openCon;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT empID, lastName, firstName, middleName, position, address, contactNo FROM tblEmpInfo where lastName like '" + tbSearch.Text + "%'";
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    openCon.Open();
                    openCon.Close();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            else if (cmbSearch.Text == "First Name")
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = openCon;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT empID, lastName, firstName, middleName, position, address, contactNo FROM tblEmpInfo where firstName like '" + tbSearch.Text + "%'";
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    openCon.Open();
                    openCon.Close();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            else if (cmbSearch.Text == "Position")
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = openCon;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT empID, lastName, firstName, middleName, position, address, contactNo FROM tblEmpInfo where position like '" + tbSearch.Text + "%'";
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    openCon.Open();
                    openCon.Close();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        private void frmEmpRecords_Load(object sender, EventArgs e)
        {
            pbImage.Image = pbImage.InitialImage;
            btnUpdate1.Enabled = false;
            GetData();
            GetLogData();
        }

        //deleting employees
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
                        com.CommandText = "Delete from tblEmpInfo where empID = @empID";
                        com.Parameters.AddWithValue("@empID", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                        openCon.Open();
                        com.ExecuteNonQuery();
                        openCon.Close();
                    }
                    using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = openCon;
                        com.CommandType = CommandType.Text;
                        com.CommandText = "Delete from empPW where empID = @empID";
                        com.Parameters.AddWithValue("@empID", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                        openCon.Open();
                        com.ExecuteNonQuery();
                        openCon.Close();
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                        MessageBox.Show("Record Deleted");
                        tbEmpID.Text = "";
                        tbLName.Text = "";
                        tbFName.Text = "";
                        tbMName.Text = "";
                        tbAddress.Text = "";
                        tbCNum.Text = "";
                        dtpDOB.ResetText();
                        lblAge.Text = "";
                        cmbStatus.ResetText();
                        cmbPosition.ResetText();
                        pbImage.Image = pbImage.InitialImage;
                        btnSave.Enabled = true;
                        btnUpdate1.Enabled = false;
                    }

                }
            }
            else
            {
                MessageBox.Show("Please select a row.");
            }
            GetData();
        }

        //searching employee log records within two dates
        public DataTable GetLogDataWDate()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon; 
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT e.empID, e.lastName, e.firstName, e.middleName, e.position, " +
                                        "l.Logdate, l.AMtimeIN, l.AMtimeOUT ,d.AMTotalHours, l.PMtimeIN, l.PMtimeOUT, d.PMTotalHours, " +
                                        "d.totalHours, d.regularHours, d.overtimeHours, d.nightDiffHours " + 
                                        "FROM logRecords as l inner join tblEmpInfo as e on l.empID= e.empID " +
                                        "inner join DailyHours_Pay as d on d.logID = l.logID " +
                                        "WHERE l.logdate between @dateFrom and @dateTo";
                command.Parameters.AddWithValue("@dateFrom", dtpFrom.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@dateTo", dtpTo.Value.ToString("yyyy-MM-dd"));

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView2.DataSource = dt;
                    dataGridView2.Columns[0].HeaderText = "EPLOYEE ID";
                    dataGridView2.Columns[1].HeaderText = "LAST NAME";
                    dataGridView2.Columns[2].HeaderText = "FIRST NAME";
                    dataGridView2.Columns[3].HeaderText = "MIDDLE NAME";
                    dataGridView2.Columns[4].HeaderText = "POSITION";
                    dataGridView2.Columns[5].HeaderText = "DATE";
                    dataGridView2.Columns[6].HeaderText = "TIME-IN (AM)";
                    dataGridView2.Columns[7].HeaderText = "TIME-OUT (AM)";
                    dataGridView2.Columns[8].HeaderText = "HOURS (AM)";
                    dataGridView2.Columns[9].HeaderText = "TIME-IN (PM)";
                    dataGridView2.Columns[10].HeaderText = "TIME-OUT (PM)";
                    dataGridView2.Columns[11].HeaderText = "HOURS (PM)";
                    dataGridView2.Columns[12].HeaderText = "TOTAL HOURS"; 
                    dataGridView2.Columns[13].HeaderText = "REGULAR HOURS";
                    dataGridView2.Columns[14].HeaderText = "OVERTIME HOURS";
                    dataGridView2.Columns[15].HeaderText = "NIGHT DIFFERENTIAL HOURS";
                    dataGridView2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
                return dt;
            }
        }

        //employee log records
        public DataTable GetLogData()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon; 
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT e.empID, e.lastName, e.firstName, e.middleName, e.position, " +
                                        "l.Logdate, l.AMtimeIN, l.AMtimeOUT ,(d.AMTotalHours/60), l.PMtimeIN, l.PMtimeOUT, (d.PMTotalHours/60), " +
                                        "(d.totalHours/60), (d.regularHours/60), (d.overtimeHours/60), (d.nightDiffHours/60) " +
                                        "FROM tblEmpInfo as e inner join logRecords as l on l.empID= e.empID " +
                                        "full join DailyHours_Pay as d on d.logID = l.logID order by l.logDate desc";

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                try
                {
                    openCon.Open();
                    da.Fill(dt);
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView2.DataSource = dt;
                    dataGridView2.Columns[8].DefaultCellStyle.Format = "N2";
                    dataGridView2.Columns[11].DefaultCellStyle.Format = "N2";
                    dataGridView2.Columns[12].DefaultCellStyle.Format = "N2";
                    dataGridView2.Columns[13].DefaultCellStyle.Format = "N2";
                    dataGridView2.Columns[14].DefaultCellStyle.Format = "N2";
                    dataGridView2.Columns[15].DefaultCellStyle.Format = "N2";
                    dataGridView2.Columns[0].HeaderText = "EPLOYEE ID";
                    dataGridView2.Columns[1].HeaderText = "LAST NAME";
                    dataGridView2.Columns[2].HeaderText = "FIRST NAME";
                    dataGridView2.Columns[3].HeaderText = "MIDDLE NAME";
                    dataGridView2.Columns[4].HeaderText = "POSITION";
                    dataGridView2.Columns[5].HeaderText = "DATE";
                    dataGridView2.Columns[6].HeaderText = "TIME-IN (AM)";
                    dataGridView2.Columns[7].HeaderText = "TIME-OUT (AM)";
                    dataGridView2.Columns[8].HeaderText = "HOURS (AM)";
                    dataGridView2.Columns[9].HeaderText = "TIME-IN (PM)";
                    dataGridView2.Columns[10].HeaderText = "TIME-OUT (PM)";
                    dataGridView2.Columns[11].HeaderText = "HOURS (PM)";
                    dataGridView2.Columns[12].HeaderText = "TOTAL HOURS";
                    dataGridView2.Columns[13].HeaderText = "REGULAR HOURS";
                    dataGridView2.Columns[14].HeaderText = "OVERTIME HOURS";
                    dataGridView2.Columns[15].HeaderText = "NIGHT DIFFERENTIAL HOURS";
                    dataGridView2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { openCon.Close(); }
                return dt;
            }
        }

        //filtering log records table by date
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            GetLogDataWDate();
        }

        //searching for employee log records
        //private void tbSearch2_TextChanged(object sender, EventArgs e)
        //{

        //    if (cmbSearch2.Text == "Employee ID")
        //    {
        //        using (SqlCommand command = new SqlCommand())
        //        {
        //            command.Connection = openCon;
        //            command.CommandType = CommandType.Text;
        //            command.CommandText = "SELECT e.empID, e.lastName, e.firstName, e.middleName, e.position, " +
        //                                "l.Logdate, l.AMtimeIN, l.AMtimeOUT ,(d.AMTotalHours/60), l.PMtimeIN, l.PMtimeOUT, (d.PMTotalHours/60), " +
        //                                "(d.totalHours/60), (d.regularHours/60), (d.overtimeHours/60), (d.nightDiffHours/60) " +
        //                                "FROM tblEmpInfo as e inner join logRecords as l on l.empID= e.empID " +
        //                                "full join DailyHours_Pay as d on d.logID = l.logID " + 
        //                                "where l.empID like '" + tbSearch.Text + "%' and l.logDate between @dateFrom and @dateTo";
        //            command.Parameters.AddWithValue("@dateFrom", dtpFrom.Value.ToString("yyyy-MM-dd"));
        //            command.Parameters.AddWithValue("@dateTo", dtpTo.Value.ToString("yyyy-MM-dd"));
        //            SqlDataAdapter da = new SqlDataAdapter(command);
        //            openCon.Open();
        //            openCon.Close();
        //            DataTable dt = new DataTable();
        //            da.Fill(dt);
        //            dataGridView2.DataSource = dt;
        //        }
        //    }
        //    else if (cmbSearch2.Text == "Last Name")
        //    {
        //        using (SqlCommand command = new SqlCommand())
        //        {
        //            command.Connection = openCon;
        //            command.CommandType = CommandType.Text;
        //            command.CommandText = "SELECT e.empID, e.lastName, e.firstName, e.middleName, e.position, " +
        //                                "l.Logdate, l.AMtimeIN, l.AMtimeOUT ,(d.AMTotalHours/60), l.PMtimeIN, l.PMtimeOUT, (d.PMTotalHours/60), " +
        //                                "(d.totalHours/60), (d.regularHours/60), (d.overtimeHours/60), (d.nightDiffHours/60) " +
        //                                "FROM tblEmpInfo as e inner join logRecords as l on l.empID= e.empID " +
        //                                "full join DailyHours_Pay as d on d.logID = l.logID " +
        //                                "where e.lastName like '" + tbSearch.Text + "%' and l.logDate between @dateFrom and @dateTo";
        //            command.Parameters.AddWithValue("@dateFrom", dtpFrom.Value.ToString("yyyy-MM-dd"));
        //            command.Parameters.AddWithValue("@dateTo", dtpTo.Value.ToString("yyyy-MM-dd"));
        //            SqlDataAdapter da = new SqlDataAdapter(command);
        //            openCon.Open();
        //            openCon.Close();
        //            DataTable dt = new DataTable();
        //            da.Fill(dt);
        //            dataGridView2.DataSource = dt;
        //        }
        //    }
        //    else if (cmbSearch2.Text == "First Name")
        //    {
        //        using (SqlCommand command = new SqlCommand())
        //        {
        //            command.Connection = openCon;
        //            command.CommandType = CommandType.Text;
        //            command.CommandText = "SELECT e.empID, e.lastName, e.firstName, e.middleName, e.position, " +
        //                                "l.Logdate, l.AMtimeIN, l.AMtimeOUT ,(d.AMTotalHours/60), l.PMtimeIN, l.PMtimeOUT, (d.PMTotalHours/60), " +
        //                                "(d.totalHours/60), (d.regularHours/60), (d.overtimeHours/60), (d.nightDiffHours/60) " +
        //                                "FROM tblEmpInfo as e inner join logRecords as l on l.empID= e.empID " +
        //                                "full join DailyHours_Pay as d on d.logID = l.logID " +
        //                                "where e.firstName like '" + tbSearch.Text + "%' and l.logDate between @dateFrom and @dateTo";
        //            command.Parameters.AddWithValue("@dateFrom", dtpFrom.Value.ToString("yyyy-MM-dd"));
        //            command.Parameters.AddWithValue("@dateTo", dtpTo.Value.ToString("yyyy-MM-dd"));
        //            SqlDataAdapter da = new SqlDataAdapter(command);
        //            openCon.Open();
        //            openCon.Close();
        //            DataTable dt = new DataTable();
        //            da.Fill(dt);
        //            dataGridView2.DataSource = dt;
        //        }
        //    }
        //}

        //updating employee log records
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtEmpID.Text == "")
            {
                MessageBox.Show("Please select a record first.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridView2.Focus();
            }
            else if (txtIN.MaskCompleted == false)
            {
                MessageBox.Show("Time-in field can't be empty!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIN.Focus();
            }
            else if (txtOUT.MaskCompleted == false)
            {
                MessageBox.Show("Time-out field can't be empty!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOUT.Focus();
            }
            else if (txtINP.MaskCompleted == false)
            {
                MessageBox.Show("Time-in field can't be empty!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtINP.Focus();
            }
            else if (txtOUTP.MaskCompleted == false)
            {
                MessageBox.Show("Time-out field can't be empty!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOUTP.Focus();
            }
            else
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = openCon;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "update logRecords set AMtimeIN = @timeIn, AMtimeOUT = @timeOut, PMtimeIN = @timeIn1, PMtimeOUT = @timeOut1 where empID = @empID and  logdate = @date";
                    command.Parameters.AddWithValue("@timeIn", txtIN.Text);
                    command.Parameters.AddWithValue("@timeOut", txtOUT.Text);
                    command.Parameters.AddWithValue("@timeIn1", txtINP.Text);
                    command.Parameters.AddWithValue("@timeOut1", txtOUTP.Text);
                    command.Parameters.AddWithValue("@empID", txtEmpID.Text);
                    command.Parameters.AddWithValue("@date", dtpLogDate.Value);

                    try
                    {
                        openCon.Open();
                        int recordsAffected = command.ExecuteNonQuery();
                        MessageBox.Show("Record updated successfully.");
                        clear();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        openCon.Close();
                    }
                    btnUpdate.Enabled = false;
                    GetLogData();
                }
            }
        }

        //cancelling transaction
        private void btnCancel2_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to cancel?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            clear();
            btnUpdate.Enabled = false;
            btnADD.Enabled = true;
        }

        //clearing textboxes
        private void clear()
        {
            txtEmpID.Text = "";
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtPosition.Text = "";
            txtOUT.Text = "";
            txtIN.Text = ""; 
            txtOUTP.Text = "";
            txtINP.Text = "";
            dtpLogDate.ResetText();
        }

        //validation for updating employee information
        private void btnUpdate1_Click(object sender, EventArgs e)
        {
                try
                {
                    if (tbEmpID.Text == "")
                    { MessageBox.Show("Employee ID field can't be empty!"); tbEmpID.Focus(); }
                    else if (tbLName.Text == "")
                    { MessageBox.Show("Last name field can't be empty!"); tbLName.Focus(); }
                    else if (tbFName.Text == "")
                    { MessageBox.Show("First name field can't be empty!"); tbFName.Focus(); }
                    if (rbMale.Checked == true)
                    { gender = "Male"; }
                    else if (rbFemale.Checked == true)
                    { gender = "Female"; }
                    else
                    { MessageBox.Show("Please choose a gender."); }
                    int a = cmbStatus.SelectedIndex;
                    switch (a)
                    {
                        case 0:
                            stat = "Single";
                            break;
                        case 1:
                            stat = "Married";
                            break;
                        case 2:
                            stat = "Widowed";
                            break;
                        case 3:
                            stat = "Separated";
                            break;
                        default:
                            MessageBox.Show("Please select a status.");
                            cmbStatus.Focus();
                            return;
                    }
                    int b = cmbPosition.SelectedIndex;
                    switch (b)
                    {
                        case 0:
                            position = "Admin";
                            break;
                        case 1:
                            position = "Manager";
                            break;
                        case 2:
                            position = "Secretary";
                            break;
                        case 3:
                            position = "Staff";
                            break;
                        default:
                            MessageBox.Show("Please select a position.");
                            cmbPosition.Focus();
                            return;
                    }
                    if (tbCNum.Text == "")
                    { MessageBox.Show("Contact number field can't be empty!"); tbCNum.Focus(); }
                    else if (tbAddress.Text == "")
                    { MessageBox.Show("Address field can't be empty!"); tbAddress.Focus(); }

                    bDay = dtpDOB.Value.ToString("MM-dd-yyyy");
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                updateData();
                GetData();
                btnUpdate1.Enabled = false;
        }

        //compute total hours
        private void btnCompute_Click(object sender, EventArgs e)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "INSERT INTO DailyHours_Pay (Logid) " +
                                        "SELECT l.logID FROM logRecords as l " +
                                        "WHERE l.logid NOT IN (SELECT logID FROM DailyHours_Pay)";
                try
                {
                    openCon.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                { MessageBox.Show(ex.Message); }
                openCon.Close();
                
            }
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "UPDATE DailyHours_Pay " +
                                        "SET EmpID = (" +
                                        "SELECT EmpID " +
                                        "FROM LogRecords " +
                                        "WHERE LogRecords.LogID = DailyHours_Pay.LogID" +
                                        ");";
                try
                {
                    openCon.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                openCon.Close();
            }
            double time;
            TimeSpan timein;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "UPDATE DailyHours_Pay set " +
                                        "AMTotalHours = " +
                                        "(Select (DATEDIFF(minute, AMtimeIN, AMtimeOUT)) from LogRecords where LogRecords.LogID = DailyHours_Pay.LogID ), " +
                                        "PMTotalHours = " +
                                        "(Select (DATEDIFF(minute, PMtimeIN, PMtimeOUT)) from LogRecords where LogRecords.LogID = DailyHours_Pay.LogID )";

                try
                {
                    openCon.Open();
                    int recordsAffected = command.ExecuteNonQuery();
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
            TimeSpan startTime = new TimeSpan(7, 30, 0);

            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = openCon;
                com.CommandText = "Select AMTimeIN from LogRecords";

                try
                {
                    openCon.Open();
                    SqlDataReader sdr = com.ExecuteReader();
                    if (sdr.Read() == true)
                    {
                        timein = sdr.GetTimeSpan(0);

                        if (timein > startTime)
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = openCon;
                                comm.CommandText = "UPDATE DailyHours_Pay set AMTotalHours = 240;";

                                try
                                {
                                    comm.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                { MessageBox.Show(ex.Message); }
                            }
                        }
                        else if (timein < startTime)
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = openCon;
                                comm.CommandText = "UPDATE DailyHours_Pay set AMTotalHours = 240;";

                                try
                                {
                                    comm.ExecuteNonQuery();
                                }
                                catch(Exception ex)
                                { MessageBox.Show(ex.Message); }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }

                openCon.Close();
            }
            using (SqlCommand comm = new SqlCommand())
            {
                comm.Connection = openCon;
                comm.CommandText = "UPDATE DailyHours_Pay set TotalHours = AMTotalHours + PMTotalHours ;";

                try
                {
                    openCon.Open();
                    comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                openCon.Close();
            }
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = openCon;
                                comm.CommandText = "UPDATE DailyHours_Pay set " +
                                                        "RegularHours = 480, " +
                                                        "NightDiffHours = ((TotalHours - RegularHours) - 330)," +
                                                        "OvertimeHours = 570 - NightDiffHours where totalHours > 810";
                                try
                                {
                                    openCon.Open();
                                    comm.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                { MessageBox.Show(ex.Message); }
                                openCon.Close();
                            }

                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = openCon;
                                comm.CommandText = "UPDATE DailyHours_Pay set " +
                                                        "RegularHours = 480, " +
                                                        "OvertimeHours = TotalHours - RegularHours, " +
                                                        "NightDiffHours = 0 where totalHours > 480 AND totalHours < 810";
                                try
                                {
                                    openCon.Open();
                                    comm.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                { MessageBox.Show(ex.Message); }
                                openCon.Close();
                            }
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = openCon;
                                comm.CommandText = "UPDATE DailyHours_Pay set " +
                                                        "RegularHours = AMTotalHours + PMTotalHours, " +
                                                        "OvertimeHours = 0, " +
                                                        "NightDiffHours = 0 where totalHours <= 480";
                                try
                                {
                                    openCon.Open();
                                    comm.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                { MessageBox.Show(ex.Message); }
                                openCon.Close();
                            }
               

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "UPDATE DailyHours_Pay set regularPay = ((regularHours/60)+(RegularHours%60/100)) * 46.625, " +
                                        "overtimePay = ((overtimeHours/60) + (OvertimeHours%60/100)) * (46.625*1.25), " +
                                        "nightDiffPay = ((nightDiffHours/60) + (nightDiffHours%60/100)) * (.1 * 46.625) " +
                                        "where LogID in (Select LogID from logRecords where sunday = 0 and legalHoliday = 0 and specialHoliday = 0)";
                try
                {
                    openCon.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                { MessageBox.Show(ex.Message); }
                openCon.Close();
            }
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "UPDATE DailyHours_Pay set regularPay = ((((regularHours/60)+(RegularHours%60/100)) * 46.625)*2), " +
                                        "overtimePay = (((overtimeHours/60) + (OvertimeHours%60/100) * 46.625*1.25)*2), " +
                                        "nightDiffPay = ((nightDiffHours/60) + (nightDiffHours%60/100)) * (.1 * 46.625)*2 " +
                                        "where LogID in (Select LogID from logRecords where sunday = 0 and legalHoliday = 1 and specialHoliday = 0)";
                try
                {
                    openCon.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                { MessageBox.Show(ex.Message); }
                openCon.Close();
            }
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "UPDATE DailyHours_Pay set regularPay = ((((regularHours/60)+(RegularHours%60/100)) * 46.625)*2.3), " +
                                        "overtimePay = (((overtimeHours/60) + (OvertimeHours%60/100) * 46.625*1.25)*2*1.30), " +
                                        "nightDiffPay = ((nightDiffHours/60) + (nightDiffHours%60/100)) * (.1 * 46.625)*2.6 " +
                                        "where LogID in (Select LogID from logRecords where sunday = 1 and legalHoliday = 1 and specialHoliday = 0)";
                try
                {
                    openCon.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                { MessageBox.Show(ex.Message); }
                openCon.Close();
            }
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "UPDATE DailyHours_Pay set regularPay = ((((regularHours/60)+(RegularHours%60/100)) * 46.625)*1.3), " +
                                        "overtimePay = (((overtimeHours/60) + (OvertimeHours%60/100) * 46.625*1.25)*1.30), " +
                                        "nightDiffPay = ((nightDiffHours/60) + (nightDiffHours%60/100)) * (.1 * 46.625)*1.3 " +
                                        "where LogID in (Select LogID from logRecords where sunday = 0 and legalHoliday = 0 and specialHoliday = 1)";
                try
                {
                    openCon.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                { MessageBox.Show(ex.Message); }
                openCon.Close();
            }
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "UPDATE DailyHours_Pay set regularPay = ((((regularHours/60)+(RegularHours%60/100)) * 46.625)*1.5), " +
                                        "overtimePay = (((overtimeHours/60) + (OvertimeHours%60/100) * 46.625*1.25)*1.50), " +
                                        "nightDiffPay = ((nightDiffHours/60) + (nightDiffHours%60/100)) * (.1 * 46.625)*1.5 " +
                                        "where LogID in (Select LogID from logRecords where sunday = 1 and legalHoliday = 0 and specialHoliday = 1)";
                try
                {
                    openCon.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                { MessageBox.Show(ex.Message); }
                openCon.Close();
            }
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "UPDATE DailyHours_Pay set regularPay = ((((regularHours/60)+(RegularHours%60/100)) * 46.625)*1.3), " +
                                        "overtimePay = (((overtimeHours/60) + (OvertimeHours%60/100) * 46.625*1.25)*1.30), " +
                                        "nightDiffPay = ((nightDiffHours/60) + (nightDiffHours%60/100)) * (.1 * 46.625)*1.3 " +
                                        "where LogID in (Select LogID from logRecords where sunday = 1 and legalHoliday = 0 and specialHoliday = 0)";
                try
                {
                    openCon.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Daily hours and pay are computed successfully.");

                }
                catch (SqlException ex)
                { MessageBox.Show(ex.Message); }
                openCon.Close();
            }
            GetLogData();
        }

        private void dataGridView2_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            txtEmpID.ReadOnly = true;
            txtEmpID.Enabled = false;
            txtFirstName.ReadOnly = true;
            txtFirstName.Enabled = false;
            txtLastName.ReadOnly = true;
            txtLastName.Enabled = false;
            txtMiddleName.ReadOnly = true;
            txtMiddleName.Enabled = false;
            txtPosition.ReadOnly = true;
            txtPosition.Enabled = false;
            dtpLogDate.Enabled = false;

            btnUpdate.Enabled = true;
            btnADD.Enabled = false;

            if (dataGridView2.Rows.Count > 1 && dataGridView2.SelectedRows[0].Index != dataGridView2.Rows.Count - 1)
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = openCon;
                    com.CommandType = CommandType.Text;
                    com.CommandText = "select e.empID, e.lastName, e.firstName, e.middleName, e.position, l.logdate, l.AMtimeIN, l.AMtimeOUT , l.PMtimeIN, l.PMtimeOUT from tblEmpInfo as e inner join logRecords as l on e.empID = l.empID  where e.empID = @empID and l.LogDate = @date";
                    com.Parameters.AddWithValue("@empID", dataGridView2.SelectedRows[0].Cells[0].Value.ToString());
                    com.Parameters.AddWithValue("@date", dataGridView2.SelectedRows[0].Cells[5].Value);
                    SqlDataAdapter sda = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    txtEmpID.Text = dt.Rows[0][0].ToString();
                    txtLastName.Text = dt.Rows[0][1].ToString();
                    txtFirstName.Text = dt.Rows[0][2].ToString();
                    txtMiddleName.Text = dt.Rows[0][3].ToString();
                    txtPosition.Text = dt.Rows[0][4].ToString();
                    dtpLogDate.Text = dt.Rows[0][5].ToString();
                    txtIN.Text = dt.Rows[0][6].ToString();
                    txtOUT.Text = dt.Rows[0][7].ToString();
                    txtINP.Text = dt.Rows[0][8].ToString();
                    txtOUTP.Text = dt.Rows[0][9].ToString();
                }
            }
            else
            {
                MessageBox.Show("Please select a row.");
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            GetLogDataWDate();
        }

        private void tbLName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || char.IsWhiteSpace(e.KeyChar)))
            {
                System.Media.SystemSounds.Asterisk.Play();
                e.Handled = true;
            }
        }

        private void tbFName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || char.IsWhiteSpace(e.KeyChar)))
            {
                System.Media.SystemSounds.Asterisk.Play();
                e.Handled = true;
            }
        }

        private void tbMName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || char.IsWhiteSpace(e.KeyChar)))
            {
                System.Media.SystemSounds.Asterisk.Play();
                e.Handled = true;
            }
        }

        private void tbAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetterOrDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsWhiteSpace(e.KeyChar)))
            {
                // Stop the character from being entered into the control since it is illegal.
                System.Media.SystemSounds.Asterisk.Play();
                e.Handled = true;
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

        private void btnADD_Click(object sender, EventArgs e)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandText = "INSERT INTO logRecords (empID, logDate, AMTimeIN, AMTimeOUT, PMTimeIN, PMTimeOut) " + 
                                        "VALUES (@eID, @lDate, @AMIn, @AMOut, @PMIn, @PMOut)";
                command.Parameters.AddWithValue("@eID", txtEmpID.Text);
                command.Parameters.AddWithValue("@lDate", dtpLogDate.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@AMIn", txtIN.Text);
                command.Parameters.AddWithValue("@AMOut", txtOUT.Text);
                command.Parameters.AddWithValue("@PMIn", txtINP.Text);
                command.Parameters.AddWithValue("@PMOut", txtOUTP.Text);

                try
                {
                    openCon.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Attendance Recorded!");
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                openCon.Close();
                GetLogData();
            }
        }

        private void tbSearch2_KeyPress(object sender, KeyPressEventArgs e)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = openCon;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT e.empID, e.lastName, e.firstName, e.middleName, e.position, " +
                                    "l.Logdate, l.AMtimeIN, l.AMtimeOUT ,(d.AMTotalHours/60), l.PMtimeIN, l.PMtimeOUT, (d.PMTotalHours/60), " +
                                    "(d.totalHours/60), (d.regularHours/60), (d.overtimeHours/60), (d.nightDiffHours/60) " +
                                    "FROM tblEmpInfo as e inner join logRecords as l on l.empID= e.empID " +
                                    "full join DailyHours_Pay as d on d.logID = l.logID " +
                                    "where l.empID like '" + tbSearch.Text + "%' or e.lastName like '" + tbSearch.Text + "%' or "+
                                    " e.firstName like '" + tbSearch.Text + "%' or e.middleName like '" + tbSearch.Text + "%' or e.position like '" + tbSearch.Text + "%' and l.logDate between @dateFrom and @dateTo";
                command.Parameters.AddWithValue("@dateFrom", dtpFrom.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@dateTo", dtpTo.Value.ToString("yyyy-MM-dd"));
                SqlDataAdapter da = new SqlDataAdapter(command);
                openCon.Open();
                openCon.Close();
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                GetLogData();
            }
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 1 && dataGridView2.SelectedRows[0].Index != dataGridView2.Rows.Count - 1)
            {
                if (MessageBox.Show("Are you sure to delete this record?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = openCon;
                        com.CommandType = CommandType.Text;
                        com.CommandText = "Delete from logRecords where empID = @empID";
                        com.Parameters.AddWithValue("@empID", dataGridView2.SelectedRows[0].Cells[0].Value.ToString());
                        openCon.Open();
                        com.ExecuteNonQuery();
                        openCon.Close();
                    }
                    using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = openCon;
                        com.CommandType = CommandType.Text;
                        com.CommandText = "Delete from DailyHours_Pay where empID = @empID";
                        com.Parameters.AddWithValue("@empID", dataGridView2.SelectedRows[0].Cells[0].Value.ToString());
                        openCon.Open();
                        com.ExecuteNonQuery();
                        openCon.Close();
                        dataGridView2.Rows.RemoveAt(dataGridView2.SelectedRows[0].Index);
                        MessageBox.Show("Record Deleted");
                    }
                    txtEmpID.Text = "";
                    txtLastName.Text = "";
                    txtFirstName.Text = "";
                    txtMiddleName.Text = "";
                    txtPosition.Text = "";
                    txtIN.Text = "";
                    txtINP.Text = "";
                    txtOUT.Text = "";
                    txtOUTP.Text = "";
                    dtpLogDate.ResetText();
                    btnADD.Enabled = true;
                    btnUpdate.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Please select a row.");
            }
            GetLogData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnUpdate1.Enabled = true;
            btnSave.Enabled = false;
            if (dataGridView1.Rows.Count > 1 && dataGridView1.SelectedRows[0].Index != dataGridView1.Rows.Count - 1)
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = openCon;
                    com.CommandType = CommandType.Text;
                    com.CommandText = "select * from tblEmpInfo where empID = @empID";
                    com.Parameters.AddWithValue("@empID", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    SqlDataAdapter sda = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    tbEmpID.Text = dt.Rows[0][0].ToString();
                    tbLName.Text = dt.Rows[0][1].ToString();
                    tbFName.Text = dt.Rows[0][2].ToString();
                    tbMName.Text = dt.Rows[0][3].ToString();
                    cmbPosition.Text = dt.Rows[0][4].ToString();
                    tbAddress.Text = dt.Rows[0][10].ToString();
                    tbCNum.Text = dt.Rows[0][9].ToString();
                    if (dt.Rows[0][5].ToString() == "Male")
                        rbMale.Select();
                    else
                        rbFemale.Select();
                    dtpDOB.Text = dt.Rows[0][6].ToString();
                    if (dt.Rows[0][11] != DBNull.Value)
                    {
                        byte[] image = (byte[])dt.Rows[0][11];
                        MemoryStream ms = new MemoryStream(image);
                        pbImage.Image = Image.FromStream(ms);
                    }
                    cmbStatus.Text = dt.Rows[0][8].ToString();
                    lblAge.Text = age.ToString();
                }
            }
            else
            {
                MessageBox.Show("Please select a row.");
            }
        }
    }
}