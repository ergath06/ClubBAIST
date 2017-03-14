using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClubBAISTWebsite
{
    public partial class NewApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (this.Title != null)
            {
                this.Title += " New Application Page - Club BAIST";
            }
            else
            {
                this.Title = "New Application Page - Club BAIST";
            }
        }

        protected void butSub_Click(object sender, EventArgs e)
        {
            //This will be where we do most stuff
            bool bProblem = false;

            ClubBaistSystem cbSystem = new ClubBaistSystem();
            try
            {
                bool bConfirmation = cbSystem.AddUser(tbEmail.Text, tbName.Text, tbPass.Text, Convert.ToInt32(ddlMemberships.Text), tbHomePhone.Text, tbCellPhone.Text, null);
                if (bConfirmation)
                {
                    labMessage.ForeColor = System.Drawing.Color.DarkGreen;
                    labMessage.Text = "Successfully Added New User.<br />";
                    tbEmail.Text = "";
                    tbName.Text = "";
                    tbPass.Text = "";
                    tbPassConfirm.Text = "";
                    tbHomePhone.Text = "";
                    tbCellPhone.Text = "";

                }
                else
                {
                    labMessage.ForeColor = System.Drawing.Color.DarkRed;
                    labMessage.Text = "Error Adding New User.<br />";
                    bProblem = true;
                }
            }
            catch (Exception eep)
            {
                labMessage.ForeColor = System.Drawing.Color.DarkRed;
                labMessage.Text = "Error Adding New User.<br />";
                bProblem = true;
            }
            if (bProblem)
            {
                labMessage.Text += "Please reveiw problem and try again.";
            }

        }
    }
}