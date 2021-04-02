using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorPortalPayPal
{
    class Transaction
    {
        public string Date { get; }
        public string Time { get; }
        public string TimeZone { get; }
        public string Name { get; }
        public string Type { get; }
        public string Status { get; }
        public string Currency { get; }
        public string Gross { get; }
        public string Fee { get; }
        public string Net { get; }
        public string FromEmail { get; }
        public string ToEmail { get; }
        public string TransactionId { get; }
        public string ShippingAddress { get; }
        public string AddressStatus { get; }
        public string ItemTitle { get; }
        public string ItemId { get; }
        public string ShippingAndHandlingAmount { get; }
        public string InsuranceAmount { get; }
        public string SalesTax { get; }
        public string ReferenceTxnId { get; }
        public string InvoiceNumber { get; }
        public string CustomNumber { get; }
        public string Quantity { get; }
        public string ReceiptId { get; }
        public string Balance { get; }
        public string AddressLine1 { get; }
        public string AddressLine2 { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public string Country { get; }
        public string ContactPhoneNumber { get; }
        public string Subject { get; }
        public string Note { get; }
        public string CountryCode { get; }
        public string BalanceImpact { get; }

        public Transaction(string[] headers, string[] csvLine)
        {
            Date = csvLine[Array.IndexOf(headers, HeaderStrings.Date)];
            Time = csvLine[Array.IndexOf(headers, HeaderStrings.Time)];
            TimeZone = csvLine[Array.IndexOf(headers, HeaderStrings.TimeZone)];
            Name = csvLine[Array.IndexOf(headers, HeaderStrings.Name)];
            Type = csvLine[Array.IndexOf(headers, HeaderStrings.Type)];
            Status = csvLine[Array.IndexOf(headers, HeaderStrings.Status)];
            Currency = csvLine[Array.IndexOf(headers, HeaderStrings.Currency)];
            Gross = csvLine[Array.IndexOf(headers, HeaderStrings.Gross)];
            Fee = csvLine[Array.IndexOf(headers, HeaderStrings.Fee)];
            Net = csvLine[Array.IndexOf(headers, HeaderStrings.Net)];
            FromEmail = csvLine[Array.IndexOf(headers, HeaderStrings.FromEmail)];
            ToEmail = csvLine[Array.IndexOf(headers, HeaderStrings.ToEmail)];
            TransactionId = csvLine[Array.IndexOf(headers, HeaderStrings.TransactionId)];
            ShippingAddress = csvLine[Array.IndexOf(headers, HeaderStrings.ShippingAddress)];
            AddressStatus = csvLine[Array.IndexOf(headers, HeaderStrings.AddressStatus)];
            ItemTitle = csvLine[Array.IndexOf(headers, HeaderStrings.ItemTitle)];
            ItemId = csvLine[Array.IndexOf(headers, HeaderStrings.ItemId)];
            ShippingAndHandlingAmount = csvLine[Array.IndexOf(headers, HeaderStrings.ShippingAndHandlingAmount)];
            InsuranceAmount = csvLine[Array.IndexOf(headers, HeaderStrings.InsuranceAmount)];
            SalesTax = csvLine[Array.IndexOf(headers, HeaderStrings.SalesTax)];
            ReferenceTxnId = csvLine[Array.IndexOf(headers, HeaderStrings.ReferenceTransactionId)];
            InvoiceNumber = csvLine[Array.IndexOf(headers, HeaderStrings.InvoiceNumber)];
            CustomNumber = csvLine[Array.IndexOf(headers, HeaderStrings.CustomNumber)];
            Quantity = csvLine[Array.IndexOf(headers, HeaderStrings.Quantity)];
            ReceiptId = csvLine[Array.IndexOf(headers, HeaderStrings.ReceiptId)];
            Balance = csvLine[Array.IndexOf(headers, HeaderStrings.Balance)];
            AddressLine1 = csvLine[Array.IndexOf(headers, HeaderStrings.AddressLine1)];
            AddressLine2 = csvLine[Array.IndexOf(headers, HeaderStrings.AddressLine2)];
            City = csvLine[Array.IndexOf(headers, HeaderStrings.City)];
            State = csvLine[Array.IndexOf(headers, HeaderStrings.State)];
            ZipCode = csvLine[Array.IndexOf(headers, HeaderStrings.ZipCode)];
            Country = csvLine[Array.IndexOf(headers, HeaderStrings.Country)];
            ContactPhoneNumber = csvLine[Array.IndexOf(headers, HeaderStrings.ContactPhoneNumber)];
            Subject = csvLine[Array.IndexOf(headers, HeaderStrings.Subject)];
            Note = csvLine[Array.IndexOf(headers, HeaderStrings.Note)];
            CountryCode = csvLine[Array.IndexOf(headers, HeaderStrings.CountryCode)];
            BalanceImpact = csvLine[Array.IndexOf(headers, HeaderStrings.BalanceImpact)];
        }

        private class HeaderStrings
        {
            public static string Date = "Date";
            public static string Time = "Time";
            public static string TimeZone = "TimeZone";
            public static string Name = "Name";
            public static string Type = "Type";
            public static string Status = "Status";
            public static string Currency = "Currency";
            public static string Gross = "Gross";
            public static string Fee = "Fee";
            public static string Net = "Net";
            public static string FromEmail = "From Email Address";
            public static string ToEmail = "To Email Address";
            public static string TransactionId = "Transaction ID";
            public static string ShippingAddress = "Shipping Address";
            public static string AddressStatus = "Address Status";
            public static string ItemTitle = "Item Title";
            public static string ItemId = "Item ID";
            public static string ShippingAndHandlingAmount = "Shipping and Handling Amount";
            public static string InsuranceAmount = "Insurance Amount";
            public static string SalesTax = "Sales Tax";
            public static string ReferenceTransactionId = "Reference Txn ID";
            public static string InvoiceNumber = "Invoice Number";
            public static string CustomNumber = "Custom Number";
            public static string Quantity = "Quantity";
            public static string ReceiptId = "Receipt ID";
            public static string Balance = "Balance";
            public static string AddressLine1 = "Address Line 1";
            public static string AddressLine2 = "Address Line 2/District/Neighborhood";
            public static string City = "Town/City";
            public static string State = "State/Province/Region/County/Territory/Prefecture/Republic";
            public static string ZipCode = "Zip/Postal Code";
            public static string Country = "Country";
            public static string ContactPhoneNumber = "Contact Phone Number";
            public static string Subject = "Subject";
            public static string Note = "Note";
            public static string CountryCode = "Country Code";
            public static string BalanceImpact = "Balance Impact";
        }
    }
}
