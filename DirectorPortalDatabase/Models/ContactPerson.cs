using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class ContactPerson
    {
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        [Column("name")]
        public string GStrName { get; set; }
        
        public virtual List<Email> GRGEmails { get; set; }
    }
}
