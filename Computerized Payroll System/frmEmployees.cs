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
    public partial class frmEmployees : Form
    {
        public frmEmployees()
        {
            InitializeComponent();
            dataGridView1.DataSource = GetData();
            dataGridView1.Columns[0].HeaderText = "PICTURE";
            dataGridView1.Columns[1].HeaderText = "EMPLOYEE ID";
            dataGridView1.Columns[2].HeaderText = "LAST NAME";
            dataGridView1.Columns[3].HeaderText = "FIRST NAME";
            dataGridView1.Columns[4].HeaderText = "MIDDLE NAME";
            dataGridView1.Columns[5].HeaderText = "POSITION";
            dataGridView1.Columns[6].HeaderText = "RANK";
            dataGridView1.Columns[7].HeaderText = "ADDRESS";
            dataGridView1.Columns[8].HeaderText = "CELLPHONE NUMBER";
            dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                if (dataGridView1.Columns[i] is DataGridViewImageColumn)
                {
                    ((DataGridViewImageColumn)dataGridView1.Columns[i]).ImageLayout = DataGridViewImageCellLayout.Stretch;
                    break;
                }

            dataGridView1.RowTemplate.Height = 70;
        }

        public DataTable GetData()
        {
            using (SqlConnection openCon = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=dbSample; " +
                                       "connection timeout=30"))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = openCon;            // <== lacking
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT picture, empID, lastName, firstName, middleName, position, rank, address, cNum FROM empInfo";

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    openCon.Open();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        frmEdit edf = new frmEdit();

        private void btnEdit_Click(object sender, EventArgs e)
        {
            edf.Show();
        }
    }
}
