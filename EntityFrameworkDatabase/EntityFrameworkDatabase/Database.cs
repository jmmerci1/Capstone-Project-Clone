using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace EntityFrameworkDatabase
{
    class Database
    {
        public SQLiteConnection Connect;

        public string connectionstring { get; set; }

        string connection;

        //Accessing Database, establishing connection to the database
        public void getconnection()
        {
            connection = @"Data Source=Database.db; Version=3";
            connectionstring = connection;

        }

        //Creating a New Databas
        public Database()
        {

            if (!File.Exists("./Database.db"))
            {

                SQLiteConnection.CreateFile("Database.db");
                getconnection();

                using (SQLiteConnection con = new SQLiteConnection(connection))
                {

                   
                 
                }
            }

            else
            {
                return;
            }
        }
    }
}
