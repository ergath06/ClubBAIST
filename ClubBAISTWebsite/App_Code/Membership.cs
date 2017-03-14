using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubBAISTWebsite.App_Code
{
    public class Membership
    {
        private int _membershipID;
        private string _levelTitle;
        private string _description;
        private TimeSpan _restricStart_End; //Using time span for start and end window as to when user can golf
        private Decimal _price;

        public int MembershipID
        {
            get { return _membershipID; }
            set { _membershipID = value; }
        }

        public string LevelTitle
        {
            get { return _levelTitle; }
            set { _levelTitle = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public TimeSpan RestricStart_End
        {
            get { return _restricStart_End; }
            set { _restricStart_End = value; }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public Membership()
        {

        }
    }
}