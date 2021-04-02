using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorPortalDatabase.Models;
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

        /// <summary>
        /// Add a new field to the table. Doesn't technically do anything
        /// to the data currently there, just adds a new entry to the
        /// <see cref="AdditionalFields"/> table in the schema for UI purposes.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="field">The name of the field to add.</param>
        public void AddField(DatabaseContext context, string field)
        {
            List<string> currentFields = AvailableFields(context);
            if(currentFields.Select(x => x.ToLower()).Contains(field.ToLower()))
            {
                return;
            }
            context.AdditionalFields.Add(new AdditionalFields()
            {
                FieldName = field,
                TableName = GetType().Name
            });
            context.SaveChanges();
        }

        /// <summary>
        /// Remove a field from the table. Does not actually modify any of the
        /// db entries, but will remove the additionalfields attribute.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="field">Field name to delete</param>
        public void DeleteField(DatabaseContext context, string field)
        {
            context.AdditionalFields.RemoveRange(context.AdditionalFields.Where(x => x.FieldName == field).ToList());
            context.SaveChanges();
        }
    }
}
