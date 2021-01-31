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
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        [Column("name")]
        public string GStrBusinessName { get; set; }
        [Column("established")]
        public int GIntYearEstablished { get; set; }
        [Column("level")]
        public MembershipLevel GEnumMembershipLevel { get; set; }
        [Column("mailingAddressId")]
        public int GIntMailingAddressId { get; set; }
        [Column("physicalAddressId")]
        public int GIntPhysicalAddressId { get; set; }
        [Column("city")]
        public string GStrCity { get; set; }
        [Column("state")]
        public string GStrState { get; set; }
        [Column("zip")]
        public int GIntZipCode { get; set; }
        [Column("website")]
        public string GStrWebsite { get; set; }
        [Column("notes")]
        public string GStrExtraNotes { get; set; }
        [Column("extraFields")]
        public string GStrExtraFields { get; set; }

        public virtual List<YearlyData> GRGYearlyData { get; set; }
    }

    public enum MembershipLevel
    {
        GOLD = 0,
        SILVER = 1
    }
}
