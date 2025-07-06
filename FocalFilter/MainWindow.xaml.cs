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

namespace FocalFilter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {  
            InitializeComponent();
            Activate();
            Focus();
            EnableOrDisableBlockButton();
            SetDefaultTimeToBlock();            
        }

        // For the clickable link to the website
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private bool EnableOrDisableBlockButton()
        { 
            // Block button is only enabled if there is at least one site in the block list.
            FocalFilterSettings setting = new FocalFilterSettings();
            return buttonBlockList.IsEnabled = (setting.ReadConfigList().Count > 0);
        }

        private EditList editList;

        private void buttonEditList_Click(object sender, RoutedEventArgs e)
        {
            editList = new EditList();
            editList.Closing += new System.ComponentModel.CancelEventHandler(editList_Closing);
            editList.Edited += new EditList.EditedHandler(editList_Edited);
            editList.Show();
            editList.Focus();
            //disabled this window until we close the edit list
            this.IsEnabled = false;
        }

        void editList_Edited(EventArgs args)
        {
            if (EnableOrDisableBlockButton())
            {
                buttonBlockList.Focus();
            }
            editList.Close();
        }

        void editList_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.IsEnabled = true;
        }

        private void doneBlocking()
        {
            DoneBlockingWindow win = new DoneBlockingWindow();
            win.Show();
            win.Focus();
            //Shared.Alerts.AlertBox("FocalFilter's time is up and FocalFilter has removed the website blocks.");
        }

        private void buttonBlockList_Click(object sender, RoutedEventArgs e)
        {
            // save number of minutes to block 
            Properties.Settings.Default.BlockTimeDropDownDefaultIndex = comboBoxBlockFor.SelectedIndex;
            Properties.Settings.Default.Save(); 

            Shared.HostsFileBlocker blocker = new Shared.HostsFileBlocker();
            this.Hide();

            //Get the user's block list
            FocalFilterSettings list = new FocalFilterSettings();
            List<String> sitelist = list.ReadConfigList();
            
            blocker.ApplyBlocks(sitelist, SelectedTime);

            doneBlocking();
        }

        // Returns the amount of time to block, in minutes
        private int SelectedTime
        {
            get
            {
                ComboBoxItem item = ((ComboBoxItem)comboBoxBlockFor.SelectedItem);
                string[] contents = item.Content.ToString().Split(' ');
                int time = int.Parse(contents[0]);
                if (contents[1].Contains("hour"))
                {
                    time = time * 60;
                }
                //Shared.Alerts.Log("SelectedTime: " + time + " minutes");
                return time;
            }
        }

        private void SetDefaultTimeToBlock()
        {
            int setting = Properties.Settings.Default.BlockTimeDropDownDefaultIndex;            
            comboBoxBlockFor.SelectedIndex = setting;
        }       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }       
    }
}
