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
    public partial class ctrlPersonDetailsWithFilter : UserControl
    {
        public int PersonID
        {
            get { return ctrlPersonDetails21.PersonID; }
        }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled= value;
                gbFilterBy.Enabled = _FilterEnabled;
            }
        }
        public ctrlPersonDetailsWithFilter()
        {
            InitializeComponent();
            cbFilterBy.Text = "National No";
        }

        public void FilterFoucs()
        {
            txtFilterValue.Focus();
        }
        //public class PersonSelectedEventArgs:EventArgs
        //{
        //    public int PersonID { set;get; }

        //    public PersonSelectedEventArgs(int personID)
        //    {
        //        this.PersonID = personID;
        //    }
        //}
        //public event EventHandler<PersonSelectedEventArgs> OnPersonSelected;

        //public void RaiseOnPersonSelected(int PersonID)
        //{
        //    RaiseOnPersonSelected(new PersonSelectedEventArgs(PersonID));
        //}
        //protected virtual void RaiseOnPersonSelected(PersonSelectedEventArgs e)
        //{
        //   OnPersonSelected?.Invoke(this, e);
        //}
        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAdd_Edit_PersonInfo frmAddPerson = new frmAdd_Edit_PersonInfo();
            frmAddPerson.ShowDialog();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterBy.Text!="None")
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            if (txtFilterValue.Text == "")
                return;

            clsPeople Person = new clsPeople();

            if(cbFilterBy.Text=="National No")
            {
                Person = clsPeople.Find(txtFilterValue.Text.Trim());
                if (Person != null)
                {
                    MessageBox.Show("person is found", "Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ctrlPersonDetails21.LoadPersonInfo(Person.PersonID);
                }
                else
                {
                    MessageBox.Show($"No Person with National No={txtFilterValue.Text.Trim()}!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if(cbFilterBy.Text=="Person ID")
            {
                Person = clsPeople.Find(Convert.ToInt32(txtFilterValue.Text));
                if (Person != null)
                {
                    MessageBox.Show("person is found", "Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ctrlPersonDetails21.LoadPersonInfo(Person.PersonID);
                }
                else
                {
                    MessageBox.Show($"No Person with PersonID ={txtFilterValue.Text.Trim()}", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public void LoadPersonInfo(int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            ctrlPersonDetails21.LoadPersonInfo(PersonID);
        }
        private void ctrlPersonDetails21_Load(object sender, EventArgs e)
        {

        }

        private void ctrlPersonDetailsWithFilter_Load(object sender, EventArgs e)
        {

        }
    }
}
