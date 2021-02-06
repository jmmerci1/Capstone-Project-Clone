using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DirectorPortalDatabase.Models;

namespace DirectorPortalDatabase.Utility
{
    /// <summary>
    /// Utility class that allows the program to retrieve metadata about a table using only the table's model name.
    /// Also allows the program to grab a DbSet from the context when the type is not known at compile time.
    /// </summary>
    /// <remarks>
    /// Author: Patrick Ancel, 2/5/2021
    /// </remarks>
    public class ClsMetadataHelper
    {
        /// <summary>
        /// Stores metadata on a single table field and allows for retrieval of a record's field value.
        /// </summary>
        /// <remarks>
        /// Author: Patrick Ancel, 2/5/2021
        /// </remarks>
        public class ClsTableField
        {
            /// <summary>
            /// The PropertyInfo object used to instantiate the ClsTableField.
            /// </summary>
            private PropertyInfo UnderlyingProperty { get; set; }
            public Type TypeFieldType { get; private set; }
            public string StrPropertyName { get; private set; }
            public string StrHumanReadableName { get; private set; }
            public ClsTableField(PropertyInfo property)
            {
                UnderlyingProperty = property;
                TypeFieldType = property.PropertyType;
                StrPropertyName = property.Name;
                StrHumanReadableName = GetHumanReadableFieldName(property.Name);
            }
            /// <summary>
            /// Gets the value of the field from a specific record instance.
            /// </summary>
            /// <param name="recordInstance"></param>
            /// <remarks>
            /// Author: Patrick Ancel, 2/5/2021
            /// You must pass in an instance of the class which actually has the underlying property.
            /// </remarks>
            public object GetValue(object recordInstance)
            {
                return UnderlyingProperty.GetValue(recordInstance, null);
            }
        }

        /// <summary>
        /// Holds a collection of ClsTableField objects to describe a database table's metadata.
        /// </summary>
        /// <remarks>
        /// Author: Patrick Ancel, 2/5/2021
        /// </remarks>
        public class ClsDatabaseTable
        {
            /// <summary>
            /// The Type used to instantiate the ClsDatabaseTable.
            /// </summary>
            private Type TypeUnderlyingType { get; set; }
            private ClsTableField[] RGFields { get; set; }
            public int IntNumberOfFields => RGFields.Length;
            public ClsTableField GetField(int index)
            {
                return RGFields[index];
            }

            public ClsDatabaseTable(Type typeUnderlyingType)
            {
                TypeUnderlyingType = typeUnderlyingType;

                List<ClsTableField> rgFields = new List<ClsTableField>();

                // Iterates over the properties of the underlying class type.
                PropertyInfo[] rgProperties = typeUnderlyingType.GetProperties();
                foreach (PropertyInfo recordProperty in rgProperties)
                {
                    // Checks if this is a property that we care about.
                    if (recordProperty.PropertyType.IsPrimitive || recordProperty.PropertyType == typeof(string))
                    {
                        rgFields.Add(new ClsTableField(recordProperty));
                    }
                }

                RGFields = rgFields.ToArray();
            }
        }

        ///<summary>
        /// Used to get the human-readable version of a variable name.
        /// </summary>
        /// <remarks>
        /// Author: Patrick Ancel, 2/5/2021
        /// The reason for hardcoding this dictionary here is that it breaks nothing if
        /// class property names were to change, even if that means they no longer have
        /// an entry here.
        /// </remarks>
        private static Dictionary<string, string> GDictHumanReadableNames = new Dictionary<string, string>
        {
            // Table class names
            ["Address"] = "Address",
            ["ClsAddress"] = "Address",
            ["Business"] = "Business",
            ["ClsBusiness"] = "Business",
            ["BusinessRep"] = "Business Representative",
            ["ClsBusinessRep"] = "Business Representative",
            ["ContactPerson"] = "Contact Person",
            ["ClsContactPerson"] = "Contact Person",
            ["Email"] = "Email",
            ["ClsEmail"] = "Email",
            ["PhoneNumber"] = "Phone Number",
            ["ClsPhoneNumber"] = "Phone Number",
            ["Todo"] = "To-Do List Items",
            ["ClsTodo"] = "To-Do List Items",
            ["YearlyData"] = "Yearly Data",
            ["ClsYearlyData"] = "Yearly Data",
            // Address class properties
            ["GIntId"] = "ID",
            ["GStrAddress"] = "Address",
            ["GStrCity"] = "City",
            ["GStrState"] = "State",
            ["GIntZipCode"] = "ZIP Code",
            // Business class properties
            ["GStrBusinessName"] = "Business Name",
            ["GIntYearEstablished"] = "Year Established",
            ["GEnumMembershipLevel"] = "Membership Level",
            ["GIntMailingAddressId"] = "Mailing Address ID",
            ["GIntPhysicalAddressId"] = "Physical Address ID",
            ["GStrWebsite"] = "Website",
            ["GStrExtraNotes"] = "Extra Notes",
            ["GStrExtraFields"] = "Extra Fields",
            // BusinessRep class properties
            ["GIntBusinessId"] = "Business ID",
            ["GIntContactPersonId"] = "Contact Person ID",
            // ContactPerson class properties
            ["GStrName"] = "Name",
            // Email class properties
            ["GStrEmailAddress"] = "Email Address",
            // PhoneNumber class properties
            ["GStrPhoneNumber"] = "Phone Number",
            ["GStrNotes"] = "Notes",
            ["GEnumPhoneType"] = "Phone Type",
            // Todo class properties
            ["GStrTitle"] = "Title",
            ["GStrDescription"] = "Description",
            ["GBlnMarkedAsDone"] = "Marked As Done",
            // YearlyData properties
            ["GIntYear"] = "Year",
            ["GDblDuesPaid"] = "Dues Paid",
            ["GDblTicketsReturned"] = "Raffle Tickets Returned",
            ["GDblCredit"] = "Credit",
            ["GEnumTermLength"] = "Term Length",
            ["GIntBallotNumber"] = "Ballot Number"
        };

        /// <summary>
        /// Converts the name of a class or property into a human-readable form with spacing.
        /// </summary>
        /// <param name="strClassOrPropertyName"></param>
        /// <remarks>
        /// Author: Patrick Ancel, 2/5/2021
        /// Cannot yet parse names of unknown variables. Instead, for those, it returns what was passed in.
        /// </remarks>
        public static string GetHumanReadableFieldName(string strClassOrPropertyName)
        {
            // If the variable name has been parsed before, then its name can be found in the global dictionary.
            if (GDictHumanReadableNames.ContainsKey(strClassOrPropertyName))
            {
                return GDictHumanReadableNames[strClassOrPropertyName];
            }
            else
            {
                return strClassOrPropertyName;
            }
        }

        public class ClsModelInfo
        {
            public Type TypeModelType { get; private set; }
            public ClsDatabaseTable UdtTableMetaData { get; private set; }
            public string StrHumanReadableName { get; private set; }
            public ClsModelInfo (Type typeModelType)
            {
                TypeModelType = typeModelType;
                UdtTableMetaData = new ClsDatabaseTable(typeModelType);
                StrHumanReadableName = GetHumanReadableFieldName(typeModelType.Name);
            }
            /// <summary>
            /// Gets the DbSet from the context for this ClsModelInfo instance's model type. The return type is technically IQueryable of object.
            /// </summary>
            public IQueryable<object> GetContextDbSet(DatabaseContext context)
            {
                // Calls the generic method context.Set, which returns the DbSet for a generic type.
                // The call is modified to account for the fact that TypeModelType is not generic.
                IQueryable qryAllRecords = (IQueryable)context.GetType().GetMethod("Set").MakeGenericMethod(TypeModelType).Invoke(context, null);
                // Converts the IQueryable to IQueryable<object>.
                return qryAllRecords.Cast<object>();
            }
        }

        private static Dictionary<Type, ClsModelInfo> dictModels = new Dictionary<Type, ClsModelInfo>
        {
            [typeof(Address)] = new ClsModelInfo(typeof(Address)),
            [typeof(Business)] = new ClsModelInfo(typeof(Business)),
            [typeof(BusinessRep)] = new ClsModelInfo(typeof(BusinessRep)),
            [typeof(ContactPerson)] = new ClsModelInfo(typeof(ContactPerson)),
            [typeof(Email)] = new ClsModelInfo(typeof(Email)),
            [typeof(PhoneNumber)] = new ClsModelInfo(typeof(PhoneNumber)),
            [typeof(Todo)] = new ClsModelInfo(typeof(Todo)),
            [typeof(YearlyData)] = new ClsModelInfo(typeof(YearlyData))
        };

        private static Type[] rgModels =
        {
            typeof(Address),
            typeof(Business),
            typeof(BusinessRep),
            typeof(ContactPerson),
            typeof(Email),
            typeof(PhoneNumber),
            typeof(Todo),
            typeof(YearlyData)
        };

        public static int IntNumberOfModels => rgModels.Length;
        public static Type GetModelTypeByIndex(int intIndex)
        {
            return rgModels[intIndex];
        }
        public static ClsModelInfo GetModelInfo(Type typeModelType)
        {
            if (dictModels.ContainsKey(typeModelType))
            {
                return dictModels[typeModelType];
            }
            else
            {
                return null;
            }
        }

    }
}
