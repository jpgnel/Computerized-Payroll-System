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
    public partial class frmChangePassword : Form
    {
        public frmChangePassword()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbPassword1.Text == tbPassword2.Text)
            {
                using (SqlConnection con = new SqlConnection("server=localhost;" +
                                               "Trusted_Connection=yes;" +
                                               "database=dbPayroll; " +
                                               "connection timeout=30"))
                {
                    using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = con;
                        com.CommandType = CommandType.Text;
                        com.CommandText = "Update empPW set password = @password where empID = @empID";
                        com.Parameters.AddWithValue("@password", tbPassword1.Text);
                        com.Parameters.AddWithValue("@empID", frmLogin.empID);

                        try
                        {
                            con.Open();
                            int recordsAffected = com.ExecuteNonQuery();
                            MessageBox.Show("Password changed.");

                        }
                        catch (Exception ex)
                        { MessageBox.Show(ex.Message); }
                        finally
                        {
                            con.Close();
                            this.Close();
                        }
                    }
                }
            }
            else
                MessageBox.Show("Passwords didn't match!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void tbPassword2_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = this.btnSave;
        }

        private void tbPassword1_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = this.btnSave;
        }
    }
}
