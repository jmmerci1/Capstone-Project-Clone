﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectorPortalPayPal
{
    /// <summary>
    /// The PayPal CSV file importing logic container.
    /// </summary>
    public class CsvParser
    {
        /// <summary>
        /// Import transaction data from a PayPal CSV file into the Payments db.
        /// </summary>
        /// <param name="csvFilePath">The full file path of the PayPal CSV export file.</param>
        /// <returns>An object containing a report of the import operation.</returns>
        public static TransactionImportResult RunImport(string csvFilePath)
        {
            List<Transaction> reportItems = LoadFromCSVFile(csvFilePath);
            List<Transaction> websitePayments = reportItems.Where(x => x.Type == "Website Payment").ToList();
            List<TransactionImportFailure> importFailures = new List<TransactionImportFailure>();
            List<Transaction> importSuccesses = new List<Transaction>();
            using (var context = new DatabaseContext())
            {
                foreach (var transaction in websitePayments)
                {
                    try
                    {
                        Payment duplicateCheckPayment = context.Payments
                            .FirstOrDefault(p => p.PayPalTransactionId.ToLower() == transaction.TransactionId.ToLower());
                        if(duplicateCheckPayment != null)
                        {
                            importFailures.Add(new TransactionImportFailure(transaction,
                                TransactionImportFailure.FailureReasons.Duplicate));
                            continue;
                        }

                        List<Business> businessesRelatedToEmail = new List<Business>();
                        List<Email> emails = context.Emails
                            .Include(e => e.ContactPerson)
                            .ThenInclude(cp => cp.BusinessReps)
                            .Where(e => e.EmailAddress.ToLower() == transaction.FromEmail.ToLower())
                            .ToList();
                        if(emails.Count() == 0)
                        {
                            importFailures.Add(new TransactionImportFailure(transaction, 
                                TransactionImportFailure.FailureReasons.FromEmailNotFound));
                            continue;
                        }
                        foreach(Email email in emails)
                        {
                            List<BusinessRep> businessRelations = email.ContactPerson.BusinessReps;
                            foreach(BusinessRep businessRep in businessRelations)
                            {
                                context.Entry(businessRep).Reference(br => br.Business).Load();
                                if(businessRep.Business != null)
                                {
                                    businessesRelatedToEmail.Add(businessRep.Business);
                                }
                            }
                        }

                        if(businessesRelatedToEmail.Count == 1)
                        {
                            string dateString = transaction.Date;
                            DateTime paymentTimestamp = DateTime.Parse(dateString);
                            Payment payment = new Payment
                            {
                                Business = businessesRelatedToEmail[0],
                                GrossPay = Decimal.Parse(transaction.Gross),
                                InvoiceNumber = transaction.InvoiceNumber,
                                Subject = transaction.Subject,
                                PayPalReferenceTxnId = transaction.ReferenceTxnId,
                                PayPalTransactionId = transaction.TransactionId,
                                ProcessingFees = Decimal.Parse(transaction.Fee),
                                Timestamp = paymentTimestamp
                            };
                            context.Payments.Add(payment);
                            context.SaveChanges();
                            importSuccesses.Add(transaction);
                        }
                        else if(businessesRelatedToEmail.Count > 1)
                        {
                            importFailures.Add(new TransactionImportFailure(transaction,
                                TransactionImportFailure.FailureReasons.MoreThanOneRelatedBusiness));
                        }
                        else
                        {
                            importFailures.Add(new TransactionImportFailure(transaction,
                                TransactionImportFailure.FailureReasons.NoRelatedBusiness));
                        }
                    }
                    catch (Exception e)
                    {
                        importFailures.Add(new TransactionImportFailure(transaction, 
                            TransactionImportFailure.FailureReasons.GeneralFailure));
                    }
                }
            }

            return new TransactionImportResult(importFailures,
                importSuccesses);
        }

        /// <summary>
        /// Load rows of a PayPal CSV export into Transaction objects.
        /// </summary>
        /// <param name="filePath">The full file path to the PayPal CSV export.</param>
        /// <returns>A list of transaction objects.</returns>
        private static List<Transaction> LoadFromCSVFile(string filePath)
        {
            List<Transaction> reportItems = new List<Transaction>();

            try
            {
                StreamReader fileReader = new StreamReader(filePath);

                string[] columnHeaders = fileReader.ReadLine().Split(',');

                string line = "";
                while((line = fileReader.ReadLine()) != null)
                {
                    string[] rowData = line.Split(',');
                    reportItems.Add(new Transaction(columnHeaders, rowData));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Make sure the CSV file is not open in any other programs.",
                        "Unable to open file");
            }

            return reportItems;
        }
    }
}
