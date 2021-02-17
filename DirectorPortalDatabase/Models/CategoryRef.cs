using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class CategoryRef
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
        /// The category id from the table
        /// </summary>
        [Column("categoriesID")]
        public int GIntCategoriesId { get; set; }

        public Categories GCategory
        {
            get
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    return dbContext.Categories.FirstOrDefault(x => x.GIntId == GIntCategoriesId);
                }
            }
        }
    }
}
