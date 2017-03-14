using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace ClubBAISTWebsite
{
    public class LocHours
    {
        public static string ConnectionString = Database.ConnStrings.local;
        //functions to add or get location hours should go here

        public LocHour GetHours(short shLocationID, DateTime date)
        {
            LocHour locationHours = new LocHour();
            locationHours.LocationID = shLocationID;
            //in here I need to run stored proc and fill in my custom object locHour
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand GetHoursCommand = new SqlCommand("spGetHours", connection);
                GetHoursCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter LocationIDParameter = new SqlParameter();
                LocationIDParameter.ParameterName = "@locationID";
                LocationIDParameter.Value = shLocationID;
                LocationIDParameter.Direction = ParameterDirection.Input;
                LocationIDParameter.SqlDbType = SqlDbType.SmallInt;
                GetHoursCommand.Parameters.Add(LocationIDParameter);

                SqlParameter DateParameter = new SqlParameter();
                DateParameter.ParameterName = "@date";
                DateParameter.Value = date.ToShortDateString();
                DateParameter.Direction = ParameterDirection.Input;
                DateParameter.SqlDbType = SqlDbType.Date;
                GetHoursCommand.Parameters.Add(DateParameter);

                SqlParameter StatusParameter = new SqlParameter();
                StatusParameter.ParameterName = "@Status";
                StatusParameter.SqlDbType = SqlDbType.Int;
                StatusParameter.Direction = ParameterDirection.ReturnValue;

                GetHoursCommand.Parameters.Add(StatusParameter);

                connection.Open();
                SqlDataReader dr = GetHoursCommand.ExecuteReader();
                //int iCount = 0;
                if (dr.HasRows)
                {
                    locationHours.AltHours = new List<AltLocHour>(); //this line negates my old counter var
                    while (dr.Read())
                    {
                        locationHours.DefaultOpen = dr.GetTimeSpan(0);
                        locationHours.DefaultClose = dr.GetTimeSpan(1);
                        if (dr.FieldCount > 2) //Alt hours exist
                        {
                            //if (iCount == 0)
                            //{
                                //locationHours.AltHours = new List<AltLocHour>();
                            //}
                            AltLocHour alt = new AltLocHour();

                            alt.Date = dr.GetDateTime(2);
                            try { alt.OpenToday = dr.GetBoolean(3); }
                            catch { alt.OpenToday = false; }
                            try { alt.AltOpenTime = dr.GetTimeSpan(4); }
                            catch { alt.AltOpenTime = new TimeSpan(); }
                            try { alt.AltCloseTime = dr.GetTimeSpan(5); }
                            catch { alt.AltCloseTime = new TimeSpan(); }

                            locationHours.AltHours.Add(alt);
                        }
                        else //alt hours don't exist
                        {

                        }
                        //iCount++;
                    }
                }
            }
                return locationHours;
        }

        public LocHours()
        {

        }
    }
}