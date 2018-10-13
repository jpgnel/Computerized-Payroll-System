using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Computerized_Payroll_System
{
    public partial class frmLogIn : Form
    {
        public frmLogIn()
        {
            InitializeComponent();
        }

        frmLogIn2 frmLI = new frmLogIn2();
        public static int bt;

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            bt = 1;
            frmLI.Show();
            this.Hide();
        }

        private void btnEmp_Click(object sender, EventArgs e)
        {
            bt = 2;
            frmLI.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }
    }
}
