using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class BusinessRep
    {
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        [Column("businessId")]
        public int GIntBusinessId { get; set; }
        [Column("contactPersonId")]
        public int GIntContactPersonId { get; set; }
    }
}
