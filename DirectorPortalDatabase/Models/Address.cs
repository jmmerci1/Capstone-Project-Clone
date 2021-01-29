using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class Address
    {
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        [Column("address")]
        public string GStrAddress { get; set; }
        [Column("city")]
        public string GStrCity { get; set; }
        [Column("state")]
        public string GStrState { get; set; }
        [Column("zip")]
        public int GIntZipCode { get; set; }
    }
}
