using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
                    return dbContext.CategoryRef.Where(x => x.GIntBusinessId == GIntId).Select(b => b.GObjCategory).ToList();
                }
            }
        }

        /// <summary>
        /// Represents an array of the yearly data objects
        /// </summary>
        public List<YearlyData> GRGYearlyData
        {
            get
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    return dbContext.BusinessYearlyData.Where(x => x.GIntBusinessId == GIntId).ToList();
                }
            }
        }

        /// <summary>
        /// A method for converting the membership level from the business entity to
        /// a human readable string.
        /// </summary>
        /// <param name="strEnumVal">The membership level from the data model</param>
        /// <returns>The membership enum value of the entered string.</returns>
        public static MembershipLevel GetMemberShipEnum(string strEnumVal)
        {
            MembershipLevel enumMembershipLevel;
            strEnumVal = strEnumVal.ToLower();

            switch (strEnumVal)
            {
                case "gold":
                    enumMembershipLevel = MembershipLevel.GOLD;
                    break;

                case "silver":
                    enumMembershipLevel = MembershipLevel.SILVER;
                    break;

                case "associate":
                    enumMembershipLevel = MembershipLevel.ASSOCIATE;
                    break;

                case "individual":
                    enumMembershipLevel = MembershipLevel.INDIVIDUAL;
                    break;

                case "courtesy":
                    enumMembershipLevel = MembershipLevel.COURTESY;
                    break;

                default:
                    /* Probably shouldn't default to GOLD.
                     * The database does not currently have an option for this field to be null though.*/
                    enumMembershipLevel = MembershipLevel.GOLD;
                    break;
            }

            return enumMembershipLevel;
        }

        /// <summary>
        /// A method for converting the membership level from the business entity to
        /// a human readable string.
        /// </summary>
        /// <param name="membershipLevel">The membership level from the business entity.</param>
        /// <returns>The human readable membership string.</returns>
        public static string GetMebershipLevelString(MembershipLevel membershipLevel)
        {
            string strLevel = "";

            switch (membershipLevel)
            {
                case MembershipLevel.GOLD:
                    strLevel = "Gold";
                    break;

                case MembershipLevel.SILVER:
                    strLevel = "Silver";
                    break;

                case MembershipLevel.ASSOCIATE:
                    strLevel = "Associate";
                    break;

                case MembershipLevel.INDIVIDUAL:
                    strLevel = "Individual";
                    break;

                case MembershipLevel.COURTESY:
                    strLevel = "Courtesy";
                    break;

                default:
                    strLevel = "None";
                    break;
            }

            return strLevel;
        }
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
