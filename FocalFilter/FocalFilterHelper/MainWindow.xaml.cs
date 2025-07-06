using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Security.Principal;

namespace FocalFilterHelper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

            //Shared.Alerts.AlertBox("Test");

            Shared.HostsFileBlocker blocker = new Shared.HostsFileBlocker();

            try
            {
                if (blocker.FoundBlocks())
                {
                    RemoveBlocks();
                }
                else
                {
                    Shared.Alerts.Log("No blocks found in FFH");
                }
            }
            catch (Exception e)
            {
                Shared.Alerts.AlertBox("FocalFilter ran into a problem:\n" + e);
            }

            Application.Current.Shutdown(0);
        }

        // This calls FocalFilter with a special command line arg which tells it to remove the blocks.
        private void RemoveBlocks()
        {
            
            string alert = @"FocalFilter has found old website blocks which are still active and it is going to remove them.";

            // Message for non-admins on XP
            if (!OSNewerThanXP() && !IsAdministrator())
            {
                alert = alert + " You will now be asked to\nlet it run as an administrator to remove the blocks\nYou must choose an administrator user or the blocks won't be removed.";
            }
            // If user is an administrator on XP, the OS will not ask them for permission.   
            else if (OSNewerThanXP())
            {
                alert = alert + " You will now be asked to give it permission to remove the blocks.";
            }

            Shared.Alerts.AlertBox(alert);

            string exePath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\FocalFilter.exe";
            Shared.Alerts.Log("Going to run: " + exePath);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = exePath;
            info.UseShellExecute = true;

            // Don't use RunAs for admin users on Windows versions that
            // don't have UAC, it will give them an unnecessary dialog.
            if (!OSNewerThanXP() && IsAdministrator())                
            {
                Shared.Alerts.Log("Not using RunAS: not needed");
            }
            else
            {
                info.Verb = "runas"; // Provides Run as Administrator
            }
            info.Arguments = "/onboot";
            try
            {
                if (Process.Start(info) == null)
                {
                    Shared.Alerts.AlertBoxAndAbort("FocalFilter is unable to run " + exePath);
                }
            }
            catch (Exception ex)
            {
                Shared.Alerts.AlertBoxAndAbort("FocalFilter was unable to run " + exePath + "\nError: " + ex.ToString());
            }
        }


        public bool OSNewerThanXP()
        {
            return Environment.OSVersion.Version.Major >= 6;
        }

        // From http://stackoverflow.com/questions/3925065/correct-way-to-deal-with-uac-in-c/3925167#3925167
        public bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (null != identity)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }
    }
}