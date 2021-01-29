using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkDatabase
{
    class Products
    {
        public int iD { get; set; }

        public int merchantID { get; set; }

        public string name { get; set; }

        public double price { get; set; }

        public string status { get; set; }

        public string createAt { get; set; }

    }
}
