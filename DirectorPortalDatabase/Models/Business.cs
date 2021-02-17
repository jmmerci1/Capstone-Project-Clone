using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class Business
    {
        /// <summary>
        /// The Primary Key of the address in the database.
        /// Autoincrements.
        /// </summary>
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        /// <summary>
        /// The name of the business
        /// </summary>
        [Column("name")]
        public string GStrBusinessName { get; set; }
        /// <summary>
        /// The year that the business was established
        /// </summary>
        [Column("established")]
        public int GIntYearEstablished { get; set; }
        /// <summary>
        /// The membership level of the business
        /// </summary>
        [Column("level")]
        public MembershipLevel GEnumMembershipLevel { get; set; }
        /// <summary>
        /// The id of mailing address of the business
        /// </summary>
        [Column("mailingAddressId")]
        public int GIntMailingAddressId { get; set; }
        /// <summary>
        /// The id of physical address of the business
        /// </summary>
        [Column("physicalAddressId")]
        public int GIntPhysicalAddressId { get; set; }
        /// <summary>
        /// The business's website address
        /// </summary>
        [Column("website")]
        public string GStrWebsite { get; set; }
        /// <summary>
        /// Any additional notes about a business
        /// </summary>
        [Column("notes")]
        public string GStrExtraNotes { get; set; }
        /// <summary>
        /// Will be used as a way of adding extra fields
        /// to the database. Designed to use a string encoded
        /// json object with any additional fields that can
        /// be decoded into regular C# objects.
        /// </summary>
        [Column("extraFields")]
        public string GStrExtraFields { get; set; }
        /// <summary>
        /// Represents an array of buisness type categories
        /// </summary>
        public List<Categories> GCategories
        {
            get
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    return dbContext.CategoryRef.Where(x => x.GIntBusinessId == GIntId).Select(b => b.GCategory).ToList();
                }
            }
        }
        /// <summary>
        /// Represents an array of the yearly data objects
        /// </summary>
        public virtual List<YearlyData> GRGYearlyData { get; set; }
    }

    /// <summary>
    /// Represents the membership level of the business
    /// </summary>
    public enum MembershipLevel
    {
        GOLD = 0,
        SILVER = 1,
        ASSOCIATE = 2,
        INDIVIDUAL = 3,
        COURTESY = 4
    }
}
