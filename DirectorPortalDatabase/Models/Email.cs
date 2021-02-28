using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class Email
    {
        /// <summary>
        /// The Primary Key of the address in the database.
        /// Autoincrements.
        /// </summary>
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        /// <summary>
        /// The ID of the person the email belongs to
        /// </summary>
        [Column("contactPersonId")]
        public int GIntContactPersonId { get; set; }
        /// <summary>
        /// The email address
        /// </summary>
        [Column("email")]
        public string GStrEmailAddress { get; set; }
    }
}
