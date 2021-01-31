using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DirectorsPortal
{
    class Contact
    {
        public string contact_id { get; set; }
        public Dictionary<string, string> email_address { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string job_title { get; set; }
        public string company_name { get; set; }
        public string birthday_month { get; set; }
        public string birthday_day { get; set; }
        public string anniversary { get; set; }
        public string deleted_at { get; set; }
        public string create_source { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public List<Dictionary<string, string>> custom_fields { get; set; }
        public List<Dictionary<string, string>> phone_numbers { get; set; }
        public List<Dictionary<string, string>> street_addresses { get; set; }
        public List<string> list_memberships { get; set; }

    
    }


}
