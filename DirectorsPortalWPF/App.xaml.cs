using System;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Windows;

namespace DirectorsPortalWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public AppDomain Domain;

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public App()
        {
            this.Domain = AppDomain.CurrentDomain;
            this.Domain.UnhandledException += new UnhandledExceptionEventHandler(HandleUncaught);
               
        }
        static void HandleUncaught(object sender, UnhandledExceptionEventArgs args)
        {
            string strFname = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\DirectorsPortaaUnhandled_Error.txt";
            Exception e = (Exception)args.ExceptionObject;
            File.AppendAllText(strFname,  "------------------START Handle the unhandled-------------------\n");
            File.AppendAllText(strFname, $"{e}\n");
            File.AppendAllText(strFname, "---------------------------------------------------------\n");
            File.AppendAllText(strFname, $"{e.InnerException}");
            File.AppendAllText(strFname,  "---------------------------------------------------------\n");
            File.AppendAllText(strFname, $"{e.InnerException.InnerException}\n");
            File.AppendAllText(strFname, "------------------DONE Handle the unhandled-------------------\n");
        }
    }
    
}
