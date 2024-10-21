using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DVLD.Global_Classes
{
    public class clsValidatoin
    {
        // Validation Email

        public static bool ValidateInteger(string Number)
        {
            var pattern = @"^[0-9]*$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);

        }
        public static bool ValidateFloat(string Number) 
        {
            var Pattern = @"^[0-9]*(?:\.[0-9]*)?$";

            var regex = new Regex(Pattern);

            return regex.IsMatch(Number);
        }

        public static bool IsNumber(string Number)
        {
            return ValidateInteger(Number)||ValidateFloat(Number);
        }
    }
}
