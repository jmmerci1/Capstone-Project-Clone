using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectorsPortal
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ///Application.EnableVisualStyles();
            ///Application.SetCompatibleTextRenderingDefault(false);
            ///Application.Run(new Form1());

            ConstantContactOAuth CCO = new ConstantContactOAuth();

            CCO.LocalRoute = "http://localhost:42069/";
            CCO.AppAPIKey = "08d80131-0c76-4829-83fc-be50e14bf0b4";
            CCO.AppAPISecret = "HvdbdEaUYXhVYQcUV2XEXg";

            CCO.GetAccessToken();

            Console.Write($"\n{CCO.AccessToken}|||{CCO.RefreshToken}\n");
        }
    }
}
