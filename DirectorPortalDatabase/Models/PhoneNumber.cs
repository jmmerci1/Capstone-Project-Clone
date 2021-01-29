using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class PhoneNumber
    {
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        [Column("contactPersonId")]
        public int GIntContactPersonId { get; set; }
        [Column("phoneNumber")]
        public string GStrPhoneNumber { get; set; }
        [Column("notes")]
        public string GStrNotes { get; set; }
        [Column("type")]
        public PhoneType GIntPhoneType { get; set; }
    }

    public enum PhoneType
    {
        Mobile,
        Office,
        Fax
    }
}
