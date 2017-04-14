using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ClubBAISTWebsite
{
    public partial class NewReservation : System.Web.UI.Page
    {
        public ClubBaistSystem system;
        public UserPermission upCurrent;
        public LocHour loc;
        public List<Reservation> reservationList;
        public List<DateTime> OneOffs;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                //Need to know if employee or standard user, hopefully a quick select will be fine?
                system = new ClubBaistSystem();
                upCurrent = new UserPermission();
                loc = new LocHour();
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
                    //User is not employee or approved so don't do anything. Disable the wizard completely.
                    wizAddReservation.Enabled = false;
                    wizAddReservation.Visible = false;
                }
                
            }
        }

        protected void wizAddReservation_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            //try to make a reservation if date was selected and ddl item selected
            if (calReservations.SelectedDate != null && ddlStartTime.SelectedItem != null)
            {
                Reservation res = new Reservation();
                res.Email = tbUser.Text;
                res.DateTime = new DateTime(calReservations.SelectedDate.Year,calReservations.SelectedDate.Month,calReservations.SelectedDate.Day,Convert.ToInt32(ddlStartTime.SelectedValue),0,0);
                res.Players = Convert.ToByte(ddlNumPlayas.SelectedValue);
                res.Carts = Convert.ToByte(ddlNumCarts.SelectedValue);
                res.LocationID = Convert.ToInt16(ddlLocations.SelectedValue);
                bool bSuccess = false;
                bSuccess = system.AddReservation(res);
                if (bSuccess)
                {
                    labStatus.Text = "Success!";
                    wizAddReservation.Visible = false;
                }
                else
                {
                    labStatus.Text = "Error! Something went wrong. Please try again.";
                }
            }
        }

        protected void wizAddReservation_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (wizAddReservation.ActiveStepIndex == 0) //Checks to make sure that the current step is the first step
            {
                //ClubBaistSystem system = new ClubBaistSystem();
                
                loc = system.GetHours(Convert.ToInt16(ddlLocations.Text), DateTime.Now); //loc now contains location hours and any althours

                //Now need to get any current reservations
                reservationList = new List<Reservation>();
                reservationList = system.GetReservations(Convert.ToInt16(ddlLocations.Text), DateTime.Now);

                //Use that data to fill the calendar control and drop down control
                GenerateDisabledDays();
            }
        }

        protected void calReservations_Load(object sender, EventArgs e)
        {
            
        }

        protected void calReservations_DayRender(object sender, DayRenderEventArgs e)
        {
            GenerateDisabledDays();
            if (e.Day.Date < DateTime.Now || e.Day.Date.Month > (DateTime.Now.Month + 3))
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
            }
            if (OneOffs != null)
            {
                foreach (DateTime checkDate in OneOffs)
                {
                    if (e.Day.Date.Year == checkDate.Year && e.Day.Date.Month == checkDate.Month && e.Day.Date.Day == checkDate.Day)
                    {
                        e.Day.IsSelectable = false;
                        e.Cell.ForeColor = System.Drawing.Color.Gray;
                    }
                }
            }
            
        }

        public void GenerateDisabledDays() //Generate additional diabled days based on selected user for whom the reservation is being created under
        {
                OneOffs = new List<DateTime>();
                loc = system.GetHours(Convert.ToInt16(ddlLocations.Text), DateTime.Now);
                foreach (AltLocHour alt in loc.AltHours)
                {
                    if (!alt.OpenToday)
                    {
                        OneOffs.Add(alt.Date);
                    }
                }

                foreach (Reservation reserve in reservationList)
                {
                    if (reserve.Email == tbUser.Text)
                    {
                        OneOffs.Add(reserve.DateTime);
                    }
                }
        }

        protected void calReservations_SelectionChanged(object sender, EventArgs e)
        {
            DateTime selected = calReservations.SelectedDate;
            TimeSpan startTime = new TimeSpan();
            TimeSpan endTime = new TimeSpan();

            loc = system.GetHours(Convert.ToInt16(ddlLocations.Text), DateTime.Now);

            startTime = loc.DefaultOpen;
            endTime = loc.DefaultClose;

            if (loc.AltHours != null)
            {
                foreach (AltLocHour alt in loc.AltHours) //Check list of alternate hours for a location
                {
                    if (alt.Date == selected) //if alternate hours were found for the selected date then modify the start and end times
                    {
                        startTime = alt.AltOpenTime;
                        endTime = alt.AltCloseTime;
                    }
                }
            }

            List<int> times = new List<int>();

            for (int i = startTime.Hours; i < endTime.Hours; i += 2)
            {
                times.Add(i);
            }

            reservationList = system.GetReservations(Convert.ToInt16(ddlLocations.Text), DateTime.Now);

            foreach (Reservation reserve in reservationList)
            {
                if (reserve.DateTime.Date == selected.Date)
                {
                    times.Remove(reserve.DateTime.Hour);
                }
            }

            ddlStartTime.DataSource = times;
            ddlStartTime.DataBind();
            labStartTime.Visible = true;
            ddlStartTime.Visible = true;
        }
    }
}