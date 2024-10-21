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
    public partial class frmLicenseHistory : Form
    {
        private int _PersonID;

        public frmLicenseHistory()
        {
            InitializeComponent();

        }
        public frmLicenseHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void frmLicenseHistory_Load(object sender, EventArgs e)
        {
            if(_PersonID!=-1)
            {
                ctrlPersonDetailsWithFilter1.LoadPersonInfo(_PersonID);
                ctrlPersonDetailsWithFilter1.Enabled = false;
                ctrlDriverLicenses1.LoadInfoByPersonID(_PersonID);
            }
            else
            {
                ctrlPersonDetailsWithFilter1.Enabled = true;
                ctrlPersonDetailsWithFilter1.FilterFoucs();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Code Here about Delegate PersonID ...!
    }
}
