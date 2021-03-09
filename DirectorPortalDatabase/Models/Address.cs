using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    /// <summary>
    /// Represents an address for a business. 
    /// </summary>
    public class Address
    {
        /// <summary>
        /// The Primary Key of the address in the database.
        /// Autoincrements.
        /// </summary>
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        /// <summary>
        /// The actual address number and street of the address
        /// </summary>
        [Column("address")]
        public string GStrAddress { get; set; }
        /// <summary>
        /// The city portion of the address
        /// </summary>
        [Column("city")]
        public string GStrCity { get; set; }
        /// <summary>
        /// The state portion of the address
        /// </summary>
        [Column("state")]
        public string GStrState { get; set; }
        /// <summary>
        /// The zip code of the address
        /// </summary>
        [Column("zip")]
        public int GIntZipCode { get; set; }

        /// <summary>
        /// A mehtod for comparing two addresses.
        /// </summary>
        /// <param name="obj">The object to compare to this address.</param>
        /// <returns>A boolean sating if the addresses are equal.</returns>
        public override bool Equals(object obj)
        {
            Address addressToCompare = obj as Address;

            if (GStrAddress.Equals(addressToCompare.GStrAddress) &&
                GStrCity.Equals(addressToCompare.GStrCity) &&
                GStrState.Equals(addressToCompare.GStrState) &&
                GIntZipCode == addressToCompare.GIntZipCode)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// A method for generating the hash code of the address.
        /// </summary>
        /// <returns>The Addresses has code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
