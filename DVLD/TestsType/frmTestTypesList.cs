using BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmTestTypesList : Form
    {
        private DataTable _dtAllTestTypes;
        public frmTestTypesList()
        {
            InitializeComponent();
        }

        private void frmTestTypesList_Load(object sender, EventArgs e)
        {
            _dtAllTestTypes = clsTestTypes.GetAllTestTypes();
            dgvTestTypesList.DataSource = _dtAllTestTypes;
            lblRecords.Text = dgvTestTypesList.Rows.Count.ToString();

            dgvTestTypesList.Columns[0].HeaderText = "ID";
            dgvTestTypesList.Columns[0].Width = 120;

            dgvTestTypesList.Columns[1].HeaderText = "Title";
            dgvTestTypesList.Columns[1].Width = 120;

            dgvTestTypesList.Columns[2].HeaderText = "Description";
            dgvTestTypesList.Columns[2].Width = 300;

            dgvTestTypesList.Columns[3].HeaderText = "Fees";
            dgvTestTypesList.Columns[3].Width = 90;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateTestType frmUpdate = new frmUpdateTestType((clsTestTypes.enTestType)dgvTestTypesList.CurrentRow.Cells[0].Value);
            frmUpdate.ShowDialog();

            frmTestTypesList_Load(null, null);
        }
    }
}
