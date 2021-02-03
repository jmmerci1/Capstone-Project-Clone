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
        /// <summary>
        /// The Primary Key of the address in the database.
        /// Autoincrements.
        /// </summary>
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        /// <summary>
        /// The name of the contact person
        /// </summary>
        [Column("name")]
        public string GStrName { get; set; }

        /// <summary>
        /// The list of businesses that are represented by the person
        /// </summary>
        public virtual List<BusinessRep> GRGRepresentations { get; set; }
        /// <summary>
        /// The list of emails that a person has
        /// </summary>
        public virtual List<Email> GRGEmails { get; set; }
    }
}
