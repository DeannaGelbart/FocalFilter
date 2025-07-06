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
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Navigation;

namespace FocalFilter
{
    public partial class EditList : Window
    {

        // For the clickable link to the website
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        public delegate void EditedHandler(EventArgs args);

        // Define an Event based on the above Delegate
        public event EditedHandler Edited;
        public EditList()
        {
            InitializeComponent();
            FocalFilterSettings setting = new FocalFilterSettings();
            textBoxEditList.Text = setting.ReadConfig();
        }

        /// Click Handler to validate and save the current list of sites
        /// the user has entered in the box        
        private void buttonEditList_Click(object sender, RoutedEventArgs e)
        {
            string MessageText = null; ;
            MessageBoxImage MessageStatus = MessageBoxImage.Error;



            FocalFilterSettings setting = new FocalFilterSettings();
            FocalFilterSettings.ValidationErrors results = setting.SetConfig(textBoxEditList.Text);
            if (!setting.IsSevereValidationError(results))
            {
                MessageStatus = MessageBoxImage.Information;

                setting.SaveConfig();
                Edited(e);
            }
            // Some of these messages are unnecessary and slow the user down, some others are better
            // handled in FocalFilterSettings.cs, so commenting them out here.
            //switch(results)
            //{
                //case FocalFilterSettings.ValidationErrors.Empty:
                //    MessageText =  "Please enter websites, each on a line by itself.";
                //    break;
                // No need to say anything regarding dup sites.
                //case FocalFilterSetting.ValidationErrors.DuplicateSites:
                //    MessageText = "There were one or more duplicate sites in your list. We've removed the duplicates for you.";
                //    break;
                //case FocalFilterSettings.ValidationErrors.InvalidSiteFormat:
                //    MessageText = "The format of the sites is incorrect";
                //    break;
                // case FocalFilterSettings.ValidationErrors.WildcardsUnSupported:
                //    MessageText = "Wildcards are not supported.";
                //    break;
                // No need to say anything regarding removing the http:// part of a url.
                //case FocalFilterSetting.ValidationErrors.RemovedProtocol:
                //    MessageText = "There was a protocol specifier in one of the sites that you entered. We've removed it for you.";
                //    break;
                //case FocalFilterSetting.ValidationErrors.NotProccessed:
                //case FocalFilterSetting.ValidationErrors.Successfull:                    
            //}
            if (MessageText != null)
            {
                MessageBox.Show(MessageText,"FocalFilter",
                    MessageBoxButton.OK,MessageStatus);
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //textBoxEditList.Width = e.NewSize.Width - 20;      
            //textBoxEditList.Height = e.NewSize.Height - 187;
        }

        private void textBoxEditList_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
