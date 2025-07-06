using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;

namespace FocalFilter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex = new Mutex(true, "{c0a76b5a-12ab-45c5-b9d9-d693faa6e7b9}");
        public App()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                string[] args =  Environment.GetCommandLineArgs();                
                foreach (string arg in args)
                {
                    // onboot command line option is used when FocalFilterHelper runs FocalFilter
                    if (arg.Contains("/onboot"))
                    {
                        Shared.Alerts.Log("Startup mode");
                        Shared.HostsFileBlocker blocker = new Shared.HostsFileBlocker();
                        blocker.Unblock();
                        Shutdown(0);
                    }
                }
                                
                //Start up the main window
                MainWindow = new MainWindow();
                MainWindow.Show();

                this.Exit += new ExitEventHandler(App_Exit);
            }
            else
            {
                MessageBoxResult message = MessageBox.Show(
                    "You already have FocalFilter running.",
                    "FocalFilter");
                Shutdown(0);
            }
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
            mutex.ReleaseMutex();
        }


    }
}
