using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// File Name: HTMLPreviewGenerator.cs
/// 
/// Part of Project: DirectorsPortal
/// 
/// Original Author: Benjamin J. Dore
/// 
/// File Purpose:
///     This file is designed to dynamically write an HTML document.
///     Data from the database is loaded into the HTML document to populate currrent Members to
///     be displayed on the Chamber of Commerce website.
///         
/// </summary>

namespace DirectorsPortalWPF.ValidateWebsite
{

    /// <summary>
    /// Preview generator responsible for writing an HTML file with
    /// latest membership data.
    /// </summary>
    class HtmlPreviewGenerator
    {

        // Writer will be used to write an HTML file
        private StreamWriter GWriter;

        /// <summary>
        /// Method writes an HTML document using the StreamWriter and dynamically adds Member
        /// details using the Entity Framework Database.
        /// </summary>
        public void GeneratePreview()
        {

            // Using the StreamWriter to Write to the MembershipTemplate.html file (under the resources folder)
            using (GWriter = new StreamWriter(GetTemplateLocation(),false))
            {
                // Featured businesses header
                GWriter.WriteLine("<h3 style=\"text-align: center; font-family: sans-serif; color:#448eb8; font-weight: bold;" +
                    " font-size: 1cm\">Featured Businesses</h3>");

                // Div to align content within margins
                GWriter.WriteLine("<div style=\"margin-left: 10%; margin-right:10% \">");

                WriteButtons();
                PrintMembersAZ();
                PrintMembersCategory();    
                PrintMembersAssociate();
                GWriter.WriteLine("</div>");

                WriteJavaScript();
            }

        }

        /// <summary>
        /// Prints a table containing members from the database in Alphabetical order.
        /// </summary>
        private void PrintMembersAZ()
        {
            List<string> rgAlpha = new List<string>() {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P",
                "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
            };

            // Second table to hold Member details A_Z
            GWriter.WriteLine("<table id=\"Members A-Z\" style=\"font-family: Arial, Arial, Helvetica, sans-serif; margin: auto;\">");
            GWriter.WriteLine($"<tr><td><strong>All Members from A to Z</strong></td><tr>");
            using (var dbContext = new DatabaseContext())     // Database context will be used to query membership details
            {
                List<Business> rgAllBusinesses = dbContext.Businesses.ToList();  // List of all businesses in DB

                // Iterate through each Member and put them in the table
                foreach (string strAlpha in rgAlpha)
                {
                    // Only print the Alphabetic character IF there are businesses that start with that Alphabetic character
                    List<Business> rgValidBusinessesForAlpha = rgAllBusinesses.FindAll(e => e.GStrBusinessName.ToUpper().StartsWith(strAlpha));
                    if (rgValidBusinessesForAlpha.Count > 0)
                    {
                        GWriter.WriteLine($"<tr><td>{strAlpha}</td><tr>");
                    }
                    int intNumberOfItems = 3;  // The number of Members per row
                    foreach (Business busCurrentBusiness in rgValidBusinessesForAlpha)
                    {
                        if (busCurrentBusiness.GStrBusinessName.ToUpper().StartsWith(strAlpha))
                        {
                            // Phone numbers, addresses, and email addresses for current business
                            List<PhoneNumber> rgCurrentBusPhones = dbContext.PhoneNumbers.Where(e => e.GIntContactPersonId.Equals(busCurrentBusiness.GIntId)).ToList();
                            List<Address> rgCurrentBusAddresses = dbContext.Addresses.Where(e => e.GIntId.Equals(busCurrentBusiness.GIntId)).ToList();
                            List<Email> rgCurrentBusEmails = dbContext.Emails.Where(e => e.GIntContactPersonId.Equals(busCurrentBusiness.GIntId)).ToList();

                            if (intNumberOfItems == 3)             // Create a new row if there are three items to add to the row
                                GWriter.WriteLine("<tr>");

                            PrintMemberToTemplate(busCurrentBusiness, rgCurrentBusPhones, rgCurrentBusAddresses, rgCurrentBusEmails);

                            if (intNumberOfItems == 1)             // If this is the last of the three items in the row, then close the row
                            {
                                GWriter.WriteLine("</tr>");
                                intNumberOfItems = 4;              // 4 minus 3 (from the decremator below) equals 3 items per row
                            }
                        }
                        intNumberOfItems--;    // Next item in the row
                    }
                }
            }
            GWriter.WriteLine("</table>");
        }

        /// <summary>
        /// Prints a table containing members from the database and all members are grouped under a business
        /// Category.
        /// </summary>
        private void PrintMembersCategory()
        {
            // Third table to hold Member details by Category
            GWriter.WriteLine("<table id=\"Category\" hidden=\"hidden\" style=\"font-family: Arial, Arial, Helvetica, sans-serif; margin: auto;\">");
            GWriter.WriteLine("<tr><td><strong>Members by Category</strong></td><tr>");

            using (var dbContext = new DatabaseContext())     // Database context will be used to query membership details
            {
                List<Business> rgAllBusinesses = dbContext.Businesses.ToList();  // List of all businesses in DB
                List<Categories> rgAllCategories = dbContext.Categories.ToList();

                foreach (Categories cat in rgAllCategories)
                {
                    // Only print the category IF there are businesses that are in that category.
                    List<Business> rgValidBusinessesMatchingCategory = rgAllBusinesses.FindAll(e => e.GCategories.First().GStrCategory.Equals(cat.GStrCategory));
                    if (rgValidBusinessesMatchingCategory.Count > 0) 
                    {
                        GWriter.WriteLine($"<tr><td>{cat.GStrCategory}</td><tr>");
                    }

                    // Iterate through each Member and put them in the table
                    int intNumberOfItems = 3;  // The number of Members per row
                    foreach (Business busCurrentBusiness in rgValidBusinessesMatchingCategory)
                    {
                        if (busCurrentBusiness.GCategories[0].GStrCategory.Equals(cat.GStrCategory))
                        {
                            // Phone numbers, addresses, and email addresses for current business
                            List<PhoneNumber> rgCurrentBusPhones = dbContext.PhoneNumbers.Where(e => e.GIntContactPersonId.Equals(busCurrentBusiness.GIntId)).ToList();
                            List<Address> rgCurrentBusAddresses = dbContext.Addresses.Where(e => e.GIntId.Equals(busCurrentBusiness.GIntId)).ToList();
                            List<Email> rgCurrentBusEmails = dbContext.Emails.Where(e => e.GIntContactPersonId.Equals(busCurrentBusiness.GIntId)).ToList();

                            if (intNumberOfItems == 3)             // Create a new row if there are three items to add to the row
                                GWriter.WriteLine("<tr>");

                            PrintMemberToTemplate(busCurrentBusiness, rgCurrentBusPhones, rgCurrentBusAddresses, rgCurrentBusEmails);

                            if (intNumberOfItems == 1)             // If this is the last of the three items in the row, then close the row
                            {
                                GWriter.WriteLine("</tr>");
                                intNumberOfItems = 4;              // 4 minus 3 (from the decremator below) equals 3 items per row
                            }
                        }
                        intNumberOfItems--;    // Next item in the row
                    }
                }
            }
            GWriter.WriteLine("</table>");
        }

        /// <summary>
        /// Prints a table only containing Associate members from the database in Alphabetical order.
        /// </summary>
        private void PrintMembersAssociate()
        {
            // Fourth table to hold Member details only for Associate members
            GWriter.WriteLine("<table id=\"Associates\" hidden=\"hidden\" style=\"font-family: Arial, Arial, Helvetica, sans-serif; margin: auto;\">");
            GWriter.WriteLine("<tr><td><strong>Associates Members</strong></td><tr>");

            using (var dbContext = new DatabaseContext())     // Database context will be used to query membership details
            {
                List<Business> rgAllBusinesses = dbContext.Businesses.Where(e => (int)e.GEnumMembershipLevel == 2).ToList();  // List of all associate businesses in DB
                rgAllBusinesses = rgAllBusinesses.OrderBy(e => e.GStrBusinessName).ToList();

                // Iterate through each Member and put them in the table
                int intNumberOfItems = 3;  // The number of Members per row
                foreach (Business busCurrentBusiness in rgAllBusinesses)
                {
                    // Phone numbers, addresses, and email addresses for current business
                    List<PhoneNumber> rgCurrentBusPhones = dbContext.PhoneNumbers.Where(e => e.GIntContactPersonId.Equals(busCurrentBusiness.GIntId)).ToList();
                    List<Address> rgCurrentBusAddresses = dbContext.Addresses.Where(e => e.GIntId.Equals(busCurrentBusiness.GIntId)).ToList();
                    List<Email> rgCurrentBusEmails = dbContext.Emails.Where(e => e.GIntContactPersonId.Equals(busCurrentBusiness.GIntId)).ToList();

                    if (intNumberOfItems == 3)             // Create a new row if there are three items to add to the row
                        GWriter.WriteLine("<tr>");

                    PrintMemberToTemplate(busCurrentBusiness, rgCurrentBusPhones, rgCurrentBusAddresses, rgCurrentBusEmails);

                    if (intNumberOfItems == 1)             // If this is the last of the three items in the row, then close the row
                    {
                        GWriter.WriteLine("</tr>");
                        intNumberOfItems = 4;              // 4 minus 3 (from the decremator below) equals 3 items per row
                    }

                    intNumberOfItems--;    // Next item in the row
                }
            }
            GWriter.WriteLine("</table>");
        }

        /// <summary>
        /// Writes in the JavaScript into the HTML File. This JavaScript defines the button controls,
        /// essentially hides and unhides the desired contents on the page.
        /// 
        /// JavaScript Layout (For Readability):
        /// 
        ///     function visibility(id) 
        ///     {
        ///         var x = document.getElementById(id);
        ///         
        ///         if (id === "Members A-Z")
        ///         {
        ///             x.hidden = "";
        ///             document.getElementById("Category").hidden = "hidden";
        ///             document.getElementById("Associates").hidden = "hidden";
        ///         }
        ///
        ///         if (id === "Category")
        ///         {
        ///             x.hidden = ""; document.getElementById("Members A-Z").hidden = "hidden";
        ///             document.getElementById("Associates").hidden = "hidden";
        ///         }
        ///    
        ///         if (id === "Associates")
        ///         {
        ///             x.hidden = ""; document.getElementById("Members A-Z").hidden = "hidden";
        ///             document.getElementById("Category").hidden = "hidden";
        ///         }
        ///     }
        ///     
        /// </summary>
        private void WriteJavaScript()
        {
            // JavaScript to allow the buttons to toggle between Members A-Z, Category, and Associate.
            GWriter.WriteLine("<script>");
            GWriter.WriteLine(
                "function visibility(id) {" +
                "var x = document.getElementById(id);" +
                "if (id === \"Members A-Z\") {" +
                "x.hidden = \"\";" +
                "document.getElementById(\"Category\").hidden = \"hidden\";" +
                "document.getElementById(\"Associates\").hidden = \"hidden\";" +
                "}" +
                "if (id === \"Category\") {" +
                "x.hidden = \"\";" +
                "document.getElementById(\"Members A-Z\").hidden = \"hidden\";" +
                "document.getElementById(\"Associates\").hidden = \"hidden\";" +
                "}" +
                "if (id === \"Associates\") {" +
                "x.hidden = \"\";" +
                "document.getElementById(\"Members A-Z\").hidden = \"hidden\";" +
                "document.getElementById(\"Category\").hidden = \"hidden\";" +
                "}" +
                "}"
                );
            GWriter.WriteLine("</script>");
        }

        /// <summary>
        /// Writes the buttons that will display above the Chamber member details.
        /// Will allow to cycle between Members A-Z, Members by Category, and Associate members.
        /// </summary>
        private void WriteButtons()
        {
            // First Table to hold buttons
            GWriter.WriteLine("<table style=\"font-family: Arial, Arial, Helvetica, sans-serif; margin: auto\">");

            // Include the buttons below...
            GWriter.WriteLine("<tr>");

            // Members A-Z
            GWriter.WriteLine("<td style=\"padding: 10px 10px 10px 10px; text-align:center\">");

            GWriter.WriteLine("<button type=\"button\"  onclick=\"visibility('Members A-Z')\" style=\"display: inline-block; text-decoration:none; " +
                "height: 65px; background-color:#4d97E4; border-radius: 5px; padding: 0px 30px 0px 0px\" href=\"http://www.google.com\">");

            GWriter.WriteLine("<span style=\"color:#fff !important;height: 65px; white-space:nowrap; line-height: " +
                "65px; font-size: 18px; padding: 0px 0px 0px 30px; display: block; font-weight: bold; text-shadow: 0px 1px 1px rgb(0,0,0);" +
                " text-align: center; font-family: Arial, Arial, Helvetica, sans-serif\">Members A-Z</span>");

            GWriter.WriteLine("</button>");
            GWriter.WriteLine("</td>");

            // By Category
            GWriter.WriteLine("<td style=\"padding: 10px 10px 10px 10px; text-align:center\">");

            GWriter.WriteLine("<button type=\"button\"  onclick=\"visibility('Category')\" style=\"display: inline-block; text-decoration:none;" +
                " height: 65px; background-color:#4d97E4; border-radius: 5px; padding: 0px 30px 0px 0px\" href=\"http://www.google.com\">");
            
            GWriter.WriteLine("<span style=\"color:#fff !important;height: 65px; white-space:nowrap; line-height: 65px; font-size: 18px;" +
                " padding: 0px 0px 0px 30px; display: block; font-weight: bold; text-shadow: 0px 1px 1px rgb(0,0,0); text-align: center;" +
                " font-family: Arial, Arial, Helvetica, sans-serif\">Category</span>");

            GWriter.WriteLine("</button>");
            GWriter.WriteLine("</td>");

            // Associates
            GWriter.WriteLine("<td style=\"padding: 10px 10px 10px 10px; text-align:center\">");

            GWriter.WriteLine("<button type=\"button\"  onclick=\"visibility('Associates')\" style=\"display: inline-block; text-decoration:none;" +
                " height: 65px; background-color:#4d97E4; border-radius: 5px; padding: 0px 30px 0px 0px\" href=\"http://www.google.com\">");
            
            GWriter.WriteLine("<span style=\"color:#fff !important;height: 65px; white-space:nowrap; line-height: 65px; font-size: 18px;" +
                " padding: 0px 0px 0px 30px; display: block; font-weight: bold; text-shadow: 0px 1px 1px rgb(0,0,0); text-align: center;" +
                " font-family: Arial, Arial, Helvetica, sans-serif\">Associates</span>");

            GWriter.WriteLine("</button>");
            GWriter.WriteLine("</td>");

            GWriter.WriteLine("</tr>");

            GWriter.WriteLine("</table>");
        }

        /// <summary>
        /// Prints a member and all of the member's details onto the HTML template.
        /// </summary>
        /// <param name="busCurrentBusiness">The current business to be printed onto the template.</param>
        /// <param name="rgCurrentBusPhones">All phone numbers associated with the current business</param>
        /// <param name="rgCurrentBusAddresses">All addresses associated with the current business</param>
        /// <param name="rgCurrentBusEmails">All email addresses associated with the current business</param>
        private void PrintMemberToTemplate(Business busCurrentBusiness, List<PhoneNumber> rgCurrentBusPhones, List<Address> rgCurrentBusAddresses, List<Email> rgCurrentBusEmails)
        {
            GWriter.WriteLine("<td style=\"padding: 10px 10px 10px 10px; color:#6d6d6d\">");
            GWriter.WriteLine($"<strong style=\"color: #a88d2e\">{busCurrentBusiness.GStrBusinessName}</strong>");
            GWriter.WriteLine($"<br>{rgCurrentBusAddresses[0].GStrAddress}");
            GWriter.WriteLine($"<br>{rgCurrentBusAddresses[0].GStrCity}, {rgCurrentBusAddresses[0].GStrState} {rgCurrentBusAddresses[0].GIntZipCode} ");
            GWriter.WriteLine($"<br>{rgCurrentBusPhones[0].GStrPhoneNumber}");
            GWriter.WriteLine($"<br>{rgCurrentBusPhones[0].GStrPhoneNumber}");
            GWriter.WriteLine($"<br>Map | <a href=\"mailto:{rgCurrentBusEmails[0].GStrEmailAddress} \">Email</a> | <a href=\"{busCurrentBusiness.GStrWebsite}\">Web</a>");
            GWriter.WriteLine("</td>");
        }

        /// <summary>
        /// Used to get the filepath of the HTML template that will be used to generate the Members screen
        /// on the Weebly website.
        /// </summary>
        /// <returns></returns>
        public string GetTemplateLocation()
        {
            var strExePath = AppDomain.CurrentDomain.BaseDirectory;
            var dinfPagesFolder = Directory.GetParent(strExePath).Parent.Parent;
            string strTemplateFullPath = dinfPagesFolder.FullName + "\\Resources\\MembershipTemplate.html";
            Console.WriteLine(strTemplateFullPath);

            return strTemplateFullPath;
        }

    }
}
