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
    public class clsDetainedLicenseData
    {

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);
            string query = @"select IsDetained=1 
                            from detainedLicenses 
                            where 
                            LicenseID=@LicenseID 
                            and IsReleased=0;";

            SqlCommand command=new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();
                object result=command.ExecuteScalar();
                if (result != null)
                {
                    IsDetained=Convert.ToBoolean(result);
                }

            }catch(Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                IsDetained = false;
            }
            finally
            {
                connection.Close();
            }
            return IsDetained;
        }

        public static bool GetDatainedLicenseInfoByLicenseID(int LicenseID,ref int DetainID,
            ref DateTime DetainDate,ref float FineFees,ref int CreatedByUserID,
            ref bool IsReleased,ref DateTime ReleaseDate,
            ref int ReleasedByUserID,ref int ReleaseApplicationID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);
            string query = @"SELECT top 1 * FROM DetainedLicenses WHERE LicenseID = @LiceseID order by DetainID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LiceseID", LicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader=command.ExecuteReader();
                if(reader.Read())
                {
                    isFound=true;
                    DetainID = (int)reader["DetainID"];
                    DetainDate = (DateTime)reader["DetainDate"];
                    FineFees = Convert.ToSingle(reader["FineFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsReleased = (bool)reader["IsReleased"];
                    if (reader["ReleaseDate"] ==DBNull.Value)
                    {
                        ReleaseDate = DateTime.MaxValue;
                    }else
                    {
                        ReleaseDate = (DateTime)reader["ReleaseDate"];
                    }

                    if (reader["ReleasedByUserID"]==DBNull.Value)
                    {
                        ReleasedByUserID = -1;
                    }else
                    {
                        ReleasedByUserID =(int)reader["ReleasedByUserID"];
                    }
                    if (reader["ReleaseApplicationID"] == DBNull.Value)

                        ReleaseApplicationID = -1;
                    else
                        ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                }else
                {
                    // The record was not found
                    isFound = false;
                }
                reader.Close();
            }catch(Exception ex)
            {
                // The record was not found
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static int AddNewDetainedLicense(int LicenseID,DateTime DetainDate,
            float FineFees,int CreatedByUserID)
        {
            int DetainID = -1;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);
            string query = @"INSERT INTO DetainedLicenses
                               (LicenseID,
                               DetainDate,
                               FineFees,
                               CreatedByUserID,
                               IsReleased
                               )
                            VALUES
                               (@LicenseID,
                               @DetainDate, 
                               @FineFees, 
                               @CreatedByUserID,
                               0
                             );
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if(result!=null &&int.TryParse(result.ToString(),out int InsertedID)) 
                {
                    DetainID = InsertedID;
                }
            }catch(Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return DetainID;

        }

        public static bool UpdateDetainedLicense(int DetainID,int LicenseID, DateTime DetainDate,
             float FineFees, int CreatedByUserID)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query = @"UPDATE DetainedLicenses
                              SET LicenseID = @LicenseID, 
                              DetainDate = @DetainDate, 
                              FineFees = @FineFees,
                              CreatedByUserID = @CreatedByUserID,   
                              WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@DetainID", DetainID);

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }catch(Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return (rowAffected > 0);
        }

        public static bool ReleaseDetainedLicense(int DetainID,
            int ReleasedByUserID,int ReleaseApplicationID)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate,
                              ReleasedByUserID=@ReleasedbyUserID,
                              ReleaseApplicationID = @ReleaseApplicationID   
                              WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("@ReleasedbyUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@DetainID", DetainID);

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }catch( Exception ex) 
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return (rowAffected > 0);

        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);
            string query = @"select * from detainedLicenses_View order by DetainID desc;";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader=command.ExecuteReader();
                if(reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }catch( Exception ex )
            {
                // Console.WriteLine("Error: " + ex.Message);
            }finally
            {
                connection.Close();
            }
            return dt;
        }
    }
}
