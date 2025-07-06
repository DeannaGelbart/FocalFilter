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
using System.Threading;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Shared
{
    // This class blocks and unblocks sites by manipulating the hosts file.
    public class HostsFileBlocker
    {

        // sites: List of sites to block.
        // totalMinutes: Total number of minutes to block the web for.        
        public void ApplyBlocks(List<String> sites, int totalMinutes)
        {
            if (totalMinutes <= 0)
            {  
                throw new Exception("Invalid number of minutes to block for");
            }  

            Block(sites);

            int totalMilliseconds = totalMinutes * 60 * 1000;
            Thread.Sleep(totalMilliseconds);
            Alerts.Log("About to unblock");
  
            Unblock();
        }

        // Block sites by appending a bogus entry for each site to the hosts file.
        public void Block(List<String> sites)
        {

            bool needNewline = false;
            try
            {
                string text = File.ReadAllText(HostsFilename());

                // If file does not end with newline
                if (!text.EndsWith(Environment.NewLine))
                    needNewline = true;
            }
            catch (Exception e)
            {
                Alerts.AlertBoxAndAbort("FocalFilter error: Unable to update file in Block.  This might happen if you have another site-blocking tool like Cold Turkey currently blocking sites.  If you need help, you can contact us at info@focalfilter.com.  If you contact us, please mention that the problem is 'Unable to update file in Block' and that you got the following error message: " + e.Message);
            }
            
            try
            {
                StreamWriter SW;
                SW = File.AppendText(HostsFilename());

                // Add a newline if needed to make sure our first entry starts on a new line 
                if (needNewline)
                    SW.WriteLine("");

                foreach (String site in sites)
                {
                    Alerts.Log("Blocking " + site);
                    // Add bogus entry to file with a tag so we recognize it as ours upon cleanup.
                    SW.WriteLine("127.0.0.1 " + site + OurTag());
                    // We'll also prepend "www." in case the user overlooked that.
                    SW.WriteLine("127.0.0.1 www." + site + OurTag());
                }
                SW.Close();
            }
            catch (Exception e)
            {
                Alerts.NotAdmin(e); 
            }

        }

        // Check if there are blocks in the hosts file.  We use this to see if we need to cleanup on startup.
        public bool FoundBlocks()
        {
            bool foundBlocks = false;
            try
            {
                string[] file = File.ReadAllLines(HostsFilename());
                foreach (string line in file)
                {
                    if (line.Contains(OurTag()))
                       foundBlocks = true;
                }
            }
            catch (Exception e)
            {
                Alerts.AlertBoxAndAbort("FocalFilter error: Unable to check for blocks.\nReason: " + e.Message);
            }
            return foundBlocks;
        }

        // Unblock sites by removing any entries in the hosts file that are tagged as from this tool.
        public void Unblock()
        {
            StringBuilder newfile = new StringBuilder();
            bool changingFile = false;

            try
            {
                string[] file = File.ReadAllLines(HostsFilename());

                foreach (string line in file)
                {
                    if (line.Contains(OurTag()))
                    {
                        Alerts.Log("Removing line: " + line);

                        changingFile = true;
                    }
                    else
                    {
                        //Alerts.Log("Appending line: " + line);
                        newfile.Append(line + "\r\n");
                    }
                }

                // Only update the file if we have a change to make
                if (changingFile)
                {
                    Alerts.Log("Writing to file in Unblock");
                    try
                    {
                        File.WriteAllText(HostsFilename(), newfile.ToString());
                    }
                    catch (Exception e)
                    {
                        Alerts.NotAdmin(e); 
                    }
                }
            }
            catch (Exception e)
            {
                Alerts.AlertBoxAndAbort("FocalFilter error: Unable to update file in Unblock.\nReason: " + e.Message);
            }

        }

        private string HostsFilename()
        {
            string windir = System.Environment.GetEnvironmentVariable("windir"); 
            if ((windir == null) || (windir == ""))
                Alerts.AlertBoxAndAbort("FocalFilter error: Unable to locate Windows directory");

            return windir + "\\system32\\drivers\\etc\\hosts";
        }

        // String we use to tag lines in the host file that are from us.
        private string OurTag()
        {
            return " # FocalFilter - Leave This Comment Here So FocalFilter Can Remove Block";
            //return " # FocalFilter";
        }
    }
}
