using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsTest
    {
        public enum  enMode { AddNew=0,Update=1};
        public enMode Mode=enMode.AddNew;
        public int TestID { set; get; }
        public int TestAppointmentID { set; get; }
        public clsTestAppointment TestAppointmentInfo { set; get; }
        public bool TestResult { set; get; }
        public string Notes { set; get; }
        public int CreatedByUserID { set; get; }


        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        public clsTest(int testID, int testAppointmentID,
            bool testResult, string notes, int createdByUserID)
        { 
            this.TestID = testID;
            this.TestAppointmentID = testAppointmentID;
            this.TestAppointmentInfo = clsTestAppointment.Find(testAppointmentID);
            this.TestResult = testResult;
            this.Notes = notes;
            this.CreatedByUserID = createdByUserID;

            this.Mode = enMode.Update;
        }

        public static clsTest FindLastTestPerPersonAndLicenseClass(
            int PersonID, int LicenseClassID, clsTestTypes.enTestType TestTypeID)
        {
            int TestID = -1;
            int TestAppoimentID = -1;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = -1;

            if (clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass(PersonID, LicenseClassID, (int)TestTypeID, ref TestID,
                ref TestAppoimentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppoimentID, TestResult, Notes, CreatedByUserID);
            }
            else
                return null;

        }

        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = ""; int CreatedByUserID = -1;

            if (clsTestData.GetTestInfoByID(TestID,
                ref TestAppointmentID, ref TestResult,
                ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID,
                    TestAppointmentID, TestResult,
                    Notes, CreatedByUserID);
            }
            else
                return null;
        }

        private bool _AddNewTest()
        {
            this.TestID=clsTestData.AddNewTest(this.TestAppointmentID,this.TestResult,this.Notes,this.CreatedByUserID);
            return (this.TestID != -1);
        }
        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(this.TestID,this.TestAppointmentID,this.TestResult,
                this.Notes,this.CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if(_AddNewTest())
                        {
                            Mode = enMode.Update;
                            return true;
                        }else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateTest();
                    }

            }
            return false;
        }

        public static byte GetPassTestCount(int LocalDrivingLicenseAppID)
        {
            return clsTestData.GetPassTestCount(LocalDrivingLicenseAppID);
        }

        public static bool PassAllTest(int LocalDrivingLicenseAppID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return GetPassTestCount(LocalDrivingLicenseAppID) == 3;
        }
    }
}
