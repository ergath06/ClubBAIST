using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubBAISTWebsite
{
    public class AltLocHour //lists of these can then occur underneath a locHour object
    {
        private DateTime _date;
        private bool _openToday;
        private TimeSpan _altOpenTime;
        private TimeSpan _altCloseTime;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
        public bool OpenToday
        {
            get { return _openToday; }
            set { _openToday = value; }
        }
        public TimeSpan AltOpenTime
        {
            get { return _altOpenTime; }
            set { _altOpenTime = value; }
        }
        public TimeSpan AltCloseTime
        {
            get { return _altCloseTime; }
            set { _altCloseTime = value; }
        }

        public AltLocHour()
        {

        }
    }
}