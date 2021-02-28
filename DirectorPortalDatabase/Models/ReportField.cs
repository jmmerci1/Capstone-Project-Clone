using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Models
{
    public class ReportField
    {
        [Key]
        [Column("id")]
        public int GIntId { get; set; }
        /// <summary>
        /// Foreign key to the ReportTemplate that this ReportField belongs to.
        /// </summary>
        [Column("reportTemplateId")]
        public int GIntReportTemplateId { get; set; }
        [Column("propertyName")]
        public string GStrModelPropertyName { get; set; }
    }
}
