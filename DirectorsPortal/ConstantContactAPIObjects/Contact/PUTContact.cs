using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorsPortal.ConstantContactAPIObjects.Contact
{

    public class PUTContact
    {
        public GETEmailAddress email_address { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string job_title { get; set; }
        public string company_name { get; set; }
        public int birthday_month { get; set; }
        public int birthday_day { get; set; }
        public string anniversary { get; set; }
        public string update_source { get; set; }
        public List<GETCustomField> custom_fields { get; set; }
        public List<POSTPhoneNumber> phone_numbers { get; set; }
        public List<POSTStreetAddress> street_addresses { get; set; }
        public List<string> list_memberships { get; set; }
    }

}
