using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkDatabase
{
    class Merchants
    {
        public int iD { get; set; }

        public string merchantName { get; set; }

        public int adminID { get; set; }

        public int countryCode { get; set; }

        public string createAt { get; set; }
    }
}
