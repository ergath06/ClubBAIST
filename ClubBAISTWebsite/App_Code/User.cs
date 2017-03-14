using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

// The below class is a protected implementer interface for our user data model
// It allows us to create a user object using data going to or coming from the SQL database, 
// though this file doesn't concern itself with the database functions
namespace ClubBAISTWebsite
{
    //This class holds information about the user
    public class User
    {
        private string _email;
        private string _name;
        private string _password;
        private int _membershipID;
        private string _homePhone;
        private string _cellPhone;
        private Image _picture;

        //Alternate employee members are not implemented at this stage because out of scope

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public int MembershipID
        {
            get { return _membershipID; }
            set { _membershipID = value; }
        }

        public string HomePhone
        {
            get { return _homePhone; }
            set { _homePhone = value; }
        }

        public string CellPhone
        {
            get { return _cellPhone; }
            set { _cellPhone = value; }
        }

        public Image Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        //Default blank constructor so I know what is going on
        public User()
        {

        }
    }

    //Object referencing user permissions and preferences
    public class UserPermission
    {
        private string _email;
        private string _name;
        private int _membershipID;
        private bool _approvedFlag;
        private bool _employeeFlag;
        private int _locationID;
        private TimeSpan _restrictionStart;
        private TimeSpan _restrictionEnd;

        //Alternate employee members are not implemented at this stage because out of scope

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int MembershipID
        {
            get { return _membershipID; }
            set { _membershipID = value; }
        }

        public bool ApprovedFlag
        {
            get { return _approvedFlag; }
            set { _approvedFlag = value; }
        }

        public bool EmployeeFlag
        {
            get { return _employeeFlag; }
            set { _employeeFlag = value; }
        }

        public int LocationID
        {
            get { return _locationID; }
            set { _locationID = value; }
        }

        public TimeSpan RestrictionStart
        {
            get { return _restrictionStart; }
            set { _restrictionStart = value; }
        }
        public TimeSpan RestrictionEnd
        {
            get { return _restrictionEnd; }
            set { _restrictionEnd = value; }
        }

        //Default blank constructor so I know what is going on
        public UserPermission()
        {

        }
    }
}