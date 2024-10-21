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
    public partial class frmScheduleTest : Form
    {
        private int _LocalDrivingLicenseAppID = -1;
        private clsTestTypes.enTestType _TestTypeID = clsTestTypes.enTestType.VisionTest;
        private int _AppointmentID = -1;
        public frmScheduleTest(int LocalDrivingLicenseAppID,clsTestTypes.enTestType TestTypeID,int AppointmentID=-1)
        {
            InitializeComponent();

            this._TestTypeID = TestTypeID;
            this._LocalDrivingLicenseAppID = LocalDrivingLicenseAppID;
            this._AppointmentID = AppointmentID;
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID= _TestTypeID;
            ctrlScheduleTest1.LoadInfo(_LocalDrivingLicenseAppID, _AppointmentID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
