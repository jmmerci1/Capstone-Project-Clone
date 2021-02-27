using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class ReportTemplate
    {
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        [Column("reportTemplateName")]
        public string GStrReportTemplateName { get; set; }
        [Column("modelName")]
        public string GStrModelName { get; set; }
    }
}
