﻿using DataAccessLayer;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsLicense
    {
        public enum enMode { AddNew=0,Update=1};
        public enMode Mode = enMode.AddNew;

        public enum enIssueReason { FirstTime=1,Renew=2,DamagedReplacement=3,LostReplacement=4};
        public clsDrivers DriverInfo;
        public int LicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public int LicenseClass { set; get; }
        public clsLicenseClass LicenseClassInfo;
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
        public float PaidFees { set; get; }
        public bool IsActive { set; get; }
        public enIssueReason IssueReason { set; get; }

        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }
        public int CreatedByUserID { set; get; }
        
        public bool IsDetained
        {
            get
            {
                return clsDetainedLicense.IsLicenseDetained(this.LicenseID);
            }
        }
        public clsDetainedLicense DetainedInfo {  set; get; }

        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClass = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate=DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        
        public clsLicense(int LicenseID,int ApplicationID,int DriverID,int LicenseClass,
            DateTime IssueDate,DateTime ExpirationDate,string Notes,float PaidFees,
            bool IsActive,enIssueReason IssueReason,int CreatedByUserID)
        {
            this.LicenseID=LicenseID;
            this.ApplicationID=ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate=IssueDate;
            this.ExpirationDate=ExpirationDate;
            this.Notes = Notes;
            this.PaidFees=PaidFees;
            this.IsActive=IsActive;
            this.IssueReason=IssueReason;
            this.CreatedByUserID=CreatedByUserID;

            this.DriverInfo=clsDrivers.FindByDriverID(this.DriverID);
            this.LicenseClassInfo = clsLicenseClass.Find(this.LicenseClass);
            this.DetainedInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);

            Mode = enMode.Update;
        }
        public static bool IsLicenseExistByPersonID(int PersonID,int LicenseClassID)
        {
            return (clsLicenseData.GetActiveLicenseIDForPersonID(PersonID, LicenseClassID) != -1);
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID,int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDForPersonID(PersonID, LicenseClassID);
        }

        public static string GetIssueReasonText(enIssueReason IssueReason)
        {
            switch(IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replace For Damaged";
                case enIssueReason.LostReplacement:
                    return "Replace For Lost";
                default:
                    return "First Time";

            }
        }

        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClass,
                this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive,
                (byte)this.IssueReason, this.CreatedByUserID);

            return (this.LicenseID != -1);
        }

        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClass,
                this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive,(byte)this.IssueReason, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }else
                    {
                        return false;
                    }
                    case enMode.Update:
                    return _UpdateLicense();
            }
            return false;
        }

        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1; int DriverID = -1; int LicenseClass = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0; bool IsActive = true; int CreatedByUserID = 1;
            byte IssueReason = 1;

            if(clsLicenseData.GetLincenseInfoByID(LicenseID,ref ApplicationID,ref DriverID,ref LicenseClass,ref IssueDate,
               ref ExpirationDate,ref Notes,ref PaidFees,ref IsActive,ref IssueReason,ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate,
                    ExpirationDate, Notes, PaidFees, IsActive,(enIssueReason)IssueReason, CreatedByUserID);
            }else
            {
                return null;
            }
        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }

        public Boolean IsLicenseExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }
        public bool DeactivateCurrentLicense()
        {
            return (clsLicenseData.DeactivateLicense(this.LicenseID));
        }
        public clsLicense RenewLicense(string Notes,int CreatedByUserID)
        {
            //First Create Applicaiton
            clsApplication Application = new clsApplication();
            Application.ApplicationPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).fees;
            Application.CreatedByUserID = CreatedByUserID;

            if(!Application.Save())
            {
                return null;
            }

            clsLicense NewLicense=new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;

            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if(!NewLicense.Save())
            {
                return null;
            }

            //we need to deactivate the old License.
            DeactivateCurrentLicense();
            return NewLicense;
        }

        public clsLicense Replace(enIssueReason IssueReason,int CreatedByUserID)
        {
            //First Create Applicaiton 
            clsApplication Application = new clsApplication();
            Application.ApplicationPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (IssueReason == enIssueReason.DamagedReplacement) ?
                (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense :
                (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;

            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate=DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).fees;
            Application.CreatedByUserID = CreatedByUserID;

            if(!Application.Save())
            {
                return null;
            }

            //Second Create New License

            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;//but The Old ExpirationDate For Old License
            NewLicense.Notes=this.Notes;
            NewLicense.PaidFees = 0;// no fees for the license because it's a replacement.
            NewLicense.IsActive= true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if(!NewLicense.Save())
            {
                return null;
            }


            //we need to deactivate the old License.
            DeactivateCurrentLicense();
            return NewLicense;
        }

        public int Detain(float FineFees,int CreatedByUserID)
        {
            clsDetainedLicense detainedLicense = new clsDetainedLicense();
            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = Convert.ToSingle(FineFees);
            detainedLicense.CreatedByUserID= CreatedByUserID;

            if(!detainedLicense.Save())
            {
                return -1;
            }
            return detainedLicense.DetainID;
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID,ref int ApplicationID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicationPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate=DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).fees;
            Application.CreatedByUserID = ReleasedByUserID;

            if(!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }
            ApplicationID = Application.ApplicationID;

            return this.DetainedInfo.ReleaseDetainedLicense(ReleasedByUserID, Application.ApplicationID);
        }
    }
}
