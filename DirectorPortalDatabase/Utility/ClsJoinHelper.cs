using DirectorPortalDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPortalDatabase.Utility
{
    public class ClsJoinHelper
    {

        //public class ClsPrimaryKey
        //{
        //    /// <summary>
        //    /// Represents the table in which the PK is stored.
        //    /// </summary>
        //    public ClsMetadataHelper.ClsModelInfo UdtTable { get; private set; }
        //    /// <summary>
        //    /// The value of the PK.
        //    /// </summary>
        //    public int IntValue { get; private set; }

        //    public ClsPrimaryKey (ClsMetadataHelper.ClsModelInfo udtTable, int intValue)
        //    {
        //        UdtTable = udtTable;
        //        IntValue = intValue;
        //    }
        //}

        //public class ClsForeignKey
        //{
        //    /// <summary>
        //    /// Represents the table in which the FK is stored.
        //    /// </summary>
        //    public ClsMetadataHelper.ClsModelInfo UdtSourceTable { get; private set; }
        //    /// <summary>
        //    /// Represents the table that the FK points to.
        //    /// </summary>
        //    public ClsMetadataHelper.ClsModelInfo UdtReferencedTable { get; private set; }

        //    /// <summary>
        //    /// The value of the FK.
        //    /// </summary>
        //    public int IntValue { get; private set; }

        //    public ClsForeignKey (ClsMetadataHelper.ClsModelInfo udtSourceTable, ClsMetadataHelper.ClsModelInfo udtReferencedTable, int intValue)
        //    {
        //        UdtSourceTable = udtSourceTable;
        //        UdtReferencedTable = udtReferencedTable;
        //        IntValue = intValue;
        //    }
        //}

        ///// <summary>
        ///// The purpose of this class is to facilitate report generation.
        ///// Each instance represents some of the data contained in either a single database record or a joining of multiple database records.
        ///// The data fields are separated into two parts: the data fields that are desired for the final report (herein, the report data),
        ///// and the fields (i.e., primary and foreign keys) that are used to join the records together (herein, the join data). 
        ///// </summary>
        ///// <remarks>
        ///// Author: Patrick Ancel, 3/28/2021
        ///// </remarks>
        //public class ClsRecordData
        //{
        //    /// <summary>
        //    /// Holds only the data that will be included in the final report.
        //    /// </summary>
        //    private Dictionary<ClsMetadataHelper.ClsTableField, string> dictReportData;
        //    public List<ClsPrimaryKey> rgPrimaryKeys;
        //    public List<ClsForeignKey> rgForeignKeys;


        //    /// <summary>
        //    /// Gets the value of the primary key of a record selected from a single table.
        //    /// </summary>
        //    /// <param name="objRecordInstance"></param>
        //    /// <param name="udtTableMetadata"></param>
        //    /// <returns></returns>
        //    private static int GetPrimaryKey(object objRecordInstance, ClsMetadataHelper.ClsModelInfo udtTableMetadata)
        //    {
        //        if (udtTableMetadata.TypeModelType == typeof(Address))
        //        {
        //            Address udtAddress = (Address)objRecordInstance;
        //            return udtAddress.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(Business))
        //        {
        //            Business udtBusiness = (Business)objRecordInstance;
        //            return udtBusiness.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(BusinessRep))
        //        {
        //            BusinessRep udtBusinessRep = (BusinessRep)objRecordInstance;
        //            return udtBusinessRep.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(Categories))
        //        {
        //            Categories udtCategories = (Categories)objRecordInstance;
        //            return udtCategories.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(CategoryRef))
        //        {
        //            CategoryRef udtCategoryRef = (CategoryRef)objRecordInstance;
        //            return udtCategoryRef.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(ContactPerson))
        //        {
        //            ContactPerson udtContactPerson = (ContactPerson)objRecordInstance;
        //            return udtContactPerson.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(Email))
        //        {
        //            Email udtEmail = (Email)objRecordInstance;
        //            return udtEmail.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(Item))
        //        {
        //            Item udtItem = (Item)objRecordInstance;
        //            return udtItem.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(Payment))
        //        {
        //            Payment udtPayment = (Payment)objRecordInstance;
        //            return udtPayment.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(PaymentItem))
        //        {
        //            PaymentItem udtPaymentItem = (PaymentItem)objRecordInstance;
        //            return udtPaymentItem.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(PhoneNumber))
        //        {
        //            PhoneNumber udtPhoneNumber = (PhoneNumber)objRecordInstance;
        //            return udtPhoneNumber.Id;
        //        }
        //        else if (udtTableMetadata.TypeModelType == typeof(YearlyData))
        //        {
        //            YearlyData udtYearlyData = (YearlyData)objRecordInstance;
        //            return udtYearlyData.Id;
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Type is either not a database model or not supported by the JOIN utility.");
        //        }
        //    }

        //    /// <summary>
        //    /// Gets all foreign keys belonging to a record selected from a single table.
        //    /// </summary>
        //    /// <param name="objRecordInstance"></param>
        //    /// <param name="udtTableMetadata"></param>
        //    /// <returns></returns>
        //    private static List<ClsForeignKey> GetForeignKeys(object objRecordInstance, ClsMetadataHelper.ClsModelInfo udtTableMetadata)
        //    {

        //    }

        //    public ClsRecordData(object objRecordInstance, ClsMetadataHelper.ClsModelInfo udtTableMetadata, List<ClsMetadataHelper.ClsTableField> rgDesiredFields)
        //    {
        //        dictReportData = new Dictionary<ClsMetadataHelper.ClsTableField, string>();
        //        // Extracts the value of each desired field from the object.
        //        foreach (ClsMetadataHelper.ClsTableField udtField in rgDesiredFields)
        //        {
        //            // Gets the value from the instance.
        //            object objFieldValue = udtField.GetValue(objRecordInstance);
        //            // Converts the value to string form, as it will appear in the final report.
        //            string strFieldValue = objFieldValue?.ToString() ?? "";
        //            // Stores the field-value pair in the dictionary.
        //            dictReportData[udtField] = strFieldValue;
        //        }
        //        // Gets the value of the record's primary key.
        //        int intPrimaryKeyValue = GetPrimaryKey(objRecordInstance, udtTableMetadata);
        //        // Stores the value and table information in a ClsPrimaryKey object.
        //        ClsPrimaryKey udtPrimaryKey = new ClsPrimaryKey(udtTableMetadata, intPrimaryKeyValue);
        //        // Stores the primary key information in the list of primary keys.
        //        rgPrimaryKeys = new List<ClsPrimaryKey>(1) { udtPrimaryKey };
        //    }
        //}

        ///// <summary>
        ///// Represents a set of data collected from one or more database tables.
        ///// </summary>
        //public class ClsTableData
        //{

        //    private List<ClsRecordData> rgRecordDataList;

        //    /// <summary>
        //    /// This constructor selects all records from a specific table.
        //    /// </summary>
        //    /// <param name="dbContext"></param>
        //    /// <param name="udtTableMetadata"></param>
        //    /// <param name="rgDesiredFields"></param>
        //    public ClsTableData(DatabaseContext dbContext, ClsMetadataHelper.ClsModelInfo udtTableMetadata, List<ClsMetadataHelper.ClsTableField> rgDesiredFields)
        //    {
        //        // Queries for all instances of the specified model.
        //        object[] rgModels = udtTableMetadata.GetContextDbSet(dbContext).Cast<object>().ToArray();

        //        // The initial capacity of the record list is set to the length of the array of model instances.
        //        rgRecordDataList = new List<ClsRecordData>(rgModels.Length);

        //        for (int i = 0; i < rgModels.Length; i++)
        //        {
        //            object objModel = rgModels[i];
        //        }
        //    }
        //}
    }
}
