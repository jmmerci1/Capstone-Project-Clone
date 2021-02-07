using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class YearlyData
    {
        /// <summary>
        /// The Primary Key of the address in the database.
        /// Autoincrements.
        /// </summary>
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        /// <summary>
        /// The year that the data represents
        /// </summary>
        [Column("year")]
        public int GIntYear { get; set; }
        /// <summary>
        /// The business that the data represents
        /// </summary>
        [Column("businessId")]
        public int GIntBusinessId { get; set; }
        /// <summary>
        /// Shows the amount of dues that have been
        /// paid in a term by the business.
        /// </summary>
        [Column("duesPaid")]
        public double GDblDuesPaid { get; set; }
        /// <summary>
        /// The amount of money in raffle tickets that
        /// has been returned
        /// </summary>
        [Column("raffleTicketsReturned")]
        public double GDblTicketsReturned { get; set; }
        /// <summary>
        /// The amount of credit that a business has
        /// </summary>
        [Column("credit")]
        public double GDblCredit { get; set; }
        /// <summary>
        /// The term length of the data
        /// </summary>
        [Column("terms")]
        public TermLength GEnumTermLength { get; set; }
        /// <summary>
        /// The ballot number that the business gets
        /// </summary>
        [Column("ballotNum")]
        public int GIntBallotNumber { get; set; }
        /// <summary>
        /// Will be used as a way of adding extra fields
        /// to the database. Designed to use a string encoded
        /// json object with any additional fields that can
        /// be decoded into regular C# objects.
        /// </summary>
        [Column("extraFields")]
        public string GStrExtraFields { get; set; }
    }

    /// <summary>
    /// The length of the term of "yearly" data.
    /// </summary>
    public enum TermLength
    {
        Annually = 0,
        Semiannually = 1,
        Quarterly = 2,
        Monthly = 3
    }
}
