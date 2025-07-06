using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FocalFilter
{
    // This class reads in settings from a file.
    class FocalFilterSettings
    {
	    private string siteListFileName = "FocalFilterSettings.bin";
        private string[] siteDeliminator = { "\r\n" , " "};
        private string siteListData = string.Empty;
        private ValidationErrors _validData = ValidationErrors.NotProccessed;
        private SimpleAES aes = new SimpleAES();

	    public FocalFilterSettings()
	    {

            // Find user's application settings folder.
            // See http://blog.kowalczyk.info/article/Getting-user-specific-application-data-directory.html
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            dir = System.IO.Path.Combine(dir, "FocalFilter");
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch (Exception ex)
                {
                    Shared.Alerts.AlertBoxAndAbort("FocalFilter could not create the " + dir + " directory to store your blocked sites list. Details:\n " + ex);
                }
            }
            siteListFileName = System.IO.Path.Combine(dir, siteListFileName);
            Shared.Alerts.Log("Filename: " + siteListFileName);

            try
            {
                if (File.Exists(siteListFileName))
                {
                    siteListData = aes.Decrypt(File.ReadAllBytes(siteListFileName));
                }
                    
            }
            catch (Exception ex)
            {
                Shared.Alerts.AlertBoxAndAbort("FocalFilter could not find the " + siteListFileName  + " file listing your blocked sites, and was unable to create one for you.  Details:\n " + ex);
                siteListData = null;
            }

            if (siteListData == null) 
                siteListData = string.Empty;
	    }
    	
    	
	    public String ReadConfig()
	    {
            return siteListData;
        }

        public List<String> ReadConfigList()
        {
            String[] splitArray = siteListData.Split( siteDeliminator, StringSplitOptions.RemoveEmptyEntries);
            List<String> hostEntries = new List<String>(splitArray);

            return hostEntries;
        }

        /// <summary>
        /// Returns whether the validation status is a blocking error and the user
        /// should correct the mistake or if we corrected it form them
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool IsSevereValidationError(ValidationErrors error)
        {
            return (error > ValidationErrors.Successfull);
        }

        public enum ValidationErrors
        {
            Empty = -3,
            DuplicateSites = -2,
            TrimmedUrl = -1, 
            Successfull = 0,
            NotProccessed = 1,
            // The following validation statuses will block the user.
            WildcardsUnSupported = 2,
            InvalidSiteFormat = 3,            
        }

        /// <summary>
        /// Validate and save the host list at one step
        /// </summary>
        /// <param name="configureData">whitespace (multiple allowed) seperated hostnames </param>
        public ValidationErrors SetConfig(String configureData)
        {
            String [] splitArray = configureData.Trim().Split(siteDeliminator, 
                StringSplitOptions.RemoveEmptyEntries);
            // List of cities we need to join
            List<String> hostEntries = new List<String>(splitArray);
                Dictionary<string, int> uniqueStore = new Dictionary<string, int>();

            List<string> finalList = new List<string>();
            _validData = ValidationErrors.Successfull;
            foreach (String currValue in hostEntries)
            {
                string parsedValue = currValue;
                if (parsedValue.Contains("://"))
                {
                    // strip the protocol
                    parsedValue = parsedValue.Substring(parsedValue.IndexOf("://") + 3);
                }
                if (parsedValue.Contains("/"))
                {
                    // strip out a / after the site name, and anything following it
                    int i = parsedValue.IndexOf("/");
                    if (i != -1)
                        parsedValue = parsedValue.Substring(0, i);                        
                }
                if (parsedValue.Contains("*"))
                {
                    _validData = ValidationErrors.WildcardsUnSupported;
                    MessageBox.Show("The name " + currValue + " is not a valid site name.  Please see our website www.focalfilter.com for instructions.",
                        "FocalFilter", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                }
                if (
                    !parsedValue.Contains(".")     // does not contains a .
                    || parsedValue.StartsWith(".") // starts with .
                    || parsedValue.EndsWith(".")   // ends with . 
                    )
                {
                    _validData = ValidationErrors.InvalidSiteFormat;
                    MessageBox.Show("The name " + currValue + " is not a valid site name.  Please see our website www.focalfilter.com for instructions.", 
                        "FocalFilter", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                }
                if (!uniqueStore.ContainsKey(parsedValue))
                {

                    uniqueStore.Add(parsedValue, 0);
                    finalList.Add(parsedValue);
                }else
                {
                    _validData = ValidationErrors.DuplicateSites;
                }
            }
            if (finalList.Count == 0 && 
                _validData == ValidationErrors.Successfull)
            {
                _validData = ValidationErrors.Empty;
            }

            siteListData = string.Join(siteDeliminator[0], finalList.ToArray());

            return _validData;
        }

    	
	    public bool SaveConfig( )
	    {
            bool successfullSave = false;

            if (_validData <= ValidationErrors.Successfull)
            {
                try
                {
                    File.WriteAllBytes(siteListFileName, aes.Encrypt(siteListData));
                    successfullSave = true;
                }
                finally
                {
                }
            }

            return successfullSave;
	    }
    	
    }

}
