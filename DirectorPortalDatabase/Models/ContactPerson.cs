using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        [Obsolete("This property is obsolete. Use GBusinesses instead.")]
        public virtual List<BusinessRep> GRGRepresentations { get; set; }

        /// <summary>
        /// Gives a reference to the physical address object from the database
        /// </summary>
        public List<Business> GRGBusinesses
        {
            get
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    // Select the list of BusinessRep objects and return their business property
                    return dbContext.BusinessReps.Where(x => x.GIntContactPersonId == GIntId).Select(b => b.GBusiness).ToList();
                }
            }
        }
        /// <summary>
        /// The list of emails that a person has
        /// </summary>
        public List<Email> GRGEmails
        {
            get
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    // Select the list of BusinessRep objects and return their business property
                    return dbContext.Emails.Where(x => x.GIntContactPersonId == GIntId).ToList();
                }
            }
        }
    }
}
