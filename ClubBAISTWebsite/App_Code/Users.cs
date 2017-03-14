using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Drawing;
using System.Data.SqlClient;
using System.Data;

namespace ClubBAISTWebsite
{
    //This class deals with user specific functions
    public class Users
    {
        public static string connectionString = Database.ConnStrings.local;

        public bool AddUser(string sEmail, string sName, string sPassword, int iMembership, string sHomePhone, string sCellPhone, Image imgPicture)
        {
            bool bSuccess = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand InsertUserCommand = new SqlCommand("spAddUser", connection);
                InsertUserCommand.CommandType = CommandType.StoredProcedure;

                //Next add parameters below
                SqlParameter UserEmailParameter = new SqlParameter();
                UserEmailParameter.ParameterName = "@Email";
                UserEmailParameter.Value = sEmail;
                UserEmailParameter.Direction = ParameterDirection.Input;
                UserEmailParameter.SqlDbType = SqlDbType.NVarChar;
                UserEmailParameter.Size = 320;
                InsertUserCommand.Parameters.Add(UserEmailParameter);

                SqlParameter UserNameParameter = new SqlParameter();
                UserNameParameter.ParameterName = "@Name";
                UserNameParameter.Value = sName;
                UserNameParameter.Direction = ParameterDirection.Input;
                UserNameParameter.SqlDbType = SqlDbType.NVarChar;
                UserNameParameter.Size = 100;
                InsertUserCommand.Parameters.Add(UserNameParameter);

                SqlParameter UserPasswordParameter = new SqlParameter();
                UserPasswordParameter.ParameterName = "@Password";
                UserPasswordParameter.Value = sPassword;
                UserPasswordParameter.Direction = ParameterDirection.Input;
                UserPasswordParameter.SqlDbType = SqlDbType.NVarChar;
                UserPasswordParameter.Size = 40;
                InsertUserCommand.Parameters.Add(UserPasswordParameter);

                SqlParameter UserMembIDParameter = new SqlParameter();
                UserMembIDParameter.ParameterName = "@membershipID";
                UserMembIDParameter.Value = iMembership;
                UserMembIDParameter.Direction = ParameterDirection.Input;
                UserMembIDParameter.SqlDbType = SqlDbType.TinyInt;
                InsertUserCommand.Parameters.Add(UserMembIDParameter);

                SqlParameter UserHomePhoneParameter = new SqlParameter();
                UserHomePhoneParameter.ParameterName = "@HomePhone";
                UserHomePhoneParameter.Value = sHomePhone;
                UserHomePhoneParameter.Direction = ParameterDirection.Input;
                UserHomePhoneParameter.SqlDbType = SqlDbType.NVarChar;
                UserHomePhoneParameter.Size = 15;
                InsertUserCommand.Parameters.Add(UserHomePhoneParameter);

                SqlParameter UserCellPhoneParameter = new SqlParameter();
                UserCellPhoneParameter.ParameterName = "@CellPhone";
                UserCellPhoneParameter.Value = sCellPhone;
                UserCellPhoneParameter.Direction = ParameterDirection.Input;
                UserCellPhoneParameter.SqlDbType = SqlDbType.NVarChar;
                UserCellPhoneParameter.Size = 15;
                InsertUserCommand.Parameters.Add(UserCellPhoneParameter);

                SqlParameter UserImageParameter = new SqlParameter();
                UserImageParameter.ParameterName = "@picture";
                UserImageParameter.Value = DBNull.Value; //For now just passing it a null since picture is not implemented
                UserImageParameter.Direction = ParameterDirection.Input;
                UserImageParameter.SqlDbType = SqlDbType.Image;
                InsertUserCommand.Parameters.Add(UserImageParameter);

                SqlParameter StatusParameter = new SqlParameter();
                StatusParameter.ParameterName = "@Status";
                StatusParameter.SqlDbType = SqlDbType.Int;
                StatusParameter.Direction = ParameterDirection.ReturnValue;
                InsertUserCommand.Parameters.Add(StatusParameter);

                connection.Open();
                InsertUserCommand.ExecuteNonQuery();
                int iStatus = (int)StatusParameter.Value;
                if (iStatus == 0)
                {
                    bSuccess = true;
                }
            }

            return bSuccess;
        }

        //This function handles the SQL stored proc and returns success or fail status depending on the login
        public bool loginUser(string email, string password)
        {
            bool bSuccess = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand LoginUserCommand = new SqlCommand("spLoginUser", connection);
                LoginUserCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter UserEmailParameter = new SqlParameter();
                UserEmailParameter.ParameterName = "@email";
                UserEmailParameter.Value = email;
                UserEmailParameter.Direction = ParameterDirection.Input;
                UserEmailParameter.SqlDbType = SqlDbType.NVarChar;
                UserEmailParameter.Size = 320;
                LoginUserCommand.Parameters.Add(UserEmailParameter);

                SqlParameter UserPasswordParameter = new SqlParameter();
                UserPasswordParameter.ParameterName = "@password";
                UserPasswordParameter.Value = password;
                UserPasswordParameter.Direction = ParameterDirection.Input;
                UserPasswordParameter.SqlDbType = SqlDbType.NVarChar;
                UserPasswordParameter.Size = 40;
                LoginUserCommand.Parameters.Add(UserPasswordParameter);

                connection.Open();
                string sResult = Convert.ToString(LoginUserCommand.ExecuteScalar());

                bSuccess = sResult.Equals(email, StringComparison.Ordinal);
            }
                return bSuccess;
        }

        public UserPermission GetUser(string sCurrentUser)
        {
            UserPermission youPerm = new UserPermission();

            //This would be where the SQL stored proc is executed
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand GetUserPermCommand = new SqlCommand("spGetUserPermissions", connection);
                GetUserPermCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter UserEmailParameter = new SqlParameter();
                UserEmailParameter.ParameterName = "@Email";
                UserEmailParameter.Value = sCurrentUser;
                UserEmailParameter.Direction = ParameterDirection.Input;
                UserEmailParameter.SqlDbType = SqlDbType.NVarChar;
                UserEmailParameter.Size = 320;
                GetUserPermCommand.Parameters.Add(UserEmailParameter);

                SqlParameter StatusParameter = new SqlParameter();
                StatusParameter.ParameterName = "@Status";
                StatusParameter.SqlDbType = SqlDbType.Int;
                StatusParameter.Direction = ParameterDirection.ReturnValue;

                GetUserPermCommand.Parameters.Add(StatusParameter);

                connection.Open();
                SqlDataReader dr = GetUserPermCommand.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        youPerm.Email = dr.GetString(0);
                        youPerm.Name = dr.GetString(1);
                        try { youPerm.MembershipID = dr.GetByte(2); }
                        catch { youPerm.MembershipID = -1; }
                        try { youPerm.ApprovedFlag = dr.GetBoolean(3); }
                        catch { youPerm.ApprovedFlag = false; }
                        try { youPerm.EmployeeFlag = dr.GetBoolean(4); }
                        catch { youPerm.EmployeeFlag = false; }
                        try { youPerm.LocationID = dr.GetByte(5); }
                        catch { youPerm.LocationID = -1; }
                        try { youPerm.RestrictionStart = dr.GetTimeSpan(6); }
                        catch { youPerm.RestrictionStart = new TimeSpan(); }
                        try { youPerm.RestrictionEnd = dr.GetTimeSpan(7); }
                        catch { youPerm.RestrictionEnd = new TimeSpan(); }
                        
                    }
                }

            }
            return youPerm;
        }

        public Users()
        {

        }
    }
}