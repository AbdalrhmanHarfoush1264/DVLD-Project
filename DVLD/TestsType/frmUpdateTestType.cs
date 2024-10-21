using BussinessLayer;
using DVLD.Global_Classes;
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
    public partial class frmUpdateTestType : Form
    {
        private clsTestTypes.enTestType _TestTypeID = clsTestTypes.enTestType.VisionTest;
        private clsTestTypes _clsTestTypes;

        public frmUpdateTestType(clsTestTypes.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestTypeID= TestTypeID;
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            _clsTestTypes = clsTestTypes.Find(this._TestTypeID);
            lbTestTypeID.Text=_TestTypeID.ToString();

            if(_clsTestTypes!= null)
            {
                txtTitleTestType.Text = _clsTestTypes.Title;
                txtDescriptionTestType.Text = _clsTestTypes.Description;
                txtFeesTestType.Text = _clsTestTypes.Fees.ToString();
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _clsTestTypes.Title = txtTitleTestType.Text.Trim();
            _clsTestTypes.Description=txtDescriptionTestType.Text.Trim();
            _clsTestTypes.Fees = Convert.ToSingle(txtFeesTestType.Text.Trim());

            if (_clsTestTypes.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void txtTitleTestType_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitleTestType.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitleTestType, "Title cannot be empty!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTitleTestType, null);
            }
        }

        private void txtDescriptionTestType_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescriptionTestType.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDescriptionTestType, "Description cannot be empty!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtDescriptionTestType, null);
            }
        }

        private void txtFeesTestType_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFeesTestType.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFeesTestType, "Fees cannot be empty!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFeesTestType, null);
            }

            if (!clsValidatoin.IsNumber(txtFeesTestType.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFeesTestType, "Invalid Number.");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFeesTestType, null);
            }
        }
    }
}
