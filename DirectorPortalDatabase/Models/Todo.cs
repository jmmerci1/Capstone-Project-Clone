using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectorPortalDatabase.Models
{
    public class Todo
    {
        /// <summary>
        /// The Primary Key of the address in the database.
        /// Autoincrements.
        /// </summary>
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        /// <summary>
        /// The title of the todo item
        /// </summary>
        [Column("title")]
        public string GStrTitle { get; set; }
        /// <summary>
        /// The description of the todo item
        /// </summary>
        [Column("description")]
        public string GStrDescription { get; set; }
        /// <summary>
        /// Whether or not the item is marked as completed
        /// </summary>
        [Column("complete")]
        public bool GBlnMarkedAsDone { get; set; }
    }
}
