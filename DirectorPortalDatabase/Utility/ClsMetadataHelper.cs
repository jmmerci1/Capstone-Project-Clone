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
    public class ClsMetadataHelper
    {
        /// <summary>
        /// Stores metadata on a single table field and allows for retrieval of a record's field value.
        /// </summary>
        public class ClsTableField : ClsFieldHelper.IDataField
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
            /// You must pass in an instance of the class which actually has the underlying property.
            /// </remarks>
            public object GetValue(object objRecordInstance)
            {
                return UnderlyingProperty.GetValue(objRecordInstance, null);
            }
        }

        /// <summary>
        /// Holds a collection of ClsTableField objects to describe a database table's metadata.
        /// </summary>
        public class ClsDatabaseTable
        {
            /// <summary>
            /// The Type used to instantiate the ClsDatabaseTable.
            /// </summary>
            private Type TypeUnderlyingType { get; set; }
            private ClsFieldHelper.IDataField[] RGFields { get; set; }
            public int IntNumberOfFields => RGFields.Length;
            
            /// <summary>
            /// Gets an object that provides metadata on one of this table's columns.
            /// </summary>
            /// <param name="intIndex"></param>
            /// <returns></returns>
            public ClsFieldHelper.IDataField GetField(int intIndex)
            {
                return RGFields[intIndex];
            }

            public ClsDatabaseTable(Type typeUnderlyingType)
            {
                TypeUnderlyingType = typeUnderlyingType;

                List<ClsFieldHelper.IDataField> rgFields = new List<ClsFieldHelper.IDataField>();

                // Iterates over the properties of the underlying class type.
                PropertyInfo[] rgProperties = typeUnderlyingType.GetProperties();
                foreach (PropertyInfo recordProperty in rgProperties)
                {
                    // Checks if this is a property type that we care about.
                    if (recordProperty.PropertyType.IsPrimitive
                        || recordProperty.PropertyType == typeof(string)
                        || recordProperty.PropertyType.IsEnum
                        || recordProperty.PropertyType == typeof(DateTime)
                        || recordProperty.PropertyType == typeof(decimal))
                    {
                        // Checks if this property is listed as one not to display.
                        if (!(GDictPropertiesToAvoidByModelType.ContainsKey(TypeUnderlyingType) 
                            && GDictPropertiesToAvoidByModelType[TypeUnderlyingType].Contains(recordProperty.Name)))
                        {
                            rgFields.Add(new ClsTableField(recordProperty));
                        }
                    }
                }

                string strModelName = TypeUnderlyingType.Name;
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    List<ClsFieldHelper.IDataField> rgExtraFields = dbContext.AdditionalFields
                        // Queries for all additional fields pointing to the underlying model.
                        .Where(udtField => udtField.TableName == strModelName)
                        // Stores the field names in objects that implement the ClsFieldHelper.IDataField interface.
                        .Select(udtField => (ClsFieldHelper.IDataField)new ClsFieldHelper.ClsCustomField(udtField.FieldName)).ToList();

                    rgFields.AddRange(rgExtraFields);
                }

                RGFields = rgFields.ToArray();
            }
        }

        ///<summary>
        /// Used to get the human-readable version of a variable name.
        /// </summary>
        private static Dictionary<string, string> GDictHumanReadableNames = new Dictionary<string, string>
        {
            // Table class names
            ["Categories"] = "Category",
            ["BusinessRep"] = "Business Representative",
            ["Todo"] = "To-Do List Items",
            // Address class properties
            ["ZipCodeExt"] = "Extended Zip Code",
            // PhoneNumber class properties
            ["GEnumPhoneType"] = "Phone Type"
        };

        /// <summary>
        /// Used to get an array of the names of properties to avoid displaying to the user.
        /// The key is the model type, and the value is the array of names.
        /// </summary>
        private static Dictionary<Type, string[]> GDictPropertiesToAvoidByModelType = new Dictionary<Type, string[]>
        {
            [typeof(Address)] = new string[] { "Id" },
            [typeof(Business)] = new string[] { "Id", "MailingAddressId", "PhysicalAddressId", "ExtraFields" },
            [typeof(Categories)] = new string[] { "Id" },
            [typeof(ContactPerson)] = new string[] { "Id" },
            [typeof(Email)] = new string[] { "Id", "ContactPersonId" },
            [typeof(EmailGroup)] = new string[] { "Id" },
            [typeof(Payment)] = new string[] { "Id" },
            [typeof(PaymentItem)] = new string[] { "Id" },
            [typeof(PhoneNumber)] = new string[] { "Id", "ContactPersonId" },
            [typeof(YearlyData)] = new string[] { "Id", "BusinessId", "ExtraFields" }
        };

        /// <summary>
        /// Converts the name of a class or property into a human-readable form with spacing.
        /// </summary>
        /// <param name="strClassOrPropertyName"></param>
        /// <remarks>
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
                // Adds a space before every capital letter and then removes the leading space if there is one.
                return System.Text.RegularExpressions.Regex.Replace(strClassOrPropertyName, "[A-Z]", " $0").TrimStart();
            }
        }

        public class ClsModelInfo
        {
            public Type TypeModelType { get; private set; }
            public ClsDatabaseTable UdtTableMetaData { get; private set; }
            public string StrHumanReadableName { get; private set; }
            public ClsModelInfo(Type typeModelType)
            {
                TypeModelType = typeModelType;
                UdtTableMetaData = new ClsDatabaseTable(typeModelType);
                StrHumanReadableName = GetHumanReadableFieldName(typeModelType.Name);
            }
            /// <summary>
            /// Gets the DbSet from the context for this ClsModelInfo instance's model type. The return type is technically IQueryable of object.
            /// </summary>
            public IQueryable<object> GetContextDbSet(DatabaseContext dbContext)
            {
                // Calls the generic method dbContext.Set, which returns the DbSet for a generic type.
                // The call is modified to account for the fact that TypeModelType is not generic.
                IQueryable qryAllRecords = (IQueryable)dbContext.GetType().GetMethod("Set").MakeGenericMethod(TypeModelType).Invoke(dbContext, null);
                // Converts the IQueryable to IQueryable<object>.
                return qryAllRecords.Cast<object>();
            }

            /// <summary>
            /// For ClsModelInfo instances, returns whether the model types are the same. For all other types, returns false.
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object objOther)
            {
                Type typeOtherType = objOther.GetType();
                if (typeOtherType == typeof(ClsModelInfo))
                {
                    ClsModelInfo udtOther = (ClsModelInfo)objOther;
                    return this.TypeModelType == udtOther.TypeModelType;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Maps model types to ClsModelInfo instances that describe them.
        /// </summary>
        private static Dictionary<Type, ClsModelInfo> GDictModels = new Dictionary<Type, ClsModelInfo>
        {
            [typeof(Address)] = new ClsModelInfo(typeof(Address)),
            [typeof(Business)] = new ClsModelInfo(typeof(Business)),
            [typeof(BusinessRep)] = new ClsModelInfo(typeof(BusinessRep)),
            [typeof(Categories)] = new ClsModelInfo(typeof(Categories)),
            [typeof(ContactPerson)] = new ClsModelInfo(typeof(ContactPerson)),
            [typeof(Email)] = new ClsModelInfo(typeof(Email)),
            [typeof(EmailGroup)] = new ClsModelInfo(typeof(EmailGroup)),
            [typeof(Payment)] = new ClsModelInfo(typeof(Payment)),
            [typeof(PaymentItem)] = new ClsModelInfo(typeof(PaymentItem)),
            [typeof(PhoneNumber)] = new ClsModelInfo(typeof(PhoneNumber)),
            [typeof(YearlyData)] = new ClsModelInfo(typeof(YearlyData))
        };

        /// <summary>
        /// Updates the GDictModels dictionary entries for models with extra fields.
        /// This allows the report generator to respond to new fields without restarting the program.
        /// </summary>
        public static void RefreshModelInfo()
        {
            // Gets only the model types with extra fields.
            Type[] rgModelsWithExtraFields = GDictModels.Keys
                .Where(typeModelType => typeModelType.IsSubclassOf(typeof(HasExtraFields))).ToArray();

            foreach (Type typeModelType in rgModelsWithExtraFields)
            {
                if (typeModelType.IsSubclassOf(typeof(HasExtraFields)))
                {
                    GDictModels[typeModelType] = new ClsModelInfo(typeModelType);
                }
            }
        }

        private static Type[] GRGModels =
        {
            typeof(Address),
            typeof(Business),
            //typeof(BusinessRep),
            typeof(Categories),
            typeof(ContactPerson),
            typeof(Email),
            typeof(EmailGroup),
            typeof(Payment),
            typeof(PaymentItem),
            typeof(PhoneNumber),
            typeof(YearlyData)
        };

        /// <summary>
        /// Maps the names of models to their types.
        /// </summary>
        private static Dictionary<string, Type> GDictTypeByName = new Dictionary<string, Type>
        {
            [typeof(Address).Name] = typeof(Address),
            [typeof(Business).Name] = typeof(Business),
            [typeof(BusinessRep).Name] = typeof(BusinessRep),
            [typeof(Categories).Name] = typeof(Categories),
            [typeof(ContactPerson).Name] = typeof(ContactPerson),
            [typeof(Email).Name] = typeof(Email),
            [typeof(EmailGroup).Name] = typeof(EmailGroup),
            [typeof(Payment).Name] = typeof(Payment),
            [typeof(PaymentItem).Name] = typeof(PaymentItem),
            [typeof(PhoneNumber).Name] = typeof(PhoneNumber),
            [typeof(YearlyData).Name] = typeof(YearlyData)
        };

        /// <summary>
        /// Maps model types to their corresponding enums.
        /// </summary>
        private static Dictionary<Type, ClsJoinHelper.EnumTable> GDictEnumTableByType = new Dictionary<Type, ClsJoinHelper.EnumTable>
        {
            [typeof(Address)] = ClsJoinHelper.EnumTable.Address,
            [typeof(Business)] = ClsJoinHelper.EnumTable.Business,
            [typeof(BusinessRep)] = ClsJoinHelper.EnumTable.BusinessRep,
            [typeof(Categories)] = ClsJoinHelper.EnumTable.Categories,
            [typeof(ContactPerson)] = ClsJoinHelper.EnumTable.ContactPerson,
            [typeof(Email)] = ClsJoinHelper.EnumTable.Email,
            [typeof(EmailGroup)] = ClsJoinHelper.EnumTable.EmailGroup,
            [typeof(Payment)] = ClsJoinHelper.EnumTable.Payment,
            [typeof(PaymentItem)] = ClsJoinHelper.EnumTable.PaymentItem,
            [typeof(PhoneNumber)] = ClsJoinHelper.EnumTable.PhoneNumber,
            [typeof(YearlyData)] = ClsJoinHelper.EnumTable.YearlyData
        };

        public static int IntNumberOfModels => GRGModels.Length;
        /// <summary>
        /// Used to access the array of model types.
        /// </summary>
        /// <param name="intIndex"></param>
        /// <returns></returns>
        public static Type GetModelTypeByIndex(int intIndex)
        {
            return GRGModels[intIndex];
        }

        /// <summary>
        /// Returns an object that provides metadata pertaining to the model type that is passed in.
        /// </summary>
        /// <param name="typeModelType"></param>
        /// <returns></returns>
        public static ClsModelInfo GetModelInfo(Type typeModelType)
        {
            if (GDictModels.ContainsKey(typeModelType))
            {
                return GDictModels[typeModelType];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a model type into an enum value that is used by the ClsJoinHelper utility class.
        /// </summary>
        /// <param name="typeModelType"></param>
        /// <returns></returns>
        public static ClsJoinHelper.EnumTable GetEnumTable(Type typeModelType)
        {
            if (GDictEnumTableByType.ContainsKey(typeModelType))
            {
                return GDictEnumTableByType[typeModelType];
            }
            else
            {
                throw new ArgumentException("This Type does not have an EnumTable.");
            }
        }

        /// <summary>
        /// Returns an object that provides metadata pertaining to the model type whose name matches the string passed in.
        /// </summary>
        /// <param name="strModelName"></param>
        /// <returns></returns>
        public static ClsModelInfo GetModelInfoByName(string strModelName)
        {
            if (GDictTypeByName.ContainsKey(strModelName))
            {
                Type typeModelType = GDictTypeByName[strModelName];
                return GetModelInfo(typeModelType);
            }
            else
            {
                return null;
            }
        }
    }
}
