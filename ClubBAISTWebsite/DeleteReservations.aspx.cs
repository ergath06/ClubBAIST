﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;

namespace ClubBAISTWebsite
{
    public partial class DeleteReservations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OutputTable.Rows.Clear();
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                ClubBaistSystem system = new ClubBaistSystem();
                UserPermission upCurrent = new UserPermission();
                upCurrent = system.GetUser(Page.User.Identity.Name);
                if (upCurrent.EmployeeFlag)
                {
                    //User is employee so load all features. (Admin User can change username)
                    tbUser.Enabled = true;
                }
                else if (upCurrent.ApprovedFlag)
                {
                    //User is standard user so load single user only functions. (Cannot change username)
                    tbUser.Text = Page.User.Identity.Name;
                    tbUser.Enabled = false;
                }
                else
                {
                    //User is not employee or approved so don't do anything.
                    wizSearchReservation.Visible = false;
                }
            }
        }

        protected void wizSearchReservation_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (wizSearchReservation.ActiveStepIndex == 1)
            {
                if (calStartDate.SelectedDate == new DateTime())
                {
                    e.Cancel = true;
                    labMessage.Text = "Start Date is Required!";
                    labMessage.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    labMessage.Text = "";
                    labMessage.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        protected void wizSearchReservation_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (calEndDate.SelectedDate == new DateTime())
            {
                e.Cancel = true;
                labMessage.Text = "End Date is Required!";
                labMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                labMessage.Text = "";
                labMessage.ForeColor = System.Drawing.Color.Black;
                SearchRes();
            }
        }

        protected void calEndDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date <= calStartDate.SelectedDate)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
            }
        }

        public void SearchRes()
        {
            OutputTable.Rows.Clear();
            List<int> reserveIDs = new List<int>();
            ddlReservations.DataSource = reserveIDs;
            ddlReservations.DataBind();
            ClubBaistSystem system = new ClubBaistSystem();
            DataTable ReservationsTable = system.GetReservations(tbUser.Text, Convert.ToInt16(ddlLocations.SelectedValue), calStartDate.SelectedDate, calEndDate.SelectedDate);

            TableHeaderRow headRow = new TableHeaderRow();
            TableHeaderCell headCell = new TableHeaderCell();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();

            for (int y = 0; y < ReservationsTable.Rows.Count; y++)
            {
                row = new TableRow();
                for (int x = 0; x < ReservationsTable.Columns.Count; x++)
                {
                    cell = new TableCell();
                    if (OutputTable.Rows.Count == 0)
                    {
                        headCell = new TableHeaderCell();
                        headCell.Text = ReservationsTable.Columns[x].ToString();
                        headRow.Cells.Add(headCell);
                    }
                    cell.Text = ReservationsTable.Rows[y][x].ToString();
                    try
                    {
                        if (x == 0) //Make sure to look at the first column only
                        {
                            reserveIDs.Add(Convert.ToInt32(cell.Text)); //This will throw an exception on the header row, so we try catch all
                        }
                    }
                    catch { }
                    row.Cells.Add(cell);
                }

                if (OutputTable.Rows.Count == 0)
                {
                    OutputTable.Rows.Add(headRow);
                }
                
                OutputTable.Rows.Add(row);
            }
            ddlReservations.DataSource = reserveIDs;
            ddlReservations.DataBind();
            labReservationID.Visible = true;
            ddlReservations.Visible = true;
            butDelete.Visible = true;
            wizSearchReservation.Visible = false;
        }

        protected void calStartDate_DayRender(object sender, DayRenderEventArgs e) //Limit start date because shouldn't cancel past events
        {
            if (e.Day.Date < DateTime.Now)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
            }
        }

        protected void butDelete_Click(object sender, EventArgs e)
        {
            if (ddlReservations.SelectedValue != "")
            {
                ddlReservations.Visible = true;
                butDelete.Visible = true;
                labMessage.ForeColor = System.Drawing.Color.DarkGreen;
                labMessage.Text = "";
                if (labMessage.Text == "")
                {
                    labMessage.ForeColor = System.Drawing.Color.Red;
                    labMessage.Text = "Are you sure you want to delete reservation number " + ddlReservations.SelectedValue + "?";
                }
                else
                {
                    //Now try to delete the reservation
                    ClubBaistSystem system = new ClubBaistSystem();
                    bool bSuccess = false;
                    bSuccess = system.DeleteReservation(Convert.ToInt64(ddlReservations.SelectedValue));
                    if (bSuccess)
                    {
                        labMessage.ForeColor = System.Drawing.Color.Black;
                        labMessage.Text = "Delete Successful!";
                        ddlReservations.Visible = false;
                        butDelete.Visible = false;
                        labReservationID.Visible = false;
                    }
                    else
                    {
                        labMessage.ForeColor = System.Drawing.Color.Red;
                        labMessage.Text = "Delete Failed!";
                    }
                }
            }
            else
            {
                ddlReservations.Visible = false;
                butDelete.Visible = false;
                labMessage.ForeColor = System.Drawing.Color.Red;
                labMessage.Text = "No reservations exist for that time frame!";
            }
        }
    }
}