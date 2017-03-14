using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data;

namespace ClubBAISTWebsite.App_Code
{
    public class Memberships
    {
        public static string ConnectionString = Database.ConnStrings.local;

        public DataTable GetAllMembershipTypes()
        {
            DataTable OutTable = new DataTable();

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                DataSet MembershipTypesDataSet = new DataSet("MembershipTypesDataSet");

                SqlCommand GetMembershipTypesCommand = new SqlCommand("spGetMembershipTypes", Connection);
                GetMembershipTypesCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter StatusParameter = new SqlParameter();
                StatusParameter.ParameterName = "@Status";
                StatusParameter.SqlDbType = SqlDbType.Int;
                StatusParameter.Direction = ParameterDirection.ReturnValue;

                GetMembershipTypesCommand.Parameters.Add(StatusParameter);

                SqlDataAdapter GetMembershipTypesAdapter = new SqlDataAdapter(GetMembershipTypesCommand);

                GetMembershipTypesAdapter.Fill(MembershipTypesDataSet, "Memberships");

                if (((int)StatusParameter.Value) == 0)
                {
                    foreach (DataTable ItemTable in MembershipTypesDataSet.Tables)
                    {
                        OutTable = ItemTable;
                    }

                }
            }

            return OutTable;
        }
    }
}