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
    public partial class frmListInternationalLicenseApplications : Form
    {
        private DataTable _dtInternationalLicenseApplications;
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private void btnNewApplication_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();

            frmListInternationalLicenseApplications_Load(null, null);
        }

        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            _dtInternationalLicenseApplications = clsInternationalLicense.GetAllInternationalLicenses();
            dgvListInternationalLicenses.DataSource = _dtInternationalLicenseApplications;
            lblRecordsCount.Text = dgvListInternationalLicenses.Rows.Count.ToString();

            if(dgvListInternationalLicenses.Rows.Count>0)
            {
                dgvListInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvListInternationalLicenses.Columns[0].Width = 160;

                dgvListInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvListInternationalLicenses.Columns[1].Width = 160;

                dgvListInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvListInternationalLicenses.Columns[2].Width = 160;

                dgvListInternationalLicenses.Columns[3].HeaderText = "L.License ID";
                dgvListInternationalLicenses.Columns[3].Width = 160;

                dgvListInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvListInternationalLicenses.Columns[4].Width = 200;

                dgvListInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvListInternationalLicenses.Columns[5].Width = 200;

                dgvListInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvListInternationalLicenses.Columns[6].Width = 100;
            }
            

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterBy.Text=="Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
            }else
            {
                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsActive.Visible = false;

                if (cbFilterBy.Text == "None")
                {
                    txtFilterValue.Enabled = false;
                }
                else
                    txtFilterValue.Enabled = true;

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text;

            switch(FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }

            if (FilterValue == "All")
                _dtInternationalLicenseApplications.DefaultView.RowFilter = "";
            else
                _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}]= {1}", FilterColumn, FilterValue);

            lblRecordsCount.Text = dgvListInternationalLicenses.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch(cbFilterBy.Text)
            {
                case "International License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;
                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;
                case "Local License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;
                case "Is Active":
                    FilterColumn = "IsActive";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }


            if(txtFilterValue.Text.Trim()==""||FilterColumn=="None")
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvListInternationalLicenses.Rows.Count.ToString();
                return;
            }

            _dtInternationalLicenseApplications.DefaultView.RowFilter =
                string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvListInternationalLicenses.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow numbers only becasue all fiters are numbers.
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID =(int)dgvListInternationalLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDrivers.FindByDriverID(DriverID).PersonID;

            frmPersonDetails frm = new frmPersonDetails(PersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dgvListInternationalLicenses.CurrentRow.Cells[0].Value;
            frmShowInternationalLicenseInfo frm =new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvListInternationalLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDrivers.FindByDriverID(DriverID).PersonID;

            frmLicenseHistory frm = new frmLicenseHistory(PersonID);
            frm.ShowDialog();
        }
    }
}
