using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class Todo
    {
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        [Column("title")]
        public string GStrTitle { get; set; }
        [Column("description")]
        public string GStrDescription { get; set; }
        [Column("complete")]
        public bool GBlnMarkedAsDone { get; set; }
    }
}
