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
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        [Column("year")]
        public int GIntYear { get; set; }
        [Column("businessId")]
        public int GIntBusinessId { get; set; }
        [Column("duesPaid")]
        public double GDblDuesPaid { get; set; }
        [Column("raffleTicketsReturned")]
        public double GDblTicketsReturned { get; set; }
        [Column("credit")]
        public double GDblCredit { get; set; }
        [Column("terms")]
        public TermLength GIntTermLength { get; set; }
        [Column("ballotNum")]
        public int GIntBallotNumber { get; set; }
        [Column("extraFields")]
        public string GStrExtraFields { get; set; }
    }

    public enum TermLength
    {
        Annually,
        Semiannually,
        Quarterly,
        Monthly
    }
}
