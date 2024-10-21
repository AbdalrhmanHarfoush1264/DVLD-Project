using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsUser
    {
        public enum enMode { AddNew =0, Update = 1 };
        public enMode Mode= enMode.AddNew;
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }

        public clsUser()
        {
            this.UserID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = true;
            Mode = enMode.AddNew;
        }

        private clsUser(int UserID,int PersonID,string UserName,string Password,bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = enMode.Update;
        }
        
       
        public static DataTable GetAllUsers()
        {
            return clsUsersDataAccess._GetAllUsers();
        }
        public static clsUser FindByUserID(int UserID)
        {
            int PersonID = -1;
            bool IsActive = false;
            string Password="", UserName = "";

            bool IsFound = clsUsersDataAccess._GetUserInfoByUserID(UserID, ref PersonID, ref UserName,
                ref Password, ref IsActive);

            if(IsFound)
                return new clsUser(UserID, PersonID,UserName,Password, IsActive);
            else
                return null;
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUsersDataAccess._IsUserExistForPersonID(PersonID);
        }
        public static clsUser FindByUserNameAndPassword(string UserName,string Password)
        {
            int UserID = -1;
            int PersonID = -1;
            bool IsActive = false;

            bool IsFound = clsUsersDataAccess.GetUserInfoByUserNameAndPassword(UserName, Password,
                ref UserID, ref PersonID, ref IsActive);

            if(IsFound)
                return new clsUser(UserID,PersonID,UserName,Password, IsActive);
            else
                return null;

        }

        private bool _AddNewUser()
        {
            this.UserID=clsUsersDataAccess.AddNewUser(this.PersonID,this.UserName,this.Password,
                this.IsActive);
            return true;
        }

        private bool _UpdateUser()
        {
            return clsUsersDataAccess.UpdateUser(this.UserID, this.PersonID, this.UserName,
                this.Password, this.IsActive);
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUsersDataAccess.DeleteUser(UserID);
        }
        public bool Save()
        {
           switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enMode.Update:
                    return _UpdateUser();
                  
            }

            return false;
        }
    }
}
