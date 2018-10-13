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
    public partial class frmTimekeeping : Form
    {
        public frmTimekeeping()
        {
            InitializeComponent();
            DateTime today = DateTime.Today;
            lblDate.Text = today.ToString("ddd, MMM dd, yyyy");
            this.lblTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Tick += new EventHandler(timer1_Tick);
            this.timer1.Interval = 1000;
            this.timer1.Enabled = true;
            getInfo();
            btnOut.Enabled = false;
        }

        private void getInfo()
        {
            using (SqlConnection con = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=sampleDB; " +
                                       "connection timeout=30"))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = con;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = "Select * From empInfo where employeeID2 = '" + frmLogIn2.empID + "';";
                    con.Open();
                    using (SqlDataReader oReader = comm.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            lblEmpID.Text = oReader["employeeID2"].ToString();
                            lblName.Text = oReader["firstName"].ToString() + " " + oReader["lastName"].ToString();
                            lblPosition.Text = oReader["position"].ToString();
                            lblRank.Text = oReader["rank"].ToString();
                        }

                        con.Close();
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lblTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            btnIn.Enabled = false;
            btnOut.Enabled = true;

            String timeIn = DateTime.Now.ToString("HH:mm:ss tt");
            MessageBox.Show("Time-in: " + timeIn);

            using (SqlConnection openCon = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=sampleDB; " +
                                       "connection timeout=30"))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = openCon;            // <== lacking
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT into logRecords (empID, timeIn, date)" +
                                          "VALUES (@empID, @timeIn, @date)";
                    command.Parameters.AddWithValue("@empID", frmLogIn2.empID);
                    command.Parameters.AddWithValue("@timeIn", timeIn);
                    command.Parameters.AddWithValue("@date", DateTime.Today.ToString("MM/dd/yyyy"));

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
        }
    }
}
