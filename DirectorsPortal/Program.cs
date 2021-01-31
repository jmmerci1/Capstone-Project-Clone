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
            ///

            //Constant Contact Dev Account
            //Username: edwalk@svsu.edu
            //password: ayC&Aybab6sC422
            //
            // yes this is intentional, this is an accoutn we can all use for dev




            ConstantContact CC = new ConstantContact();
            CC.Authenticate();

            CC.RefreshCCData();
            Console.WriteLine(CC.mdctContacts.ToString());

        }
    }
}
