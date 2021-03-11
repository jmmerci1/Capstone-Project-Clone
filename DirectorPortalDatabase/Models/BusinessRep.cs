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
    }
}
