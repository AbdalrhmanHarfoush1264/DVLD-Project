using BussinessLayer;
using DVLD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private int _LicenseID;
        private clsLicense _License;
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public int LicenseID
        {
            get
            {
                return _LicenseID;
            }
        }
        public clsLicense SelectLicenseInfo
        {
            get
            {
                return _License;
            }
        }
        private void _LoadPersonImage()
        {
           

            string ImagePath = _License.DriverInfo.PersonInfo.ImagePath;

            if(ImagePath =="")
            {
                //MessageBox.Show("Could not find this image: = " + ImagePath,
                //"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (_License.DriverInfo.PersonInfo.Gendor == 0)
                    pbDriverImage.Image = Resources.Male_512;
                else
                    pbDriverImage.Image = Resources.Female_512;
            }
            else
            {
                if(File.Exists(ImagePath))
                {
                    pbDriverImage.Load(ImagePath);
                }
            }
        }
        public void LoadInfo(int LicenseID)
        {
            _LicenseID = LicenseID;
            _License = clsLicense.Find(_LicenseID);

            if(_License == null ) 
            {
                MessageBox.Show("Could not find License ID = " + _LicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            lblLicenseID.Text = _License.LicenseID.ToString();
            lblClassName.Text = _License.LicenseClassInfo.ClassName;
            lblFullName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text=_License.DriverInfo.PersonInfo.NationalNo;
            lblGendor.Text = _License.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblIssueDate.Text = clsFormat.DateToShort(_License.IssueDate);
            lblIssueReason.Text = _License.IssueReasonText;
            lblNotes.Text = _License.Notes == "" ? "Not Notes" : _License.Notes;
            lblIsActive.Text = _License.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = clsFormat.DateToShort(_License.DriverInfo.PersonInfo.DateOfBirth);
            lblDriverID.Text = _License.DriverID.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(_License.ExpirationDate);
            lblIsDetained.Text = _License.IsDetained ? "Yes" : "No";
            _LoadPersonImage();
        }
        private void ctrlDriverLicenseInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
