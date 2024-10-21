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
    public partial class frmPersonDetails : Form
    {
        private int _PersonID;
       
        public frmPersonDetails(int personID)
        {
            InitializeComponent();

            
            ctrlPersonDetails21.LoadPersonInfo(personID);
        }

        private void frmPersonDetails_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
