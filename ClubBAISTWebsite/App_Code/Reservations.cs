using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace ClubBAISTWebsite
{
    public class Reservations
    {
        public static string connectionString = Database.ConnStrings.local;

        //functions to add or get reservations should go here
        public List<Reservation> GetReservations(short shLocationID, DateTime date)
        {
            List<Reservation> reservationList = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand GetReservationsCommand = new SqlCommand("spGetReservations", connection);
                GetReservationsCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter LocationIDParameter = new SqlParameter();
                LocationIDParameter.ParameterName = "@locationID";
                LocationIDParameter.Value = shLocationID;
                LocationIDParameter.Direction = ParameterDirection.Input;
                LocationIDParameter.SqlDbType = SqlDbType.SmallInt;
                GetReservationsCommand.Parameters.Add(LocationIDParameter);

                SqlParameter DateParameter = new SqlParameter();
                DateParameter.ParameterName = "@date";
                DateParameter.Value = date.ToShortDateString();
                DateParameter.Direction = ParameterDirection.Input;
                DateParameter.SqlDbType = SqlDbType.Date;
                GetReservationsCommand.Parameters.Add(DateParameter);

                SqlParameter StatusParameter = new SqlParameter();
                StatusParameter.ParameterName = "@Status";
                StatusParameter.SqlDbType = SqlDbType.Int;
                StatusParameter.Direction = ParameterDirection.ReturnValue;

                GetReservationsCommand.Parameters.Add(StatusParameter);

                connection.Open();
                SqlDataReader dr = GetReservationsCommand.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Reservation res = new Reservation();

                        res.ReservationNumber = dr.GetInt64(0);
                        res.Email = dr.GetString(1);
                        res.DateTime = dr.GetDateTime(2);
                        res.Players = dr.GetByte(3);
                        res.Carts = dr.GetByte(4);

                        reservationList.Add(res);
                    }
                }

                }
                return reservationList;
        }

        public bool AddReservation(Reservation res)
        {
            //Using reservation class, but don't use reservation number for insert stored proc because database will autogenerate that
            bool bSuccess = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand AddReservationCommand = new SqlCommand("spAddReservation", connection);
                AddReservationCommand.CommandType = CommandType.StoredProcedure;

                //Next add parameters below
                SqlParameter UserEmailParameter = new SqlParameter();
                UserEmailParameter.ParameterName = "@email";
                UserEmailParameter.Value = res.Email;
                UserEmailParameter.Direction = ParameterDirection.Input;
                UserEmailParameter.SqlDbType = SqlDbType.NVarChar;
                UserEmailParameter.Size = 320;
                AddReservationCommand.Parameters.Add(UserEmailParameter);

                SqlParameter DateTimeParameter = new SqlParameter();
                DateTimeParameter.ParameterName = "@dateAndTime";
                DateTimeParameter.Value = res.DateTime;
                DateTimeParameter.Direction = ParameterDirection.Input;
                DateTimeParameter.SqlDbType = SqlDbType.DateTime;
                AddReservationCommand.Parameters.Add(DateTimeParameter);

                SqlParameter PlayersParameter = new SqlParameter();
                PlayersParameter.ParameterName = "@players";
                PlayersParameter.Value = res.Players;
                PlayersParameter.Direction = ParameterDirection.Input;
                PlayersParameter.SqlDbType = SqlDbType.TinyInt;
                AddReservationCommand.Parameters.Add(PlayersParameter);

                SqlParameter CartsParameter = new SqlParameter();
                CartsParameter.ParameterName = "@carts";
                CartsParameter.Value = res.Carts;
                CartsParameter.Direction = ParameterDirection.Input;
                CartsParameter.SqlDbType = SqlDbType.TinyInt;
                AddReservationCommand.Parameters.Add(CartsParameter);

                SqlParameter LocationIDParameter = new SqlParameter();
                LocationIDParameter.ParameterName = "@locationID";
                LocationIDParameter.Value = res.LocationID;
                LocationIDParameter.Direction = ParameterDirection.Input;
                LocationIDParameter.SqlDbType = SqlDbType.SmallInt;
                AddReservationCommand.Parameters.Add(LocationIDParameter);

                SqlParameter StatusParameter = new SqlParameter();
                StatusParameter.ParameterName = "@Status";
                StatusParameter.SqlDbType = SqlDbType.Int;
                StatusParameter.Direction = ParameterDirection.ReturnValue;
                AddReservationCommand.Parameters.Add(StatusParameter);

                connection.Open();
                AddReservationCommand.ExecuteNonQuery();
                int iStatus = (int)StatusParameter.Value;
                if (iStatus == 0)
                {
                    bSuccess = true;
                }
            }

                return bSuccess;
        }

        public DataTable GetReservations(string email, short locationID, DateTime startDate, DateTime endDate)
        {
            DataTable OutTable = new DataTable();

            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                Connection.Open();
                DataSet ReservationsDataSet = new DataSet("ReservationsDataSet");

                SqlCommand GetReservationsCommand = new SqlCommand("spGetReservationsByUser", Connection);
                GetReservationsCommand.CommandType = CommandType.StoredProcedure;

                //Next add parameters below
                SqlParameter UserEmailParameter = new SqlParameter();
                UserEmailParameter.ParameterName = "@email";
                if(email == "")
                { UserEmailParameter.Value = DBNull.Value; }
                else
                { UserEmailParameter.Value = email; }
                UserEmailParameter.Direction = ParameterDirection.Input;
                UserEmailParameter.SqlDbType = SqlDbType.NVarChar;
                UserEmailParameter.Size = 320;
                GetReservationsCommand.Parameters.Add(UserEmailParameter);

                SqlParameter UserLocationIDParameter = new SqlParameter();
                UserLocationIDParameter.ParameterName = "@locationID";
                UserLocationIDParameter.Value = locationID;
                UserLocationIDParameter.Direction = ParameterDirection.Input;
                UserLocationIDParameter.SqlDbType = SqlDbType.SmallInt;
                GetReservationsCommand.Parameters.Add(UserLocationIDParameter);

                SqlParameter UserStartDateParameter = new SqlParameter();
                UserStartDateParameter.ParameterName = "@startDate";
                UserStartDateParameter.Value = startDate;
                UserStartDateParameter.Direction = ParameterDirection.Input;
                UserStartDateParameter.SqlDbType = SqlDbType.Date;
                GetReservationsCommand.Parameters.Add(UserStartDateParameter);

                SqlParameter UserEndDateParameter = new SqlParameter();
                UserEndDateParameter.ParameterName = "@endDate";
                UserEndDateParameter.Value = endDate;
                UserEndDateParameter.Direction = ParameterDirection.Input;
                UserEndDateParameter.SqlDbType = SqlDbType.Date;
                GetReservationsCommand.Parameters.Add(UserEndDateParameter);

                SqlParameter StatusParameter = new SqlParameter();
                StatusParameter.ParameterName = "@Status";
                StatusParameter.SqlDbType = SqlDbType.Int;
                StatusParameter.Direction = ParameterDirection.ReturnValue;
                GetReservationsCommand.Parameters.Add(StatusParameter);

                SqlDataAdapter GetReservationsAdapter = new SqlDataAdapter(GetReservationsCommand);

                GetReservationsAdapter.Fill(ReservationsDataSet, "Reservation");

                if (((int)StatusParameter.Value) == 0)
                {
                    foreach (DataTable ItemTable in ReservationsDataSet.Tables)
                    {
                        OutTable = ItemTable;
                    }

                }
            }
            return OutTable;
        }

        public bool DeleteReservation(long lReservation)
        {
            bool bSuccess = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand DeleteReservationCommand = new SqlCommand("spDeleteReservationByID", connection);
                DeleteReservationCommand.CommandType = CommandType.StoredProcedure;

                //Next add parameters below
                SqlParameter ReservationIDParameter = new SqlParameter();
                ReservationIDParameter.ParameterName = "@reservationNumber";
                ReservationIDParameter.Value = lReservation;
                ReservationIDParameter.Direction = ParameterDirection.Input;
                ReservationIDParameter.SqlDbType = SqlDbType.BigInt;
                DeleteReservationCommand.Parameters.Add(ReservationIDParameter);

                SqlParameter StatusParameter = new SqlParameter();
                StatusParameter.ParameterName = "@Status";
                StatusParameter.SqlDbType = SqlDbType.Int;
                StatusParameter.Direction = ParameterDirection.ReturnValue;
                DeleteReservationCommand.Parameters.Add(StatusParameter);

                connection.Open();
                DeleteReservationCommand.ExecuteNonQuery();
                int iStatus = (int)StatusParameter.Value;
                if (iStatus == 0)
                {
                    bSuccess = true;
                }
            }

                return bSuccess;
        } 

        public Reservations()
        {

        }
    }
}