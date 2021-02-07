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
        /// <summary>
        /// The Primary Key of the address in the database.
        /// Autoincrements.
        /// </summary>
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        /// <summary>
        /// The ID of the person the phone number belongs to
        /// </summary>
        [Column("contactPersonId")]
        public int GIntContactPersonId { get; set; }
        /// <summary>
        /// The actual phone number
        /// </summary>
        [Column("phoneNumber")]
        public string GStrPhoneNumber { get; set; }
        /// <summary>
        /// Any notes about the phone number
        /// </summary>
        [Column("notes")]
        public string GStrNotes { get; set; }
        /// <summary>
        /// The type of phone number
        /// </summary>
        [Column("type")]
        public PhoneType GEnumPhoneType { get; set; }
    }

    public enum PhoneType
    {
        Mobile = 0,
        Office = 1,
        Fax = 2
    }
}
