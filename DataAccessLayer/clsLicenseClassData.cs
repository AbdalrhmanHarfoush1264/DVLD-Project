using StringDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsLicenseClassData
    {

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt =new DataTable();
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query = "select * from LicenseClasses order by ClassName";
            SqlCommand command =new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();
            }catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static bool GetLicenseClassInfoByClassName(string ClassName,ref int LicenseClassID,ref string ClassDescription,
            ref byte MinimumAllowedAge,ref byte DefaultValidityLength,ref float ClassFees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query = @"select * from LicenseClasses where ClassName=@ClassName";
            SqlCommand command =new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClassName", ClassName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    isFound= true;

                    LicenseClassID = (int)reader["LicenseClassID"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle(reader["ClassFees"]);
                }else
                {
                    isFound = false;
                }
                reader.Close();

            }catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();

            }
            return isFound;

        }

        public static bool GetLicenseClassInfoByLicenseClassID(int LicenseClassID,ref string ClassName,ref string ClassDescription,
            ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query = @"select * from LicenseClasses
                               where LicenseClassID=@LicenseClassID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;

                    ClassName = (string)reader["ClassName"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle(reader["ClassFees"]);
                }
                else
                {
                    isFound = false;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();

            }
            return isFound;
        }
    }
}
