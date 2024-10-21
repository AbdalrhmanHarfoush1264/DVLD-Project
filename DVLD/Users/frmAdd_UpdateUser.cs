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
    public partial class frmAdd_UpdateUser : Form
    {
        public enum enMode { Add=0, Update=1 };
        private enMode _Mode;

        private int _UserID = -1;
        private clsUser _User;
        public frmAdd_UpdateUser()
        {
            InitializeComponent();
            _Mode= enMode.Add;

        }

        public frmAdd_UpdateUser(int UserID)
        {
            InitializeComponent();

            _Mode= enMode.Update;
            _UserID = UserID;
        }

        private void _ResetdefualtValues()
        {
            if( _Mode == enMode.Add ) 
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";

                _User = new clsUser();

                tabLoginInfo.Enabled = false;
                btnSave.Enabled = false;
                ctrlPersonDetailsWithFilter1.FilterFoucs();
            }
            else
            {
                
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                tabLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            lblUserID.Text = "???";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            cbIsActive.Checked = true;

        }
        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if(_Mode==enMode.Update)
            {
                btnSave.Enabled = true;
                tabLoginInfo.Enabled = true;
                tpUserInfo.SelectedTab = tpUserInfo.TabPages["tabLoginInfo"];
                return;
            }


            if(ctrlPersonDetailsWithFilter1.PersonID!=-1)
            {
                if(clsUser.IsUserExistForPersonID(ctrlPersonDetailsWithFilter1.PersonID))
                {
                    MessageBox.Show("Select Person already has a User,Choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonDetailsWithFilter1.FilterFoucs();
                }
                else
                {
                    btnSave.Enabled = true;
                    tabLoginInfo.Enabled=true;
                    tpUserInfo.SelectedTab = tpUserInfo.TabPages["tabLoginInfo"];
                }
            }
            else
            {
                MessageBox.Show("Please Select a person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonDetailsWithFilter1.FilterFoucs();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAdd_UpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonDetailsWithFilter1.FilterFoucs();
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtPassword.Text.Trim()!=txtConfirmPassword.Text.Trim())
            {
                e.Cancel = true;
                epConfirmPassword.SetError(txtConfirmPassword, "Password Confirm does not match Password!");
            }
            else
            {
                e.Cancel= false;
                epConfirmPassword.SetError(txtConfirmPassword, null);

            }
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                epConfirmPassword.SetError(txtPassword, "Password text is empty!");
            }
            else
            {
                e.Cancel = false;
                epConfirmPassword.SetError(txtPassword, null);
            }
        }
        //More Code in txtUserName_Vlidating.....?
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                epConfirmPassword.SetError(txtUserName, "UserName text is empty!");
            }
            else
            {
                e.Cancel = false;
                epConfirmPassword.SetError(txtUserName, null);
            }
        }

        private void _LoadData()
        {
            _User = clsUser.FindByUserID(_UserID);
            ctrlPersonDetailsWithFilter1.FilterEnabled = false;


            if(_User == null)
            {
                MessageBox.Show("No User with ID = " + _UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;
            }

            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            cbIsActive.Checked = _User.IsActive;
            ctrlPersonDetailsWithFilter1.LoadPersonInfo(_User.PersonID);
        }
        private void frmAdd_UpdateUser_Load(object sender, EventArgs e)
        {
            _ResetdefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro"
                    , "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _User.PersonID = ctrlPersonDetailsWithFilter1.PersonID;
            _User.UserName = txtUserName.Text;
            _User.Password = clsGlobal.ComputeHash(txtPassword.Text);
            _User.IsActive = cbIsActive.Checked;

            if(_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();

                _Mode = enMode.Update;
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
