using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsLicenseClass
    {
        public enum enMode { New=1,Update=2};
        public enMode Mode = enMode.New;
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }

        public clsLicenseClass()
        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;
            Mode=enMode.New;
        }
        public clsLicenseClass(int LicenseClassID,string ClassName,string ClassDescription,
            byte MinimumAllowedAge,byte DefaultValidityLength,float ClassFees)
        {
            this.LicenseClassID= LicenseClassID;
            this.ClassName= ClassName;
            this.ClassDescription= ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees= ClassFees;

            Mode = enMode.Update;
        }
        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }
       public static clsLicenseClass Find(string ClassName)
       {
            int LicenseClassID = -1;
            string ClassDescription = "";
            byte MinimumAllowedAge=18, DefaultValidityLength=10;
            float ClassFees = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID, ref ClassDescription,
                ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge,
                    DefaultValidityLength, ClassFees);
            }
            else
                return null;

       }

        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = "";
            string ClassDescription = "";
            byte MinimumAllowedAge = 18, DefaultValidityLength = 10;
            float ClassFees = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByLicenseClassID(LicenseClassID, ref ClassName, ref ClassDescription,
                ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge,
                   DefaultValidityLength, ClassFees);
            }
            else
                return null; 
        }

    }
}
