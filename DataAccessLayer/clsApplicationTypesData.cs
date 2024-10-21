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
    public class clsApplicationTypesData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query = "SELECT * FROM ApplicationTypes order by ApplicationTypeTitle";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader= command.ExecuteReader();

                if(reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();

            }catch(Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID,string Title,float Fees)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query = @"Update ApplicationTypes
                              set ApplicationTypeTitle=@Title,
                                  ApplicationFees=@Fees
                              where ApplicationTypeID=@ApplicationTypeID";

            SqlCommand command=new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Fees", Fees);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();
                rowsAffected=command.ExecuteNonQuery();

            }catch(Exception ex )
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);
        }

        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, ref string ApplicationTypeTitle,
            ref float ApplicationFees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query = @"SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";
            SqlCommand command=new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader= command.ExecuteReader();

                if(reader.Read())
                {
                    // the record was found
                    isFound = true;
                    ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
                }
                else
                {
                    // the record was not found
                    isFound = false;
                }
                reader.Close();

            }catch(Exception ex )
            {
                // the record was not found
                isFound = false;

            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static int AddNewApplicationType(string Title,
            float Fees)
        {
            int ApplicationTypeID = -1;

            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);
            string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@Title,@Fees)  
                            SELECT SCOPE_IDENTITY();";
            SqlCommand command=new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Fees", Fees);

            try
            {
                connection.Open();
                object result= command.ExecuteScalar();

                if(result!=null&&int.TryParse(result.ToString(),out int InsertedID))
                {
                    ApplicationTypeID = InsertedID;
                }

            }catch(Exception ex)
            {
                // Donot any thing
            }
            finally
            {
                connection.Close();
            }
            return ApplicationTypeID;
        }
    }
}
