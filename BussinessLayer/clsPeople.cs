using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsPeople
    {
        public enum enMode { AddNew=1,Update=2};
        public enMode Mode = enMode.AddNew; 
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName;}
        }
        public string NationalNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public short Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set;}
        private string _ImagePath;
        public string ImagePath
        {
            get { return _ImagePath; }
            set {_ImagePath = value; }
        }
        public clsCountries CountryInfo;
        

        public clsPeople()
        {
            this.PersonID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.NationalNo = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";

            Mode = enMode.AddNew;
        }
        private clsPeople( int personID, string firstName, string secondName, string thirdName, string lastName,
            string nationalNo, DateTime dateOfBirth, short gendor, string address, string phone, string email,
            int nationalityCountryID, string imagePath)
        {
            
            this.PersonID = personID;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;

            this.NationalNo = nationalNo;
            this.DateOfBirth = dateOfBirth;
            this.Gendor = gendor;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.NationalityCountryID = nationalityCountryID;
            this.ImagePath = imagePath;
            
            Mode= enMode.Update;
        }

        public static DataTable GetAllPeople()
        {
            //Call People DataAccess Layer
            return clsPeopleDataAccess._GetAllPeople();
        }
        public static bool IsNationalNoExist(string NationalNo)
        {
            //Call People DataAccess Layer
            return clsPeopleDataAccess._IsNationalExist(NationalNo);
        }
        public static clsPeople Find(int PersonID)
        {
            string FirstName="", SecondName="", ThirdName="", LastName="", NationalNo="", Address="",
                   Phone="", Email="", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = 0;
            int CountryIndex = -1;

            if (clsPeopleDataAccess._GetPersonInfoByID(PersonID, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref NationalNo, ref DateOfBirth, ref Gendor, ref Phone, ref Email, ref CountryIndex,
                ref Address, ref ImagePath))
            {
                return new clsPeople(PersonID, FirstName, SecondName, ThirdName, LastName, NationalNo, DateOfBirth,
                    Gendor, Address, Phone, Email, CountryIndex, ImagePath);
            }
            else
                return null;

        }
        public static clsPeople Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "",
                   Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = 0;
            int CountryIndex = -1;
            int PersonID = -1;

            if (clsPeopleDataAccess._GetPersonInfoByNationalNo(NationalNo,ref PersonID, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref DateOfBirth, ref Gendor, ref Phone, ref Email, ref CountryIndex,
                ref Address, ref ImagePath))
            {
                return new clsPeople(PersonID, FirstName, SecondName, ThirdName, LastName, NationalNo, DateOfBirth,
                    Gendor, Address, Phone, Email, CountryIndex, ImagePath);
            }
            else
                return null;
        }
        private bool _AddNewPerson()
        {
            // Call People DataAccess.....
            this.PersonID = clsPeopleDataAccess._AddNewPerson(this.FirstName, this.SecondName, this.ThirdName, this.LastName,
                this.NationalNo, this.DateOfBirth, this.Gendor, this.Phone, this.Email, this.NationalityCountryID, this.Address, this.ImagePath);

            return (this.PersonID != -1);
        }
        private bool _UpdatePerson()
        {
            return (clsPeopleDataAccess._UpdatePerson(this.PersonID, this.FirstName, this.SecondName,this.ThirdName,
                this.LastName, this.NationalNo,this.DateOfBirth,this.Gendor, this.Phone,this.Email,
                this.NationalityCountryID,this.Address, this.ImagePath));
        }
        public static bool _DeletePerson(int PersonID)
        {
            return (clsPeopleDataAccess._DeletePerson(PersonID));
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdatePerson();
                
            }
            return false;
        }
    }
}
