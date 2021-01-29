using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkDatabase
{
    class Orders
    {
        public int iD { get; set; }

        public int userID { get; set; }

        public string status { get; set; }

        public string createdAt { get; set; }

    }
}
