using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;

namespace DirectorPortalPayPal
{
    public class CsvParser
    {
        public static void RunImport(string csvFilePath)
        {
            List<Transaction> reportItems = LoadFromCSVFile(csvFilePath);
            List<Transaction> websitePayments = reportItems.Where(x => x.Type == "Website Payment").ToList();
            List<Transaction> failedToImportTransactions = new List<Transaction>();
            int successCount = 0;
            using (var context = new DatabaseContext())
            {
                foreach (var transaction in websitePayments)
                {
                    try
                    {
                        
                        successCount++;
                    }
                    catch (Exception)
                    {
                        failedToImportTransactions.Add(transaction);
                    }
                }
            }

            /* Show popup to handle failed imports & give a summary of what happened */
            Console.WriteLine("PayPal Web Payment Import: " + successCount 
                + " successes, " + failedToImportTransactions.Count()
                + " failures.");
        }

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
            catch (Exception ex)
            {
                throw ex;
            }

            return reportItems;
        }
    }
}
