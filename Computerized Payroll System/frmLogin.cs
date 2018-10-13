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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void tbUsername_Leave(object sender, EventArgs e)
        {
            if (tbUsername.Text.Length == 0)
            {
                lblUsername.Text = "Username";
                lblUsername.ForeColor = SystemColors.ControlLightLight;
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
                lblPassword.ForeColor = SystemColors.ControlLightLight;
            }
        }

        private void tbPassword_Enter(object sender, EventArgs e)
        {
            lblPassword.Text = "";
            this.AcceptButton = this.btnLogIn;
        }

        public static string empID;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            frmMain m = new frmMain();
            m.Show();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            empID = tbUsername.Text;
            using (SqlConnection openCon = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=dbPayroll; " +
                                       "connection timeout=30"))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = openCon;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = "SELECT * FROM EmpPW WHERE EmpID = '" + tbUsername.Text +
                                        "' COLLATE Latin1_General_CS_AS AND password = '" + tbPassword.Text + "' COLLATE Latin1_General_CS_AS ;";

                    try
                    {
                        openCon.Open();
                        SqlDataReader sdr = comm.ExecuteReader();
                        if (sdr.Read() == true)
                        {
                            int eAccess = sdr.GetInt32(2);
                            if (eAccess == 1)
                            {
                                frmAdmin ad = new frmAdmin();
                                ad.Show();
                                this.Hide();
                            }
                            else
                            {
                                frmEmployee emp = new frmEmployee();
                                emp.Show();
                                this.Hide();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect username and/or password!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.tbUsername.Leave += new System.EventHandler(this.tbUsername_Leave);
            this.tbUsername.Enter += new System.EventHandler(this.tbUsername_Enter);
            this.tbPassword.Leave += new System.EventHandler(this.tbPassword_Leave);
            this.tbPassword.Enter += new System.EventHandler(this.tbPassword_Enter);
        }

        private void lblUsername_Click(object sender, EventArgs e)
        {
            tbUsername.Focus();
        }

        private void lblPassword_Click(object sender, EventArgs e)
        {
            tbPassword.Focus();
        }
    }
}
