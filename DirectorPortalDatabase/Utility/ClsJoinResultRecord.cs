using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorPortalDatabase.Models;

namespace DirectorPortalDatabase.Utility
{
    /// <summary>
    /// Used to store a row of the result of a JOIN operation.
    /// Each class that could be involved in the joining is included as a property.
    /// </summary>
    public class ClsJoinResultRecord
    {
        public Address UdtAddress { get; set; }
        public Business UdtBusiness { get; set; }
        public BusinessRep UdtBusinessRep { get; set; }
        public Categories UdtCategories { get; set; }
        public CategoryRef UdtCategoryRef { get; set; }
        public ContactPerson UdtContactPerson { get; set; }
        public Email UdtEmail { get; set; }
        public EmailGroup UdtEmailGroup { get; set; }
        public EmailGroupMember UdtEmailGroupMember { get; set; }
        public Payment UdtPayment { get; set; }
        public PaymentItem UdtPaymentItem { get; set; }
        public PhoneNumber UdtPhoneNumber { get; set; }
        public YearlyData UdtYearlyData { get; set; }

        public ClsJoinResultRecord()
        {

        }

        /// <summary>
        /// Gets a single model from this record. The record returned will match the Type passed in.
        /// Throws an exception if the Type passed in does not correspond to a property of this class.
        /// </summary>
        /// <param name="typeModelType"></param>
        /// <returns></returns>
        public object GetObjectByType(Type typeModelType)
        {
            if (typeModelType == typeof(Address))
                return this.UdtAddress;
            else if (typeModelType == typeof(Business))
                return this.UdtBusiness;
            else if (typeModelType == typeof(BusinessRep))
                return this.UdtBusinessRep;
            else if (typeModelType == typeof(Categories))
                return this.UdtCategories;
            else if (typeModelType == typeof(CategoryRef))
                return this.UdtCategoryRef;
            else if (typeModelType == typeof(ContactPerson))
                return this.UdtContactPerson;
            else if (typeModelType == typeof(Email))
                return this.UdtEmail;
            else if (typeModelType == typeof(EmailGroup))
                return this.UdtEmailGroup;
            else if (typeModelType == typeof(EmailGroupMember))
                return this.UdtEmailGroupMember;
            else if (typeModelType == typeof(Payment))
                return this.UdtPayment;
            else if (typeModelType == typeof(PaymentItem))
                return this.UdtPaymentItem;
            else if (typeModelType == typeof(PhoneNumber))
                return this.UdtPhoneNumber;
            else if (typeModelType == typeof(YearlyData))
                return this.UdtYearlyData;
            else
                throw new ArgumentException($"ClsJoinResultRecord contains no property of type {typeModelType.Name}");
        }

        /// <summary>
        /// Returns a copy of this ClsJoinResultRecord. Only the references are copied, not the objects themselves.
        /// </summary>
        /// <returns></returns>
        private ClsJoinResultRecord Copy ()
        {
            return new ClsJoinResultRecord
            {
                UdtAddress = UdtAddress,
                UdtBusiness = UdtBusiness,
                UdtBusinessRep = UdtBusinessRep,
                UdtCategories = UdtCategories,
                UdtCategoryRef = UdtCategoryRef,
                UdtContactPerson = UdtContactPerson,
                UdtEmail = UdtEmail,
                UdtEmailGroup = UdtEmailGroup,
                UdtEmailGroupMember = UdtEmailGroupMember,
                UdtPayment = UdtPayment,
                UdtPaymentItem = UdtPaymentItem,
                UdtPhoneNumber = UdtPhoneNumber,
                UdtYearlyData = UdtYearlyData
            };
        }

        /// <summary>
        /// Returns a copy of this object, but with the appropriate property value replaced by the one provided.
        /// </summary>
        /// <param name="objReplacement"></param>
        /// <returns></returns>
        public ClsJoinResultRecord CopyAndReplace(object objReplacement)
        {

            ClsJoinResultRecord udtNewRecord = Copy();

            // If the replacement is null, don't replace anything.
            if (objReplacement != null)
            {
                // The property value to replace depends on the type of the replacement object.
                Type typeReplacementType = objReplacement.GetType();

                if (typeReplacementType == typeof(Address))
                {
                    udtNewRecord.UdtAddress = (Address)objReplacement;
                }
                else if (typeReplacementType == typeof(Business))
                {
                    udtNewRecord.UdtBusiness = (Business)objReplacement;
                }
                else if (typeReplacementType == typeof(BusinessRep))
                {
                    udtNewRecord.UdtBusinessRep = (BusinessRep)objReplacement;
                }
                else if (typeReplacementType == typeof(Categories))
                {
                    udtNewRecord.UdtCategories = (Categories)objReplacement;
                }
                else if (typeReplacementType == typeof(CategoryRef))
                {
                    udtNewRecord.UdtCategoryRef = (CategoryRef)objReplacement;
                }
                else if (typeReplacementType == typeof(ContactPerson))
                {
                    udtNewRecord.UdtContactPerson = (ContactPerson)objReplacement;
                }
                else if (typeReplacementType == typeof(Email))
                {
                    udtNewRecord.UdtEmail = (Email)objReplacement;
                }    
                else if (typeReplacementType == typeof(EmailGroup))
                {
                    udtNewRecord.UdtEmailGroup = (EmailGroup)objReplacement;
                }
                else if (typeReplacementType == typeof(EmailGroupMember))
                {
                    udtNewRecord.UdtEmailGroupMember = (EmailGroupMember)objReplacement;
                }
                else if (typeReplacementType == typeof(Payment))
                {
                    udtNewRecord.UdtPayment = (Payment)objReplacement;
                }
                else if (typeReplacementType == typeof(PaymentItem))
                {
                    udtNewRecord.UdtPaymentItem = (PaymentItem)objReplacement;
                }
                else if (typeReplacementType == typeof(PhoneNumber))
                {
                    udtNewRecord.UdtPhoneNumber = (PhoneNumber)objReplacement;
                }
                else if (typeReplacementType == typeof(YearlyData))
                {
                    udtNewRecord.UdtYearlyData = (YearlyData)objReplacement;
                }
                else
                {
                    throw new ArgumentException($"There is no property of type {typeReplacementType}");
                }
            }

            return udtNewRecord;
        }

    }
}
