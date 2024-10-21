using BussinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace DVLD
{
    internal static class clsGlobal
    {

        public static clsUser CurrentUser;
        public static bool RememberUsernameAndPassword(string Username,string Password)
        {

            //This Code For Writting in the Windows Registry!
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD_Program";
            string ValueName = "DVLD_User";
            string ValueData = Username + "#//#" + Password;

            try
            {
                Registry.SetValue(KeyPath, ValueName, ValueData, RegistryValueKind.String);
                //MessageBox.Show("Data Saved Successfully", "Successfully");
                return true;

            }catch(Exception ex)
            {
                MessageBox.Show($"Error :{ex.Message}");
                return false;
            }

            ////This Code For Save in The folder !
            //try
            //{
            //    //this will get the current project directory folder.
            //    string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

            //    // Define the path to the text file where you want to save the data
            //    string FilePath = CurrentDirectory + "\\data.txt";

            //    if(Username ==""&&File.Exists(FilePath))
            //    {
            //        File.Delete(FilePath);
            //        return true;
            //    }

            //    string DataToSave = Username + "#//#" + Password;

            //    //create a StreamWriter to write to the file
            //    using (StreamWriter writer = new StreamWriter(FilePath))
            //    {

            //        //write the data to file
            //        writer.WriteLine(DataToSave);
            //        return true;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //    return false;
            //}
        }

        public static bool GetStoredCredential(ref string Username,ref string Password)
        {
            //This Code For Reading from the Windows Registry!
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD_Program";
            string ValueName = "DVLD_User";

            try
            {
                string Line =Registry.GetValue(KeyPath, ValueName,null) as string;

                if (Line != null)
                {

                    string[] result = Line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                    Username = result[0];
                    Password = result[1];

                    //MessageBox.Show("Data is Found");
                    return true;
                }else
                {
                    //MessageBox.Show("Data is Not Found");
                    return false;
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }

            //// this is the old Code !.
            ////this will get the stored username and password and will return true if found and false if not found.
            //try
            //{
            //    string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

            //    string FilePath = CurrentDirectory + "\\data.txt";

            //    // Check if the file exists before attempting to read it
            //    if(File.Exists(FilePath))
            //    {
            //        //create a StreamReader to read from File.
            //        using(StreamReader reader =new StreamReader(FilePath))
            //        {

            //            //read data line by line until the end of the file
            //            string line;
            //            while((line=reader.ReadLine())!=null)
            //            {
            //                Console.WriteLine(line);// Output each line of data to the console
            //                string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

            //                Username = result[0];
            //                Password = result[1];
            //            }
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //    return false;
            //}

        }

        public static string ComputeHash(string Password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hasedBytes=sha256.ComputeHash(Encoding.UTF8.GetBytes(Password));

                return BitConverter.ToString(hasedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
