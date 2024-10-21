using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsApplicationType
    {
        public enum enMode { AddNew=0,Update=1};
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        public string Title { get;set; }
        public float fees { get; set; }

        public clsApplicationType()
        {
            this.ID = -1;
            this.Title = "";
            this.fees = 0;

            Mode= enMode.AddNew;
        }
        public clsApplicationType(int ID,string ApplicationTypeTitle,float ApplicationFees)
        {
            this.ID=ID;
            this.Title = ApplicationTypeTitle;
            this.fees= ApplicationFees;

            Mode = enMode.Update;
        }
        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();
        }

        private bool _AddNewApplicationType()
        {
            this.ID=clsApplicationTypesData.AddNewApplicationType(this.Title,this.fees);

            return (this.ID != -1);
        }
        private bool _UpdateApplicationType()
        {
            return clsApplicationTypesData.UpdateApplicationType(this.ID, this.Title, this.fees);
        }

        public static clsApplicationType Find(int ID)
        {
            string Title = "";
            float Fees = 0;

            if (clsApplicationTypesData.GetApplicationTypeInfoByID((int)ID, ref Title, ref Fees))

                return new clsApplicationType(ID, Title, Fees);
            else
                return null;
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                   if(_AddNewApplicationType())
                   {
                        Mode = enMode.Update;
                        return true;
                   }
                   else
                   {
                        return false;
                   }

                case enMode.Update:
                    return _UpdateApplicationType();
            }
            return false;
        }

    }
}
