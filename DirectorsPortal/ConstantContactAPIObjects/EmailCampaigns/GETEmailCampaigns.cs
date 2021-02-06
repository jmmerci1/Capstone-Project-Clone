using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorsPortal
{
    class GETEmailCampaigns
    {
        public string campaign_id { get; set; }
        public DateTime created_at { get; set; }
        public string current_status { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public DateTime updated_at { get; set; }
    }
}
