using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Drawing;
using System.Data.SqlClient;
using System.Data;

namespace ClubBAISTWebsite
{
    //This class exists to redirect requests accordingly depending on the request being called by the webpage
    public class ClubBaistSystem
    {
        public bool AddUser(string sEmail, string sName, string sPassword, int iMembership, string sHomePhone, string sCellPhone, Image imgPicture)
        {
            bool bConfirmation = false;

            Users UserManager = new Users();
            bConfirmation = UserManager.AddUser(sEmail, sName, sPassword, iMembership, sHomePhone, sCellPhone, imgPicture);

            return bConfirmation;
        }

        public bool LoginUser(string email, string password)
        {
            bool bSuccess = false;
            Users UserManager = new Users();

            bSuccess = UserManager.loginUser(email, password);

            return bSuccess;
        }

        public UserPermission GetUser(string sCurrentUser)
        {
            UserPermission youPerm = new UserPermission();
            Users UserManager = new Users();

            youPerm = UserManager.GetUser(sCurrentUser);

            return youPerm;
        }

        public LocHour GetHours(short shLocationID, DateTime date)
        {
            LocHour locationHours = new LocHour();
            LocHours hoursMan = new LocHours();

            locationHours = hoursMan.GetHours(shLocationID, date);

            return locationHours;
        }

        public List<Reservation> GetReservations(short shLocationID, DateTime date)
        {
            List<Reservation> reservationList = new List<Reservation>();
            Reservations reservationHandler = new Reservations();

            reservationList = reservationHandler.GetReservations(shLocationID, date);

            return reservationList;
        }

        public bool AddReservation(Reservation res)
        {
            bool bSuccess = false;
            Reservations resSystem = new Reservations();

            bSuccess = resSystem.AddReservation(res);

            return bSuccess;
        }

        public DataTable GetReservations(string email, short locationID, DateTime startDate, DateTime endDate)
        {
            Reservations ReservationManager = new Reservations();
            DataTable ReservationsTable = ReservationManager.GetReservations(email,locationID,startDate,endDate);
            return ReservationsTable;
        }

        public bool DeleteReservation(long lReservation)
        {
            bool bSuccess = false;
            Reservations resSystem = new Reservations();

            bSuccess = resSystem.DeleteReservation(lReservation);

            return bSuccess;
        }

        public ClubBaistSystem()
        {

        }
    }
}