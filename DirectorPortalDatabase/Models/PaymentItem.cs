﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    /// <summary>
    /// The association between a Payment and an Item on that payment.
    /// </summary>
    public class PaymentItem
    {
        /// <summary>
        /// The Primary Key of the PaymentItem in the database.
        /// Autoincrements.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The payment this item is on.
        /// </summary>
        public virtual Payment Payment { get; set; }

        /// <summary>
        /// The item added to the payment.
        /// </summary>
        public virtual Item Item { get; set; }

        /// <summary>
        /// The quantity of the item on the payment.
        /// </summary>
        public int Quantity { get; set; }
    }
}
