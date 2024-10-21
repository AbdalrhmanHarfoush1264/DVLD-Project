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
    public partial class frmUpdateApplicationType : Form
    {
        private int _ApplicationTypeID = -1;
        private clsApplicationType _ApplicationType;
        public frmUpdateApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
           
            _ApplicationType = clsApplicationType.Find(this._ApplicationTypeID);
            lblApplicationTypeID.Text = _ApplicationType.ID.ToString();

            if ( _ApplicationType != null )
            {
                txtApplicationTypeTitle.Text = _ApplicationType.Title;
                txtApplicationTypeFees.Text = _ApplicationType.fees.ToString();
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation error",MessageBoxButtons.OK,MessageBoxIcon.Error);

                return;
            }

            _ApplicationType.Title = txtApplicationTypeTitle.Text.Trim();
            _ApplicationType.fees = Convert.ToSingle(txtApplicationTypeFees.Text.Trim());

            if(_ApplicationType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void txtApplicationTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtApplicationTypeTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeTitle, "Title cannot be empty!");
                return;
            }else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtApplicationTypeTitle, null);
            }
        }

        private void txtApplicationTypeFees_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtApplicationTypeFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtApplicationTypeFees,null);
            }

            if(!clsValidatoin.IsNumber(txtApplicationTypeFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeFees, "Invalid Number.");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtApplicationTypeFees, null);
            }
        }
    }
}
