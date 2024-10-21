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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            clsUser user = clsUser.FindByUserNameAndPassword(txtUserName.Text.Trim(),clsGlobal.ComputeHash(txtPassword.Text.Trim()));

            if(user != null )
            {
                //User is Found.

                if(chkRemberMe.Checked)
                {
                    //store Username and Password.
                    clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(),txtPassword.Text.Trim());
                }
                else
                {
                    //Store empty Username and Password
                    clsGlobal.RememberUsernameAndPassword("", "");
                }


                if(!user.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your Account is not Active, Contact Admin", "In Active Acount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser=user;
                this.Hide();
                frmMain frmMain =new frmMain(this);
                frmMain.ShowDialog();
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password","Wrong Credintials",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";
            if (clsGlobal.GetStoredCredential(ref Username, ref Password))
            {
                txtUserName.Text = Username;
                txtPassword.Text = Password;
                chkRemberMe.Checked = true;
            }
            else
                chkRemberMe.Checked = false;
        }
    }
}
