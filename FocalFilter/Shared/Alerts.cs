using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Shared
{
    /* The Alerts class is responsible for displaying alert dialogs to the user. */
    public class Alerts
    {
        // For fatal problems that require program shutdown
        public static void AlertBoxAndAbort(String message)
        {
            MessageBox.Show(message, "FocalFilter");
            Environment.Exit(1);
        }

        // For regular alerts to the user.
        public static void AlertBox(String message)
        {
            MessageBox.Show(message, "FocalFilter");
        }

        // For programmer logging 
        public static void Log(String message)
        {
#if DEBUG
            MessageBox.Show(message, "FocalFilter");
#endif
        }

       
        public static void NotAdmin(Exception e)
        {

            Alerts.AlertBoxAndAbort("FocalFilter was unable to change your system settings.  If you want to try " +
                                    "another site-blocking tool, see the Related Tools section on our website www.focalfilter.com.\n\n" +                                    
    "If you would like help resolving your problem with FocalFilter, contact us for technical support at info@focalfilter.com. Please provide us with the following error message:\n\n" + e.Message + "\n\n" +
    "If you are using Windows XP, see the note for Windows XP users in the Installation Instructions section of our website.");          
            
           Environment.Exit(1);
        }

    }
}
