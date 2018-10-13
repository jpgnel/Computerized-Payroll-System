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
    public partial class frmAdmin : Form
    {
        public frmAdmin()
        {
            InitializeComponent();
        }

        frmRegistration rf = new frmRegistration();
        frmEmployees ef = new frmEmployees();

        private Button lastButton = null;


        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            panel4.Controls.Clear();
            rf.TopLevel = false;
            rf.AutoScroll = true;
            panel4.Controls.Add(rf);
            rf.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmLogIn mf = new frmLogIn();
            mf.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel4.Controls.Clear();
            ef.TopLevel = false;
            ef.AutoScroll = true;
            panel4.Controls.Add(ef);
            ef.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {
            // Change the background color of the button that was clicked
            Button current = (Button)sender;
            current.BackColor = SystemColors.Highlight;

            // Revert the background color of the previously-colored button, if any
            if (lastButton != null)
                lastButton.BackColor = Color.Transparent;

            // Update the previously-colored button
            lastButton = current;
        }

    }
}
