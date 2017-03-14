using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubBAISTWebsite
{
    public class Reservation
    {
        private long _reservationNumber;
        private string _email;
        private DateTime _dateTime;
        private byte _players;
        private byte _carts;
        private short _locationID;

        public long ReservationNumber
        {
            get { return _reservationNumber; }
            set { _reservationNumber = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }
        public byte Players
        {
            get { return _players; }
            set { _players = value; }
        }
        public byte Carts
        {
            get { return _carts; }
            set { _carts = value; }
        }
        public short LocationID
        {
            get { return _locationID; }
            set { _locationID = value; }
        }
        public Reservation()
        {

        }
    }
}