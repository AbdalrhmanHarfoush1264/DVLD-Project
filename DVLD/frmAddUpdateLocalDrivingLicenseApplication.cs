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
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {
        public enum enMode { AddNew=1,Update=2};
        private enMode _Mode;
        private int _LocalDrivingLicenseAppID;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        // Delegate
        // i am not Do this Delegate..?
        private int _SelectedPersonID = -1;

        
        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseAppID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _LocalDrivingLicenseAppID = LocalDrivingLicenseAppID;
        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            foreach(DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }

        }
        private void _ResetDefualtValues()
        {
            _FillLicenseClassesInComboBox();
            if(_Mode==enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                ctrlPersonDetailsWithFilter1.FilterFoucs();
                tabPApplicationInfo.Enabled = false;

                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                cbLicenseClass.SelectedIndex = 2;
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).fees.ToString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
                btnSave.Enabled= false;
               
                
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text= "Update Local Driving License Application";

                btnSave.Enabled = true;
                tabPApplicationInfo.Enabled= true;
            }


        }

        private void _LoadData()
        {
            ctrlPersonDetailsWithFilter1.Enabled = false;
            _LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseAppID(_LocalDrivingLicenseAppID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application With ID = "+_LocalDrivingLicenseAppID,"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlPersonDetailsWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicationPersonID);
            lblDLApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LocalDrivingLicenseApplication.ApplicationDate);
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblApplicationFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedBy.Text = clsUser.FindByUserID(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;
        }
        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues(); 
            
            if(_Mode==enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //More Code...if Update Here!...


            if (ctrlPersonDetailsWithFilter1.PersonID != -1)
            {
                btnSave.Enabled = true;
                tabPApplicationInfo.Enabled = true;
                tabCNewLocalDriving.SelectedTab = tabCNewLocalDriving.TabPages["tabPApplicationInfo"];
            }
            else
            { 
                MessageBox.Show("Select a Person","Select",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ctrlPersonDetailsWithFilter1.FilterFoucs();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Files are not valide!,put the mouse over the red icon(s) to see the erro",
                    "Validation Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;

           
            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(ctrlPersonDetailsWithFilter1.PersonID,
                clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);

            if(ActiveApplicationID !=-1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID,
                    "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }




            //check if user already have issued license of the same driving  class.
            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonDetailsWithFilter1.PersonID,LicenseClassID))
            {
                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class",
                    "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            _LocalDrivingLicenseApplication.ApplicationPersonID = ctrlPersonDetailsWithFilter1.PersonID;
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = 1;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(lblApplicationFees.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID= LicenseClassID;


            if(_LocalDrivingLicenseApplication.Save())
            {
                lblDLApplicationID.Text=_LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";

                MessageBox.Show("Data Saved Successfully.","Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }else
                MessageBox.Show("Error: Data is not Saved Successfully.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }
}
