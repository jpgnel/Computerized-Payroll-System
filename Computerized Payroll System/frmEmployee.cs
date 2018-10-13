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
    public partial class frmEmployee : Form
    {
        public frmEmployee()
        {
            InitializeComponent();
            getImageAndText();
            tk.TopLevel = false;
            tk.AutoScroll = true;
            panel5.Controls.Add(tk);
            tk.Show();
        }

        frmTimekeeping tk = new frmTimekeeping();
        frmLogRecords lr = new frmLogRecords();

        private void getImageAndText()
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
                            lblWelcome.Text = "Welcome " + oReader["firstName"].ToString();
                            byte[] image = (byte[])oReader[11];
                            MemoryStream ms = new MemoryStream(image);
                            pbEmployee.Image = Image.FromStream(ms);
                        }

                        con.Close();
                    }
                }
            }
        }

        private void btnTimekeeping_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            tk.TopLevel = false;
            tk.AutoScroll = true;
            panel5.Controls.Add(tk);
            tk.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogIn mf = new frmLogIn();
            mf.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            lr.TopLevel = false;
            lr.AutoScroll = true;
            panel5.Controls.Add(lr);
            lr.Show();
        }
    }
}
