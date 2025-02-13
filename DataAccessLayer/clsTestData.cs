﻿using StringDataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsTestData
    {

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(
            int PersonID,int LicenseClassID,int TestTypeID,ref int TestID,ref int TestAppointmentID,
            ref bool TestResult,ref string Notes,ref int CreatedByUserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query= @"SELECT  top 1 Tests.TestID, 
                Tests.TestAppointmentID, Tests.TestResult, 
			    Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
                FROM            LocalDrivingLicenseApplications INNER JOIN
                                         Tests INNER JOIN
                                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                         Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                WHERE        (Applications.ApplicantPersonID = @PersonID) 
                        AND (LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID)
                        AND ( TestAppointments.TestTypeID=@TestTypeID)
                ORDER BY Tests.TestAppointmentID DESC";

            SqlCommand command =new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    // The record was found
                    isFound = true;
                    TestID = (int)reader["TestID"];
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];
                    if (reader["Notes"] == DBNull.Value)

                        Notes = "";
                    else
                        Notes = (string)reader["Notes"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }else
                {
                    isFound = false;
                }

                reader.Close();
            }catch(Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool GetTestInfoByID(int TestID,
            ref int TestAppointmentID,ref bool TestResult,
            ref string Notes,ref int CreatedByUserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query = "select * from Tests where TestID=@TestID";
            SqlCommand command =new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    isFound = true;
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    if (reader["Notes"] == DBNull.Value)
                        Notes = "";
                    else
                    Notes = (string)reader["Notes"];

                }else
                {
                    isFound = false;
                }
                reader.Close();
            }catch(Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static int AddNewTest(int TestAppointmentID,bool TestResult,
            string Notes,int CreatedByUserID)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);
            string query = @"Insert Into Tests (TestAppointmentID,TestResult,
                                                Notes,   CreatedByUserID)
                            Values (@TestAppointmentID,@TestResult,
                                                @Notes,   @CreatedByUserID);
                            
                                UPDATE TestAppointments 
                                SET IsLocked=1 where TestAppointmentID = @TestAppointmentID;
                                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            if (Notes != "" && Notes != null)
                command.Parameters.AddWithValue("@Notes", Notes);
            else
                command.Parameters.AddWithValue("@Notes", System.DBNull.Value);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if(result !=null && int.TryParse(result.ToString(),out int insertID ))
                {
                    TestID = insertID;
                }
            }catch(Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return TestID;
        }

        public static bool UpdateTest(int TestID,int TestAppointmentID,bool TestResult,
            string Notes,int CreatedByUserID)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);

            string query = @"Update  Tests  
                            set TestAppointmentID = @TestAppointmentID,
                                TestResult=@TestResult,
                                Notes = @Notes,
                                CreatedByUserID=@CreatedByUserID
                                where TestID = @TestID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            command.Parameters.AddWithValue("@Notes", Notes);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();

            }catch(Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return (rowAffected > 0);
        }

        public static byte GetPassTestCount(int LocalDrivingLicenseAppID)
        {
            byte PassTestCount = 0;

            SqlConnection connection = new SqlConnection(clsStringDataAccessLayer.ConnectionString);
            string query = @"select PassTestCount=(COUNT(Tests.TestID))
               from TestAppointments inner join Tests on TestAppointments.TestAppointmentID=Tests.TestAppointmentID
               where TestAppointments.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseAppID and Tests.TestResult=1;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseAppID", LocalDrivingLicenseAppID);

            try
            {
                connection.Open();
                object result= command.ExecuteScalar();

                if(result!=null&&byte.TryParse(result.ToString(),out byte InsertedCount))
                {
                    PassTestCount = InsertedCount;

                }

            }catch( Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return PassTestCount;
        }
    }
}
