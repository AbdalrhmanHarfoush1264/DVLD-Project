using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsCountries
    {
        public string CountryName { get; set; }
        public int ID { get; set; }

        public clsCountries()
        {
            this.ID = -1;
            this.CountryName = "";
        }
        private clsCountries(int ID,string CountryName)
        {
            this.ID = ID;
            this.CountryName = CountryName;
        }
        public static DataTable GetAllCountries()
        {
            // Call Data Access Layer.....
            return clsCountriesDataAccess._GetAllCountries();
        }

        public static clsCountries Find(int CountryID)
        {
            string CountryName = "";
            if(clsCountriesDataAccess._GetCountryInfoByID(CountryID,ref CountryName))
            {
                return new clsCountries(CountryID,CountryName);
            }else
            {
                return null;
            }

        }
        public static clsCountries Find(string CountryName)
        {
            int ID = -1;

            if (clsCountriesDataAccess._GetCountryInfoByName(CountryName, ref ID))
                return new clsCountries(ID, CountryName);
            else
                return null;
        }

    }
}
