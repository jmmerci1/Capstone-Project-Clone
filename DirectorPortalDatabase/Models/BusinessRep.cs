using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DirectorPortalDatabase.Models
{
    public class BusinessRep
    {
        /// <summary>
        /// The Primary Key of the address in the database.
        /// Autoincrements.
        /// </summary>
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        /// <summary>
        /// The id of the business that the representative
        /// belongs to in the database
        /// </summary>
        [Column("businessId")]
        public int GIntBusinessId { get; set; }
        /// <summary>
        /// The representative's id in the table
        /// </summary>
        [Column("contactPersonId")]
        public int GIntContactPersonId { get; set; }
        /// <summary>
        /// Gives a reference to the business object from the database
        /// </summary>
        public Business GBusiness
        {
            get
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    return dbContext.Businesses.FirstOrDefault(x => x.GIntId == GIntBusinessId);
                }
            }
        }
        /// <summary>
        /// Gives a reference to the contact person object from the database
        /// </summary>
        public ContactPerson GContactPerson
        {
            get
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    return dbContext.ContactPeople.FirstOrDefault(x => x.GIntId == GIntBusinessId);
                }
            }
        }
    }
}
