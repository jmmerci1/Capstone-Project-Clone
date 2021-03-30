using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DirectorPortalDatabase.Utility
{
    /// <summary>
    /// Utility class that tables can extend from to add additional fields to them.
    /// </summary>
    public class HasExtraFields
    {
        /// <summary>
        /// Raw string form of the extra fields in the database. Not useful
        /// for any applications other than internal use. Should use <see cref="ExtraFieldData"/>,
        /// or the other functions provided.
        /// </summary>
        public string ExtraFields { get; set; }
        /// <summary>
        /// The parsed version of the extra fields.
        /// Can be used directly, but I would suggest using
        /// <see cref="SetField(string, string)"/>, <see cref="GetField(string)"/>,
        /// and <see cref="AvailableFields(DatabaseContext)"/> for usage.
        /// </summary>
        [NotMapped]
        public Dictionary<string, string> ExtraFieldData { get
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(ExtraFields);
            }
            set {
                ExtraFields = JsonConvert.SerializeObject(value);
            }
        }
        /// <summary>
        /// Returns a list of all of the available fields.
        /// Really only useful for frontend, giving a list of what
        /// the possible values are, as any value can be stored in the table,
        /// and retrieved.
        /// </summary>
        /// <param name="context">A database context object.</param>
        /// <returns>List of all available extra fields on a table</returns>
        public List<string> AvailableFields(DatabaseContext context)
        {
            string strClassName = GetType().Name;
            return context.AdditionalFields
                .Where(x => x.TableName == strClassName)
                .Select(x => x.FieldName)
                .ToList();
        }
        /// <summary>
        /// Set the value of a field
        /// </summary>
        /// <param name="field">Name of the field to set</param>
        /// <param name="value">Value to place in the field</param>
        public void SetField(string field, string value)
        {
            var data = ExtraFieldData;
            data.Add(field, value);
            ExtraFieldData = data;
        }
        /// <summary>
        /// Get the value stored in the extra field key.
        /// </summary>
        /// <param name="field">Name of the field to get the value of</param>
        /// <returns>Field value, or empty string if field doesn't exist</returns>
        public string GetField(string field)
        {
            var data = ExtraFieldData;
            if (data.Keys.Contains(field))
            {
                return data[field];
            }
            return "";
        }
    }
}
