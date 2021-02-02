using DirectorPortalDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext() : base()
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessRep> BusinessReps { get; set; }
        public DbSet<ContactPerson> ContactPeople { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<YearlyData> BusinessYearlyData { get; set; }

        /// <summary>
        /// Pulls the connection string from the App.config file,
        /// then manipulates it to remove the %APPDATA% and replace it
        /// with the value of <code>Environment.GetFolderPath(ApplicationData)</code>.
        /// This gives an easy to find folder for the database to reside in, while
        /// not being immediately accessible to any person that's directly looking
        /// for it.
        /// </summary>
        /// <returns>The connection string with the %APPDATA% replaced with the
        /// actual file path to the AppData/Roaming folder.</returns>
        private static string GetConnectionString()
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["DatabaseContext"].ConnectionString;
            string newConnectionString = strConnectionString.Replace("%APPDATA%",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Console.WriteLine(newConnectionString);
            return newConnectionString;
        }

        /// <summary>
        /// Needs a good implementation still
        /// Currently just returns a hardcoded value
        /// 
        /// Parses out the folder from the connection string to create the path to it.
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <returns></returns>
        private static string GetFolderPathFromConnectionString(string strConnectionString)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChamberOfCommerce", "DirectorsPortal");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string strConnectionString = GetConnectionString();
            Directory.CreateDirectory(GetFolderPathFromConnectionString(strConnectionString));
            optionsBuilder.UseSqlite(strConnectionString);
        }

    }
}
