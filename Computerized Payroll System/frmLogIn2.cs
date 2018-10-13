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
    public partial class frmLogIn2 : Form
    {
        public frmLogIn2()
        {
            InitializeComponent();
            this.FormClosing += Form2_FormClosing;
            this.tbUsername.Leave += new System.EventHandler(this.tbUsername_Leave);
            this.tbUsername.Enter += new System.EventHandler(this.tbUsername_Enter);
            this.tbPassword.Leave += new System.EventHandler(this.tbPassword_Leave);
            this.tbPassword.Enter += new System.EventHandler(this.tbPassword_Enter);
        }

        private void tbUsername_Leave(object sender, EventArgs e)
        {
            if (tbUsername.Text.Length == 0)
            {
                lblUsername.Text = "Username";
                lblUsername.ForeColor = SystemColors.Control;
            }
        }

        private void tbUsername_Enter(object sender, EventArgs e)
        {
            lblUsername.Text = "";
            this.AcceptButton = this.btnLogIn;

        }

        private void tbPassword_Leave(object sender, EventArgs e)
        {
            if (tbPassword.Text.Length == 0)
            {
                lblPassword.Text = "Password";
                lblPassword.ForeColor = SystemColors.Control;
            }
        }

        private void tbPassword_Enter(object sender, EventArgs e)
        {
            lblPassword.Text = "";
            this.AcceptButton = this.btnLogIn;
        }

        void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmLogIn frm = new frmLogIn();
            this.Hide();
            e.Cancel = true;
            frm.Show();
        }

        public static string empID = "";

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            empID = tbUsername.Text;

            using (SqlConnection openCon = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=sampleDB; " +
                                       "connection timeout=30"))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = openCon;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = "SELECT * FROM emp_pw WHERE username = '" + tbUsername.Text +
                                        "' AND password = '" + tbPassword.Text + "';";

                    try
                    {
                        openCon.Open();
                        SqlDataReader sdr = comm.ExecuteReader();
                        if (sdr.Read() == true)
                        {
                            int eAccess = sdr.GetInt32(3);
                            if (eAccess == 1 && frmLogIn.bt == 1)
                            {
                                frmAdmin rf = new frmAdmin();
                                rf.Show();
                                this.Hide();
                            }
                            else if (eAccess == 2 && frmLogIn.bt == 2)
                            {
                                frmEmployee ef = new frmEmployee();
                                ef.Show();
                                this.Hide();
                            }
                            else
                            {
                                lblIncorrect.Text = "*Invalid username or password";
                            }
                        }
                        else
                        {
                            lblIncorrect.Text = "*Invalid username or password";
                        }
                    }
                    catch (Exception ex)
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
