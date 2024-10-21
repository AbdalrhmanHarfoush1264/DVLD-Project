using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsTestTypes
    {
        public enum enMode { AddNew=1,Update=2};
        public enMode Mode = enMode.AddNew;

        public enum enTestType { VisionTest=1,WrittenTest=2,StreetTest=3};

        public clsTestTypes.enTestType ID { set; get; }
        //public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Fees { get; set; }

        public clsTestTypes() 
        {
            this.ID = clsTestTypes.enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;

            Mode = enMode.AddNew;
        }
        public clsTestTypes(clsTestTypes.enTestType ID,string Title,string Description,float Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.Fees = Fees;

            Mode = enMode.Update;

        }
        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }

        //Update in code Here Add New Type...!
        private bool _AddNewTestType()
        {
            return false;
        }
        private bool _UpdateTestType()
        {
            return clsTestTypesData.UpdateTestType((int)this.ID,this.Title,this.Description,
                this.Fees);
        }
        public static clsTestTypes Find(clsTestTypes.enTestType TestTypeID)
        {
            string Title = "", Description = "";
            float Fees = 0;

            if (clsTestTypesData.GetTestTypeInfoByID((int)TestTypeID, ref Title, ref Description, ref Fees))
            {
                return new clsTestTypes(TestTypeID, Title, Description, Fees);
            }
            else
                return null;

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateTestType();
            }
            return false;
        }
    }
}
