using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubBAISTWebsite
{
    //this class will store information about default and custom hours for a given location
    public class LocHour
    {
        private short _locationID;
        private TimeSpan _defaultOpen;
        private TimeSpan _defaultClose;
        private List<AltLocHour> _altHours;

        public short LocationID
        {
            get { return _locationID; }
            set { _locationID = value; }
        }
        public TimeSpan DefaultOpen
        {
            get { return _defaultOpen; }
            set { _defaultOpen = value; }
        }
        public TimeSpan DefaultClose
        {
            get { return _defaultClose; }
            set { _defaultClose = value; }
        }

        public List<AltLocHour> AltHours //May need to modify if can't straight up set/get lists
        {
            get { return _altHours; }
            set { _altHours = value; }
        }

        public LocHour()
        {

        }
    }
}