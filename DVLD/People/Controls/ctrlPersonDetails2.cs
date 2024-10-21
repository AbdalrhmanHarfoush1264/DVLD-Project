using BussinessLayer;
using DVLD.Properties;
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
    public partial class ctrlPersonDetails2 : UserControl
    {
        private clsPeople _Person;
        private int _PersonID = -1;

        public int PersonID
        {

            get { return _PersonID; }
        }
        public ctrlPersonDetails2()
        {
            InitializeComponent();
            lnklEditPerson.Enabled= false;
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPeople.Find(PersonID);

            if(_Person==null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FillDetailsCard();
        }
        private void ctrlPersonDetails2_Load(object sender, EventArgs e)
        {

        }

        public void FillDetailsCard()
        {
            lnklEditPerson.Enabled = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblName.Text = _Person.FullName;
            lblNationalNo.Text = _Person.NationalNo;
            lblGendor.Text = _Person.Gendor == 0 ? "Male" : "Female";
            pbGendor.Image = ((_Person.Gendor == 0) ? Resources.Man_32 : Resources.Woman_32);
            _LoadPersonImage();
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _Person.Phone;
            lblCountry.Text = clsCountries.Find(_Person.NationalityCountryID).CountryName;

        }
        public void ResetPersonInfo()
        {
            lblPersonID.Text = "[????]";
            lblName.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGendor.Text = "[????]";
            pbGendor.Image = Resources.Man_32;
            pbPersonImage.Image = Resources.Male_512;
            lblEmail.Text = "[????]";
            lblAddress.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";
        }
        private void _LoadPersonImage()
        {
            if (_Person.ImagePath != "")
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;
                return;
            }
            else if (_Person.Gendor == 0)
            {
                pbPersonImage.Image = Resources.Male_512;
                return;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
                return;
            }
        }

        private void lnklEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAdd_Edit_PersonInfo frmEditPerson = new frmAdd_Edit_PersonInfo(_Person.PersonID);
            frmEditPerson.ShowDialog();

            //Refresh
            LoadPersonInfo(_Person.PersonID);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
