using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorsPortal
{
    public class List
    {
        public string list_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool favorite { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int membership_count { get; set; }
    }

    public class Next
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Next next { get; set; }
    }

    public class GETContactList
    {
        public List<List> lists { get; set; }
        public int lists_count { get; set; }
        public Links _links { get; set; }
    }
}